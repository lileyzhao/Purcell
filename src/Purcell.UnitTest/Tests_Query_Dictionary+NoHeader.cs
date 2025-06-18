using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public partial class Tests_Query_Dictionary
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FilePath_NoHeader(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HasHeader = false };
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["H"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["M"]);
                    else Assert.Equal("36.36", item["M"]);
                    if (extension != "csv") Assert.Equal(60.0, item["N"]);
                    else Assert.Equal("60", item["N"]);
                    Assert.Equal("11/24/2003", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["L"]);
                    else Assert.Equal("2:31:49 AM", item["L"]);
                    Assert.Equal("Q4", item["P"]);
                    Assert.Equal("H2", item["Q"]);
                    Assert.Equal("303-572-8492", item["AC"]);
                    Assert.Equal("Denver", item["AD"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["H"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["M"]);
                    else Assert.Equal("43.63", item["M"]);
                    if (extension != "csv") Assert.Equal(90.0, item["N"]);
                    else Assert.Equal("90", item["N"]);
                    Assert.Equal("6/9/2007", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["L"]);
                    else Assert.Equal("2:18:24 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("479-740-7633", item["AC"]);
                    Assert.Equal("Wright", item["AD"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["H"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["M"]);
                    else Assert.Equal("38.38", item["M"]);
                    if (extension != "csv") Assert.Equal(55.0, item["N"]);
                    else Assert.Equal("55", item["N"]);
                    Assert.Equal("4/25/2015", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["L"]);
                    else Assert.Equal("10:26:52 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("231-290-3075", item["AC"]);
                    Assert.Equal("Alma", item["AD"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FilePath_NoHeader(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HasHeader = false };
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["H"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["M"]);
                    else Assert.Equal("36.36", item["M"]);
                    if (extension != "csv") Assert.Equal(60.0, item["N"]);
                    else Assert.Equal("60", item["N"]);
                    Assert.Equal("11/24/2003", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["L"]);
                    else Assert.Equal("2:31:49 AM", item["L"]);
                    Assert.Equal("Q4", item["P"]);
                    Assert.Equal("H2", item["Q"]);
                    Assert.Equal("303-572-8492", item["AC"]);
                    Assert.Equal("Denver", item["AD"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["H"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["M"]);
                    else Assert.Equal("43.63", item["M"]);
                    if (extension != "csv") Assert.Equal(90.0, item["N"]);
                    else Assert.Equal("90", item["N"]);
                    Assert.Equal("6/9/2007", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["L"]);
                    else Assert.Equal("2:18:24 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("479-740-7633", item["AC"]);
                    Assert.Equal("Wright", item["AD"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["H"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["M"]);
                    else Assert.Equal("38.38", item["M"]);
                    if (extension != "csv") Assert.Equal(55.0, item["N"]);
                    else Assert.Equal("55", item["N"]);
                    Assert.Equal("4/25/2015", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["L"]);
                    else Assert.Equal("10:26:52 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("231-290-3075", item["AC"]);
                    Assert.Equal("Alma", item["AD"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FileStream_NoHeader(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HasHeader = false };
        await using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["H"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["M"]);
                    else Assert.Equal("36.36", item["M"]);
                    if (extension != "csv") Assert.Equal(60.0, item["N"]);
                    else Assert.Equal("60", item["N"]);
                    Assert.Equal("11/24/2003", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["L"]);
                    else Assert.Equal("2:31:49 AM", item["L"]);
                    Assert.Equal("Q4", item["P"]);
                    Assert.Equal("H2", item["Q"]);
                    Assert.Equal("303-572-8492", item["AC"]);
                    Assert.Equal("Denver", item["AD"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["H"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["M"]);
                    else Assert.Equal("43.63", item["M"]);
                    if (extension != "csv") Assert.Equal(90.0, item["N"]);
                    else Assert.Equal("90", item["N"]);
                    Assert.Equal("6/9/2007", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["L"]);
                    else Assert.Equal("2:18:24 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("479-740-7633", item["AC"]);
                    Assert.Equal("Wright", item["AD"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["H"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["M"]);
                    else Assert.Equal("38.38", item["M"]);
                    if (extension != "csv") Assert.Equal(55.0, item["N"]);
                    else Assert.Equal("55", item["N"]);
                    Assert.Equal("4/25/2015", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["L"]);
                    else Assert.Equal("10:26:52 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("231-290-3075", item["AC"]);
                    Assert.Equal("Alma", item["AD"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FileStream_NoHeader(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HasHeader = false };
        using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in Purcell.Query(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["H"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["M"]);
                    else Assert.Equal("36.36", item["M"]);
                    if (extension != "csv") Assert.Equal(60.0, item["N"]);
                    else Assert.Equal("60", item["N"]);
                    Assert.Equal("11/24/2003", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["L"]);
                    else Assert.Equal("2:31:49 AM", item["L"]);
                    Assert.Equal("Q4", item["P"]);
                    Assert.Equal("H2", item["Q"]);
                    Assert.Equal("303-572-8492", item["AC"]);
                    Assert.Equal("Denver", item["AD"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["H"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["M"]);
                    else Assert.Equal("43.63", item["M"]);
                    if (extension != "csv") Assert.Equal(90.0, item["N"]);
                    else Assert.Equal("90", item["N"]);
                    Assert.Equal("6/9/2007", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["L"]);
                    else Assert.Equal("2:18:24 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("479-740-7633", item["AC"]);
                    Assert.Equal("Wright", item["AD"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["H"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["M"]);
                    else Assert.Equal("38.38", item["M"]);
                    if (extension != "csv") Assert.Equal(55.0, item["N"]);
                    else Assert.Equal("55", item["N"]);
                    Assert.Equal("4/25/2015", item["O"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["L"]);
                    else Assert.Equal("10:26:52 PM", item["L"]);
                    Assert.Equal("Q2", item["P"]);
                    Assert.Equal("H1", item["Q"]);
                    Assert.Equal("231-290-3075", item["AC"]);
                    Assert.Equal("Alma", item["AD"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }
}