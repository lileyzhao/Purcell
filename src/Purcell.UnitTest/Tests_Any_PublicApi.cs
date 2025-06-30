using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Any_PublicApi(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx", TableFileType.Xlsx)]
    [InlineData("xls", TableFileType.Xls)]
    [InlineData("csv", TableFileType.Csv)]
    public void TestGeneric(string extension, TableFileType queryType)
    {
        string filePath = $"Resources/Employee_100.{extension}";

        #region 01. 简单的强类型查询

        int rowIndex = -1;
        testHelper.WriteLine("01. 简单的强类型查询");
        foreach (MockData.Employee item in Purcell.Query<MockData.Employee>(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item.Id} : {item.Name} {item.Age}岁");
            Assert.Equal(rowIndex + 1, item.Id);
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 01. 简单的强类型查询

        #region 02. 简单的强类型查询(自定义Stream)

        rowIndex = -1;
        testHelper.WriteLine("02. 简单的强类型查询(自定义Stream)");
        foreach (MockData.Employee item in Purcell.Query<MockData.Employee>(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item.Id} : {item.Name} {item.Age}岁");
            Assert.Equal(rowIndex + 1, item.Id);
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 02. 简单的强类型查询(自定义Stream)

        #region 03. 首行查询

        testHelper.WriteLine("03. 首行查询");
        MockData.Employee firstItem = Purcell.Query<MockData.Employee>(filePath).First();
        testHelper.WriteLine($"\t[01]{firstItem.Id} : {firstItem.Name} {firstItem.Age}岁");
        Assert.Equal(1, firstItem.Id);

        #endregion 03. 首行查询

        #region 04. Dynamic查询

        rowIndex = -1;
        testHelper.WriteLine("04. Dynamic查询");
        foreach (IDictionary<string, object?> item in Purcell.QueryDynamic(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item["编号"]} : {item["姓名"]} {item["年龄"]}岁");
            Assert.Equal(rowIndex + 1, Convert.ToInt32(item["编号"]));
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 04. Dynamic查询

        #region 05. Dynamic查询(动态设置列)

        rowIndex = -1;
        testHelper.WriteLine("05. Dynamic查询(动态设置列)");
        List<PurColumn> dynamicColumns =
        [
            PurColumn.FromProperty("Id").AddName("编号"),
            PurColumn.FromProperty("Name").AddName("姓名"),
            PurColumn.FromProperty("Age").AddName("年龄")
        ];
        foreach (dynamic item in Purcell.QueryDynamic(filePath, PurTable.From(dynamicColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item.Id} : {item.Name} {item.Age}岁");
            Assert.Equal(rowIndex + 1, Convert.ToInt32(item.Id));
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 05. Dynamic查询(动态设置列)

        #region 06. 无列名自动使用A1表示法作为列名

        rowIndex = -1;
        testHelper.WriteLine("06. 无列名自动使用A1表示法作为列名");
        foreach (dynamic item in Purcell.QueryDynamic(filePath, new PurTable(0).WithHasHeader(false)))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item.A} : {item.B} {item.C}岁");
            Assert.Equal(rowIndex + 1, Convert.ToInt32(item.A));
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 06. 无列名自动使用A1表示法作为列名

        #region 07. 获取工作表名列表

        rowIndex = -1;
        testHelper.WriteLine("07. 指定工作表名");
        var tableConfig = new PurTable("Sheet2").WithHasHeader(false);
        foreach (dynamic item in Purcell.QueryDynamic(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"\t[{rowIndex + 1:00}]{item.A} : {item.B} {item.C}岁");
            Assert.Equal(rowIndex + 1, Convert.ToInt32(item.A));
            if (rowIndex + 1 >= 5) break;
        }

        #endregion 07. 指定工作表名
    }
}