namespace PurcellLibs;

/// <summary>
/// Purcell 工厂类
/// </summary>
public static partial class Purcell
{
    /// <summary>
    /// 根据数据流和类型创建相应的表格导出器
    /// </summary>
    /// <param name="stream">数据流</param>
    /// <param name="fileType">导出类型枚举</param>
    /// <returns>对应类型的表格导出器实例</returns>
    /// <exception cref="NotSupportedException">当指定了不支持的导出类型时抛出</exception>
    public static IPurExporter CreateExporter(Stream stream, TableFileType fileType)
    {
        return new PurExporter(stream, fileType);
    }

    /// <summary>
    /// 根据文件路径创建相应的表格导出器
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>对应类型的表格导出器实例</returns>
    /// <exception cref="NotSupportedException">当指定了不支持的文件类型时抛出</exception>
    public static IPurExporter CreateExporter(string filePath)
    {
        return new PurExporter(filePath);
    }

    /// <summary>
    /// 根据数据流和类型创建相应的表格查询器
    /// </summary>
    /// <param name="stream">数据流</param>
    /// <param name="type">导入类型枚举</param>
    /// <returns>对应类型的表格查询器实例</returns>
    /// <exception cref="NotSupportedException">当指定了不支持的文件类型时抛出</exception>
    public static IPurQuerier CreateQuerier(Stream stream, TableFileType type)
    {
        return new PurQuerier(stream, type);
    }

    /// <summary>
    /// 根据文件路径创建相应的表格查询器
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>对应类型的表格查询器实例</returns>
    /// <exception cref="NotSupportedException">当指定了不支持的文件类型时抛出</exception>
    public static IPurQuerier CreateQuerier(string filePath)
    {
        return new PurQuerier(filePath);
    }
}