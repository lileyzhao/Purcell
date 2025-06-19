using BenchmarkDotNet.Attributes;
using LargeXlsx;
using MiniExcelLibs;
using SharpCompress.Compressors.Deflate;

namespace PurcellLibs.Benchmarks;

[Config(typeof(CustomConfig))]
public class BenchmarkExport
{
    // 测试库
    [Params("Purcell", "MiniExcel", "LargeXlsx")] public string? Lib { get; set; }

    // 测试文件扩展名
    [Params("xlsx")] public string? Ext { get; set; }

    [Benchmark(Description = $"*{nameof(ExportDict_LargeXlsx)}*", Baseline = true)]
    public void ExportDict_LargeXlsx()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);

        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var xlsxWriter = new XlsxWriter(stream, CompressionLevel.BestSpeed, false, false);

        xlsxWriter.BeginWorksheet("Sheet1").BeginRow();

        int rowIndex = 0;
        foreach (var dict in _dictData)
        {
            if (rowIndex == 0)
            {
                xlsxWriter.BeginRow();
                foreach ((string key, object? _) in dict)
                {
                    xlsxWriter.Write(key);
                }
            }

            xlsxWriter.BeginRow();
            foreach (var kvp in dict)
            {
                switch (kvp.Value)
                {
                    case bool boolValue:
                        xlsxWriter.Write(boolValue);
                        break;
                    case DateTime dateTime:
                        xlsxWriter.Write(dateTime);
                        break;
                    case decimal dec:
                        xlsxWriter.Write(dec);
                        break;
                    case double dbl:
                        xlsxWriter.Write(dbl);
                        break;
                    case int intValue:
                        xlsxWriter.Write(intValue);
                        break;
                    case long longValue:
                        xlsxWriter.Write(longValue.ToString());
                        break;
                    case string str:
                        xlsxWriter.Write(str);
                        break;
                    default:
                        xlsxWriter.Write(string.Empty);
                        break;
                }
            }

            rowIndex++;
        }
    }

    [Benchmark(Description = nameof(ExportDict_Purcell))]
    public void ExportDict_Purcell()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        Purcell.Export(_dictData, filePath);
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    [Benchmark(Description = nameof(ExportDict_MiniExcel))]
    public void ExportDict_MiniExcel()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        MiniExcel.SaveAs(filePath, _dictData);
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    [Benchmark(Description = nameof(ExportGeneric_Purcell))]
    public void ExportGeneric_Purcell()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        Purcell.Export(_genericData, filePath);
        if (File.Exists(filePath)) File.Delete(filePath);
    }
    
    [Benchmark(Description = nameof(ExportGeneric_MiniExcel))]
    public void ExportGeneric_MiniExcel()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        MiniExcel.SaveAs(filePath, _genericData);
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    //[Benchmark(Description = "ExportAnonymousObject")]
    public void ExportAnonymousObject()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        if (Lib == "Purcell")
        {
            Purcell.Export(_anonObjectData, filePath);
        }
        else
        {
            MiniExcel.SaveAs(filePath, _anonObjectData);
        }

        if (File.Exists(filePath)) File.Delete(filePath);
    }

    //[Benchmark(Description = "ExportAnonymousDynamic")]
    public void ExportAnonymousDynamic()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        var anonymousList = _genericData.Select(item => new
        {
            item.Id, // 编号
            item.Name,
            item.Age, // 年龄 (18-60岁)
            item.Gender, // 性别
            item.BirthDate, // 出生日期 (1960-2000年)
            item.Salary, // 工资 (5000-50000)
            item.PerformanceScore, // 绩效评分 (1-5分)
            item.AttendanceRate, // 出勤率 (0.7-1.0)
            item.Profile, // 个人简介
            item.EntryDate // 入职时间 (2010-2023年)
        });
        if (Lib == "Purcell")
        {
            Purcell.Export(PurTable.From(anonymousList), filePath);
        }
        else
        {
            MiniExcel.SaveAs(filePath, anonymousList);
        }

        if (File.Exists(filePath)) File.Delete(filePath);
    }
    
    private List<SampleData.Employee> _genericData = [];
    private List<Dictionary<string, object?>> _dictData = [];
    private List<object> _anonObjectData = [];

    [GlobalSetup]
    public void Setup()
    {
        _genericData = SampleData.GetGenericData(100_0000).ToList();

        _dictData = _genericData.Select(item => new Dictionary<string, object?>
        {
            ["Id"] = item.Id, // 编号
            ["Name"] = item.Name,
            ["Age"] = item.Age, // 年龄 (18-60岁)
            ["Gender"] = item.Gender, // 性别
            ["BirthDate"] = item.BirthDate, // 出生日期 (1960-2000年)
            ["Salary"] = item.Salary, // 工资 (5000-50000)
            ["PerformanceScore"] = item.PerformanceScore, // 绩效评分 (1-5分)
            ["AttendanceRate"] = item.AttendanceRate, // 出勤率 (0.7-1.0)
            ["Profile"] = item.Profile, // 个人简介
            ["EntryDate"] = item.EntryDate // 入职时间 (2010-2023年)
        }).ToList();

        _anonObjectData = _genericData.Select(object (item) => new
        {
            item.Id, // 编号
            item.Name,
            item.Age, // 年龄 (18-60岁)
            item.Gender, // 性别
            item.BirthDate, // 出生日期 (1960-2000年)
            item.Salary, // 工资 (5000-50000)
            item.PerformanceScore, // 绩效评分 (1-5分)
            item.AttendanceRate, // 出勤率 (0.7-1.0)
            item.Profile, // 个人简介
            item.EntryDate // 入职时间 (2010-2023年)
        }).ToList();
    }
}