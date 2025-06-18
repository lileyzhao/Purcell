using System.Data;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Export_WorkSheet
{
    [Fact]
    public void TestSheetData()
    {
        var localAnonymous = new[] { new { Name = "张三", Age = 18 } };

        PurTable sheetDataLocalAnonymous = PurTable.From(localAnonymous).WithHeaderStart(CellLocator.Create(2, 3));
        Assert.Equal(string.Empty, sheetDataLocalAnonymous.SheetName);
        Assert.Equal(localAnonymous.Length, sheetDataLocalAnonymous.Records.Count());
        Assert.Equal(2, sheetDataLocalAnonymous.GetHeaderStart().RowIndex);
        Assert.Equal(3, sheetDataLocalAnonymous.GetHeaderStart().ColumnIndex);
        Assert.True(sheetDataLocalAnonymous.AutoFilter);

        DataTable dataDataTable = MockData.GetDataTable();
        PurTable sheetDataDataTable = PurTable.From(dataDataTable, "DT数据表格")
            .WithName("DT数据表格2")
            .WithHeaderStart(CellLocator.Create(5, 6))
            .WithAutoFilter(false);
        Assert.Equal("DT数据表格2", sheetDataDataTable.SheetName);
        Assert.Equal(dataDataTable.Rows.Count, sheetDataDataTable.Records.Count());
        Assert.Equal(5, sheetDataDataTable.GetHeaderStart().RowIndex);
        Assert.Equal(6, sheetDataDataTable.GetHeaderStart().ColumnIndex);
        Assert.False(sheetDataDataTable.AutoFilter);

        List<dynamic?> dataDynamic = MockData.GetDynamicData();
        PurTable sheetDataDynamic = PurTable.From(dataDynamic, "动态数据表格")
            .WithAutoFilter(false)
            .WithPassword("12345");
        Assert.Equal("动态数据表格", sheetDataDynamic.SheetName);
        Assert.Equal(dataDynamic.Count, sheetDataDynamic.Records.Count());
        Assert.False(sheetDataDynamic.AutoFilter);
        Assert.Equal("12345", sheetDataDynamic.Password);

        List<MockData.Employee?> dataGenericType = MockData.GetGenericData();
        PurTable sheetDataGenericType = PurTable.From(dataGenericType, "泛型数据表格")
            .WithTableStyle(PurStyle.CozyAutumn);
        Assert.Equal("泛型数据表格", sheetDataGenericType.SheetName);
        Assert.Equal(dataGenericType.Count, sheetDataGenericType.Records.Count());
        Assert.Equal(PurStyle.CozyAutumn.HeaderStyle.Fill.Color, sheetDataGenericType.TableStyle.HeaderStyle.Fill.Color);

        object?[] dataAnonymous = MockData.GetAnonymousObjectData();
        PurTable sheetDataAnonymous = PurTable.From(dataAnonymous, "匿名数据表格")
            .WithTableStyle(PurStyle.Default.SetMinColumnWidth(15).SetMaxColumnWidth(25));
        Assert.Equal("匿名数据表格", sheetDataAnonymous.SheetName);
        Assert.Equal(dataAnonymous.Cast<object>().Count(), sheetDataAnonymous.Records.Count());
        Assert.Equal(15, sheetDataAnonymous.TableStyle.MinColumnWidth);
        Assert.Equal(25, sheetDataAnonymous.TableStyle.MaxColumnWidth);
    }
}