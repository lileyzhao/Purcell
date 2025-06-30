using Sylvan.Data.Excel;

namespace PurcellLibs.Providers.Excel;

/// <summary>
/// 使用 Sylvan.Data.Excel 实现的 Excel 表格查询器
/// </summary>
public class SylvanExcelTableReader(Stream stream, TableFileType tableFileType) : TableReaderBase(stream)
{
    private static readonly double DayTicks = 864000000000.0;
    private static readonly long DayTicksL = 864000000000L;
    private static readonly DateTime Epoch1900 = new(1899, 12, 30);

    private readonly ExcelWorkbookType _workbookType = tableFileType switch
    {
        TableFileType.Xlsx => ExcelWorkbookType.ExcelXml,
        TableFileType.Xls => ExcelWorkbookType.Excel,
        _ => throw new InvalidEnumArgumentException($"不支持的 {tableFileType} 类型")
    };

    /// <inheritdoc/>
    public override IEnumerable<string> GetWorksheets()
    {
        if (!_stream.CanSeek)
        {
            throw new NotSupportedException(
                "流不支持随机访问，不允许获取工作表列表。顺序流在枚举工作表后无法重置再进行表格数据读取。");
        }

        // 重置流位置以确保从头开始读取
        _stream.Position = 0;

        // 创建 Excel 读取器
        using ExcelDataReader reader = ExcelDataReader.Create(
            _stream,
            _workbookType,
            new ExcelDataReaderOptions
            {
                Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true, OwnsStream = false
            }
        );

        // 返回所有工作表列表
        foreach (string worksheetName in reader.WorksheetNames)
        {
            yield return worksheetName;
        }
    }

    /// <inheritdoc/>
    public override IEnumerable<IDictionary<int, object?>> ReadTable(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        // 重置流位置以确保从头开始读取
        if (_stream.CanSeek) _stream.Position = 0;

        // 创建 Excel 读取器
        using ExcelDataReader reader = ExcelDataReader.Create(
            _stream,
            _workbookType,
            new ExcelDataReaderOptions
            {
                Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true, OwnsStream = false
            }
        );

        LocateSheet(reader, tableConfig.SheetIndex, tableConfig.SheetName);

        int columnLength = 0;
        int rowIndex = -1;
        while (reader.Read())
        {
            rowIndex++;
            progress?.Report(rowIndex + 1); // 报告进度
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            // 读取表头并把表头作为第一行数据返回且记录表头行的列数作为数据的列数
            if (tableConfig.HasHeader && rowIndex == tableConfig.GetHeaderStart().RowIndex)
            {
                if (reader.RowFieldCount == 0)
                {
                    throw new InvalidDataException($"无法解析表头：第 {rowIndex + 1} 行为空行");
                }

                // 记录表头行列数
                columnLength = reader.RowFieldCount;

                Dictionary<int, object?> headerData = new(columnLength);
                for (int colIndex = 0; colIndex < columnLength; colIndex++)
                {
                    if (colIndex < tableConfig.GetHeaderStart().ColumnIndex)
                        headerData[colIndex] = null;
                    else
                    {
                        headerData[colIndex] =
                            GetExcelValueSafely(reader, colIndex, rowIndex, tableConfig.IgnoreParseError);
                    }
                }

                // 第一个返回的是表头行数据
                yield return headerData;
            }

            // 未到达数据行直接跳过
            if (rowIndex < tableConfig.GetDataStart().RowIndex) continue;

            // ⬇⬇⬇ 读取行数据

            if (columnLength == 0)
            {
                if (reader.RowFieldCount == 0)
                {
                    throw new InvalidDataException($"无法解析首行数据行：第 {rowIndex + 1} 行为空行");
                }

                columnLength = reader.RowFieldCount;
            }

            Dictionary<int, object?> rowData = new(columnLength);

            for (int colIndex = 0; colIndex < columnLength; colIndex++)
            {
                if (colIndex < tableConfig.GetHeaderStart().ColumnIndex || colIndex >= reader.RowFieldCount)
                    rowData[colIndex] = null;
                else
                {
                    rowData[colIndex] = GetExcelValueSafely(reader, colIndex, rowIndex, tableConfig.IgnoreParseError);
                }
            }

            yield return rowData;
        }
    }

    /// <summary>
    /// 定位到目标工作表。根据配置优先使用表名查找，若不存在则使用表索引，
    /// 并将Excel读取器指针移动到该工作表位置。
    /// </summary>
    /// <param name="reader">读取器</param>
    /// <param name="sheetIndex"></param>
    /// <param name="sheetName"></param>
    /// <exception cref="IndexOutOfRangeException">当指定的工作表索引超出有效范围时抛出</exception>
    private void LocateSheet(ExcelDataReader reader, int sheetIndex, string? sheetName = null)
    {
        if (sheetIndex < 0)
            throw new InvalidOperationException("工作表索引不能小于0");

        int targetIndex = sheetIndex;

        int worksheetsCount = 0;
        foreach (string worksheetName in reader.WorksheetNames)
        {
            if (!string.IsNullOrEmpty(sheetName) && worksheetName.Equals(sheetName))
            {
                targetIndex = worksheetsCount;
            }

            worksheetsCount++;
        }

        // 检查指定的targetIndex是否在有效范围内
        if (targetIndex > worksheetsCount)
            throw new IndexOutOfRangeException($"工作表索引超出范围：索引 {targetIndex} 超过了工作表总数 {worksheetsCount + 1}");

        // 定位到目标工作表：默认从第一个工作表开始，逐步移动到目标工作表
        for (int i = 0; i < targetIndex; i++) reader.NextResult();
    }

    /// <summary>
    /// 将Excel单元格值安全地转换为.NET类型对象
    /// </summary>
    /// <param name="reader">Excel数据读取器</param>
    /// <param name="columnIndex">要读取的列索引</param>
    /// <param name="rowIndex">当前行索引(仅用于错误提示)</param>
    /// <param name="ignoreParseError">是否在值解析失败时抛出异常，默认为 false</param>
    /// <returns>转换后的值。单元格为空或转换失败时返回 null</returns>
    /// <remarks>
    /// 根据Excel数据类型智能转换:
    /// <list type="bullet">
    ///   <item><description>空指针 → Null</description></item>
    ///   <item><description>布尔值 → bool</description></item>
    ///   <item><description>日期 → DateTime</description></item>
    ///   <item><description>日期 → TimeSpan</description></item>
    ///   <item><description>数值 → double</description></item>
    ///   <item><description>字符串 → string</description></item>
    /// </list>
    /// </remarks>
    private object? GetExcelValueSafely(ExcelDataReader reader, int columnIndex, int rowIndex, bool ignoreParseError)
    {
        bool isDbNull = reader.IsDBNull(columnIndex);
        if (isDbNull) return null;

        object excelValue = reader.GetExcelValue(columnIndex);
        ExcelDataType excelDataType = reader.GetExcelDataType(columnIndex);
        FormatKind? formatKind = reader.GetFormat(columnIndex)?.Kind;

        try
        {
            switch (excelDataType)
            {
                case ExcelDataType.Boolean:
                    return excelValue is bool outBoolean ? outBoolean : reader.GetBoolean(columnIndex);
                case ExcelDataType.DateTime:
                    return excelValue is DateTime outDateTime ? outDateTime : reader.GetDateTime(columnIndex);
                case ExcelDataType.Numeric when formatKind is FormatKind.Date:
                {
                    double doubleValue = excelValue is double outDouble ? outDouble : reader.GetDouble(columnIndex);
                    DateTime? numDateObject = ConvertToDate(doubleValue);
                    // tick 9999999 视为 double 精度问题
                    return numDateObject.HasValue && numDateObject.Value.Ticks % TimeSpan.TicksPerSecond == 9_999_999
                        ? numDateObject.Value.AddTicks(1)
                        : numDateObject;
                }
                case ExcelDataType.Numeric when formatKind is FormatKind.Time:
                {
                    double doubleValue = excelValue is double outDouble ? outDouble : reader.GetDouble(columnIndex);
                    DateTime? numDateObject = ConvertToDate(doubleValue);
                    // tick 9999999 视为 double 精度问题
                    return numDateObject.HasValue && numDateObject.Value.Ticks % TimeSpan.TicksPerSecond == 9_999_999
                        ? numDateObject.Value.AddTicks(1).TimeOfDay
                        : numDateObject?.TimeOfDay;
                }
                case ExcelDataType.Numeric:
                {
                    return excelValue is double outDouble ? outDouble : reader.GetDouble(columnIndex);
                }
                case ExcelDataType.String:
                    return excelValue is bool outString ? outString : reader.GetString(columnIndex);
                case ExcelDataType.Null:
                case ExcelDataType.Error:
                default:
                    return null;
            }
        }
        catch
        {
            if (!ignoreParseError) throw;
            return null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private DateTime? ConvertToDate(double value)
    {
        if (value is >= 60.0 and < 61.0 or < -657435.0 or >= 2958466.0)
        {
            // Excel 1900 日期系统的特殊处理
            // 1900年2月29日是一个错误日期，Excel错误地将其视为有效日期
            return null;
        }

        long saltTicks = value < 61 ? DayTicksL : 0; // 处理 Excel 1900 日期系统闰年Bug的偏移
        return Epoch1900.AddTicks(saltTicks + (long)(value * DayTicks));
    }
}