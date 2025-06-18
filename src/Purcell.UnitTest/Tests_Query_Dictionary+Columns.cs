using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public partial class Tests_Query_Dictionary
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FilePath_Columns(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        List<PurColumn> dynamicColumns =
        [
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName)).AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.AgeInYears)).AddName("Age in Yrs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.WeightInKgs)).AddName("Weight in Kgs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.QuarterOfJoining)).WithIndex("P"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.HalfOfJoining)).WithIndex("Q"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.PhoneNo)).AddName("Phone No")
                .WithMatchStrategy(MatchStrategy.IgnoreCasePrefix),

            PurColumn.FromProperty(nameof(EmployeeNoAttr.PlaceName)).AddName("PLACE.*")
                .WithMatchStrategy(MatchStrategy.IgnoreCaseRegex)
        ];
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(filePath, PurTable.FromColumns(dynamicColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["AgeInYears"]);
                    else Assert.Equal("36.36", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(60.0, item["WeightInKgs"]);
                    else Assert.Equal("60", item["WeightInKgs"]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["QuarterOfJoining"]);
                    Assert.Equal("H2", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["PhoneNo"]);
                    Assert.Equal("Denver", item["PlaceName"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["AgeInYears"]);
                    else Assert.Equal("43.63", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(90.0, item["WeightInKgs"]);
                    else Assert.Equal("90", item["WeightInKgs"]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["PhoneNo"]);
                    Assert.Equal("Wright", item["PlaceName"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["AgeInYears"]);
                    else Assert.Equal("38.38", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(55.0, item["WeightInKgs"]);
                    else Assert.Equal("55", item["WeightInKgs"]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["PhoneNo"]);
                    Assert.Equal("Alma", item["PlaceName"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FilePath_Columns(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        List<PurColumn> dynamicColumns =
        [
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName)).AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName) + "_Copy").AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.AgeInYears)).AddName("Age in Yrs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.WeightInKgs)).AddName("Weight in Kgs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.QuarterOfJoining)).WithIndex("P"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.HalfOfJoining)).WithIndex("Q"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.PhoneNo)).AddName("Phone No")
                .WithMatchStrategy(MatchStrategy.IgnoreCasePrefix),

            PurColumn.FromProperty(nameof(EmployeeNoAttr.PlaceName)).AddName("PLACE.*")
                .WithMatchStrategy(MatchStrategy.IgnoreCaseRegex)
        ];
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath, PurTable.FromColumns(dynamicColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["AgeInYears"]);
                    else Assert.Equal("36.36", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(60.0, item["WeightInKgs"]);
                    else Assert.Equal("60", item["WeightInKgs"]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["QuarterOfJoining"]);
                    Assert.Equal("H2", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["PhoneNo"]);
                    Assert.Equal("Denver", item["PlaceName"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["AgeInYears"]);
                    else Assert.Equal("43.63", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(90.0, item["WeightInKgs"]);
                    else Assert.Equal("90", item["WeightInKgs"]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["PhoneNo"]);
                    Assert.Equal("Wright", item["PlaceName"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["AgeInYears"]);
                    else Assert.Equal("38.38", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(55.0, item["WeightInKgs"]);
                    else Assert.Equal("55", item["WeightInKgs"]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["PhoneNo"]);
                    Assert.Equal("Alma", item["PlaceName"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public async Task TestQueryDictionaryAsync_Complex_FileStream_Columns(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        List<PurColumn> dynamicColumns =
        [
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName)).AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName) + "_Copy").AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.AgeInYears)).AddName("Age in Yrs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.WeightInKgs)).AddName("Weight in Kgs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.QuarterOfJoining)).WithIndex("P"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.HalfOfJoining)).WithIndex("Q"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.PhoneNo)).AddName("Phone No")
                .WithMatchStrategy(MatchStrategy.IgnoreCasePrefix),

            PurColumn.FromProperty(nameof(EmployeeNoAttr.PlaceName)).AddName("PLACE.*")
                .WithMatchStrategy(MatchStrategy.IgnoreCaseRegex)
        ];
        await using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(fileStream, queryType,
                     PurTable.FromColumns(dynamicColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["AgeInYears"]);
                    else Assert.Equal("36.36", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(60.0, item["WeightInKgs"]);
                    else Assert.Equal("60", item["WeightInKgs"]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["QuarterOfJoining"]);
                    Assert.Equal("H2", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["PhoneNo"]);
                    Assert.Equal("Denver", item["PlaceName"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["AgeInYears"]);
                    else Assert.Equal("43.63", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(90.0, item["WeightInKgs"]);
                    else Assert.Equal("90", item["WeightInKgs"]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["PhoneNo"]);
                    Assert.Equal("Wright", item["PlaceName"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["AgeInYears"]);
                    else Assert.Equal("38.38", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(55.0, item["WeightInKgs"]);
                    else Assert.Equal("55", item["WeightInKgs"]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["PhoneNo"]);
                    Assert.Equal("Alma", item["PlaceName"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestQueryDictionary_Complex_FileStream_Columns(string extension, QueryType queryType)
    {
        string domain = "Complex";
        string filePath = $"Resources/{domain}.{extension}";

        int rowIndex = -1;
        List<PurColumn> dynamicColumns =
        [
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName)).AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.FathersName) + "_Copy").AddName("Father's Name"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.AgeInYears)).AddName("Age in Yrs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.WeightInKgs)).AddName("Weight in Kgs."),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.QuarterOfJoining)).WithIndex("P"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.HalfOfJoining)).WithIndex("Q"),
            PurColumn.FromProperty(nameof(EmployeeNoAttr.PhoneNo)).AddName("Phone No")
                .WithMatchStrategy(MatchStrategy.IgnoreCasePrefix),

            PurColumn.FromProperty(nameof(EmployeeNoAttr.PlaceName)).AddName("PLACE.*")
                .WithMatchStrategy(MatchStrategy.IgnoreCaseRegex)
        ];
        using FileStream fileStream = File.OpenRead(filePath);
        foreach (IDictionary<string, object?> item in Purcell.Query(fileStream, queryType, PurTable.FromColumns(dynamicColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            switch (rowIndex)
            {
                case 0:
                    Assert.Equal("Donald Walker", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(36.36d, item["AgeInYears"]);
                    else Assert.Equal("36.36", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(60.0, item["WeightInKgs"]);
                    else Assert.Equal("60", item["WeightInKgs"]);
                    Assert.Equal("11/24/2003", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("02:31:49"), item["Time of Birth"]);
                    else Assert.Equal("2:31:49 AM", item["Time of Birth"]);
                    Assert.Equal("Q4", item["QuarterOfJoining"]);
                    Assert.Equal("H2", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("303-572-8492", item["PhoneNo"]);
                    Assert.Equal("Denver", item["PlaceName"]);
                    break;
                case 38:
                    Assert.Equal("Gerald Collins", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(43.63d, item["AgeInYears"]);
                    else Assert.Equal("43.63", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(90.0, item["WeightInKgs"]);
                    else Assert.Equal("90", item["WeightInKgs"]);
                    Assert.Equal("6/9/2007", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("14:18:24"), item["Time of Birth"]);
                    else Assert.Equal("2:18:24 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("479-740-7633", item["PhoneNo"]);
                    Assert.Equal("Wright", item["PlaceName"]);
                    break;
                case 99:
                    Assert.Equal("Andrew Young", item["FathersName"]);
                    if (extension != "csv") Assert.Equal(38.38d, item["AgeInYears"]);
                    else Assert.Equal("38.38", item["AgeInYears"]);
                    if (extension != "csv") Assert.Equal(55.0, item["WeightInKgs"]);
                    else Assert.Equal("55", item["WeightInKgs"]);
                    Assert.Equal("4/25/2015", item["Date of Joining"]);
                    if (extension != "csv") Assert.Equal(TimeSpan.Parse("22:26:52"), item["Time of Birth"]);
                    else Assert.Equal("10:26:52 PM", item["Time of Birth"]);
                    Assert.Equal("Q2", item["QuarterOfJoining"]);
                    Assert.Equal("H1", item["HalfOfJoining"]);
                    Assert.False(item.TryGetValue("ShortMonth", out _));
                    Assert.Equal("231-290-3075", item["PhoneNo"]);
                    Assert.Equal("Alma", item["PlaceName"]);
                    break;
            }
        }

        Assert.True(rowIndex > -1, $"Query查询迭代循环并没有进行, rowIndex: {rowIndex}");
    }
}