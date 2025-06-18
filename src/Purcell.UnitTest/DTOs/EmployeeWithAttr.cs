using System.ComponentModel;
using System.Runtime.Serialization;

namespace PurcellLibs.UnitTest;

[PurTable]
public class EmployeeWithAttr
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum GenderEnum
    {
        Unknown,

        [Description("M")]
        Male,

        [EnumMember(Value = "F")]
        Female
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
    [PurColumn("Father's Name")]
    public string? FathersName { get; set; }

    /// <summary>
    /// 母亲的名字
    /// </summary>
    [PurColumn("Mother's Name")]
    public string? MothersName { get; set; }

    /// <summary>
    /// 母亲的娘家姓
    /// </summary>
    [PurColumn("Mother's Maiden Name")]
    public string? MothersMaidenName { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [PurColumn("Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// 出生时间
    /// </summary>
    [PurColumn("Time of Birth")]
    public TimeSpan? TimeOfBirth { get; set; }

    /// <summary>
    /// 年龄（年）
    /// </summary>
    [PurColumn("Age in Yrs.")]
    public decimal AgeInYears { get; set; }

    /// <summary>
    /// 体重（公斤）
    /// </summary>
    [PurColumn("Weight in Kgs.")]
    public double WeightInKgs { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    [PurColumn("Date of Joining")]
    public DateTime DateOfJoining { get; set; }

    /// <summary>
    /// 入职季度
    /// </summary>
    [PurColumn(15)]
    public string? QuarterOfJoining { get; set; }

    /// <summary>
    /// 入职半年期
    /// </summary>
    [PurColumn(16)]
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
    [PurColumn(IgnoreInQuery = true)]
    public int? ShortMonth { get; set; }

    /// <summary>
    /// 入职日期（天）
    /// </summary>
    [PurColumn("Day of Joining")]
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
    [PurColumn("Age in Company (Years)")]
    public decimal AgeInCompanyYears { get; set; }

    /// <summary>
    /// 薪水
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// 上次涨幅百分比
    /// </summary>
    [PurColumn("Last % Hike")]
    public double LastPercentHike { get; set; }

    /// <summary>
    /// 社会安全号码
    /// </summary>
    public string? Ssn { get; set; }

    /// <summary>
    /// 电话号码
    /// </summary>
    [PurColumn("Phone", MatchStrategy = MatchStrategy.IgnoreCasePrefix)]
    public string? PhoneNo { get; set; }

    /// <summary>
    /// 地点名称
    /// </summary>
    [PurColumn("PLACE.*", MatchStrategy = MatchStrategy.IgnoreCaseRegex)]
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