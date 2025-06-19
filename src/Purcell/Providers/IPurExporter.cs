#pragma warning disable CS1572 // XML 注释中有 param 标记，但是没有该名称的参数

namespace PurcellLibs.Providers;

/// <summary>
/// 表格导出器接口 - 为导出方法提供统一的注释。
/// </summary>
public interface IPurExporter : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 将数据导出到指定的目标中。
    /// </summary>
    /// <param name="tableConfigs">要导出的工作表数据集合，每个元素对应一个工作表的配置和数据。</param>
    /// <param name="tableConfig">要导出的单个工作表数据配置。</param>
    /// <param name="records">要导出的数据记录集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="stream">目标数据流，用于接收导出的表格数据。流必须支持写入操作。</param>
    /// <param name="filePath">目标文件路径，导出器将根据文件扩展名自动识别导出格式（.csv/.xlsx）。</param>
    /// <param name="exportType">导出类型枚举，指定导出格式（Csv 或 Xlsx）。当使用流导出时必须指定此参数。</param>
    /// <param name="sheetName">导出的工作表名称。默认为空字符串，系统将自动生成名称。</param>
    /// <param name="hasHeader">导出时是否包含表头行，默认为 true。为 true 时会输出表头行，为 false 时仅输出数据行。</param>
    /// <param name="headerStart">导出时表头起始单元格位置，默认为 A1，仅在 hasHeader 为 true 时有效。表示表头行在导出文件中的起始位置，格式如 "A1"、"B2" 等。</param>
    /// <param name="dataStart">导出时数据起始单元格位置，默认为 HeaderStart 下方单元格。指定数据行在导出文件中的起始位置，必须与表头起始列对齐。</param>
    /// <param name="columns">导出时的列配置集合，用于控制每列的导出行为、格式化和样式设置。为 null 时将根据数据源自动生成列配置。</param>
    /// <param name="maxWriteRows">导出时的最大行数限制，默认为 -1（不限制）。当数据量超过此值时，将只导出前 N 行数据。</param>
    /// <param name="fileEncoding">导出 CSV 文件时使用的字符编码，默认为 null（使用 UTF-8）。仅对 CSV 格式有效。支持的编码如："UTF-8"、"GBK"、"GB2312" 等。</param>
    /// <param name="csvDelimiter">导出 CSV 文件时的字段分隔符，默认为逗号 ","。用于分隔 CSV 文件中的字段。常用值还包括分号 ";" 和制表符 "\t"。</param>
    /// <param name="csvEscape">导出 CSV 文件时的文本限定符，默认为双引号 '"'。用于包围包含特殊字符（如分隔符、换行符）的字段内容。</param>
    /// <param name="sampleRows">导出时用于自动列宽计算的样本行数，默认值为 5。系统分析前 N 行数据来优化列宽显示效果。设置为 0 则不进行自动列宽计算，仅使用列名长度作为初始宽度。</param>
    /// <param name="autoFilter">导出 Excel 文件时是否启用表头自动筛选功能，默认值为 true。仅对 Excel 格式有效，启用后用户可以在 Excel 中使用下拉筛选。</param>
    /// <param name="password">导出 Excel 文件时的工作表保护密码。仅对 Excel 格式有效，设置后工作表将被密码保护。默认为空字符串（不保护）。</param>
    /// <param name="tableStyle">导出 Excel 文件时的工作表样式，默认为 PurStyle.Default。用于设置字体、颜色、边框等样式。为 null 时使用默认样式。</param>
    /// <param name="presetStyle">导出 Excel 文件时的预设样式，默认为 Default（未设置）。提供多种内置的样式主题，运行时会自动映射到对应的 PurStyle 实例。</param>
    /// <param name="progress">进度报告回调，用于监控导出进度。回调参数包含当前处理的工作表索引和行索引。</param>
    /// <param name="cancelToken">取消令牌，用于支持异步操作的取消。在长时间运行的导出任务中可以通过此令牌取消操作。</param>
    /// <remarks>
    /// <para>
    /// 此方法支持多种数据源类型的导出，包括：
    /// </para>
    /// <list type="bullet">
    /// <item><description><c>IEnumerable&lt;T&gt;</c> - 泛型对象集合</description></item>
    /// <item><description><c>DataTable</c> - 数据表</description></item>
    /// <item><description><c>IDictionary&lt;string, object&gt;</c> - 字典集合</description></item>
    /// <item><description>匿名对象集合</description></item>
    /// </list>
    /// <para>
    /// 支持的导出格式：
    /// </para>
    /// <list type="bullet">
    /// <item><description><c>CSV</c> - 逗号分隔值文件，兼容性好，文件体积小</description></item>
    /// <item><description><c>XLSX</c> - Excel 文件，支持多工作表、样式、公式等高级功能</description></item>
    /// </list>
    /// <para>
    /// 进度报告说明：进度回调大约每处理 50 行数据触发一次，可用于更新 UI 进度条或记录处理状态。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">当 <paramref name="tableConfigs"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当 <paramref name="tableConfigs"/> 为空集合时抛出。</exception>
    /// <exception cref="NotSupportedException">当指定了不支持的导出类型时抛出。</exception>
    /// <exception cref="UnauthorizedAccessException">当文件路径无访问权限时抛出。</exception>
    /// <exception cref="DirectoryNotFoundException">当文件路径的目录不存在时抛出。</exception>
    /// <exception cref="IOException">当发生 I/O 错误时抛出。</exception>
    /// <exception cref="OperationCanceledException">当操作被取消时抛出。</exception>
    void Export(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);
}