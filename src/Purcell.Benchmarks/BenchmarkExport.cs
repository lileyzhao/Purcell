using BenchmarkDotNet.Attributes;

namespace PurcellLibs.Benchmarks;

[Config(typeof(CustomConfig))]
public class BenchmarkExport
{
    // 测试文件扩展名
    [Params("xlsx")]
    public string? Ext { get; set; }

    [Benchmark(Description = "*ExportDict*", Baseline = true)]
    public void ExportDict()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        Purcell.Export(_dictData, filePath);
        File.Delete(filePath);
    }

    [Benchmark(Description = "ExportGeneric")]
    public void ExportGeneric()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        Purcell.Export(_genericData, filePath);
        File.Delete(filePath);
    }

    [Benchmark(Description = "ExportAnonymousObject")]
    public void ExportAnonymousObject()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        Purcell.Export(_anonymousData, filePath);
        File.Delete(filePath);
    }

    //[Benchmark(Description = "ExportAnonymous")]
    public void ExportAnonymous()
    {
        string filePath = FileHelper.GenExportFilePath(Ext);
        var anonymousList = _genericData.Select(item => new
        {
            Id = item.Id, // 编号
            Name = item.Name,
            Age = item.Age, // 年龄 (18-60岁)
            Gender = item.Gender, // 性别
            BirthDate = item.BirthDate, // 出生日期 (1960-2000年)
            Salary = item.Salary, // 工资 (5000-50000)
            PerformanceScore = item.PerformanceScore, // 绩效评分 (1-5分)
            AttendanceRate = item.AttendanceRate, // 出勤率 (0.7-1.0)
            Profile = item.Profile, // 个人简介
            EntryDate = item.EntryDate // 入职时间 (2010-2023年)
        });
        Purcell.Export(PurTable.From(anonymousList), filePath);
        File.Delete(filePath);
    }

    private IEnumerable<SampleData.Employee> _genericData = [];
    private List<Dictionary<string, object?>> _dictData = [];
    private List<object> _anonymousData = [];

    [GlobalSetup]
    public void Setup()
    {
        _genericData = SampleData.GetGenericData(100_0000);

        List<Dictionary<string, object?>> dictList = new();
        foreach (SampleData.Employee item in _genericData)
        {
            dictList.Add(new Dictionary<string, object?>
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
            });
        }

        _dictData = dictList;

        List<dynamic> anonymousList = new();
        foreach (SampleData.Employee item in _genericData)
        {
            anonymousList.Add(new
            {
                Id = item.Id, // 编号
                Name = item.Name,
                Age = item.Age, // 年龄 (18-60岁)
                Gender = item.Gender, // 性别
                BirthDate = item.BirthDate, // 出生日期 (1960-2000年)
                Salary = item.Salary, // 工资 (5000-50000)
                PerformanceScore = item.PerformanceScore, // 绩效评分 (1-5分)
                AttendanceRate = item.AttendanceRate, // 出勤率 (0.7-1.0)
                Profile = item.Profile, // 个人简介
                EntryDate = item.EntryDate // 入职时间 (2010-2023年)
            });
        }

        _anonymousData = anonymousList;
    }
}