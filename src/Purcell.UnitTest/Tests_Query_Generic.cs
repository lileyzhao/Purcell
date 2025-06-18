using Newtonsoft.Json;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public partial class Tests_Query_Generic(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryGenericAsync_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.Trim };
        foreach (EmployeeWithAttr item in await Purcell.QueryAsync<EmployeeWithAttr>(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item.FathersName);
                    Assert.Equal(36.36m, item.AgeInYears);
                    Assert.Equal(60.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2003-11-24"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeOfBirth);
                    Assert.Equal("Q4", item.QuarterOfJoining);
                    Assert.Equal("H2", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("303-572-8492", item.PhoneNo);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item.FathersName);
                    Assert.Equal(43.63m, item.AgeInYears);
                    Assert.Equal(90.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2007-06-09"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("479-740-7633", item.PhoneNo);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item.FathersName);
                    Assert.Equal(38.38m, item.AgeInYears);
                    Assert.Equal(55.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2015-04-25"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("231-290-3075", item.PhoneNo);
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
    public void TestQueryGeneric_Complex_FilePath(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.Trim };
        foreach (EmployeeWithAttr item in Purcell.Query<EmployeeWithAttr>(filePath, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item.FathersName);
                    Assert.Equal(36.36m, item.AgeInYears);
                    Assert.Equal(60.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2003-11-24"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeOfBirth);
                    Assert.Equal("Q4", item.QuarterOfJoining);
                    Assert.Equal("H2", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("303-572-8492", item.PhoneNo);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item.FathersName);
                    Assert.Equal(43.63m, item.AgeInYears);
                    Assert.Equal(90.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2007-06-09"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("479-740-7633", item.PhoneNo);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item.FathersName);
                    Assert.Equal(38.38m, item.AgeInYears);
                    Assert.Equal(55.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2015-04-25"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("231-290-3075", item.PhoneNo);
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
    public async Task TestQueryGenericAsync_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.Trim };
        await using FileStream fileStream = File.OpenRead(filePath);
        foreach (EmployeeWithAttr item in await Purcell.QueryAsync<EmployeeWithAttr>(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item.FathersName);
                    Assert.Equal(36.36m, item.AgeInYears);
                    Assert.Equal(60.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2003-11-24"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeOfBirth);
                    Assert.Equal("Q4", item.QuarterOfJoining);
                    Assert.Equal("H2", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("303-572-8492", item.PhoneNo);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item.FathersName);
                    Assert.Equal(43.63m, item.AgeInYears);
                    Assert.Equal(90.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2007-06-09"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("479-740-7633", item.PhoneNo);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item.FathersName);
                    Assert.Equal(38.38m, item.AgeInYears);
                    Assert.Equal(55.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2015-04-25"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("231-290-3075", item.PhoneNo);
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
    public void TestQueryGeneric_Complex_FileStream(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        PurTable tableConfig = new() { HeaderSpaceMode = WhiteSpaceMode.Trim };
        using FileStream fileStream = File.OpenRead(filePath);
        foreach (EmployeeWithAttr item in Purcell.Query<EmployeeWithAttr>(fileStream, queryType, tableConfig))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item.FathersName);
                    Assert.Equal(36.36m, item.AgeInYears);
                    Assert.Equal(60.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2003-11-24"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("02:31:49"), item.TimeOfBirth);
                    Assert.Equal("Q4", item.QuarterOfJoining);
                    Assert.Equal("H2", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("303-572-8492", item.PhoneNo);
                    Assert.Equal("Denver", item.PlaceName);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item.FathersName);
                    Assert.Equal(43.63m, item.AgeInYears);
                    Assert.Equal(90.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2007-06-09"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("14:18:24"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("479-740-7633", item.PhoneNo);
                    Assert.Equal("Wright", item.PlaceName);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item.FathersName);
                    Assert.Equal(38.38m, item.AgeInYears);
                    Assert.Equal(55.0, item.WeightInKgs);
                    Assert.Equal(DateTime.Parse("2015-04-25"), item.DateOfJoining);
                    Assert.Equal(TimeSpan.Parse("22:26:52"), item.TimeOfBirth);
                    Assert.Equal("Q2", item.QuarterOfJoining);
                    Assert.Equal("H1", item.HalfOfJoining);
                    Assert.Null(item.ShortMonth);
                    Assert.Equal("231-290-3075", item.PhoneNo);
                    Assert.Equal("Alma", item.PlaceName);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }
}