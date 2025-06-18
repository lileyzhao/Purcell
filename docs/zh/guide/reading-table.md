# 读取表格

## 功能概述

**读取表格**功能支持将 `Excel/CSV` 格式的表格文件解析为 C# 对象，提供以下三种读取方式：

- **强类型读取**：将表格数据读取为指定的 C# 实体类对象，提供类型安全和编译时检查
- **字典读取**：将表格数据读取为 `IDictionary<string, object?>` 类型，灵活处理动态结构
- **动态读取**：将表格数据读取为 `Dynamic` 类型对象，支持动态属性访问

> [!TIP] 支持的文件格式
>
> - **`.xlsx`** - Office Open XML 格式（推荐使用）
> - **`.xlsb`** - Excel 二进制格式（大文件高性能处理）
> - **`.xls`** - 传统 Excel 格式（兼容性支持）
> - **`.csv`** - 逗号分隔值格式（通用文本格式）

## 快速开始

最简单的使用方式：

```csharp
using PurcellLibs;

// 1. 定义实体类
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

// 2. 读取表格
var students = Purcell.Query<Student>("data.xlsx").ToList();

// 3. 使用数据
foreach (var student in students)
{
    Console.WriteLine($"ID: {student.Id}, 姓名: {student.Name}, 年龄: {student.Age}");
}
```

## 读取方式

### 2.1 基于文件路径读取

支持直接传入文件路径进行读取：

::: code-group

```csharp [强类型读取]
using PurcellLibs;

// 方式一：获取完整列表
var students = Purcell.Query<Student>("data.xlsx").ToList();

// 方式二：逐行处理（内存友好）
foreach (var student in Purcell.Query<Student>("data.xlsx"))
{
    Console.WriteLine($"姓名: {student.Name}, 年龄: {student.Age}");
}

// 指定格式的读取方法
var xlsxData = Purcell.QueryXlsx<Student>("data.xlsx");
var xlsbData = Purcell.QueryXlsb<Student>("data.xlsb");
var xlsData = Purcell.QueryXls<Student>("data.xls");
var csvData = Purcell.QueryCsv<Student>("data.csv");
```

```csharp [字典读取]
using PurcellLibs;

// 返回字典集合，列名作为键
foreach (var row in Purcell.Query("data.xlsx"))
{
    Console.WriteLine($"姓名: {row["Name"]}, 年龄: {row["Age"]}");
}
```

```csharp [动态读取]
using PurcellLibs;

// 返回动态对象，支持点号访问
foreach (dynamic row in Purcell.QueryDynamic("data.xlsx"))
{
    Console.WriteLine($"姓名: {row.Name}, 年龄: {row.Age}");
}
```

```csharp [Student.cs]
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```

```text [data.xlsx 示例数据]
| Id  | Name | Age |
| --- | ---- | --- |
| 1   | 张三 | 14  |
| 2   | 李四 | 15  |
| 3   | 王五 | 16  |
```

:::

### 2.2 基于 Stream 读取

适用于从内存、网络或其他来源读取数据流：

```csharp
using var fileStream = File.OpenRead("data.xlsx");

// 通用方法（需要指定格式）
var students = Purcell.Query<Student>(fileStream, QueryType.Xlsx);

// 格式特定方法（推荐）
var xlsxStudents = Purcell.QueryXlsx<Student>(fileStream);
```

## 列配置详解

### 3.1 特性配置（推荐方式）

使用特性是最直观和易维护的配置方式：

::: code-group

```csharp [完整示例]
using PurcellLibs;

public class Student
{
    // 基本列名映射
    [PurColumn("编号")]
    public int Id { get; set; }

    [PurColumn("姓名")]
    public string Name { get; set; }

    // 数据清理：自动去除空白字符
    [PurColumn("年龄", TrimValue = true)]
    public int Age { get; set; }

    // 多列名支持：支持不同的列名变体
    [PurColumn("得分", "分数", "成绩")]
    public decimal Score { get; set; }

    // 索引定位：使用列索引而非列名（从0开始）
    [PurColumn(4)] // 对应第5列（E列）
    public string StudentId { get; set; }

    // 正则匹配：灵活的列名匹配
    [PurColumn("学习阶段|年级|班级", MatchStrategy = MatchStrategy.IgnoreCaseRegex)]
    public string Grade { get; set; }

    // 条件忽略：跳过某些属性的读取
    [PurColumn("生日", IgnoreInQuery = true)]
    public DateTime? Birthday { get; set; }

    // 只读属性会自动忽略，无需标记
    public string FullInfo => $"{Name}-{Age}岁";
}

// 使用配置读取
var students = Purcell.Query<Student>("data.xlsx").ToList();
```

```text [data.xlsx 示例数据]
| 编号 | 姓名 | 年龄 | 得分 | 学号 | 年级 | 生日       |
| ---- | ---- | ---- | ---- | ---- | ---- | ---------- |
| 1    | 张三 | 14   | 81.5 | 2501 | 高二 | 1998-07-01 |
| 2    | 李四 | 15   | 92.5 | 2502 | 高三 | 1997-03-02 |
```

:::

### 3.2 动态配置

适用于运行时动态生成配置的场景：

```csharp
// 构建列配置
var columns = new List<PurColumn>
{
    // 链式配置
    new PurColumn("编号").WithProperty("Id"),
    
    // 从属性名创建
    PurColumn.FromProperty("Name").AddName("姓名"),
    
    // 复杂配置
    PurColumn.FromProperty("Age")
        .AddName("年龄")
        .WithTrimValue(true),
    
    // 多列名支持
    PurColumn.FromProperty("Score")
        .WithNames(["得分", "分数", "成绩"]),
    
    // 索引配置
    PurColumn.FromProperty("StudentId").WithIndex(4),
    
    // 正则匹配
    PurColumn.FromProperty("Grade")
        .WithNames(["学习阶段", "年级", "班级"])
        .WithMatchStrategy(MatchStrategy.IgnoreCaseRegex),
    
    // 忽略属性
    PurColumn.FromProperty("Birthday").WithIgnoreInQuery(true)
};

// 使用动态配置读取
var students = Purcell.Query<Student>("data.xlsx", PurTable.FromColumns(columns));
```

## 表格配置详解

`PurTable` 提供了丰富的表格读取配置选项：

### 4.1 基本配置

```csharp
// 创建表格配置的多种方式
var config = new PurTable("Sheet1");           // 按工作表名称
var config = new PurTable(0);                 // 按工作表索引
var config = PurTable.FromName("数据表");       // 静态方法创建
var config = PurTable.FromIndex(1);           // 静态方法创建
```

### 4.2 完整配置示例

```csharp
// 方式一：构造器配置
var tableConfig = new PurTable
{
    SheetName = "学生数据",           // 工作表名称（优先级高于索引）
    SheetIndex = 0,                 // 工作表索引（从0开始）
    HeaderStart = "B2",             // 表头起始位置
    DataStart = "B3",               // 数据起始位置
    HasHeader = true,               // 是否包含表头
    MaxReadRows = 1000,             // 最大读取行数
    IgnoreParseError = true,        // 忽略解析错误
    HeaderSpaceMode = WhiteSpaceMode.Trim, // 表头空白字符处理
    SkipEmptyRows = true,           // 跳过空行
    Culture = "zh-CN",              // 区域设置
    Encoding = "utf-8",             // 文件编码（CSV）
    CsvDelimiter = ",",             // CSV分隔符
    CsvEscape = '"'                 // CSV文本限定符
};

// 方式二：链式配置（推荐）
var tableConfig = PurTable.FromName("学生数据")
    .WithIndex(0)
    .WithHeaderStart("B2")
    .WithDataStart("B3")
    .WithHasHeader(true)
    .WithMaxReadRows(1000)
    .WithIgnoreParseError(true)
    .WithHeaderSpaceMode(WhiteSpaceMode.Trim)
    .WithSkipEmptyRows(true)
    .WithCulture("zh-CN")
    .WithEncoding("utf-8")
    .WithCsvDelimiter(",")
    .WithCsvEscape('"');

// 使用配置读取
var students = Purcell.Query<Student>("data.xlsx", tableConfig);
```

## 高级用法

### 5.1 处理复杂表格结构

```csharp
// 处理表头不在第一行的表格
var config = PurTable.New()
    .WithHeaderStart("A5")    // 表头从第5行开始
    .WithDataStart("A6");     // 数据从第6行开始

// 处理无表头的表格
var config = PurTable.New()
    .WithHasHeader(false)     // 无表头
    .WithDataStart("A1")      // 数据从第1行开始
    .WithColumns(new[]        // 必须使用索引配置
    {
        PurColumn.FromProperty("Id").WithIndex(0),
        PurColumn.FromProperty("Name").WithIndex(1),
        PurColumn.FromProperty("Age").WithIndex(2)
    });
```

### 5.2 错误处理

```csharp
// 方式一：忽略解析错误
var config = PurTable.New().WithIgnoreParseError(true);
var students = Purcell.Query<Student>("data.xlsx", config);

// 方式二：手动处理错误
try
{
    var students = Purcell.Query<Student>("data.xlsx");
}
catch (PurcellException ex)
{
    Console.WriteLine($"读取失败: {ex.Message}");
}
```

### 5.3 性能优化

```csharp
// 大文件处理：逐行读取而非全部加载
foreach (var student in Purcell.Query<Student>("large-data.xlsx"))
{
    // 逐行处理，内存占用低
    ProcessStudent(student);
}

// 限制读取行数
var config = PurTable.New().WithMaxReadRows(1000);
var students = Purcell.Query<Student>("data.xlsx", config);

// 使用二进制格式提升性能
var students = Purcell.QueryXlsb<Student>("data.xlsb");
```

## 常见问题

### Q: 如何处理中文列名？
A: 直接在 `PurColumn` 特性中使用中文即可，如 `[PurColumn("姓名")]`。

### Q: 如果表格列名不固定怎么办？
A: 使用正则表达式匹配或多列名支持：
```csharp
[PurColumn("姓名|名字|学生姓名", MatchStrategy = MatchStrategy.IgnoreCaseRegex)]
```

### Q: 如何跳过表格中的某些行？
A: 使用 `SkipEmptyRows = true` 跳过空行，或在业务逻辑中过滤。

### Q: 日期格式解析失败怎么办？
A: 设置正确的区域文化：
```csharp
var config = PurTable.New().WithCulture("zh-CN");
```

## API 参考

### PurColumn 属性

> 仅列出读取表格时可用属性，对于导出表格时可用的属性，请查看导出表格文档。

| 属性              | 类型            | 默认值                     | 说明                                                   |
| ----------------- | --------------- | -------------------------- | ------------------------------------------------------ |
| **PropertyName**  | `string`        | `string.Empty`             | 实体类属性名称（特性方式自动获取）                     |
| **Names**         | `List<string>`  | `[]`                       | 表格列名集合，支持多列名匹配                           |
| **Index**         | `int?`          | `null`                     | 列索引（0-16383），对应 A-ZYX 列，优先级高于列名      |
| **TrimValue**     | `bool`          | `false`                    | 是否自动去除字符串首尾空白字符                         |
| **MatchStrategy** | `MatchStrategy` | `MatchStrategy.IgnoreCase` | 列名匹配策略：忽略大小写、包含、正则表达式等           |
| **IgnoreInQuery** | `bool`          | `false`                    | 是否在查询时忽略此属性（只读属性自动忽略）             |

### PurTable 属性

> 仅列出读取表格时可用属性，对于导出表格时可用的属性，请查看导出表格文档。

| 属性                 | 类型                   | 默认值                | 说明                                                 |
| -------------------- | ---------------------- | --------------------- | ---------------------------------------------------- |
| **SheetName**        | `string`               | `null`                | 工作表名称，优先级高于索引                           |
| **SheetIndex**       | `int`                  | `0`                   | 工作表索引，从0开始                                  |
| **HeaderStart**      | `string`/`CellLocator` | `"A1"`                | 表头起始位置，如 "A1"、"B2"                          |
| **DataStart**        | `string`/`CellLocator` | `"A2"`                | 数据起始位置，如 "A2"、"B3"                          |
| **HasHeader**        | `bool`                 | `true`                | 是否包含表头，无表头时只能使用索引匹配               |
| **MaxReadRows**      | `int`                  | `-1`                  | 最大读取行数，-1 表示不限制                          |
| **IgnoreParseError** | `bool`                 | `false`               | 是否忽略解析错误，避免程序中断                       |
| **HeaderSpaceMode**  | `WhiteSpaceMode`       | `WhiteSpaceMode.Trim` | 表头空白字符处理模式                                 |
| **SkipEmptyRows**    | `bool`                 | `false`               | 是否跳过空行                                         |
| **Culture**          | `string`               | `null`                | 区域文化设置，影响日期时间解析                       |
| **Encoding**         | `string`               | `null`                | 文件编码（仅CSV），null 时自动检测                   |
| **CsvDelimiter**     | `string`               | `","`                 | CSV字段分隔符                                        |
| **CsvEscape**        | `char`                 | `'"'`                 | CSV文本限定符，用于包围包含分隔符的字段内容          |
