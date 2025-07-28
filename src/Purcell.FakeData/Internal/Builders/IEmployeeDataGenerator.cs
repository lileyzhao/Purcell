namespace PurcellLibs.FakeData.Internal.Builders;

/// <summary>
/// 员工数据生成器接口，内部流畅API抽象
/// </summary>
internal interface IEmployeeDataGenerator
{
    /// <summary>
    /// 设置生成数量
    /// </summary>
    IEmployeeDataGenerator WithCount(int count);

    /// <summary>
    /// 设置年龄范围
    /// </summary>
    IEmployeeDataGenerator WithAgeRange(int minAge, int maxAge);

    /// <summary>
    /// 设置工资范围
    /// </summary>
    IEmployeeDataGenerator WithSalaryRange(decimal minSalary, decimal maxSalary);

    /// <summary>
    /// 设置身高范围
    /// </summary>
    IEmployeeDataGenerator WithHeightRange(double minHeight, double maxHeight);

    /// <summary>
    /// 设置工作年限范围
    /// </summary>
    IEmployeeDataGenerator WithWorkYearsRange(uint minWorkYears, uint maxWorkYears);

    /// <summary>
    /// 设置起始ID
    /// </summary>
    IEmployeeDataGenerator WithStartId(long startId);

    /// <summary>
    /// 使用 Bogus 生成数据
    /// </summary>
    IEmployeeDataGenerator UseBogus();

    /// <summary>
    /// 使用 AutoFixture 生成数据
    /// </summary>
    IEmployeeDataGenerator UseAutoFixture();

    /// <summary>
    /// 包含边界数据
    /// </summary>
    IEmployeeDataGenerator WithBoundaryData();

    /// <summary>
    /// 包含特殊字符数据
    /// </summary>
    IEmployeeDataGenerator WithSpecialCharacters();

    /// <summary>
    /// 生成单个员工数据
    /// </summary>
    EmployeeData AsOne();

    /// <summary>
    /// 生成员工数据列表
    /// </summary>
    List<EmployeeData> AsList();

    /// <summary>
    /// 生成字典格式数据
    /// </summary>
    List<Dictionary<string, object?>> AsDictionary();
}