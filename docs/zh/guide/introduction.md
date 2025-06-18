# Purcell 是什么？ {#what-is-purcell}

Purcell 是一个专为 Excel/CSV 文件设计的对象关系映射（O/RM）库，提供强类型的表格数据与 C# 对象之间的双向映射能力。

使用 [LargeXlsx](https://github.com/salvois/LargeXlsx) 、 [Sylvan.Data.Excel](https://github.com/MarkPflug/Sylvan.Data.Excel) 和 [CsvHelper](https://github.com/JoshClose/CsvHelper) 作为读写 Excel/CSV 文件的底层库，实现对 XLS、XLSX、XLSB 和 CSV 格式的高效读写。

> 支持读取的格式：`.csv` 、`.xls` 、`.xlsx` 、`.xlsb`
>
> 支持导出的格式：`.csv` 、`.xlsx`

Purcell 采用 [Fluent API](https://martinfowler.com/bliki/FluentInterface.html) 风格设计，简化操作流程，使开发者能够轻松地将 Excel/CSV 数据映射到强类型对象模型，或从任意可枚举数据源导出为 Excel/CSV 文件。

<div class="tip custom-block" style="padding-top: 8px">

想尝试一下？跳到[快速开始](./getting-started)。

</div>

## 特点 {#features}

- **强类型 O/RM**：提供表格数据与 C# 对象之间的类型安全映射

- **逐行访问**：流式处理，内存消耗极低

- **简单易用**：Fluent API 风格，链式调用

- **LINQ 支持**：支持 LINQ 方式查询 Excel/CSV 数据

- **纯托管实现**：底层库纯托管实现，无需安装 Microsoft Office

## 性能 {#performance}

Purcell 使用的 [LargeXlsx](https://github.com/salvois/LargeXlsx) 、 [Sylvan.Data.Excel](https://github.com/MarkPflug/Sylvan.Data.Excel) 和 [CsvHelper](https://github.com/JoshClose/CsvHelper) 库以逐行读写的方式处理 Excel/CSV 文件，确保内存占用极低，避免 OOM、Full GC 情况的出现。

## 依赖库 {#dependencies}

Purcell 的高效性能和广泛格式支持得益于以下第三方库的强大功能：

- **[LargeXlsx](https://github.com/salvois/LargeXlsx)**：提供高效的 XLSX 文件生成和读取能力，支持大文件处理，确保内存使用最小化。

- **[Sylvan.Data.Excel](https://github.com/MarkPflug/Sylvan.Data.Excel)**：专注于 Excel 数据的快速解析，支持多种 Excel 格式，提供高性能的数据读取和写入功能。

- **[CsvHelper](https://github.com/JoshClose/CsvHelper)**：强大的Csv文件读写库，提供简单易用的API和高性能的数据处理能力。

这些库的结合使 Purcell 能够在处理复杂 Excel/CSV 数据时保持卓越的性能和稳定性。
