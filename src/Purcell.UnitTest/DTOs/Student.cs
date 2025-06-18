namespace PurcellLibs.UnitTest;

/// <summary>
/// 学生信息表
/// </summary>
public class Student
{
    /// <summary>
    /// 学生ID - 主键
    /// </summary>
    public virtual int Id { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    public virtual string Name { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    public virtual string? Gender { get; set; }

    /// <summary>
    /// 年级
    /// </summary>
    public virtual int? Grade { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public virtual DateTime? Birthday { get; set; }

    /// <summary>
    /// 入学日期
    /// </summary>
    public virtual DateOnly EnrollmentDate { get; set; }

    /// <summary>
    /// 学分绩点 (0.0-4.0)
    /// </summary>
    public virtual decimal? Gpa { get; set; }

    /// <summary>
    /// 身高 (厘米)
    /// </summary>
    public virtual double Height { get; set; }

    /// <summary>
    /// 体重 (公斤)
    /// </summary>
    public virtual float? Weight { get; set; }

    /// <summary>
    /// 是否在校状态
    /// </summary>
    public virtual bool? IsActive { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public virtual TimeOnly? LastLoginTime { get; set; }

    /// <summary>
    /// 日均学习时长
    /// </summary>
    public virtual TimeSpan StudyHours { get; set; }

    /// <summary>
    /// 记录创建时间
    /// </summary>
    public virtual DateTime CreatedAt { get; set; }

    /// <summary>
    /// 记录更新时间
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 毕业年份
    /// </summary>
    public virtual int? GraduationYear { get; set; }
}