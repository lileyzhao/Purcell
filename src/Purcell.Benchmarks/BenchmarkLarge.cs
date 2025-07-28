using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using MiniExcelLibs;

namespace PurcellLibs.Benchmarks;

public class BenchmarkLarge
{
    [Benchmark(Description = "*Purcell*", Baseline = true)]
    public void TestPurcell()
    {
        if (Operation == "First")
        {
            string filePath = FileHelper.GetDesktopFilePath("100_0000x10", Ext);
            Consumer consumer = new();
            foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
            {
                consumer.Consume(item); // 防止被编译器优化掉
                break;
            }
        }
        else if (Operation == "Query")
        {
            string filePath = FileHelper.GetDesktopFilePath("100_0000x10", Ext);
            Consumer consumer = new();
            foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
            {
                consumer.Consume(item); // 防止被编译器优化掉
            }
        }
        else
        {
            string filePath = FileHelper.GenExportFilePath(Ext);
            Purcell.Export(SampleData.GetGenericData(100_0000), filePath);
            File.Delete(filePath);
        }
    }

    [Benchmark(Description = "MiniExcel")]
    public void TestMiniExcel()
    {
        if (Operation == "First")
        {
            string filePath = FileHelper.GetDesktopFilePath("100_0000x10", Ext);
            Consumer consumer = new();
            foreach (IDictionary<string, object?> item in MiniExcel.Query(filePath, true))
            {
                consumer.Consume(item); // 防止被编译器优化掉
                break;
            }
        }
        else if (Operation == "Query")
        {
            string filePath = FileHelper.GetDesktopFilePath("100_0000x10", Ext);
            Consumer consumer = new();
            foreach (IDictionary<string, object?> item in MiniExcel.Query(filePath))
            {
                consumer.Consume(item); // 防止被编译器优化掉
            }
        }
        else
        {
            string filePath = FileHelper.GenExportFilePath(Ext);
            MiniExcel.SaveAs(filePath, SampleData.GetGenericData(100_0000));
            File.Delete(filePath);
        }
    }

    // 测试操作类型
    [Params("First", "Query", "Export")] public string? Operation { get; set; }

    // 测试文件扩展名
    [Params("xlsx", "csv")] public string? Ext { get; set; }

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