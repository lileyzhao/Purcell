using PurcellLibs.FakeData.Internal.Builders;

namespace PurcellLibs.FakeData;

/// <summary>
/// 学生数据模型，用于测试数据生成
/// </summary>
public class StudentData
{
    /// <summary>
    /// 学生ID
    /// </summary>
    public long StudentId { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 身高 (厘米)
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 出生日期时间
    /// </summary>
    public DateTime BirthDateTime { get; set; }

    /// <summary>
    /// 入学时间
    /// </summary>
    public TimeOnly EnrollmentTime { get; set; }

    /// <summary>
    /// 毕业日期
    /// </summary>
    public DateOnly GraduationDate { get; set; }

    /// <summary>
    /// 是否活跃状态
    /// </summary>
    public bool IsActive { get; set; }

    #region 静态生成方法

    /// <summary>
    /// 快速生成指定数量的学生数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <returns>学生数据列表</returns>
    public static List<StudentData> Generate(int count = 1)
    {
        return new StudentDataBuilder()
            .WithCount(count)
            .AsList();
    }

    /// <summary>
    /// 快速生成单个学生数据
    /// </summary>
    /// <returns>学生数据</returns>
    public static StudentData GenerateOne()
    {
        return new StudentDataBuilder().AsOne();
    }

    /// <summary>
    /// 生成指定年龄范围的学生数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="minAge">最小年龄</param>
    /// <param name="maxAge">最大年龄</param>
    /// <returns>学生数据列表</returns>
    public static List<StudentData> GenerateWithAgeRange(int count, int minAge, int maxAge)
    {
        return new StudentDataBuilder()
            .WithCount(count)
            .WithAgeRange(minAge, maxAge)
            .AsList();
    }

    /// <summary>
    /// 生成指定分数范围的学生数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="minScore">最小分数</param>
    /// <param name="maxScore">最大分数</param>
    /// <returns>学生数据列表</returns>
    public static List<StudentData> GenerateWithScoreRange(int count, decimal minScore, decimal maxScore)
    {
        return new StudentDataBuilder()
            .WithCount(count)
            .WithScoreRange(minScore, maxScore)
            .AsList();
    }

    /// <summary>
    /// 生成边界测试数据
    /// </summary>
    /// <returns>边界测试数据列表</returns>
    public static List<StudentData> GenerateBoundaryData()
    {
        return new StudentDataBuilder()
            .WithBoundaryData()
            .AsList();
    }

    /// <summary>
    /// 生成特殊字符测试数据
    /// </summary>
    /// <returns>特殊字符测试数据列表</returns>
    public static List<StudentData> GenerateSpecialCharacterData()
    {
        return new StudentDataBuilder()
            .WithSpecialCharacters()
            .AsList();
    }

    /// <summary>
    /// 生成场景数据集合
    /// </summary>
    /// <returns>不同场景的数据集合</returns>
    public static Dictionary<string, List<StudentData>> GenerateScenarios()
    {
        return new Dictionary<string, List<StudentData>>
        {
            ["小数据集"] = Generate(10),
            ["中等数据集"] = Generate(100),
            ["大数据集"] = Generate(1000),
            ["边界数据集"] = GenerateBoundaryData(),
            ["特殊字符数据集"] = GenerateSpecialCharacterData()
        };
    }

    /// <summary>
    /// 生成字典格式的数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <returns>字典格式数据列表</returns>
    public static List<Dictionary<string, object?>> GenerateAsDictionary(int count)
    {
        return new StudentDataBuilder()
            .WithCount(count)
            .AsDictionary();
    }

    #endregion
}