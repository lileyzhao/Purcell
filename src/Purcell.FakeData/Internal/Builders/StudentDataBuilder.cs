using AutoFixture;
using Bogus;
using Purcell.FakeData.Internal.Data;

namespace Purcell.FakeData.Internal.Builders;

/// <summary>
/// 学生数据建造者，内部实现流畅API
/// </summary>
internal class StudentDataBuilder : IStudentDataGenerator
{
    private int _count = 1;
    private int _minAge = 16;
    private int _maxAge = 30;
    private decimal _minScore = 0;
    private decimal _maxScore = 100;
    private double _minHeight = 150;
    private double _maxHeight = 200;
    private bool _useBogus = true;
    private bool _includeBoundaryData = false;
    private bool _includeSpecialCharacters = false;
    private long _startId = 100000;

    private readonly IFixture _autoFixture;
    private readonly Faker<StudentData> _faker;

    internal StudentDataBuilder()
    {
        // 配置 AutoFixture
        _autoFixture = new Fixture();
        ConfigureAutoFixture();

        // 配置 Bogus Faker
        _faker = new Faker<StudentData>("zh_CN")
            .RuleFor(s => s.StudentId, f => f.Random.Long(100000000, 999999999))
            .RuleFor(s => s.StudentName, _ => ChineseNames.GenerateChineseName())
            .RuleFor(s => s.Age, f => f.Random.Int(_minAge, _maxAge))
            .RuleFor(s => s.Score, f => f.Random.Decimal(_minScore, _maxScore))
            .RuleFor(s => s.Height, f => f.Random.Double(_minHeight, _maxHeight))
            .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
            .RuleFor(s => s.BirthDateTime, f => f.Date.Between(DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-16)))
            .RuleFor(s => s.EnrollmentTime, f => TimeOnly.FromDateTime(f.Date.Between(
                DateTime.Today.AddHours(8), DateTime.Today.AddHours(18))))
            .RuleFor(s => s.GraduationDate, f => DateOnly.FromDateTime(f.Date.Future(4, DateTime.Now.AddYears(2))))
            .RuleFor(s => s.IsActive, f => f.Random.Bool(0.8f));
    }

    /// <summary>
    /// 设置生成数量
    /// </summary>
    public IStudentDataGenerator WithCount(int count)
    {
        _count = Math.Max(1, count);
        return this;
    }

    /// <summary>
    /// 设置年龄范围
    /// </summary>
    public IStudentDataGenerator WithAgeRange(int minAge, int maxAge)
    {
        _minAge = minAge;
        _maxAge = maxAge;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置分数范围
    /// </summary>
    public IStudentDataGenerator WithScoreRange(decimal minScore, decimal maxScore)
    {
        _minScore = minScore;
        _maxScore = maxScore;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置身高范围
    /// </summary>
    public IStudentDataGenerator WithHeightRange(double minHeight, double maxHeight)
    {
        _minHeight = minHeight;
        _maxHeight = maxHeight;
        UpdateFakerRules();
        return this;
    }

    /// <summary>
    /// 设置起始ID
    /// </summary>
    public IStudentDataGenerator WithStartId(long startId)
    {
        _startId = startId;
        return this;
    }

    /// <summary>
    /// 使用 Bogus 生成数据
    /// </summary>
    public IStudentDataGenerator UseBogus()
    {
        _useBogus = true;
        return this;
    }

    /// <summary>
    /// 使用 AutoFixture 生成数据
    /// </summary>
    public IStudentDataGenerator UseAutoFixture()
    {
        _useBogus = false;
        return this;
    }

    /// <summary>
    /// 包含边界数据
    /// </summary>
    public IStudentDataGenerator WithBoundaryData()
    {
        _includeBoundaryData = true;
        return this;
    }

    /// <summary>
    /// 包含特殊字符数据
    /// </summary>
    public IStudentDataGenerator WithSpecialCharacters()
    {
        _includeSpecialCharacters = true;
        return this;
    }

    /// <summary>
    /// 生成单个学生数据
    /// </summary>
    public StudentData AsOne()
    {
        return _useBogus ? GenerateWithBogus() : GenerateWithAutoFixture();
    }

    /// <summary>
    /// 生成学生数据列表
    /// </summary>
    public List<StudentData> AsList()
    {
        var result = new List<StudentData>();

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
                normalData[i].StudentId = _startId + result.Count + i;
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
        var students = AsList();
        return students.Select(student => new Dictionary<string, object?>
        {
            ["StudentId"] = student.StudentId,
            ["StudentName"] = student.StudentName,
            ["Age"] = student.Age,
            ["Score"] = student.Score,
            ["Height"] = student.Height,
            ["Gender"] = student.Gender.ToString(),
            ["BirthDateTime"] = student.BirthDateTime,
            ["EnrollmentTime"] = student.EnrollmentTime.ToString(),
            ["GraduationDate"] = student.GraduationDate.ToString(),
            ["IsActive"] = student.IsActive
        }).ToList();
    }

    /// <summary>
    /// 配置 AutoFixture
    /// </summary>
    private void ConfigureAutoFixture()
    {
        _autoFixture.Customize<DateTime>(composer => composer.FromFactory(() =>
            DateTime.Now.AddYears(-new Random().Next(16, 30))
                .AddDays(new Random().Next(-365, 365))
                .AddHours(new Random().Next(0, 24))
                .AddMinutes(new Random().Next(0, 60))));

        _autoFixture.Customize<TimeOnly>(composer => composer.FromFactory(() =>
            new TimeOnly(new Random().Next(8, 18), new Random().Next(0, 60))));

        _autoFixture.Customize<DateOnly>(composer => composer.FromFactory(() =>
            DateOnly.FromDateTime(DateTime.Now.AddYears(new Random().Next(0, 5)))));

        _autoFixture.Customize<string>(composer => composer.FromFactory(() =>
            ChineseNames.GenerateChineseName()));

        _autoFixture.Customize<decimal>(composer => composer.FromFactory(() =>
            (decimal)(new Random().NextDouble() * 100)));

        _autoFixture.Customize<double>(composer => composer.FromFactory(() =>
            new Random().NextDouble() * 50 + 150));

        _autoFixture.Customize<long>(composer => composer.FromFactory(() =>
            new Random().NextInt64(100000000, 999999999)));

        _autoFixture.Customize<int>(composer => composer.FromFactory(() =>
            new Random().Next(16, 30)));
    }

    /// <summary>
    /// 更新 Faker 规则
    /// </summary>
    private void UpdateFakerRules()
    {
        _faker.RuleFor(s => s.Age, f => f.Random.Int(_minAge, _maxAge))
              .RuleFor(s => s.Score, f => f.Random.Decimal(_minScore, _maxScore))
              .RuleFor(s => s.Height, f => f.Random.Double(_minHeight, _maxHeight));
    }

    /// <summary>
    /// 使用 Bogus 生成单个数据
    /// </summary>
    private StudentData GenerateWithBogus()
    {
        return _faker.Generate();
    }

    /// <summary>
    /// 使用 AutoFixture 生成单个数据
    /// </summary>
    private StudentData GenerateWithAutoFixture()
    {
        var student = _autoFixture.Create<StudentData>();
        student.Score = Math.Round(student.Score, 2);
        student.Height = Math.Round(student.Height, 1);
        return student;
    }

    /// <summary>
    /// 使用 AutoFixture 生成列表数据
    /// </summary>
    private List<StudentData> GenerateListWithAutoFixture(int count)
    {
        return _autoFixture.CreateMany<StudentData>(count)
            .Select(student =>
            {
                student.StudentName = ChineseNames.GenerateChineseName();
                student.Score = Math.Round(student.Score, 2);
                student.Height = Math.Round(student.Height, 1);
                return student;
            })
            .ToList();
    }

    /// <summary>
    /// 生成边界数据
    /// </summary>
    private List<StudentData> GenerateBoundaryData()
    {
        return new List<StudentData>
        {
            // 最小值
            new StudentData
            {
                StudentId = long.MinValue,
                StudentName = "",
                Age = int.MinValue,
                Score = decimal.MinValue,
                Height = double.MinValue,
                Gender = Gender.Unknown,
                BirthDateTime = DateTime.MinValue,
                EnrollmentTime = TimeOnly.MinValue,
                GraduationDate = DateOnly.MinValue,
                IsActive = false
            },
            // 最大值
            new StudentData
            {
                StudentId = long.MaxValue,
                StudentName = new string('测', 255),
                Age = int.MaxValue,
                Score = decimal.MaxValue,
                Height = double.MaxValue,
                Gender = Gender.Female,
                BirthDateTime = DateTime.MaxValue,
                EnrollmentTime = TimeOnly.MaxValue,
                GraduationDate = DateOnly.MaxValue,
                IsActive = true
            },
            // 零值
            new StudentData
            {
                StudentId = 0,
                StudentName = "零值测试",
                Age = 0,
                Score = 0m,
                Height = 0.0,
                Gender = Gender.Unknown,
                BirthDateTime = DateTime.Now,
                EnrollmentTime = new TimeOnly(0, 0),
                GraduationDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = false
            }
        };
    }

    /// <summary>
    /// 生成特殊字符数据
    /// </summary>
    private List<StudentData> GenerateSpecialCharacterData()
    {
        var specialNames = new[]
        {
            "特殊字符\\\"测试'",
            "包含，逗号；分号。句号",
            "英文Special@#$%^&*()",
            "数字123456789",
            "混合Special测试123",
            "制表符\\t换行符\\n回车符\\r",
            "Unicode测试🎓📚🖊️",
            "空格 测试  多空格",
            "HTML<script>alert('test')</script>"
        };

        return specialNames.Select((name, index) => new StudentData
        {
            StudentId = 900000 + index,
            StudentName = name,
            Age = 20 + index,
            Score = 85.5m + index,
            Height = 170.0 + index,
            Gender = (Gender)(index % 3),
            BirthDateTime = DateTime.Now.AddYears(-(20 + index)),
            EnrollmentTime = new TimeOnly(9 + (index % 8), index % 60),
            GraduationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(2 + index)),
            IsActive = index % 2 == 0
        }).ToList();
    }
}