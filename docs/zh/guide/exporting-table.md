# 导出表格

导出 Excel 表格支持**任意可枚举的集合**作为导出数据，只支持导出`.xlsx`格式。

> [!TIP] 数据源支持
>
> - `IEnumerable<T>` where T : class, new()
> - `IEnumerable<IDictionary<string, object>>`
> - `DataTable` 和 `DataSet`
> - `new[] { new { Name = "张三" } }` 匿名数组
> - `List<int>`、`List<string>`、`List<DateTime>` 等基础类型列表
> - 其他继承`IEnumerable`的可枚举集合

## 快速入门

> 导出 Excel 表格功能的使用非常简单，只需要调用静态方法`Purcell.ExportXlsx()`，仅支持导出为`.xlsx`格式 Excel 文件，不支持导出为`.xls`。

### 简单使用

```csharp
// 导出 1 个工作表
var list = new[] { new { Name = "张三", Age = 18 } };
Purcell.ExportXlsx("c://output.xlsx", SheetData.New(list));
Purcell.ExportXlsx("c://output.xlsx", SheetData.New("人员表", list));

// 导出多个工作表
var list1 = new[] { new { Name = "张三", Age = 18 } };
var list2 = new[] { new { Name = "李四", Age = 20 } };
Purcell.ExportXlsx("c://output.xlsx", SheetData.New(list1), SheetData.New(list2));

// 导出多个工作表：SheetData 集合
var sheetDatas = new List<SheetData> { SheetData.New(list1), SheetData.New(list2) };
Purcell.ExportXlsx("c://output.xlsx", sheetDatas);

// 导出为 Stream
Purcell.ExportXlsx(stream, SheetData.New("人员表", list));
Purcell.ExportXlsx(stream, sheetDatas);
```

### 配置工作表

在上面**简单使用**的示例代码块中可以看出，导出 Excel 表格需要使用 `SheetData` 对象。那 `SheetData` 是什么呢？

`SheetData` **等同于 Excel 表格中的一个工作表(Worksheet)**，`SheetData` 对象包含了导出一个工作表所需要的`Sheet名称`、`数据集合`、`Excel 列属性`、`风格样式`...等配置。

> [!TIP]
> 了解`SheetData`才能更好的使用导出功能，一般来说，你只需要了解如何使用`Excel 列属性` 和 `预设的表格样式`就满足绝大多数场景的使用了。

使用`new SheetData()`或`SheetData.New()`创建实例，支持完整的`Fluent API`方法链来配置。

```csharp
// 创建并配置工作表：构造函数
var sheetData = new SheetData("学生表", list);
sheetData.SheetStyle = SheetStyle.SunnyDay; // 使用预设样式
sheetData.AutoFilter = false;               // 关闭自动筛选，默认开启
sheetData.Password = "123";                 // 启用密码保护工作表

// 使用 Fluent API 方法链
var sheetData = SheetData.New("学生表", list)
    .SetSheetName("员工表")              // 设置工作表名
    .SetExcelColumns(columns)           // 列属性配置列表
    .SetWriteStart(2, 3)                // 写入起始位置
    .SetAutoFilter(false)               // 是否启用自动筛选，默认值为
    .EnableAutoFilter()                 // 启用自动筛选
    .DisableAutoFilter()                // 关闭自动筛选
    .EnablePassword("123")              // 启用密码保护工作表
    .DisablePassword()                  // 关闭密码保护
    .SetContinueOnError(true)           // 是否跳过数据异常
    .SetSheetStyle(SheetStyle.Default)  // 工作表样式
    .SetHeaderStyle(XlsxStyle.Default)  // 表头样式
    .SetContentStyle(XlsxStyle.Default) // 内容样式
    .SetHeaderLineHeight(8)             // 表头行行高
    .SetContentLineHeight(8)            // 内容行行高
    .SetMinColumnWidth(10)              // 最小列宽限制
    .SetMaxColumnWidth(20);             // 最大列宽限制
```

### Excel 列属性

Excel 列属性(`ExcelColumn`)既可以当做特性(Attribute)配置到实体类属性，也可以灵活的动态配置用于任意**可枚举的集合**、**DataTable/DataSet**、**匿名数组** 等等

> [!TIP]
> `ExcelColumn`列属性配置提供了表格列与 C# 对象属性之间的双向映射关系，可同时用于读取和导出。对于导出表格来说，可配置的属性有：
>
> - `ColumnName` 作为导出列名
> - `ColumnWidth` 作为导出列宽
> - `HeaderHorizontal` 表头的水平对齐方式
> - `HeaderVertical` 表头的垂直对齐方式
> - `ContentHorizontal` 内容的水平对齐方式
> - `ContentVertical` 内容的垂直对齐方式
> - `ColumnHidden` 是否隐藏列
> - `IgnoreExport` 是否忽略导出

#### 作为特性使用

作为特性(Attribute)使用，如果感觉为实体类添加**特性(Attribute)**很麻烦，你也可以使用**动态配置列属性**的方式，**动态配置列属性**方式使用非常灵活，甚至你可以把列属性配置存储到数据库。

以下仅演示导出使用到的属性，更多属性配置请查看`ExcelColumn`实体类。

```csharp
public class StudentDto
{
    [ExcelColumn("编号", ColumnWidth = 12)]
    public int Id { get; set; }

    [ExcelColumn("姓名", ColumnWidth = 15)]
    public string Name { get; set; }

    [ExcelColumn("年龄", ContentVertical = XlsxAlignment.Horizontal.Right)]
    public int Age { get; set; }

    [ExcelColumn("邮箱", ColumnWidth = 25)]
    public string Email { get; set; }

    [ExcelColumn("电话号码", ContentVertical = XlsxAlignment.Vertical.Center)]
    public string PhoneNumber { get; set; }

    [ExcelColumn("入学日期", HeaderHorizontal = XlsxAlignment.Horizontal.Right)]
    public DateTime EnrollmentDate { get; set; }

    [ExcelColumn("专业", IgnoreExport = true)]
    public string Major { get; set; }
}
```

#### 动态配置列属性

这种方法非常灵活，甚至你可以把相关读取配置存储到数据库，也可以结合前端动态增删改配置。

以下仅演示导出使用到的属性，更多属性配置请查看`ExcelColumn`实体类。

```csharp
var excelColumns = new List<ExcelColumn>()
{
    new ExcelColumn("编号") { PropertyName = "Id", ColumnWidth = 12 },
    new ExcelColumn("姓名") { PropertyName = "Name", ColumnWidth = 15 },
    new ExcelColumn("年龄") { PropertyName = "Age", ContentHorizontal = XlsxAlignment.Horizontal.Right },
    new ExcelColumn("邮箱") { PropertyName = "Email", ColumnWidth = 25 },
    new ExcelColumn("电话号码") { PropertyName = "PhoneNumber", ContentVertical = XlsxAlignment.Vertical.Center },
    new ExcelColumn("入学日期") { PropertyName = "EnrollmentDate", HeaderHorizontal = XlsxAlignment.Horizontal.Right },
    new ExcelColumn("专业") { PropertyName = "Major", IgnoreExport = true }
};

// 使用动态列属性
Purcell.ExportXlsx("c://output.xlsx", SheetData.New(list).SetExcelColumns(excelColumns));
```

## 工作表样式

导出表格可以使用预设的工作表样式或自定义工作表样式，Purcell 预设了常见的工作表样式。

### 使用预设样式

```csharp
Purcell.ExportXlsx("c://output.xlsx",
    SheetData.New(list).SetSheetStyle(SheetStyle.SunnyDay));
```

### 自定义样式

自定义样式基于**LargeXlsx**，更多使用方式请参考 [LargeXlsx样式文档](https://github.com/salvois/LargeXlsx?tab=readme-ov-file#styling)

```csharp
var customStyle = new SheetStyle()
{
    HeaderStyle = XlsxStyle.Default
        .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
        .With(new XlsxFill(Color.FromArgb(0x00, 0xBF, 0xFF)))
        .With(XlsxStyle.Default.Border)
        .With(XlsxStyle.Default.NumberFormat)
        .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
    ContentStyle = XlsxStyle.Default
};
```

### 预设的样式 🎨

#### 默认样式

<table class="preset-style-table Default">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 明亮清新蓝

<table class="preset-style-table BrightFresh">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 优雅单色

<table class="preset-style-table ElegantMonochrome">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 大地色调

<table class="preset-style-table EarthTones">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 暖色调

<table class="preset-style-table WarmTones">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 海洋蓝

<table class="preset-style-table OceanBlue">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 复古怀旧

<table class="preset-style-table VintageNostalgia">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 极简黑白

<table class="preset-style-table MinimalistBW">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 活力能量

<table class="preset-style-table VibrantEnergy">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 复古时尚

<table class="preset-style-table RetroChic">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 温馨秋日

<table class="preset-style-table CozyAutumn">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 宁静自然

<table class="preset-style-table SereneNature">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 午夜魔幻

<table class="preset-style-table MidnightMagic">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

#### 暖阳阳光

<table class="preset-style-table SunnyDay">
    <thead>
        <tr>
            <th>编号</th>
            <th>姓名</th>
            <th>性别</th>
            <th>年龄</th>
            <th>地址</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>张三</td>
            <td>男</td>
            <td>28</td>
            <td>北京市朝阳区建国路88号国际大厦A座</td>
        </tr>
        <tr>
            <td>2</td>
            <td>李四</td>
            <td>女</td>
            <td>34</td>
            <td>上海市浦东新区世纪大道2000号东方明珠塔</td>
        </tr>
        <tr>
            <td>3</td>
            <td>王五</td>
            <td>男</td>
            <td>22</td>
            <td>广州市天河区体育东路138号天汇大厦</td>
        </tr>
    </tbody>
</table>

<style>
.preset-style-table {
  border-collapse: collapse;
}

.preset-style-table tr:nth-child(2n),.preset-style-table tr {
  background-color: #FFFFFF;
  color: #333333;
}

.preset-style-table.Default thead tr th{
  color: #FFFFFF;
  background-color: #004586;
}

.preset-style-table.BrightFresh thead tr th{
  color: #FFFFFF;
  background-color: #00BFFF;
}

.preset-style-table.ElegantMonochrome thead tr th{
  color: #FFFFFF;
  background-color: #A9A9A9;
}

.preset-style-table.EarthTones thead tr th{
  color: #FFFFFF;
  background-color: #808080;
}

.preset-style-table.WarmTones thead tr th{
  color: #FFFFFF;
  background-color: #FF0000;
}

.preset-style-table.OceanBlue thead tr th{
  color: #FFFFFF;
  background-color: #191970;
}

.preset-style-table.VintageNostalgia thead tr th{
  color: #808080;
  background-color: #FFC0CB;
}

.preset-style-table.MinimalistBW thead tr th{
  color: #808080;
  background-color: #FFFFFF;
}

.preset-style-table.VibrantEnergy thead tr th{
  color: #FFFFFF;
  background-color: #FFA500;
}

.preset-style-table.RetroChic thead tr th{
  color: #FFFFFF;
  background-color: #DA70D6;
}

.preset-style-table.CozyAutumn thead tr th{
  color: #FFFFFF;
  background-color: #CD853F;
}

.preset-style-table.SereneNature thead tr th{
  color: #FFFFFF;
  background-color: #2E8B57;
}

.preset-style-table.MidnightMagic thead tr th{
  color: #FFFFFF;
  background-color: #000080;
}

.preset-style-table.SunnyDay thead tr th{
  color: #808080;
  background-color: #FFFF00;
}
</style>
