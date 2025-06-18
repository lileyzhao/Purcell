# Purcell API 参考文档

## 配置参数

### ExcelSetting 基础配置

```cs
public class ExcelSetting
{
    // 工作表索引
    public int SheetIndex { get; set; } = 0;
    
    // 工作表名称
    public string? SheetName { get; set; }
    
    // 表头行索引,-1表示无表头
    public int HeaderRow { get; set; } = 0;
    
    // 数据开始行索引
    public int DataStartRow { get; set; } = 1;
    
    // 文件类型
    public ExcelFileType ExcelFileType { get; set; }
    
    // 编码格式
    public Encoding? Encoding { get; set; }
    
    // 是否忽略数据错误
    public bool IgnoreDataError { get; set; }
}
```

### ExcelColumn 列配置

```cs
public class ExcelColumn 
{
    // 属性名称
    public string? PropertyName { get; set; }
    
    // 列名称
    public string? ColumnName { get; set; }
    
    // 列索引
    public int ColumnIndex { get; set; } = -1;
    
    // 列宽度
    public double ColumnWidth { get; set; }
    
    // 是否忽略导入
    public bool IgnoreImport { get; set; }
    
    // 是否忽略导出
    public bool IgnoreExport { get; set; }
}
```

### SheetStyle 样式配置

```cs
public class SheetStyle
{
    // 表头样式
    public XlsxStyle HeaderStyle { get; private set; }
    
    // 内容样式 
    public XlsxStyle ContentStyle { get; private set; }
    
    // 表底样式
    public XlsxStyle FooterStyle { get; private set; }
    
    // 表头行高
    public double HeaderLineHeight { get; private set; }
    
    // 内容行高
    public double ContentLineHeight { get; private set; }
    
    // 最小列宽
    public double MinColumnWidth { get; private set; }
    
    // 最大列宽
    public double MaxColumnWidth { get; private set; }
}
```

## 主要API

### 文件操作

```cs
// 创建实例
Purcell.File(string filePath)                      // 使用文件路径
Purcell.File(Stream fileStream)                    // 使用文件流
Purcell.File(Stream fileStream, ExcelFileType)     // 指定文件类型

// 导出方法
void Export()                                      // 执行导出
```

### 工作表操作

```cs
// 设置工作表
SetSheet(int sheetIndex)                    // 设置工作表索引
SetSheet(string sheetName)                  // 设置工作表名称
SetSheet(SheetIndex sheetIndex)             // 使用枚举设置索引

// 查询工作表
List<string> GetSheetNames()                // 获取所有工作表名称
Task<List<string>> GetSheetNamesAsync()     // 异步获取工作表名称
```

### 数据操作

```cs
// 导出数据
SetSheetData<T>(IEnumerable<T> data)                    // 设置单个工作表数据
AddSheetData<T>(IEnumerable<T> data)                    // 添加工作表数据
SetSheetDatas(params SheetData[] sheetDatas)           // 设置多个工作表
AddSheetDatas(params SheetData[] sheetDatas)           // 添加多个工作表

// 查询数据 - 强类型映射
IEnumerable<T> Query<T>()                              // 将表格数据映射为指定实体类型
IEnumerable<IDictionary<string,object?>> Query()       // 将表格数据映射为字典类型
IAsyncEnumerable<T> QueryAsync<T>()                    // 异步强类型映射
IAsyncEnumerable<IDictionary<string,object?>> QueryAsync() // 异步字典映射
```

### 列配置

```cs
// 设置列配置
SetExcelColumns(List<ExcelColumn> columns)     // 设置列配置
AddExcelColumn(ExcelColumn column)             // 添加单个列配置
ClearExcelColumn()                             // 清空列配置
```

### 样式操作

```cs
// 样式设置
SetSheetStyle(SheetStyle style)                // 设置工作表样式
SetHeaderStyle(Color textColor, Color fillColor)  // 设置表头样式
SetContentStyle(Color textColor, Color fillColor) // 设置内容样式
SetFooterStyle(Color textColor, Color fillColor)  // 设置表底样式

// 行高列宽
SetHeaderLineHeight(double height)             // 设置表头行高
SetContentLineHeight(double height)            // 设置内容行高
SetFooterLineHeight(double height)             // 设置表底行高
SetMinColumnWidth(double width)                // 设置最小列宽
SetMaxColumnWidth(double width)                // 设置最大列宽
```

### 其他配置

```cs
// 设置基础配置
SetEncoding(Encoding encoding)                 // 设置编码格式
```
