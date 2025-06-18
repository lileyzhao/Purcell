using System.ComponentModel;
using System.Runtime.Serialization;

namespace PurcellLibs.UnitTest;

public class EmployeeNoAttr
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum GenderEnum
    {
        Unknown,
        [Description("M")] Male,
        [EnumMember(Value = "F")] Female
    }

    /// <summary>
    /// 员工ID
    /// </summary>
    public int EmpId { get; set; }

    /// <summary>
    /// 名称前缀
    /// </summary>
    public string? NamePrefix { get; set; }

    /// <summary>
    /// 名
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// 中间名首字母
    /// </summary>
    public char MiddleInitial { get; set; }

    /// <summary>
    /// 姓
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderEnum? Gender { get; set; }

    /// <summary>
    /// 电子邮件
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 父亲的名字
    /// </summary>
    public string? FathersName { get; set; }

    /// <summary>
    /// 母亲的名字
    /// </summary>
    public string? MothersName { get; set; }

    /// <summary>
    /// 母亲的娘家姓
    /// </summary>
    public string? MothersMaidenName { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// 出生时间
    /// </summary>
    public TimeSpan? TimeOfBirth { get; set; }

    /// <summary>
    /// 年龄（年）
    /// </summary>
    public decimal AgeInYears { get; set; }

    /// <summary>
    /// 体重（公斤）
    /// </summary>
    public double WeightInKgs { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    public DateTime DateOfJoining { get; set; }

    /// <summary>
    /// 入职季度
    /// </summary>
    public string? QuarterOfJoining { get; set; }

    /// <summary>
    /// 入职半年期
    /// </summary>
    public string? HalfOfJoining { get; set; }

    /// <summary>
    /// 入职年份
    /// </summary>
    public int YearOfJoining { get; set; }

    /// <summary>
    /// 入职月份
    /// </summary>
    public int MonthOfJoining { get; set; }

    /// <summary>
    /// 入职月份名称
    /// </summary>
    public string? MonthNameOfJoining { get; set; }

    /// <summary>
    /// 入职月份缩写
    /// </summary>
    public int? ShortMonth { get; set; }

    /// <summary>
    /// 入职日期（天）
    /// </summary>
    public int DayOfJoining { get; set; }

    /// <summary>
    /// 入职星期几
    /// </summary>
    public string? DowOfJoining { get; set; }

    /// <summary>
    /// 入职星期几缩写
    /// </summary>
    public string? ShortDow { get; set; }

    /// <summary>
    /// 公司年龄（年）
    /// </summary>
    public decimal AgeInCompanyYears { get; set; }

    /// <summary>
    /// 薪水
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// 上次涨幅百分比
    /// </summary>
    public double LastPercentHike { get; set; }

    /// <summary>
    /// 社会安全号码
    /// </summary>
    public string? Ssn { get; set; }

    /// <summary>
    /// 电话号码
    /// </summary>
    public string? PhoneNo { get; set; }

    /// <summary>
    /// 地点名称
    /// </summary>
    public string? PlaceName { get; set; }

    /// <summary>
    /// 县
    /// </summary>
    public string? County { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 州
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// 地区
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }
}