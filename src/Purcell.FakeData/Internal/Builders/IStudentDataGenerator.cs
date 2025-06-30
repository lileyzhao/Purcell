namespace Purcell.FakeData.Internal.Builders;

/// <summary>
/// 学生数据生成器接口，内部流畅API抽象
/// </summary>
internal interface IStudentDataGenerator
{
    /// <summary>
    /// 设置生成数量
    /// </summary>
    IStudentDataGenerator WithCount(int count);

    /// <summary>
    /// 设置年龄范围
    /// </summary>
    IStudentDataGenerator WithAgeRange(int minAge, int maxAge);

    /// <summary>
    /// 设置分数范围
    /// </summary>
    IStudentDataGenerator WithScoreRange(decimal minScore, decimal maxScore);

    /// <summary>
    /// 设置身高范围
    /// </summary>
    IStudentDataGenerator WithHeightRange(double minHeight, double maxHeight);

    /// <summary>
    /// 设置起始ID
    /// </summary>
    IStudentDataGenerator WithStartId(long startId);

    /// <summary>
    /// 使用 Bogus 生成数据
    /// </summary>
    IStudentDataGenerator UseBogus();

    /// <summary>
    /// 使用 AutoFixture 生成数据
    /// </summary>
    IStudentDataGenerator UseAutoFixture();

    /// <summary>
    /// 包含边界数据
    /// </summary>
    IStudentDataGenerator WithBoundaryData();

    /// <summary>
    /// 包含特殊字符数据
    /// </summary>
    IStudentDataGenerator WithSpecialCharacters();

    /// <summary>
    /// 生成单个学生数据
    /// </summary>
    StudentData AsOne();

    /// <summary>
    /// 生成学生数据列表
    /// </summary>
    List<StudentData> AsList();

    /// <summary>
    /// 生成字典格式数据
    /// </summary>
    List<Dictionary<string, object?>> AsDictionary();
}