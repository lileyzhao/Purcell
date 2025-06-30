using Purcell.FakeData.Internal.Builders;

namespace Purcell.FakeData;

/// <summary>
/// 员工数据模型，用于测试数据生成
/// </summary>
public class EmployeeData
{
    /// <summary>
    /// 员工ID
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 基本工资
    /// </summary>
    public decimal Salary { get; set; }

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
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    public TimeOnly HireTime { get; set; }

    /// <summary>
    /// 合同到期日期
    /// </summary>
    public DateOnly ContractEndDate { get; set; }

    /// <summary>
    /// 是否在职
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 员工编号 (短整型)
    /// </summary>
    public short EmployeeNumber { get; set; }

    /// <summary>
    /// 绩效评分 (单精度浮点)
    /// </summary>
    public float PerformanceScore { get; set; }

    /// <summary>
    /// 工作年限 (无符号整型)
    /// </summary>
    public uint WorkYears { get; set; }

    /// <summary>
    /// 身份证号 (64位无符号整型)
    /// </summary>
    public ulong IdCardNumber { get; set; }

    /// <summary>
    /// 部门代码 (字节)
    /// </summary>
    public byte DepartmentCode { get; set; }

    /// <summary>
    /// 职级等级 (有符号字节)
    /// </summary>
    public sbyte JobLevel { get; set; }

    /// <summary>
    /// 婚姻状况 (字符)
    /// </summary>
    public char MaritalStatus { get; set; }

    /// <summary>
    /// 月工作小时数 (无符号短整型)
    /// </summary>
    public ushort MonthlyWorkHours { get; set; }

    /// <summary>
    /// 员工GUID
    /// </summary>
    public Guid EmployeeGuid { get; set; }

    /// <summary>
    /// 备注信息 (可空字符串)
    /// </summary>
    public string? Remarks { get; set; }

    #region 静态生成方法

    /// <summary>
    /// 快速生成指定数量的员工数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <returns>员工数据列表</returns>
    public static List<EmployeeData> Generate(int count = 1)
    {
        return new EmployeeDataBuilder()
            .WithCount(count)
            .AsList();
    }

    /// <summary>
    /// 快速生成单个员工数据
    /// </summary>
    /// <returns>员工数据</returns>
    public static EmployeeData GenerateOne()
    {
        return new EmployeeDataBuilder().AsOne();
    }

    /// <summary>
    /// 生成指定年龄范围的员工数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="minAge">最小年龄</param>
    /// <param name="maxAge">最大年龄</param>
    /// <returns>员工数据列表</returns>
    public static List<EmployeeData> GenerateWithAgeRange(int count, int minAge, int maxAge)
    {
        return new EmployeeDataBuilder()
            .WithCount(count)
            .WithAgeRange(minAge, maxAge)
            .AsList();
    }

    /// <summary>
    /// 生成指定工资范围的员工数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="minSalary">最小工资</param>
    /// <param name="maxSalary">最大工资</param>
    /// <returns>员工数据列表</returns>
    public static List<EmployeeData> GenerateWithSalaryRange(int count, decimal minSalary, decimal maxSalary)
    {
        return new EmployeeDataBuilder()
            .WithCount(count)
            .WithSalaryRange(minSalary, maxSalary)
            .AsList();
    }

    /// <summary>
    /// 生成指定工作年限范围的员工数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="minWorkYears">最小工作年限</param>
    /// <param name="maxWorkYears">最大工作年限</param>
    /// <returns>员工数据列表</returns>
    public static List<EmployeeData> GenerateWithWorkYearsRange(int count, uint minWorkYears, uint maxWorkYears)
    {
        return new EmployeeDataBuilder()
            .WithCount(count)
            .WithWorkYearsRange(minWorkYears, maxWorkYears)
            .AsList();
    }

    /// <summary>
    /// 生成边界测试数据
    /// </summary>
    /// <returns>边界测试数据列表</returns>
    public static List<EmployeeData> GenerateBoundaryData()
    {
        return new EmployeeDataBuilder()
            .WithBoundaryData()
            .AsList();
    }

    /// <summary>
    /// 生成特殊字符测试数据
    /// </summary>
    /// <returns>特殊字符测试数据列表</returns>
    public static List<EmployeeData> GenerateSpecialCharacterData()
    {
        return new EmployeeDataBuilder()
            .WithSpecialCharacters()
            .AsList();
    }

    /// <summary>
    /// 生成部门员工数据
    /// </summary>
    /// <param name="departmentCode">部门代码</param>
    /// <param name="count">员工数量</param>
    /// <returns>部门员工数据列表</returns>
    public static List<EmployeeData> GenerateForDepartment(byte departmentCode, int count)
    {
        var employees = Generate(count);
        foreach (var employee in employees)
        {
            employee.DepartmentCode = departmentCode;
        }
        return employees;
    }

    /// <summary>
    /// 生成场景数据集合
    /// </summary>
    /// <returns>不同场景的数据集合</returns>
    public static Dictionary<string, List<EmployeeData>> GenerateScenarios()
    {
        return new Dictionary<string, List<EmployeeData>>
        {
            ["小数据集"] = Generate(10),
            ["中等数据集"] = Generate(100),
            ["大数据集"] = Generate(1000),
            ["边界数据集"] = GenerateBoundaryData(),
            ["特殊字符数据集"] = GenerateSpecialCharacterData(),
            ["高薪员工数据集"] = GenerateWithSalaryRange(50, 15000, 50000),
            ["新员工数据集"] = GenerateWithWorkYearsRange(30, 0, 2),
            ["资深员工数据集"] = GenerateWithWorkYearsRange(20, 10, 30)
        };
    }

    /// <summary>
    /// 生成字典格式的数据
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <returns>字典格式数据列表</returns>
    public static List<Dictionary<string, object?>> GenerateAsDictionary(int count)
    {
        return new EmployeeDataBuilder()
            .WithCount(count)
            .AsDictionary();
    }

    #endregion
}