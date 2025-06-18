using Newtonsoft.Json;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public partial class Tests_Query_Dictionary(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;

        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["Age in Yrs."]);
                    else Assert.Equal("36.36", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(60.0, item["Weight in Kgs."]);
                    else Assert.Equal("60", item["Weight in Kgs."]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["Quarter of Joining"]);
                    Assert.Equal("H2", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["Phone No."]);
                    Assert.Equal("Denver", item["Place Name"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["Age in Yrs."]);
                    else Assert.Equal("43.63", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(90.0, item["Weight in Kgs."]);
                    else Assert.Equal("90", item["Weight in Kgs."]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["Phone No."]);
                    Assert.Equal("Wright", item["Place Name"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["Age in Yrs."]);
                    else Assert.Equal("38.38", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(55.0, item["Weight in Kgs."]);
                    else Assert.Equal("55", item["Weight in Kgs."]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["Phone No."]);
                    Assert.Equal("Alma", item["Place Name"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["Age in Yrs."]);
                    else Assert.Equal("36.36", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(60.0, item["Weight in Kgs."]);
                    else Assert.Equal("60", item["Weight in Kgs."]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["Quarter of Joining"]);
                    Assert.Equal("H2", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["Phone No."]);
                    Assert.Equal("Denver", item["Place Name"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["Age in Yrs."]);
                    else Assert.Equal("43.63", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(90.0, item["Weight in Kgs."]);
                    else Assert.Equal("90", item["Weight in Kgs."]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["Phone No."]);
                    Assert.Equal("Wright", item["Place Name"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["Age in Yrs."]);
                    else Assert.Equal("38.38", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(55.0, item["Weight in Kgs."]);
                    else Assert.Equal("55", item["Weight in Kgs."]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["Phone No."]);
                    Assert.Equal("Alma", item["Place Name"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        await using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(fileStream, queryType))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["Age in Yrs."]);
                    else Assert.Equal("36.36", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(60.0, item["Weight in Kgs."]);
                    else Assert.Equal("60", item["Weight in Kgs."]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["Quarter of Joining"]);
                    Assert.Equal("H2", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["Phone No."]);
                    Assert.Equal("Denver", item["Place Name"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["Age in Yrs."]);
                    else Assert.Equal("43.63", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(90.0, item["Weight in Kgs."]);
                    else Assert.Equal("90", item["Weight in Kgs."]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["Phone No."]);
                    Assert.Equal("Wright", item["Place Name"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["Age in Yrs."]);
                    else Assert.Equal("38.38", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(55.0, item["Weight in Kgs."]);
                    else Assert.Equal("55", item["Weight in Kgs."]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["Phone No."]);
                    Assert.Equal("Alma", item["Place Name"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in Purcell.Query(fileStream, queryType))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["Age in Yrs."]);
                    else Assert.Equal("36.36", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(60.0, item["Weight in Kgs."]);
                    else Assert.Equal("60", item["Weight in Kgs."]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["Quarter of Joining"]);
                    Assert.Equal("H2", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["Phone No."]);
                    Assert.Equal("Denver", item["Place Name"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["Age in Yrs."]);
                    else Assert.Equal("43.63", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(90.0, item["Weight in Kgs."]);
                    else Assert.Equal("90", item["Weight in Kgs."]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["Phone No."]);
                    Assert.Equal("Wright", item["Place Name"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["Father's Name"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["Age in Yrs."]);
                    else Assert.Equal("38.38", item["Age in Yrs."]);
                    if (extension != "csv") Assert.Equal(55.0, item["Weight in Kgs."]);
                    else Assert.Equal("55", item["Weight in Kgs."]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["Quarter of Joining"]);
                    Assert.Equal("H1", item["Half of Joining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["Phone No."]);
                    Assert.Equal("Alma", item["Place Name"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }
}