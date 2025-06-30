using System.Dynamic;
using Shapeless;

namespace PurcellLibs.Providers;

/// <summary>
/// 表格读取适配器，提供统一的表格数据读取接口。将底层的文件格式读取器包装为统一的数据访问层。
/// </summary>
public class TableReaderAdapter(Stream stream, TableFileType fileType, bool ownsStream) : DisposableBase
{
    /// <summary>
    /// 底层表格读取器，根据文件类型创建相应的实现。
    /// </summary>
    private readonly ITableReader _reader = fileType switch
    {
        TableFileType.Csv => new CsvHelperTableReader(stream),
        TableFileType.Xlsx => new SylvanExcelTableReader(stream, fileType),
        TableFileType.Xls => new SylvanExcelTableReader(stream, fileType),
        _ => throw new NotSupportedException($"不支持读取的表格类型：{fileType}")
    };

    /// <summary>
    /// 读取表格数据并返回强类型对象序列。
    /// </summary>
    /// <typeparam name="T">目标类型，必须是引用类型且具有无参构造函数。</typeparam>
    /// <param name="tableConfig">表格配置信息。</param>
    /// <param name="progress">可选的进度报告器。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    /// <returns>强类型对象序列。</returns>
    /// <exception cref="NotImplementedException">该功能暂未实现。</exception>
    internal IEnumerable<T> ReadTyped<T>(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        Dictionary<int, List<PurColumn>>? indexedColumns = null; // 用于存储列索引和对应的列配置

        int dataIndex = 0; // 数据行计数器
        foreach (IDictionary<int, object?> rawRowData in _reader.ReadTable(tableConfig, progress, cancelToken))
        {
            // 解析列配置，并返回是否数据行（true 表示是数据行，应该继续执行循环体来作为数据行处理），
            // 如果返回 false 则表示当前行不是数据行，应该跳过当前行。
            if (!ResolveGenericColumns<T>(rawRowData, tableConfig, ref indexedColumns))
            {
                continue;
            }

            // 如果没有任何表头配置则抛出异常
            if (indexedColumns == null || indexedColumns.Keys.Count == 0)
            {
                if (tableConfig.HasHeader)
                    throw new InvalidOperationException("无法将表格表头映射到目标类型的任何属性。请使用 [PurColumn] 特性或动态配置映射规则来指定属性与列的对应关系。");
                throw new InvalidOperationException(
                    "当前配置为无表头读取模式 (PurTable.HasHeader=false)。请使用 [PurColumn] 特性或动态配置指定列索引，或配置相应的映射规则。");
            }

            if (tableConfig.MaxReadRows >= 0 && dataIndex >= tableConfig.MaxReadRows)
                yield break; // 如果已达到最大读取行数，则停止迭代

            // 读取数据行
            T dataItem = new();
            for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
            {
                object? cellValue = rawRowData[colIndex];

                if (!indexedColumns.TryGetValue(colIndex, out List<PurColumn>? matchedColumns))
                    continue;

                foreach (PurColumn columnConfig in matchedColumns)
                {
                    // 使用转换器转换值为目标类型，且判断 finalValue == null 防止覆盖默认值
                    if (!TryConvert(cellValue, columnConfig.PropertyType, columnConfig, tableConfig,
                            out object? finalValue)
                        || finalValue == null)
                    {
                        if (columnConfig.DefaultValue == null ||
                            columnConfig.DefaultValue.GetType().GetActualType() != columnConfig.UnwrappedType)
                        {
                            continue;
                        }

                        finalValue = columnConfig.DefaultValue;
                    }

                    columnConfig.Property?.SetValue(dataItem,
                        finalValue is string stringValue && columnConfig.TrimValue
                            ? stringValue.Trim()
                            : finalValue);
                }
            }

            yield return dataItem;
            dataIndex++; // 增加数据行计数器
        }
    }

    /// <summary>
    /// 读取表格数据并返回字典序列，键为列名字符串。
    /// </summary>
    /// <param name="tableConfig">表格配置信息。</param>
    /// <param name="progress">可选的进度报告器。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    /// <returns>字典序列，每个字典代表一行数据，键为列名，值为单元格数据。</returns>
    internal IEnumerable<IDictionary<string, object?>> ReadDict(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        Dictionary<int, List<PurColumn>>? indexedColumns = null; // 用于存储列索引和对应的列配置

        int dataIndex = 0; // 数据行计数器
        foreach (IDictionary<int, object?> rawRowData in _reader.ReadTable(tableConfig, progress, cancelToken))
        {
            // 解析字典列配置，并返回是否数据行（true 表示是数据行，应该继续执行循环体来作为数据行处理），
            // 如果返回 false 则表示当前行不是数据行，应该跳过当前行。
            if (!ResolveDictionaryColumns(rawRowData, tableConfig, ref indexedColumns))
            {
                continue;
            }

            // 如果没有任何表头配置则抛出异常
            if (indexedColumns == null || indexedColumns.Keys.Count == 0)
                throw new InvalidOperationException("无法创建数据列定义：未找到任何匹配的列配置");

            if (tableConfig.MaxReadRows >= 0 && dataIndex >= tableConfig.MaxReadRows)
                yield break; // 如果已达到最大读取行数，则停止迭代

            Dictionary<string, object?> rowData = new(rawRowData.Keys.Count);
            foreach (KeyValuePair<int, object?> kvp in rawRowData)
            {
                object? cellValue = kvp.Value;

                if (!indexedColumns.TryGetValue(kvp.Key, out List<PurColumn>? matchedColumns))
                    continue;

                foreach (PurColumn columnConfig in matchedColumns)
                {
                    if (cellValue == null
                        && columnConfig.DefaultValue != null
                        && columnConfig.DefaultValue.GetType().GetActualType() == columnConfig.UnwrappedType)
                    {
                        cellValue = columnConfig.DefaultValue;
                    }

                    rowData[columnConfig.PropertyName] =
                        cellValue is string stringValue && columnConfig.TrimValue ? stringValue.Trim() : cellValue;
                }
            }

            yield return rowData;
        }
    }

    /// <summary>
    /// 读取表格数据并返回动态对象序列，使用 <see cref="ExpandoObject"/> 实现。
    /// </summary>
    /// <param name="tableConfig">表格配置信息。</param>
    /// <param name="progress">可选的进度报告器。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    /// <returns>动态对象序列，每个对象的属性名为列名，属性值为单元格数据。</returns>
    internal IEnumerable<dynamic> ReadDynamic(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        foreach (IDictionary<string, object?> item in ReadDict(tableConfig, progress, cancelToken))
        {
            dynamic rowData = new ExpandoObject();

            IDictionary<string, object?> rowDict = rowData;
            foreach (KeyValuePair<string, object?> kvp in item)
            {
                rowDict[kvp.Key] = kvp.Value;
            }

            yield return rowData;
        }
    }

    /// <summary>
    /// 读取表格数据并返回流变对象序列，使用 <see cref="Clay"/> 实现。
    /// </summary>
    /// <param name="tableConfig">表格配置信息。</param>
    /// <param name="progress">可选的进度报告器。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    /// <returns>流变对象序列，每个对象的属性名为列名，属性值为单元格数据。</returns>
    internal IEnumerable<dynamic> ReadShapeless(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        foreach (IDictionary<string, object?> item in ReadDict(tableConfig, progress, cancelToken))
        {
            yield return Clay.Parse(item);
        }
    }


    #region 通用工具方法

    /// <summary>
    /// 解析泛型列配置
    /// </summary>
    /// <returns>返回是否数据行，如果是 HasHeader==false，则返回 true 表示当前行是数据行，外部应该继续执行循环体。</returns>
    private bool ResolveGenericColumns<T>(IDictionary<int, object?> rawRowData, PurTable tableConfig,
        ref Dictionary<int, List<PurColumn>>? indexedColumns)
    {
        if (indexedColumns != null) return true;

        // 合并特性和动态列配置，动态配置优先，配置会覆盖特性
        List<PurColumn> mergedColumns = MergeColumns<T>(tableConfig.CombinedColumns);

        // 映射表头索引
        List<PurColumn> tempColumns = [];
        for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
        {
            string? cellValue = rawRowData[colIndex]?.ToString();
            string propName = tableConfig.HasHeader && !string.IsNullOrEmpty(cellValue)
                ? cellValue.ProcessWhiteSpace(tableConfig.HeaderSpaceMode)
                : ExcelHelper.ToColumnLetter(colIndex);

            List<PurColumn> matchedColumns = mergedColumns.Count > 0
                ? ColumnUtils.MatchColumns(colIndex, propName, mergedColumns, tableConfig.HeaderSpaceMode)
                : [];

            foreach (PurColumn mtc in matchedColumns)
            {
                if (tempColumns.Any(rc => rc.PropertyName == mtc.PropertyName))
                    continue;

                tempColumns.Add(mtc.WithIndex(colIndex));
            }
        }

        // 检查是否存在未匹配到的必需列
        var missingRequiredColumns = mergedColumns
            .Where(mc => mc is { IsRequired: true, IgnoreInQuery: false })
            .ExceptBy(tempColumns.Select(tc => tc.PropertyName), rc => rc.PropertyName)
            .ToList();
        if (missingRequiredColumns.Any())
        {
            var missingColumnNames = string.Join("、", missingRequiredColumns.Select(rc => rc.PropertyName));
            throw new InvalidDataException(
                $"数据映射失败：配置了 IsRequired=true 的必需列 [{missingColumnNames}] 在表格中找不到对应的列。请检查列名映射配置或表格结构是否正确。");
        }

        // 映射索引字典
        indexedColumns = tempColumns.Where(rsc => rsc.Index >= 0)
            .GroupBy(rsc => rsc.Index)
            .Select(g => new { g.Key, Value = g.ToList() })
            .ToDictionary(g => g.Key, g => g.Value);

        return tableConfig.HasHeader == false;
    }

    /// <summary>
    /// 解析字典列配置
    /// </summary>
    /// <returns>返回是否数据行，如果是 HasHeader==false，则返回 true 表示当前行是数据行，外部应该继续执行循环体。</returns>
    private bool ResolveDictionaryColumns(IDictionary<int, object?> rawRowData, PurTable tableConfig,
        ref Dictionary<int, List<PurColumn>>? indexedColumns)
    {
        if (indexedColumns != null) return true;

        List<PurColumn> tempColumns = []; // 创建空集合
        for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
        {
            string? cellValue = rawRowData[colIndex]?.ToString();
            string propName = tableConfig.HasHeader && !string.IsNullOrEmpty(cellValue)
                ? cellValue.ProcessWhiteSpace(tableConfig.HeaderSpaceMode)
                : ExcelHelper.ToColumnLetter(colIndex);

            List<PurColumn> matchedColumns = tableConfig.CombinedColumns is { Count: > 0 }
                ? ColumnUtils.MatchColumns(colIndex, propName, tableConfig.CombinedColumns, tableConfig.HeaderSpaceMode)
                : [];

            foreach (PurColumn mtc in matchedColumns)
            {
                if (tempColumns.Any(rc => rc.PropertyName == mtc.PropertyName))
                    continue;

                tempColumns.Add(mtc.WithIndex(colIndex));
            }

            // 如果当前列的索引已经存在于已解析的列中，则跳过
            if (matchedColumns.Count > 0) continue;

            if (tempColumns.All(rsc => !rsc.PropertyName.Equals(propName, StringComparison.OrdinalIgnoreCase)))
            {
                tempColumns.Add(PurColumn.FromIndex(colIndex).WithProperty(propName));
            }
        }

        // 检查是否存在未匹配到的必需列
        var missingRequiredColumns = tableConfig.CombinedColumns
            .Where(mc => mc is { IsRequired: true, IgnoreInQuery: false })
            .ExceptBy(tempColumns.Select(tc => tc.PropertyName), rc => rc.PropertyName)
            .ToList();
        if (missingRequiredColumns.Any())
        {
            var missingColumnNames = string.Join("、", missingRequiredColumns.Select(rc => rc.PropertyName));
            throw new InvalidDataException(
                $"列映射失败：配置了 IsRequired=true 的必需列 [{missingColumnNames}] 在表格中找不到对应的列。请检查列名映射配置或表格结构是否正确。");
        }

        // 映射索引字典
        indexedColumns = tempColumns.Where(rsc => rsc.Index >= 0)
            .GroupBy(rsc => rsc.Index)
            .Select(g => new { g.Key, Value = g.ToList() })
            .ToDictionary(g => g.Key, g => g.Value);

        return tableConfig.HasHeader == false;
    }

    private static readonly Dictionary<Type, PropertyInfo[]> TypePropsCache = new();

    /// <summary>
    /// 解析最终列配置：合并特性和动态配置，动态配置会覆盖特性
    /// </summary>
    /// <param name="dynamicColumns">表头列配置的集合</param>
    private List<PurColumn> MergeColumns<T>(IList<PurColumn>? dynamicColumns)
    {
        // 获取或缓存类型的属性信息
        if (!TypePropsCache.TryGetValue(typeof(T), out PropertyInfo[]? typeProperties))
        {
            typeProperties = typeof(T).GetProperties().Where(p => p.CanWrite).ToArray();
            TypePropsCache[typeof(T)] = typeProperties;
        }

        // 创建结果集合，预分配容量
        List<PurColumn> tempColumns = new(typeProperties.Length);

        StringComparer strCpr = StringComparer.OrdinalIgnoreCase;
        // 构建用户定义列的查找字典，提高查找效率
        Dictionary<string, PurColumn> dictColumns =
            dynamicColumns?
                .GroupBy(c => c.PropertyName, strCpr)
                .ToDictionary(g => g.Key, g => g.Last(), strCpr)
            ?? new Dictionary<string, PurColumn>(strCpr);

        // 合并特性和动态配置，动态配置优先，配置会覆盖特性
        foreach (PropertyInfo propertyInfo in typeProperties)
        {
            if (!dictColumns.TryGetValue(propertyInfo.Name, out PurColumn? resolvedColumn))
            {
                // 如果不存在特性，则创建默认配置
                resolvedColumn = propertyInfo.GetCustomAttribute<PurColumn>() ??
                                 PurColumn.From(propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                                                propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ??
                                                propertyInfo.Name);
            }

            if (resolvedColumn.Names.Count == 0)
            {
                resolvedColumn.AddName(propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                                       propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ??
                                       propertyInfo.Name);
            }

            // 设置属性信息和类型信息
            resolvedColumn.WithProperty(propertyInfo);

            // 确保列的Names不为空或空字符串
            if (resolvedColumn.Names.Count(n => !string.IsNullOrWhiteSpace(n)) == 0)
            {
                resolvedColumn.AddName(propertyInfo.Name);
            }

            tempColumns.Add(resolvedColumn);
        }

        return tempColumns;
    }

    /// <summary>
    /// 设置对象的属性值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryConvert(object? cellValue, Type? targetType, PurColumn columnConfig, PurTable tableConfig,
        out object? result)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        result = targetType.CanBeNull() ? null : targetType.GetDefaultValue();

        if (targetType.TryGetValueConverter(out IValueConverter? converter))
        {
            result = converter.Convert(cellValue, targetType, columnConfig, tableConfig.GetCulture());
            return true;
        }

        try
        {
            Type actualType = targetType.GetActualType();
            result = Convert.ChangeType(cellValue, actualType.IsPrimitive ? targetType.GetActualType() : targetType);
            return true;
        }
        catch
        {
            // ignored
        }

        return false;
    }

    #endregion 通用工具方法

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        SafeDispose(_reader);
        SafeDispose(stream, ownsStream);
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeResourcesAsync()
    {
        await SafeDisposeAsync(_reader);
        await SafeDisposeAsync(stream, ownsStream);
    }
}