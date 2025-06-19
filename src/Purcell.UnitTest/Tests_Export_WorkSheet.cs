using System.Data;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Export_WorkSheet
{
    [Fact]
    public void TestTableConfig()
    {
        var localAnonymous = new[] { new { Name = "张三", Age = 18 } };

        PurTable tableConfigLocalAnonymous = PurTable.From(localAnonymous).WithHeaderStart(CellLocator.Create(2, 3));
        Assert.Equal(string.Empty, tableConfigLocalAnonymous.SheetName);
        Assert.Equal(localAnonymous.Length, tableConfigLocalAnonymous.Records.Count());
        Assert.Equal(2, tableConfigLocalAnonymous.GetHeaderStart().RowIndex);
        Assert.Equal(3, tableConfigLocalAnonymous.GetHeaderStart().ColumnIndex);
        Assert.True(tableConfigLocalAnonymous.AutoFilter);

        DataTable dataDataTable = MockData.GetDataTable();
        PurTable tableConfigDataTable = PurTable.From(dataDataTable, "DT数据表格")
            .WithName("DT数据表格2")
            .WithHeaderStart(CellLocator.Create(5, 6))
            .WithAutoFilter(false);
        Assert.Equal("DT数据表格2", tableConfigDataTable.SheetName);
        Assert.Equal(dataDataTable.Rows.Count, tableConfigDataTable.Records.Count());
        Assert.Equal(5, tableConfigDataTable.GetHeaderStart().RowIndex);
        Assert.Equal(6, tableConfigDataTable.GetHeaderStart().ColumnIndex);
        Assert.False(tableConfigDataTable.AutoFilter);

        List<dynamic?> dataDynamic = MockData.GetDynamicData();
        PurTable tableConfigDynamic = PurTable.From(dataDynamic, "动态数据表格")
            .WithAutoFilter(false)
            .WithPassword("12345");
        Assert.Equal("动态数据表格", tableConfigDynamic.SheetName);
        Assert.Equal(dataDynamic.Count, tableConfigDynamic.Records.Count());
        Assert.False(tableConfigDynamic.AutoFilter);
        Assert.Equal("12345", tableConfigDynamic.Password);

        List<MockData.Employee?> dataGenericType = MockData.GetGenericData();
        PurTable tableConfigGenericType = PurTable.From(dataGenericType, "泛型数据表格")
            .WithTableStyle(PurStyle.CozyAutumn);
        Assert.Equal("泛型数据表格", tableConfigGenericType.SheetName);
        Assert.Equal(dataGenericType.Count, tableConfigGenericType.Records.Count());
        Assert.Equal(PurStyle.CozyAutumn.HeaderStyle.Fill.Color, tableConfigGenericType.TableStyle.HeaderStyle.Fill.Color);

        object?[] dataAnonymous = MockData.GetAnonymousObjectData();
        PurTable tableConfigAnonymous = PurTable.From(dataAnonymous, "匿名数据表格")
            .WithTableStyle(PurStyle.Default.SetMinColumnWidth(15).SetMaxColumnWidth(25));
        Assert.Equal("匿名数据表格", tableConfigAnonymous.SheetName);
        Assert.Equal(dataAnonymous.Cast<object>().Count(), tableConfigAnonymous.Records.Count());
        Assert.Equal(15, tableConfigAnonymous.TableStyle.MinColumnWidth);
        Assert.Equal(25, tableConfigAnonymous.TableStyle.MaxColumnWidth);
    }
}