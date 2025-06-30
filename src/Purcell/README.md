# Purcell

一个超高性能的强类型读写表格文件的ORM库，支持 `.xls`、`.xlsx`、`.csv` 格式，专为 .NET 设计，极致性能与易用性兼备。

## 特性

- 🚀 **极致性能**：大批量数据导入导出毫无压力
- 🏷️ **强类型映射**：支持对象与表格字段自动映射
- 📦 **多格式支持**：兼容 `.xls`、`.xlsx`、`.csv`
- 🧩 **灵活扩展**：支持自定义导入导出、字段映射、样式等
- 🧪 **完善测试**：单元测试与基准测试齐全
- 🛠️ **异步/同步 API**：满足不同场景需求

## 安装

通过 NuGet 安装：

```shell
dotnet add package Purcell
```

Purcell 采用四段式版本号 (W.X.Y.Z)，遵循以下规则：

- W：主版本号，表示重大架构变更或不兼容API调整
- X：次版本号，表示功能新增，向下兼容
- Y：修订版本号，表示bug修复和小改进
- Z：构建版本号，表示构建标识

指定特定版本安装：

```shell
dotnet add package Purcell --version 1.2.3.4
```

## 快速上手

### 读取 Excel 到对象列表

```csharp
using PurcellLibs;

// 假设有一个 Person 类
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// 读取 .xlsx 文件
var people = Purcell.Query<Person>("data.xlsx").ToList();
```

### 导出对象列表到 Excel

```csharp
using PurcellLibs;

var people = new List<Person>
{
    new Person { Name = "张三", Age = 18 },
    new Person { Name = "李四", Age = 20 }
};

Purcell.Export(people, "output.xlsx");
```

### 支持异步操作

```csharp
await foreach (var person in Purcell.QueryAsAsync<Person>("data.xlsx"))
{
    Console.WriteLine(person.Name);
}
```

## API 概览

- `Purcell.Query<T>(...)`：读取 Excel/CSV 到强类型对象集合
- `Purcell.Export<T>(...)`：导出对象集合到 Excel/CSV
- 支持流式、文件路径、同步/异步多种方式
- 支持自定义字段映射、表头、样式、密码保护等高级功能

详细 API 参考 [Wiki](https://github.com/lileyzhao/Purcell/wiki) 或源码注释。

## 生态与扩展

- [Purcell.UnitTest](../Purcell.UnitTest)：单元测试项目
- [Purcell.Benchmarks](../Purcell.Benchmarks)：性能基准测试
- [Purcell.Sample.ConsoleApp](../Purcell.Sample.ConsoleApp)：控制台示例

## 贡献

欢迎 Issue、PR 和建议！请阅读 [CONTRIBUTING.md](../CONTRIBUTING.md) 了解贡献流程。

## 许可证

MIT License © LileyZhao

---

如需更详细的用法、进阶特性或遇到问题，欢迎访问 [项目主页](https://github.com/lileyzhao/Purcell) 或提交 Issue。