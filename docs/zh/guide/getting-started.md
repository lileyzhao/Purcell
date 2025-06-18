# 快速开始 {#getting-started}

## 安装 {#installation}

```bash
# 使用 NuGet 包管理器
dotnet add package Purcell

# 或使用 Package Manager Console
Install-Package Purcell
```

## 基础用法 {#basic-usage}

### 创建 Purcell 对象实例

要使用 Purcell，需要先调用工厂函数`Purcell.File()`，这是 [Fluent API](https://martinfowler.com/bliki/FluentInterface.html) 方法链的入口点，它负责创建一个 Purcell 对象实例（需要传入参数来指定读写 Excel 文件所需的**文件路径**或**文件流**）。

```csharp
// 读取 Excel 文件：通过文件路径
Purcell.File("sample.xlsx");
// 读取 Excel 文件：通过文件流（需指定 Excel 文件类型）
Purcell.File(fileStream, ExcelFileType.Xlsx);

// 导出 Excel 文件：指定导出文件路径
Purcell.File("output.xlsx");
// 导出 Excel 文件：导出为 xlsx 文件流
Purcell.File(fileStream);
```

### 读取 Excel 文件

```csharp
// 强类型映射：将表格数据映射为指定的 C# 实体类对象（逐行读取，超低内存消耗）
using(var userItem in Purcell.File("input.xlsx").Query<User>()){
    // Do something...
}

// 一次性加载到内存（数据量大时不推荐）
var users = Purcell.File("input.xlsx").Query<User>().ToList();
```

### 导出 Excel 文件

`AddSheetData`可以逐个添加工作表，`AddSheetDatas`可以一次性添加多个工作表

```csharp
// 导出单个工作表
Purcell.File("output.xlsx").AddSheet(data).Export();

// 导出多个工作表: 使用 AddSheetData 逐个添加工作表
Purcell.File("output.xlsx").AddSheet(data1).AddSheet(data2).Export();

// 导出多个工作表：使用 AddSheetDatas 一次性添加多个工作表
Purcell.File("output.xlsx").AddSheets(data1,data2).Export();

// 使用 FluentAPI 自定义工作表
Purcell.File("output.xlsx")
       .AddSheet(SheetData.New("MySheet", data))
       .Export();
Purcell.File("output.xlsx")
       .AddSheets(
           SheetData.New(data1).SetSheetName("Sheet1"),
           SheetData.New(data2).SetSheetStyle(SheetStyle.SunnyDay)
       ).Export();
```

## 下一步 {#what-s-next}

- 进一步了解 [表格读取的用法](./read-excel) 。

- 进一步了解 [表格导出的用法](./export-xlsx) 。

- 想要了解如何 [自定义导出的样式](./export-xlsx#工作表样式) 。
