using Newtonsoft.Json;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Export_Anonymous(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx")]
    [InlineData("csv")]
    public void ExportAnonymous_FilePath(string extension)
    {
        string filePath = FileHelper.GenExportFilePath(extension);

        // var dataDynamic = MockData.GetAnonymousDynamicData();
        var dataObject = MockData.GetAnonymousObjectData();
        var dataAnonymous = MockData.GetAnonymousObjectData();

        List<PurTable> tableConfigs =
        [
            new PurTable { TableStyle = PurStyle.MidnightMagic }.WithRecords(dataAnonymous),
            PurTable.From(dataObject).WithTableStyle(PurStyle.EarthTones).WithHeaderStart("B2"),
            PurTable.From(dataObject).WithTableStyle(PurStyle.SunnyDay)
                .WithHeaderStart(CellLocator.Create(3, 3)),
            PurTable.From(dataObject).WithTableStyle(PurStyle.CozyAutumn)
                .WithHeaderStart(CellLocator.Create(5, 2))
        ];
        Purcell.Export(tableConfigs, filePath);
        filePath.Export(tableConfigs); // 覆盖写入文件测试

        int rowIndex = -1;
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            if (item.TryGetValue("Id", out object? id) && id != null && !string.IsNullOrEmpty(id.ToString()))
            {
                if (rowIndex + 1 != 2 && rowIndex + 1 != 3)
                {
                    Assert.Equal(rowIndex + 1, Convert.ToInt32(id));
                }
            }

            if (item.TryGetValue("Gender", out object? gender) && gender != null && !string.IsNullOrEmpty(gender.ToString()))
                Assert.True(gender.ToString() == "男" || gender.ToString() == "女");
            if (item.TryGetValue("BirthDate", out object? birth) && birth != null && !string.IsNullOrEmpty(birth.ToString()))
            {
                if (extension == "csv") Assert.True(DateTime.TryParse(birth.ToString(), out DateTime _));
                else Assert.True(birth is DateTime);
            }
        }

        Assert.True(rowIndex > -1, $"导出的文件Query迭代循环并没有进行, rowIndex: {rowIndex}");
        File.Delete(filePath);
    }

    [Theory]
    [InlineData("xlsx")]
    [InlineData("csv")]
    public async Task ExportAnonymousAsync_FilePath(string extension)
    {
        string filePath = FileHelper.GenExportFilePath(extension);

        object?[] data = MockData.GetAnonymousObjectData();

        List<PurTable> tableConfigs =
        [
            PurTable.From(data).WithTableStyle(PurStyle.MidnightMagic),
            PurTable.From(data).WithTableStyle(PurStyle.EarthTones).WithHeaderStart("B2"),
            PurTable.From(data).WithTableStyle(PurStyle.SunnyDay).WithHeaderStart(CellLocator.Create(3, 3)),
            PurTable.From(data).WithTableStyle(PurStyle.CozyAutumn).WithHeaderStart(CellLocator.Create(5, 2))
        ];
        await Purcell.ExportAsync(tableConfigs, filePath);
        await filePath.ExportAsync(tableConfigs); // 覆盖写入文件测试

        int rowIndex = -1;
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            if (item.TryGetValue("Id", out object? id) && id != null && !string.IsNullOrEmpty(id.ToString()))
            {
                if (rowIndex + 1 != 2 && rowIndex + 1 != 3)
                {
                    Assert.Equal(rowIndex + 1, Convert.ToInt32(id));
                }
            }

            if (item.TryGetValue("Gender", out object? gender) && gender != null && !string.IsNullOrEmpty(gender.ToString()))
                Assert.True(gender.ToString() == "男" || gender.ToString() == "女");
            if (item.TryGetValue("BirthDate", out object? birth) && birth != null && !string.IsNullOrEmpty(birth.ToString()))
            {
                if (extension == "csv") Assert.True(DateTime.TryParse(birth.ToString(), out DateTime _));
                else Assert.True(birth is DateTime);
            }
        }

        Assert.True(rowIndex > -1, $"导出的文件Query迭代循环并没有进行, rowIndex: {rowIndex}");
        File.Delete(filePath);
    }

    [Theory]
    [InlineData("xlsx", ExportType.Xlsx)]
    [InlineData("csv", ExportType.Csv)]
    public void ExportAnonymous_FileStream(string extension, ExportType exportType)
    {
        string filePath = FileHelper.GenExportFilePath(extension);

        object?[] data = MockData.GetAnonymousObjectData();

        List<PurTable> tableConfigs =
        [
            PurTable.From(data).WithTableStyle(PurStyle.MidnightMagic),
            PurTable.From(data).WithTableStyle(PurStyle.EarthTones).WithHeaderStart("B2"),
            PurTable.From(data).WithTableStyle(PurStyle.SunnyDay).WithHeaderStart(CellLocator.Create(3, 3)),
            PurTable.From(data).WithTableStyle(PurStyle.CozyAutumn).WithHeaderStart(CellLocator.Create(5, 2))
        ];
        using (FileStream stream = new(filePath, FileMode.CreateNew))
        {
            Purcell.Export(tableConfigs, stream, exportType);
        }

        // 覆盖写入文件测试
        using (FileStream stream = new(filePath, FileMode.Create))
        {
            stream.Export(tableConfigs, exportType);
        }

        // 覆盖并指定类型写入
        using (FileStream stream = new(filePath, FileMode.Create))
        {
            switch (exportType)
            {
                case ExportType.Xlsx:
                    stream.ExportXlsx(tableConfigs);
                    break;
                case ExportType.Csv:
                    stream.ExportCsv(tableConfigs[0]);
                    break;
            }
        }

        int rowIndex = -1;
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            if (item.TryGetValue("Id", out object? id) && id != null && !string.IsNullOrEmpty(id.ToString()))
            {
                if (rowIndex + 1 != 2 && rowIndex + 1 != 3)
                {
                    Assert.Equal(rowIndex + 1, Convert.ToInt32(id));
                }
            }

            if (item.TryGetValue("Gender", out object? gender) && gender != null && !string.IsNullOrEmpty(gender.ToString()))
                Assert.True(gender.ToString() == "男" || gender.ToString() == "女");
            if (item.TryGetValue("BirthDate", out object? birth) && birth != null && !string.IsNullOrEmpty(birth.ToString()))
            {
                if (extension == "csv") Assert.True(DateTime.TryParse(birth.ToString(), out DateTime _));
                else Assert.True(birth is DateTime);
            }
        }

        Assert.True(rowIndex > -1, $"导出的文件Query迭代循环并没有进行, rowIndex: {rowIndex}");
        File.Delete(filePath);
    }

    [Theory]
    [InlineData("xlsx", ExportType.Xlsx)]
    [InlineData("csv", ExportType.Csv)]
    public async Task ExportAnonymousAsync_FileStream(string extension, ExportType exportType)
    {
        string filePath = FileHelper.GenExportFilePath(extension);

        object?[] data = MockData.GetAnonymousObjectData();

        List<PurTable> tableConfigs =
        [
            PurTable.From(data).WithTableStyle(PurStyle.MidnightMagic),
            PurTable.From(data).WithTableStyle(PurStyle.EarthTones).WithHeaderStart("B2"),
            PurTable.From(data).WithTableStyle(PurStyle.SunnyDay).WithHeaderStart(CellLocator.Create(3, 3)),
            PurTable.From(data).WithTableStyle(PurStyle.CozyAutumn).WithHeaderStart(CellLocator.Create(5, 2))
        ];
        await using (FileStream stream = new(filePath, FileMode.CreateNew))
        {
            await Purcell.ExportAsync(tableConfigs, stream, exportType);
        }

        // 覆盖写入文件测试
        await using (FileStream stream = new(filePath, FileMode.Create))
        {
            await stream.ExportAsync(tableConfigs, exportType);
        }

        // 覆盖并指定类型写入
        await using (FileStream stream = new(filePath, FileMode.Create))
        {
            switch (exportType)
            {
                case ExportType.Xlsx:
                    await stream.ExportXlsxAsync(tableConfigs);
                    break;
                case ExportType.Csv:
                    await stream.ExportCsvAsync(tableConfigs[0]);
                    break;
            }
        }

        int rowIndex = -1;
        foreach (IDictionary<string, object?> item in await Purcell.QueryAsync(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));

            if (item.TryGetValue("Id", out object? id) && id != null && !string.IsNullOrEmpty(id.ToString()))
            {
                if (rowIndex + 1 != 2 && rowIndex + 1 != 3)
                {
                    Assert.Equal(rowIndex + 1, Convert.ToInt32(id));
                }
            }

            if (item.TryGetValue("Gender", out object? gender) && gender != null && !string.IsNullOrEmpty(gender.ToString()))
                Assert.True(gender.ToString() == "男" || gender.ToString() == "女");
            if (item.TryGetValue("BirthDate", out object? birth) && birth != null && !string.IsNullOrEmpty(birth.ToString()))
            {
                if (extension == "csv") Assert.True(DateTime.TryParse(birth.ToString(), out DateTime _));
                else Assert.True(birth is DateTime);
            }
        }

        Assert.True(rowIndex > -1, $"导出的文件Query迭代循环并没有进行, rowIndex: {rowIndex}");
        File.Delete(filePath);
    }
}