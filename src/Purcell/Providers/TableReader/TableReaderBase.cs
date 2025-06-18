// ReSharper disable InconsistentNaming

using System.Dynamic;

namespace PurcellLibs.Providers.TableReader;

/// <summary>
/// 表格数据读取器抽象类
/// </summary>
internal abstract class TableReaderBase : ITableReader
{
    private int _disposed;
    protected readonly Stream _stream;
    private readonly bool _ownsStream;
    protected PurTable _tableConfig = PurTable.Default;

    /// <inheritdoc/>
    public IList<string> Worksheets { get; set; }

    /// <summary>
    /// 初始化表格数据读取器
    /// </summary>
    protected TableReaderBase(Stream stream, bool ownsStream = false)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _ownsStream = ownsStream;
        Worksheets = [];
    }

    /// <summary>
    /// 核心读取方法，子类需要实现此方法以读取表格数据
    /// </summary>
    protected abstract IEnumerable<IDictionary<int, object?>> ReadCore(PurTable tblConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);

    /// <inheritdoc/>
    public IEnumerable<T> Read<T>(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        Dictionary<int, List<PurColumn>>? indexedColumns = null; // 用于存储列索引和对应的列配置

        int dataIndex = 0; // 数据行计数器
        foreach (IDictionary<int, object?> rawRowData in ReadCore(tableConfig, progress, cancelToken))
        {
            // 解析列配置，并返回是否数据行（true 表示是数据行，应该继续执行循环体来作为数据行处理），
            // 如果返回 false 则表示当前行不是数据行，应该跳过当前行。
            if (!ResolveGenericColumns<T>(rawRowData, ref indexedColumns))
                continue;

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
                    if (!TryConvert(cellValue, columnConfig.PropertyType, columnConfig, out object? finalValue)
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

    /// <inheritdoc/>
    public IEnumerable<IDictionary<string, object?>> ReadDict(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        Dictionary<int, List<PurColumn>>? indexedColumns = null; // 用于存储列索引和对应的列配置

        foreach (IDictionary<int, object?> rawRowData in ReadCore(tableConfig, progress, cancelToken))
        {
            // 解析字典列配置，并返回是否数据行（true 表示是数据行，应该继续执行循环体来作为数据行处理），
            // 如果返回 false 则表示当前行不是数据行，应该跳过当前行。
            if (!ResolveDictionaryColumns(rawRowData, ref indexedColumns))
                continue;

            // 如果没有任何表头配置则抛出异常
            if (indexedColumns == null || indexedColumns.Keys.Count == 0)
                throw new InvalidOperationException("无法创建数据列定义：未找到任何匹配的列配置");

            Dictionary<string, object?> rowData = new();

            // 开始读取行数据
            for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
            {
                object? cellValue = rawRowData[colIndex];

                if (!indexedColumns.TryGetValue(colIndex, out List<PurColumn>? matchedColumns))
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

    /// <inheritdoc/>
    public IEnumerable<dynamic> ReadDynamic(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return ReadDict(tableConfig, progress, cancelToken).Select(item =>
        {
            ExpandoObject expando = new();
            IDictionary<string, object?> expandoDict = expando;
            foreach (KeyValuePair<string, object?> pair in item)
            {
                expandoDict[pair.Key] = pair.Value;
            }

            return expando;
        });
    }

    #region 通用工具方法

    /// <summary>
    /// 解析泛型列配置
    /// </summary>
    /// <returns>返回是否数据行，如果是 HasHeader==false，则返回 true 表示当前行是数据行，外部应该继续执行循环体。</returns>
    private bool ResolveGenericColumns<T>(
        IDictionary<int, object?> rawRowData,
        ref Dictionary<int, List<PurColumn>>? indexedColumns)
    {
        if (indexedColumns != null) return true;

        // 合并特性和动态列配置，动态配置优先，配置会覆盖特性
        List<PurColumn> mergedColumns = MergeColumns<T>(_tableConfig.CombinedColumns);

        // 映射表头索引
        List<PurColumn> tempColumns = [];
        for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
        {
            string? cellValue = rawRowData[colIndex]?.ToString();
            string propName = _tableConfig.HasHeader && !string.IsNullOrEmpty(cellValue)
                ? cellValue.ProcessWhiteSpace(_tableConfig.HeaderSpaceMode)
                : CellLocator.GetColumnLetter(colIndex);

            List<PurColumn> matchedColumns = mergedColumns.Count > 0
                ? ColumnUtils.MatchColumns(colIndex, propName, mergedColumns, _tableConfig.HeaderSpaceMode)
                : [];

            foreach (PurColumn mtc in matchedColumns)
            {
                if (tempColumns.Any(rc => rc.PropertyName == mtc.PropertyName))
                    continue;

                tempColumns.Add(mtc.WithIndex(colIndex));
            }
        }

        // 映射索引字典
        indexedColumns = tempColumns.Where(rsc => rsc.Index >= 0)
            .GroupBy(rsc => rsc.Index)
            .Select(g => new { g.Key, Value = g.ToList() })
            .ToDictionary(g => g.Key, g => g.Value);

        return _tableConfig.HasHeader == false;
    }

    /// <summary>
    /// 解析字典列配置
    /// </summary>
    /// <returns>返回是否数据行，如果是 HasHeader==false，则返回 true 表示当前行是数据行，外部应该继续执行循环体。</returns>
    private bool ResolveDictionaryColumns(
        IDictionary<int, object?> rawRowData,
        ref Dictionary<int, List<PurColumn>>? indexedColumns)
    {
        if (indexedColumns != null) return true;

        List<PurColumn> tempColumns = []; // 创建空集合
        for (int colIndex = 0; colIndex < rawRowData.Keys.Count; colIndex++)
        {
            string? cellValue = rawRowData[colIndex]?.ToString();
            string propName = _tableConfig.HasHeader && !string.IsNullOrEmpty(cellValue)
                ? cellValue.ProcessWhiteSpace(_tableConfig.HeaderSpaceMode)
                : CellLocator.GetColumnLetter(colIndex);

            List<PurColumn> matchedColumns = _tableConfig.CombinedColumns is { Count: > 0 }
                ? ColumnUtils.MatchColumns(colIndex, propName, _tableConfig.CombinedColumns, _tableConfig.HeaderSpaceMode)
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

        // 映射索引字典
        indexedColumns = tempColumns.Where(rsc => rsc.Index >= 0)
            .GroupBy(rsc => rsc.Index)
            .Select(g => new { g.Key, Value = g.ToList() })
            .ToDictionary(g => g.Key, g => g.Value);

        return _tableConfig.HasHeader == false;
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
                                 PurColumn.From([
                                     propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                                     propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ??
                                     propertyInfo.Name
                                 ]);
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
    private bool TryConvert(object? cellValue, Type? targetType, PurColumn columnConfig, out object? result)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        result = targetType.CanBeNull() ? null : targetType.GetDefaultValue();

        if (targetType.TryGetValueConverter(out IValueConverter? converter))
        {
            result = converter.Convert(cellValue, targetType, _tableConfig.GetCulture(), columnConfig.Format);
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

    #region Disposable Support

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 异步释放核心逻辑
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_ownsStream) await _stream.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// 同步释放核心逻辑
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        if (_ownsStream) _stream.Dispose();
    }

    // 终结器
    ~TableReaderBase()
    {
        Dispose(false);
    }

    #endregion Disposable Support
}