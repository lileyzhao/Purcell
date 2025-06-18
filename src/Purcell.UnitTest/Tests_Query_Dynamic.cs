using Newtonsoft.Json;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public partial class Tests_Query_Dynamic(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDynamicAsync_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.RemoveAll };
        foreach (dynamic item in await Purcell.QueryDynamicAsync(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
            IDictionary<string, object?>? itemDict = item as IDictionary<string, object?>;
            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(36.36d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("36.36", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(60.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("60", itemDict?["WeightinKgs."]);
                    Assert.Equal("11/24/2003", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeofBirth);
                    else Assert.Equal("2:31:49 AM", item.TimeofBirth);
                    Assert.Equal("Q4", item.QuarterofJoining);
                    Assert.Equal("H2", item.HalfofJoining);
                    Assert.Equal("Nov", item.ShortMonth);
                    Assert.Equal("303-572-8492", itemDict?["PhoneNo."]);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(43.63d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("43.63", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(90.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("90", itemDict?["WeightinKgs."]);
                    Assert.Equal("6/9/2007", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeofBirth);
                    else Assert.Equal("2:18:24 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Jun", item.ShortMonth);
                    Assert.Equal("479-740-7633", itemDict?["PhoneNo."]);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(38.38d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("38.38", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(55.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("55", itemDict?["WeightinKgs."]);
                    Assert.Equal("4/25/2015", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeofBirth);
                    else Assert.Equal("10:26:52 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Apr", item.ShortMonth);
                    Assert.Equal("231-290-3075", itemDict?["PhoneNo."]);
                    Assert.Equal("Alma", item.PlaceName);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDynamic_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.RemoveAll };
        foreach (dynamic item in Purcell.QueryDynamic(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
            IDictionary<string, object?>? itemDict = item as IDictionary<string, object?>;
            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(36.36d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("36.36", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(60.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("60", itemDict?["WeightinKgs."]);
                    Assert.Equal("11/24/2003", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeofBirth);
                    else Assert.Equal("2:31:49 AM", item.TimeofBirth);
                    Assert.Equal("Q4", item.QuarterofJoining);
                    Assert.Equal("H2", item.HalfofJoining);
                    Assert.Equal("Nov", item.ShortMonth);
                    Assert.Equal("303-572-8492", itemDict?["PhoneNo."]);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(43.63d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("43.63", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(90.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("90", itemDict?["WeightinKgs."]);
                    Assert.Equal("6/9/2007", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeofBirth);
                    else Assert.Equal("2:18:24 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Jun", item.ShortMonth);
                    Assert.Equal("479-740-7633", itemDict?["PhoneNo."]);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(38.38d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("38.38", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(55.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("55", itemDict?["WeightinKgs."]);
                    Assert.Equal("4/25/2015", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeofBirth);
                    else Assert.Equal("10:26:52 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Apr", item.ShortMonth);
                    Assert.Equal("231-290-3075", itemDict?["PhoneNo."]);
                    Assert.Equal("Alma", item.PlaceName);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDynamicAsync_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.RemoveAll };
        await using FileStream fileStream = File.OpenRead(filePath);
        foreach (dynamic item in await Purcell.QueryDynamicAsync(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
            IDictionary<string, object?>? itemDict = item as IDictionary<string, object?>;
            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(36.36d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("36.36", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(60.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("60", itemDict?["WeightinKgs."]);
                    Assert.Equal("11/24/2003", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeofBirth);
                    else Assert.Equal("2:31:49 AM", item.TimeofBirth);
                    Assert.Equal("Q4", item.QuarterofJoining);
                    Assert.Equal("H2", item.HalfofJoining);
                    Assert.Equal("Nov", item.ShortMonth);
                    Assert.Equal("303-572-8492", itemDict?["PhoneNo."]);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(43.63d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("43.63", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(90.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("90", itemDict?["WeightinKgs."]);
                    Assert.Equal("6/9/2007", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeofBirth);
                    else Assert.Equal("2:18:24 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Jun", item.ShortMonth);
                    Assert.Equal("479-740-7633", itemDict?["PhoneNo."]);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(38.38d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("38.38", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(55.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("55", itemDict?["WeightinKgs."]);
                    Assert.Equal("4/25/2015", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeofBirth);
                    else Assert.Equal("10:26:52 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Apr", item.ShortMonth);
                    Assert.Equal("231-290-3075", itemDict?["PhoneNo."]);
                    Assert.Equal("Alma", item.PlaceName);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDynamic_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.RemoveAll };
        using FileStream fileStream = File.OpenRead(filePath);
        foreach (dynamic item in Purcell.QueryDynamic(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
            IDictionary<string, object?>? itemDict = item as IDictionary<string, object?>;
            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(36.36d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("36.36", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(60.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("60", itemDict?["WeightinKgs."]);
                    Assert.Equal("11/24/2003", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeofBirth);
                    else Assert.Equal("2:31:49 AM", item.TimeofBirth);
                    Assert.Equal("Q4", item.QuarterofJoining);
                    Assert.Equal("H2", item.HalfofJoining);
                    Assert.Equal("Nov", item.ShortMonth);
                    Assert.Equal("303-572-8492", itemDict?["PhoneNo."]);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(43.63d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("43.63", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(90.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("90", itemDict?["WeightinKgs."]);
                    Assert.Equal("6/9/2007", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeofBirth);
                    else Assert.Equal("2:18:24 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Jun", item.ShortMonth);
                    Assert.Equal("479-740-7633", itemDict?["PhoneNo."]);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", itemDict?["Father'sName"]);
                    if (extension != "csv") Assert.Equal(38.38d, itemDict?["AgeinYrs."]);
                    else Assert.Equal("38.38", itemDict?["AgeinYrs."]);
                    if (extension != "csv") Assert.Equal(55.0, itemDict?["WeightinKgs."]);
                    else Assert.Equal("55", itemDict?["WeightinKgs."]);
                    Assert.Equal("4/25/2015", item.DateofJoining);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeofBirth);
                    else Assert.Equal("10:26:52 PM", item.TimeofBirth);
                    Assert.Equal("Q2", item.QuarterofJoining);
                    Assert.Equal("H1", item.HalfofJoining);
                    Assert.Equal("Apr", item.ShortMonth);
                    Assert.Equal("231-290-3075", itemDict?["PhoneNo."]);
                    Assert.Equal("Alma", item.PlaceName);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }
}