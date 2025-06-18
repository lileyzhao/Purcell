namespace PurcellLibs.UnitTest;

/// <summary>
/// 学生信息表，使用Purcell特性标记属性
/// </summary>
[PurTable("Sheet1")]
public class StudentWithAttr : Student
{
    [PurColumn("学生编号")]
    public override int Id { get; set; }

    [PurColumn("姓名")]
    public override string Name { get; set; } = string.Empty;

    [PurColumn("性别")]
    public override string? Gender { get; set; }

    [PurColumn("年级")]
    public override int? Grade { get; set; }

    [PurColumn("生日")]
    public override DateTime? Birthday { get; set; }

    // 使用前缀适配策略，用“入学”匹配实际列名“入学日期”
    [PurColumn("入学", MatchStrategy = MatchStrategy.Prefix)]
    public override DateOnly EnrollmentDate { get; set; }

    // 使用后缀适配策略，用“绩点”匹配实际列名“学分绩点”
    [PurColumn("绩点", MatchStrategy = MatchStrategy.Suffix)]
    public override decimal? Gpa { get; set; }

    [PurColumn("身高")]
    public override double Height { get; set; }

    [PurColumn("体重")]
    public override float? Weight { get; set; }

    [PurColumn("在校状态")]
    public override bool? IsActive { get; set; }

    [PurColumn("最后登录时间")]
    public override TimeOnly? LastLoginTime { get; set; }

    [PurColumn("学习时长")]
    public override TimeSpan StudyHours { get; set; }

    [PurColumn("创建时间")]
    public override DateTime CreatedAt { get; set; }

    [PurColumn("更新时间")]
    public override DateTime? UpdatedAt { get; set; }

    [PurColumn("毕业年份")]
    public override int? GraduationYear { get; set; }
}