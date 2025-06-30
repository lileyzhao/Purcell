# CLAUDE.md

这个文件为 Claude Code ( claude.ai/code ) 在处理此代码库中的代码时提供指导。

## 项目概述

Purcell 是一个面向 .NET 8.0 的高性能 Excel/CSV 库，提供强类型的表格数据读写功能。支持 `.xls`、`.xlsx` 和 `.csv` 格式，专注于性能、易用性和全面的编码支持。

## 开发命令

### 构建
```bash
dotnet build src/Purcell.sln
```

### 测试
```bash
# 运行所有测试
dotnet test src/Purcell.UnitTest/

# 运行特定测试类
dotnet test src/Purcell.UnitTest/ --filter "ClassName~Tests_Query_Generic"

# 运行测试并生成覆盖率报告
dotnet test src/Purcell.UnitTest/ --collect:"XPlat Code Coverage"
```

### 性能基准测试
```bash
dotnet run --project src/Purcell.Benchmarks/ -c Release
```

### 控制台示例应用
```bash
dotnet run --project src/Purcell.ConsoleApp/
```

## 核心架构

### 双重架构模式
项目采用**双重架构**设计，包含传统实现和新的基于提供程序的实现：

1. **传统架构**（在 `Purcell/` 中）：使用 TableReader/TableWriter 类的直接实现
2. **新提供程序架构**（在 `Purcell.Providers/` 中）：为未来扩展性提供的清洁抽象层

### 核心组件

**主要入口点：**
- `Purcell` 静态类：提供 `Query<T>()`、`Export<T>()` 及其异步变体的主要 API
- 工厂方法：`CreateQuerier()`、`CreateExporter()` 用于创建提供程序实例

**配置系统：**
- `PurTable`：表格操作的流畅配置（表头、范围、样式、密码）
- `PurColumn`：列级配置，包含映射、格式化和验证
- 基于特性的配置：`[PurTable]`、`[PurColumn]` 特性用于元数据

**提供程序抽象：**
- `IPurQuerier`/`PurQuerier`：数据读取操作，支持泛型、字典和动态类型
- `IPurExporter`/`PurExporter`：数据写入操作，支持多工作表
- 文件格式处理器：Excel (Sylvan)、CSV (CsvHelper)、大型 XLSX (LargeXlsx)

**类型系统：**
- 全面的转换器：DateTime、Enum、Numeric、Boolean、Guid、IPAddress 等
- 编码支持：UTF-8、GBK、GB2312、UTF-16BE/LE，支持 BOM 处理
- 多种数据源：对象、DataTable、字典、匿名类型

### 设计模式

- **工厂模式**：基于文件类型检测的查询器/导出器集中创建
- **提供程序模式**：API 接口与实现细节的清洁分离
- **流畅 API**：配置的方法链调用（`PurTable.From().WithColumns().WithStyle()`）
- **策略模式**：运行时选择不同的文件格式处理器
- **模板方法**：通用表格读写操作的基类

## 项目结构

```
Purcell/                    # 主库（传统 + 提供程序集成）
├── Attributes/             # PurTable、PurColumn 特性定义
├── Common/                 # 枚举类型（ExportType、QueryType、HAlign 等）
├── Converters/            # 各种 .NET 类型的转换器
├── Extensions/            # 扩展方法和 API 扩展
├── Providers/             # 传统提供程序（正在迁移中）
│   ├── TableReader/       # 文件格式读取器
│   └── TableWriter/       # 文件格式写入器
└── Utilities/             # 辅助类（ColumnUtils、FileUtils 等）

Purcell.Providers/         # 新抽象层
├── Abstractions/          # 基类和接口
├── Csv/                   # CSV 特定实现
├── Excel/                 # Excel 特定实现
└── Extensions/            # 提供程序特定扩展

Purcell.UnitTest/          # 综合测试套件
├── DTOs/                  # 测试数据模型
├── Resources/             # 多种格式/编码的示例文件
└── Tests_*.cs             # 按操作和数据类型分类的测试文件
```

## 主要 API

### 查询操作
```csharp
// 泛型类型查询
var people = Purcell.Query<Person>("data.xlsx").ToList();

// 基于字典的查询
var data = Purcell.Query("data.csv").ToList();  

// 动态查询
var dynamic = Purcell.QueryDynamic("data.xlsx").ToList();

// 带配置的查询
var config = PurTable.From("Sheet1").WithHeaderStart("B2");
var results = Purcell.Query<Employee>("file.xlsx", config);

// 基于流的操作
using var stream = File.OpenRead("data.xlsx");
var data = Purcell.Query<Person>(stream, TableFileType.Excel);
```

### 导出操作
```csharp
// 简单导出
Purcell.Export(people, "output.xlsx");

// 多工作表导出
var tables = new List<PurTable> {
    PurTable.From(employees, "Employees"),
    PurTable.From(departments, "Departments") 
};
Purcell.Export(tables, "multi-sheet.xlsx");

// 高级配置
var table = PurTable.From(data, "Results")
    .WithHeaderStart("A1")
    .WithAutoFilter(true)
    .WithPassword("secret")
    .WithPresetStyle(PresetStyle.TableStyleMedium2);
Purcell.Export(table, "styled.xlsx");
```

## 测试策略

使用 **xUnit** 进行全面覆盖：
- **单元测试**：所有主要组件，使用 FluentAssertions
- **集成测试**：真实的 Excel/CSV 文件，包含各种编码
- **性能测试**：单独项目中的 BenchmarkDotNet
- **示例数据**：`Resources/` 目录中的大量测试文件

测试文件命名约定：`Tests_{操作}_{数据类型}.cs`（例如：`Tests_Query_Generic.cs`）

## 文件格式支持

**Excel 格式：**
- `.xls`、`.xlsx` 通过 **Sylvan.Data.Excel**（读取）
- `.xlsx` 通过 **LargeXlsx**（高效写入大文件）

**CSV 支持：**
- 多种编码：UTF-8、GBK、GB2312、UTF-16BE/LE
- 可配置的分隔符、转义字符
- BOM 处理以正确检测编码

**性能特性：**
- 大文件流式处理以最小化内存使用
- 全面的异步/等待模式
- 长时间运行操作的进度报告

## 主要依赖项

- **Sylvan.Data.Excel** (0.4.29)：快速 Excel 文件读取
- **CsvHelper** (33.1.0)：健壮的 CSV 操作
- **LargeXlsx** (1.12.0)：内存高效的 XLSX 写入
- **System.Text.Encoding.CodePages** (9.0.7)：扩展编码支持
- **xUnit** (2.9.3) + **FluentAssertions** (8.5.0)：测试框架
- **BenchmarkDotNet** (0.15.2)：性能基准测试

## 代码注释规范

项目采用标准的 .NET XML 文档注释，**使用中文说明，保留英文技术术语**：

### 基本格式
```csharp
/// <summary>
/// 如果 <paramref name="value"/> 为 <see langword="null"/>，则返回 <paramref name="defaultValue"/>；否则返回 <paramref name="value"/>。
/// </summary>
/// <typeparam name="T">引用类型参数的类型。</typeparam>
/// <param name="value">要检查的可空引用值。</param>
/// <param name="defaultValue">当 <paramref name="value"/> 为 <see langword="null"/> 时返回的默认值。</param>
/// <returns>如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回 <paramref name="defaultValue"/>。</returns>
/// <exception cref="ArgumentNullException"><paramref name="value"/> 为 <see langword="null"/>。</exception>
public static T DefaultIfNull<T>(this T? value, T defaultValue) where T : class
```

### 注释要素
- **`<summary>`**：简洁描述方法功能
- **`<param>`**：描述每个参数的作用
- **`<returns>`**：说明返回值内容
- **`<typeparam>`**：泛型参数说明
- **`<exception>`**：可能抛出的异常

### 引用规范
- 参数引用：`<paramref name="paramName"/>`
- 关键字引用：`<see langword="null"/>`
- 类型引用：`<see cref="Exception"/>`

### 其他代码元素
- **类/接口注释**：使用相同的 `<summary>` 格式，描述类的用途和职责
- **属性注释**：说明属性的含义和用途，必要时包含 `<value>` 标签
- **字段注释规则**：
  - **private 普通字段**：不需要注释
  - **private static 字段**：需要注释（包括 static readonly）
  - **private const 字段**：需要注释
  - **private readonly 字段**：需要注释
  - **public/protected/internal 字段**：需要注释
- **事件注释**：说明事件触发条件和参数含义

### 注释原则
- **中英混合**：说明文字用中文，技术术语、类名、方法名、关键字保持英文
- **简洁明了**：避免冗余描述，重点说明参数用途和返回值
- **完整性**：公共 API 必须有完整注释，包括异常情况
- **一致性**：保持项目内注释风格统一，类、接口、属性等注释规范与方法注释雷同，自行扩展