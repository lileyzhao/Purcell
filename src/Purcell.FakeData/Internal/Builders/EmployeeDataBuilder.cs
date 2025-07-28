using AutoFixture;
using Bogus;
using PurcellLibs.FakeData.Internal.Data;

namespace PurcellLibs.FakeData.Internal.Builders;

/// <summary>
/// 员工数据建造者，内部实现流畅API
/// </summary>
internal class EmployeeDataBuilder : IEmployeeDataGenerator
{
    private int _count = 1;
    private int _minAge = 22;
    private int _maxAge = 65;
    private decimal _minSalary = 3000;
    private decimal _maxSalary = 50000;
    private double _minHeight = 150;
    private double _maxHeight = 200;
    private uint _minWorkYears = 0;
    private uint _maxWorkYears = 40;
    private bool _useBogus = true;
    private bool _includeBoundaryData = false;
    private bool _includeSpecialCharacters = false;
    private long _startId = 100000;

    private readonly IFixture _autoFixture;
    private readonly Faker<EmployeeData> _faker;

    internal EmployeeDataBuilder()
    {
        // 配置 AutoFixture
        _autoFixture = new Fixture();
        ConfigureAutoFixture();

        // 配置 Bogus Faker
        _faker = new Faker<EmployeeData>("zh_CN")
            .RuleFor(e => e.EmployeeId, f => f.Random.Long(100000000, 999999999))
            .RuleFor(e => e.EmployeeName, _ => ChineseNames.GenerateChineseName())
            .RuleFor(e => e.Age, f => f.Random.Int(_minAge, _maxAge))
            .RuleFor(e => e.Salary, f => f.Random.Decimal(_minSalary, _maxSalary))
            .RuleFor(e => e.Height, f => f.Random.Double(_minHeight, _maxHeight))
            .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
            .RuleFor(e => e.BirthDate, f => f.Date.Between(DateTime.Now.AddYears(-65), DateTime.Now.AddYears(-22)))
            .RuleFor(e => e.HireTime, f => TimeOnly.FromDateTime(f.Date.Between(
                DateTime.Today.AddHours(8), DateTime.Today.AddHours(18))))
            .RuleFor(e => e.ContractEndDate, f => DateOnly.FromDateTime(f.Date.Future(5, DateTime.Now)))
            .RuleFor(e => e.IsActive, f => f.Random.Bool(0.85f))
            .RuleFor(e => e.EmployeeNumber, f => f.Random.Short(1000, 9999))
            .RuleFor(e => e.PerformanceScore, f => f.Random.Float(1.0f, 5.0f))
            .RuleFor(e => e.WorkYears, f => f.Random.UInt(_minWorkYears, _maxWorkYears))
            .RuleFor(e => e.IdCardNumber, f => f.Random.ULong(100000000000000000, 999999999999999999))
            .RuleFor(e => e.DepartmentCode, f => f.Random.Byte(1, 20))
            .RuleFor(e => e.JobLevel, f => f.Random.SByte(1, 10))
            .RuleFor(e => e.MaritalStatus, f => f.PickRandom('S', 'M', 'D', 'W')) // Single, Married, Divorced, Widowed
            .RuleFor(e => e.MonthlyWorkHours, f => f.Random.UShort(120, 200))
            .RuleFor(e => e.EmployeeGuid, f => f.Random.Guid())
            .RuleFor(e => e.Remarks, f => f.Random.Bool(0.7f) ? f.Lorem.Sentence(5, 15) : null);
    }

    /// <summary>
    /// 设置生成数量
    /// </summary>
    public IEmployeeDataGenerator WithCount(int count)
    {
        _count = Math.Max(1, count);
        return this;
    }

    /// <summary>
    /// 设置年龄范围
    /// </summary>
    public IEmployeeDataGenerator WithAgeRange(int minAge, int maxAge)
    {
        _minAge = minAge;
        _maxAge = maxAge;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置工资范围
    /// </summary>
    public IEmployeeDataGenerator WithSalaryRange(decimal minSalary, decimal maxSalary)
    {
        _minSalary = minSalary;
        _maxSalary = maxSalary;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置身高范围
    /// </summary>
    public IEmployeeDataGenerator WithHeightRange(double minHeight, double maxHeight)
    {
        _minHeight = minHeight;
        _maxHeight = maxHeight;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置工作年限范围
    /// </summary>
    public IEmployeeDataGenerator WithWorkYearsRange(uint minWorkYears, uint maxWorkYears)
    {
        _minWorkYears = minWorkYears;
        _maxWorkYears = maxWorkYears;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置起始ID
    /// </summary>
    public IEmployeeDataGenerator WithStartId(long startId)
    {
        _startId = startId;
        return this;
    }

    /// <summary>
    /// 使用 Bogus 生成数据
    /// </summary>
    public IEmployeeDataGenerator UseBogus()
    {
        _useBogus = true;
        return this;
    }

    /// <summary>
    /// 使用 AutoFixture 生成数据
    /// </summary>
    public IEmployeeDataGenerator UseAutoFixture()
    {
        _useBogus = false;
        return this;
    }

    /// <summary>
    /// 包含边界数据
    /// </summary>
    public IEmployeeDataGenerator WithBoundaryData()
    {
        _includeBoundaryData = true;
        return this;
    }

    /// <summary>
    /// 包含特殊字符数据
    /// </summary>
    public IEmployeeDataGenerator WithSpecialCharacters()
    {
        _includeSpecialCharacters = true;
        return this;
    }

    /// <summary>
    /// 生成单个员工数据
    /// </summary>
    public EmployeeData AsOne()
    {
        return _useBogus ? GenerateWithBogus() : GenerateWithAutoFixture();
    }

    /// <summary>
    /// 生成员工数据列表
    /// </summary>
    public List<EmployeeData> AsList()
    {
        var result = new List<EmployeeData>();

        // 添加边界数据
        if (_includeBoundaryData)
        {
            result.AddRange(GenerateBoundaryData());
        }

        // 添加特殊字符数据
        if (_includeSpecialCharacters)
        {
            result.AddRange(GenerateSpecialCharacterData());
        }

        // 添加正常数据
        var normalCount = _count - result.Count;
        if (normalCount > 0)
        {
            var normalData = _useBogus 
                ? _faker.Generate(normalCount) 
                : GenerateListWithAutoFixture(normalCount);
            
            // 确保ID唯一
            for (int i = 0; i < normalData.Count; i++)
            {
                normalData[i].EmployeeId = _startId + result.Count + i;
            }
            
            result.AddRange(normalData);
        }

        return result;
    }

    /// <summary>
    /// 生成字典格式数据
    /// </summary>
    public List<Dictionary<string, object?>> AsDictionary()
    {
        var employees = AsList();
        return employees.Select(employee => new Dictionary<string, object?>
        {
            ["EmployeeId"] = employee.EmployeeId,
            ["EmployeeName"] = employee.EmployeeName,
            ["Age"] = employee.Age,
            ["Salary"] = employee.Salary,
            ["Height"] = employee.Height,
            ["Gender"] = employee.Gender.ToString(),
            ["BirthDate"] = employee.BirthDate,
            ["HireTime"] = employee.HireTime.ToString(),
            ["ContractEndDate"] = employee.ContractEndDate.ToString(),
            ["IsActive"] = employee.IsActive,
            ["EmployeeNumber"] = employee.EmployeeNumber,
            ["PerformanceScore"] = employee.PerformanceScore,
            ["WorkYears"] = employee.WorkYears,
            ["IdCardNumber"] = employee.IdCardNumber,
            ["DepartmentCode"] = employee.DepartmentCode,
            ["JobLevel"] = employee.JobLevel,
            ["MaritalStatus"] = employee.MaritalStatus,
            ["MonthlyWorkHours"] = employee.MonthlyWorkHours,
            ["EmployeeGuid"] = employee.EmployeeGuid,
            ["Remarks"] = employee.Remarks
        }).ToList();
    }

    /// <summary>
    /// 配置 AutoFixture
    /// </summary>
    private void ConfigureAutoFixture()
    {
        _autoFixture.Customize<DateTime>(composer => composer.FromFactory(() =>
            DateTime.Now.AddYears(-new Random().Next(22, 65))
                .AddDays(new Random().Next(-365, 365))
                .AddHours(new Random().Next(0, 24))
                .AddMinutes(new Random().Next(0, 60))));

        _autoFixture.Customize<TimeOnly>(composer => composer.FromFactory(() =>
            new TimeOnly(new Random().Next(8, 18), new Random().Next(0, 60))));

        _autoFixture.Customize<DateOnly>(composer => composer.FromFactory(() =>
            DateOnly.FromDateTime(DateTime.Now.AddYears(new Random().Next(1, 6)))));

        _autoFixture.Customize<string>(composer => composer.FromFactory(() =>
            ChineseNames.GenerateChineseName()));

        _autoFixture.Customize<decimal>(composer => composer.FromFactory(() =>
            (decimal)(new Random().NextDouble() * 47000 + 3000)));

        _autoFixture.Customize<double>(composer => composer.FromFactory(() =>
            new Random().NextDouble() * 50 + 150));

        _autoFixture.Customize<long>(composer => composer.FromFactory(() =>
            new Random().NextInt64(100000000, 999999999)));

        _autoFixture.Customize<int>(composer => composer.FromFactory(() =>
            new Random().Next(22, 65)));

        _autoFixture.Customize<char>(composer => composer.FromFactory(() =>
            new[] { 'S', 'M', 'D', 'W' }[new Random().Next(4)]));

        _autoFixture.Customize<float>(composer => composer.FromFactory(() =>
            (float)(new Random().NextDouble() * 4 + 1)));

        _autoFixture.Customize<uint>(composer => composer.FromFactory(() =>
            (uint)new Random().Next(0, 40)));
    }

    /// <summary>
    /// 更新 Faker 规则
    /// </summary>
    private void UpdateFakerRules()
    {
        _faker.RuleFor(e => e.Age, f => f.Random.Int(_minAge, _maxAge))
              .RuleFor(e => e.Salary, f => f.Random.Decimal(_minSalary, _maxSalary))
              .RuleFor(e => e.Height, f => f.Random.Double(_minHeight, _maxHeight))
              .RuleFor(e => e.WorkYears, f => f.Random.UInt(_minWorkYears, _maxWorkYears));
    }

    /// <summary>
    /// 使用 Bogus 生成单个数据
    /// </summary>
    private EmployeeData GenerateWithBogus()
    {
        return _faker.Generate();
    }

    /// <summary>
    /// 使用 AutoFixture 生成单个数据
    /// </summary>
    private EmployeeData GenerateWithAutoFixture()
    {
        var employee = _autoFixture.Create<EmployeeData>();
        employee.EmployeeName = ChineseNames.GenerateChineseName();
        employee.Salary = Math.Round(employee.Salary, 2);
        employee.Height = Math.Round(employee.Height, 1);
        employee.PerformanceScore = (float)Math.Round(employee.PerformanceScore, 2);
        return employee;
    }

    /// <summary>
    /// 使用 AutoFixture 生成列表数据
    /// </summary>
    private List<EmployeeData> GenerateListWithAutoFixture(int count)
    {
        return _autoFixture.CreateMany<EmployeeData>(count)
            .Select(employee =>
            {
                employee.EmployeeName = ChineseNames.GenerateChineseName();
                employee.Salary = Math.Round(employee.Salary, 2);
                employee.Height = Math.Round(employee.Height, 1);
                employee.PerformanceScore = (float)Math.Round(employee.PerformanceScore, 2);
                return employee;
            })
            .ToList();
    }

    /// <summary>
    /// 生成边界数据
    /// </summary>
    private List<EmployeeData> GenerateBoundaryData()
    {
        return new List<EmployeeData>
        {
            // 最小值
            new EmployeeData
            {
                EmployeeId = long.MinValue,
                EmployeeName = "",
                Age = int.MinValue,
                Salary = decimal.MinValue,
                Height = double.MinValue,
                Gender = Gender.Unknown,
                BirthDate = DateTime.MinValue,
                HireTime = TimeOnly.MinValue,
                ContractEndDate = DateOnly.MinValue,
                IsActive = false,
                EmployeeNumber = short.MinValue,
                PerformanceScore = float.MinValue,
                WorkYears = uint.MinValue,
                IdCardNumber = ulong.MinValue,
                DepartmentCode = byte.MinValue,
                JobLevel = sbyte.MinValue,
                MaritalStatus = char.MinValue,
                MonthlyWorkHours = ushort.MinValue,
                EmployeeGuid = Guid.Empty,
                Remarks = null
            },
            // 最大值
            new EmployeeData
            {
                EmployeeId = long.MaxValue,
                EmployeeName = new string('测', 255),
                Age = int.MaxValue,
                Salary = decimal.MaxValue,
                Height = double.MaxValue,
                Gender = Gender.Female,
                BirthDate = DateTime.MaxValue,
                HireTime = TimeOnly.MaxValue,
                ContractEndDate = DateOnly.MaxValue,
                IsActive = true,
                EmployeeNumber = short.MaxValue,
                PerformanceScore = float.MaxValue,
                WorkYears = uint.MaxValue,
                IdCardNumber = ulong.MaxValue,
                DepartmentCode = byte.MaxValue,
                JobLevel = sbyte.MaxValue,
                MaritalStatus = char.MaxValue,
                MonthlyWorkHours = ushort.MaxValue,
                EmployeeGuid = Guid.NewGuid(),
                Remarks = new string('备', 1000)
            }
        };
    }

    /// <summary>
    /// 生成特殊字符数据
    /// </summary>
    private List<EmployeeData> GenerateSpecialCharacterData()
    {
        var specialNames = new[]
        {
            "特殊字符\\\"测试'",
            "包含，逗号；分号。句号",
            "英文Special@#$%^&*()",
            "数字123456789",
            "混合Special测试123"
        };

        return specialNames.Select((name, index) => new EmployeeData
        {
            EmployeeId = 800000 + index,
            EmployeeName = name,
            Age = 25 + index,
            Salary = 8000.5m + index * 1000,
            Height = 170.0 + index,
            Gender = (Gender)(index % 3),
            BirthDate = DateTime.Now.AddYears(-(25 + index)),
            HireTime = new TimeOnly(9 + (index % 8), index % 60),
            ContractEndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(3 + index)),
            IsActive = index % 2 == 0,
            EmployeeNumber = (short)(2000 + index),
            PerformanceScore = 3.5f + index * 0.3f,
            WorkYears = (uint)(2 + index),
            IdCardNumber = (ulong)(110000000000000000 + index * 100000000000000),
            DepartmentCode = (byte)(10 + index),
            JobLevel = (sbyte)(3 + index),
            MaritalStatus = index % 2 == 0 ? 'M' : 'S',
            MonthlyWorkHours = (ushort)(160 + index * 10),
            EmployeeGuid = Guid.NewGuid(),
            Remarks = $"特殊字符备注{index}：包含\\t制表符和\\n换行符"
        }).ToList();
    }
}