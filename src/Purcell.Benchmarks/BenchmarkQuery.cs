using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CsvHelper;
using CsvHelper.Configuration;
using MiniExcelLibs;
using Sylvan.Data.Excel;

// using Purcell.Providers.Abstractions;
// using Purcell.Providers.Csv;
// using Purcell.Providers.Excel;

namespace PurcellLibs.Benchmarks;

public class BenchmarkQuery
{
    //[Benchmark(Description = $"*{nameof(QueryDict_SylvanNative_Xlsx)}*", Baseline = true)]
    public void QueryDict_SylvanNative_Xlsx()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "xlsx");

        using FileStream fileStream = File.OpenRead(filePath);

        using ExcelDataReader reader = ExcelDataReader.Create(
            fileStream, ExcelWorkbookType.ExcelXml,
            new ExcelDataReaderOptions
            {
                Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true, OwnsStream = false
            }
        );

        int columnLength = 0;
        Consumer consumer = new();
        while (reader.Read())
        {
            if (columnLength == 0) columnLength = reader.RowFieldCount;

            Dictionary<int, object?> item = new();
            for (int colIndex = 0; colIndex < columnLength; colIndex++)
            {
                item[colIndex] = reader.GetExcelValue(colIndex);
            }

            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    //[Benchmark(Description = nameof(QueryDict_Purcell_Xlsx))]
    public void QueryDict_Purcell_Xlsx()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "xlsx");
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    //[Benchmark(Description = nameof(QueryDict_PurcellProviders_xlsx))]
    public void QueryDict_PurcellProviders_xlsx()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "xlsx");

        // using FileStream fileStream = File.OpenRead(filePath);
        // using var reader = new SylvanExcelTableReader(fileStream, TableFileType.Xlsx);
        //
        // Consumer consumer = new();
        // foreach (IDictionary<int, object?> item in reader.ReadRows(
        //              0, string.Empty,
        //              true, (0, 0),
        //              (1, 0), CultureInfo.InvariantCulture))
        // {
        //     consumer.Consume(item); // 防止被编译器优化掉
        // }
    }

    [Benchmark(Description = nameof(QueryDict_CsvHelperNative_Csv), Baseline = true)]
    public void QueryDict_CsvHelperNative_Csv()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "csv");

        using FileStream fileStream = File.OpenRead(filePath);

        // 创建 CsvReader 配置(无头读取)
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ",",
            Escape = '"'
        };

        // 创建 CsvReader 读取器
        using StreamReader streamReader = new(fileStream, Encoding.UTF8);
        using CsvReader reader = new(streamReader, config);

        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in reader.GetRecords<dynamic>())
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    [Benchmark(Description = nameof(QueryDict_PurcellProviders_Csv))]
    public void QueryDict_PurcellProviders_Csv()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "csv");

        using FileStream fileStream = File.OpenRead(filePath);
        using var reader = new PurcellLibs.Providers.Csv.CsvHelperTableReader(fileStream);

        Consumer consumer = new();
        foreach (var item in reader.ReadTable(PurTable.New()))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    //[Benchmark(Description = nameof(QueryDict_Purcell_Csv))]
    public void QueryDict_Purcell_Csv()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "csv");

        using FileStream fileStream = File.OpenRead(filePath);
        Consumer consumer = new();
        foreach (var item in Purcell.QueryCsv(fileStream))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    // [Benchmark(Description = nameof(QueryDict_MiniExcel))]
    public void QueryDict_MiniExcel()
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", "xlsx");
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in MiniExcel.Query(filePath))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        string[] extArray = ["xlsx", "csv"];
        foreach (string ext in extArray)
        {
            string filePath = FileHelper.GetDesktopFilePath("100_0000x10", ext);
            if (File.Exists(filePath)) continue;
            Purcell.Export(SampleData.GetGenericData(100_0000), filePath);
        }
    }
}