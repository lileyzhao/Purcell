using System.Xml;
using Newtonsoft.Json;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest.ReadingTableTests;

public class Tests_Query_Students(ITestOutputHelper testHelper)
{
    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestBasicReadingEn(string extension, QueryType queryType)
    {
        string domain = "StudentsEn";
        string filePath = $"Resources/{domain}.{extension}";

        testHelper.WriteLine(filePath);

        int rowIndex = -1;

        foreach (var item in Purcell.Query<Student>(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
        }

        var studentsLimit = Purcell.Query<Student>(filePath, PurTable.New().WithMaxReadRows(1)).ToList();
        Assert.Equal(1, studentsLimit.Count); // 判断是否限制读取了1行数据

        var students = Purcell.Query<Student>(filePath).ToList();

        Assert.Equal(15, students.Count); // 判断总行数

        for (var i = 0; i < students.Count; i++)
        {
            Assert.Equal(i + 1, students[i].Id); // 判断ID是否从1开始递增
            Assert.True(!string.IsNullOrWhiteSpace(students[i].Name)); // 判断姓名是否非空
        }

        #region 判断学生数据集

        if (extension != "csv")
            Assert.Null(students[3 - 1].Gender); // 空值
        else
            Assert.True(students[3 - 1].Gender == string.Empty); // 空值
        Assert.True(students[4 - 1].Gender is "Unknown"); // 无效值 Unknown
        Assert.Equal("Male", students[1 - 1].Gender); // 正常值

        Assert.Null(students[5 - 1].Grade); // 空值
        Assert.Equal(-5, students[4 - 1].Grade); // 无效值 -5
        Assert.Equal(9, students[1 - 1].Grade); // 正常值

        Assert.Null(students[5 - 1].Birthday); // 空值
        Assert.Null(students[6 - 1].Birthday); // 无效值 2025-02-30
        Assert.Equal(new DateTime(1995, 5, 15), students[1 - 1].Birthday); // 正常值

        Assert.Equal(new DateOnly(1900, 1, 1), students[8 - 1].EnrollmentDate); // 空值
        Assert.Equal(new DateOnly(1900, 1, 1), students[6 - 1].EnrollmentDate); // 无效值 invalid-date
        Assert.Equal(new DateOnly(2020, 9, 1), students[1 - 1].EnrollmentDate); // 正常值

        Assert.Null(students[7 - 1].Gpa); // 空值
        Assert.Equal(-1.5m, students[8 - 1].Gpa); // 无效值 -1.5
        Assert.Equal(3.85m, students[1 - 1].Gpa); // 正常值

        Assert.Equal(0, students[9 - 1].Height); // 空值
        Assert.Equal(-50.5d, students[8 - 1].Height); // 无效值 -50.5
        Assert.Equal(175.5d, students[1 - 1].Height); // 正常值

        Assert.Null(students[9 - 1].Weight); // 空值
        Assert.Equal(-10.2f, students[10 - 1].Weight); // 无效值 -10.2
        Assert.Equal(68.2f, students[1 - 1].Weight); // 正常值

        Assert.Null(students[11 - 1].IsActive); // 空值
        Assert.Null(students[10 - 1].IsActive); // 无效值 invalid
        Assert.True(students[1 - 1].IsActive); // 正常值

        Assert.Null(students[11 - 1].LastLoginTime); // 空值
        Assert.Null(students[12 - 1].LastLoginTime); // 无效值 25:10:10
        Assert.Equal(TimeOnly.Parse("14:30:45"), students[1 - 1].LastLoginTime); // 正常值

        Assert.Equal(TimeSpan.Zero, students[13 - 1].StudyHours); // 空值
        Assert.Equal(TimeSpan.Zero, students[12 - 1].StudyHours); // 无效值 invalid-timespan
        Assert.Equal(XmlConvert.ToTimeSpan("PT2H30M"), students[1 - 1].StudyHours); // 正常值

        // 空值
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[13 - 1].CreatedAt);
        // 无效值 invalid-datetime
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[14 - 1].CreatedAt);
        Assert.Equal(DateTime.Parse("2020/8/15 9:00:00"), students[1 - 1].CreatedAt); // 正常值

        Assert.Null(students[15 - 1].UpdatedAt); // 空值
        Assert.Null(students[14 - 1].UpdatedAt); // 无效值 2023/13/45 29:30
        Assert.Equal("2023/12/1 16:45:30", students[1 - 1].UpdatedAt?.ToString("yyyy/M/d HH:mm:ss")); // 正常值

        Assert.Null(students[14 - 1].GraduationYear); // 空值
        Assert.Equal(-2025, students[15 - 1].GraduationYear); // 无效值 -2025
        Assert.Equal(2024, students[1 - 1].GraduationYear); // 正常值

        #endregion 判断学生数据集
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestBasicReadingCn(string extension, QueryType queryType)
    {
        string domain = "StudentsCn";
        string filePath = $"Resources/{domain}.{extension}";

        testHelper.WriteLine(filePath);

        int rowIndex = -1;

        foreach (var item in Purcell.Query<StudentWithAttr>(filePath))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
        }

        var studentsLimit = Purcell.Query<StudentWithAttr>(filePath, PurTable.New().WithMaxReadRows(1)).ToList();
        Assert.Equal(1, studentsLimit.Count); // 判断是否限制读取了1行数据

        var students = Purcell.Query<StudentWithAttr>(filePath).ToList();

        Assert.Equal(15, students.Count); // 判断总行数

        for (var i = 0; i < students.Count; i++)
        {
            Assert.Equal(i + 1, students[i].Id); // 判断ID是否从1开始递增
            Assert.True(!string.IsNullOrWhiteSpace(students[i].Name)); // 判断姓名是否非空
        }

        if (extension != "csv")
            Assert.Null(students[3 - 1].Gender); // 空值
        else
            Assert.True(students[3 - 1].Gender == string.Empty); // 空值
        Assert.True(students[4 - 1].Gender is "未知"); // 无效值 未知
        Assert.Equal("男", students[1 - 1].Gender); // 正常值

        Assert.Null(students[5 - 1].Grade); // 空值
        Assert.Equal(-5, students[4 - 1].Grade); // 无效值 -5
        Assert.Equal(9, students[1 - 1].Grade); // 正常值

        Assert.Null(students[5 - 1].Birthday); // 空值
        Assert.Null(students[6 - 1].Birthday); // 无效值 2025-02-30
        Assert.Equal(new DateTime(1995, 5, 15), students[1 - 1].Birthday); // 正常值

        Assert.Equal(new DateOnly(1900, 1, 1), students[8 - 1].EnrollmentDate); // 空值
        Assert.Equal(new DateOnly(1900, 1, 1), students[6 - 1].EnrollmentDate); // 无效值 invalid-date
        Assert.Equal(new DateOnly(2020, 9, 1), students[1 - 1].EnrollmentDate); // 正常值

        Assert.Null(students[7 - 1].Gpa); // 空值
        Assert.Equal(-1.5m, students[8 - 1].Gpa); // 无效值 -1.5
        Assert.Equal(3.85m, students[1 - 1].Gpa); // 正常值

        Assert.Equal(0, students[9 - 1].Height); // 空值
        Assert.Equal(-50.5d, students[8 - 1].Height); // 无效值 -50.5
        Assert.Equal(175.5d, students[1 - 1].Height); // 正常值

        Assert.Null(students[9 - 1].Weight); // 空值
        Assert.Equal(-10.2f, students[10 - 1].Weight); // 无效值 -10.2
        Assert.Equal(68.2f, students[1 - 1].Weight); // 正常值

        Assert.Null(students[11 - 1].IsActive); // 空值
        Assert.Null(students[10 - 1].IsActive); // 无效值 invalid
        Assert.True(students[1 - 1].IsActive); // 正常值

        Assert.Null(students[11 - 1].LastLoginTime); // 空值
        Assert.Null(students[12 - 1].LastLoginTime); // 无效值 25:70:90
        Assert.Equal(TimeOnly.Parse("14:30:45"), students[1 - 1].LastLoginTime); // 正常值

        Assert.Equal(TimeSpan.Zero, students[13 - 1].StudyHours); // 空值
        Assert.Equal(TimeSpan.Zero, students[12 - 1].StudyHours); // 无效值 invalid-timespan
        Assert.Equal(XmlConvert.ToTimeSpan("PT2H30M"), students[1 - 1].StudyHours); // 正常值

        // 空值
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[13 - 1].CreatedAt);
        // 无效值 invalid-datetime
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[14 - 1].CreatedAt);
        Assert.Equal(DateTime.Parse("2020/8/15 9:00:00"), students[1 - 1].CreatedAt); // 正常值

        Assert.Null(students[15 - 1].UpdatedAt); // 空值
        Assert.Null(students[14 - 1].UpdatedAt); // 无效值 2023/13/45 29:30
        Assert.Equal("2023/12/1 16:45:30", students[1 - 1].UpdatedAt?.ToString("yyyy/M/d HH:mm:ss")); // 正常值

        Assert.Null(students[14 - 1].GraduationYear); // 空值
        Assert.Equal(-2025, students[15 - 1].GraduationYear); // 无效值 -2025
        Assert.Equal(2024, students[1 - 1].GraduationYear); // 正常值
    }

    [Theory]
    [InlineData("xlsx", QueryType.Xlsx)]
    [InlineData("xls", QueryType.Xls)]
    [InlineData("csv", QueryType.Csv)]
    public void TestBasicReadingCnDynamicColumns(string extension, QueryType queryType)
    {
        string domain = "StudentsCn";
        string filePath = $"Resources/{domain}.{extension}";

        testHelper.WriteLine(filePath);

        var dycColumns = new List<PurColumn>()
        {
            PurColumn.FromProperty(nameof(Student.Id)).AddName("学生编号"),
            PurColumn.FromProperty(nameof(Student.Name)).AddName("姓名"),
            PurColumn.FromProperty(nameof(Student.Gender)).AddName("性别"),
            PurColumn.FromProperty(nameof(Student.Grade)).AddName("年级"),
            PurColumn.FromProperty(nameof(Student.Birthday)).AddName("生日"),
            PurColumn.FromProperty(nameof(Student.EnrollmentDate)).AddName("入学").WithMatchStrategy(MatchStrategy.Prefix),
            PurColumn.FromProperty(nameof(Student.Gpa)).AddName("绩点").WithMatchStrategy(MatchStrategy.Suffix),
            PurColumn.FromProperty(nameof(Student.Height)).AddName("身高"),
            PurColumn.FromProperty(nameof(Student.Weight)).AddName("体重"),
            PurColumn.FromProperty(nameof(Student.IsActive)).AddName("在校状态"),
            PurColumn.FromProperty(nameof(Student.LastLoginTime)).AddName("最后登录时间"),
            PurColumn.FromProperty(nameof(Student.StudyHours)).AddName("学习时长"),
            PurColumn.FromProperty(nameof(Student.CreatedAt)).AddName("创建时间"),
            PurColumn.FromProperty(nameof(Student.UpdatedAt)).AddName("更新时间"),
            PurColumn.FromProperty(nameof(Student.GraduationYear)).AddName("毕业年份")
        };

        int rowIndex = -1;

        foreach (var item in Purcell.Query<Student>(filePath, PurTable.FromColumns(dycColumns)))
        {
            rowIndex++;
            testHelper.WriteLine($"第 {rowIndex + 1} 行：");
            testHelper.WriteLine(JsonConvert.SerializeObject(item));
        }

        var studentsLimit = Purcell.Query<Student>(filePath, PurTable.FromColumns(dycColumns).WithMaxReadRows(1)).ToList();
        Assert.Equal(1, studentsLimit.Count); // 判断是否限制读取了1行数据

        var students = Purcell.Query<Student>(filePath, PurTable.FromColumns(dycColumns)).ToList();

        Assert.Equal(15, students.Count); // 判断总行数

        for (var i = 0; i < students.Count; i++)
        {
            Assert.Equal(i + 1, students[i].Id); // 判断ID是否从1开始递增
            Assert.True(!string.IsNullOrWhiteSpace(students[i].Name)); // 判断姓名是否非空
        }

        if (extension != "csv")
            Assert.Null(students[3 - 1].Gender); // 空值
        else
            Assert.True(students[3 - 1].Gender == string.Empty); // 空值
        Assert.True(students[4 - 1].Gender is "未知"); // 无效值 未知
        Assert.Equal("男", students[1 - 1].Gender); // 正常值

        Assert.Null(students[5 - 1].Grade); // 空值
        Assert.Equal(-5, students[4 - 1].Grade); // 无效值 -5
        Assert.Equal(9, students[1 - 1].Grade); // 正常值

        Assert.Null(students[5 - 1].Birthday); // 空值
        Assert.Null(students[6 - 1].Birthday); // 无效值 2025-02-30
        Assert.Equal(new DateTime(1995, 5, 15), students[1 - 1].Birthday); // 正常值

        Assert.Equal(new DateOnly(1900, 1, 1), students[8 - 1].EnrollmentDate); // 空值
        Assert.Equal(new DateOnly(1900, 1, 1), students[6 - 1].EnrollmentDate); // 无效值 invalid-date
        Assert.Equal(new DateOnly(2020, 9, 1), students[1 - 1].EnrollmentDate); // 正常值

        Assert.Null(students[7 - 1].Gpa); // 空值
        Assert.Equal(-1.5m, students[8 - 1].Gpa); // 无效值 -1.5
        Assert.Equal(3.85m, students[1 - 1].Gpa); // 正常值

        Assert.Equal(0, students[9 - 1].Height); // 空值
        Assert.Equal(-50.5d, students[8 - 1].Height); // 无效值 -50.5
        Assert.Equal(175.5d, students[1 - 1].Height); // 正常值

        Assert.Null(students[9 - 1].Weight); // 空值
        Assert.Equal(-10.2f, students[10 - 1].Weight); // 无效值 -10.2
        Assert.Equal(68.2f, students[1 - 1].Weight); // 正常值

        Assert.Null(students[11 - 1].IsActive); // 空值
        Assert.Null(students[10 - 1].IsActive); // 无效值 invalid
        Assert.True(students[1 - 1].IsActive); // 正常值

        Assert.Null(students[11 - 1].LastLoginTime); // 空值
        Assert.Null(students[12 - 1].LastLoginTime); // 无效值 25:70:90
        Assert.Equal(TimeOnly.Parse("14:30:45"), students[1 - 1].LastLoginTime); // 正常值

        Assert.Equal(TimeSpan.Zero, students[13 - 1].StudyHours); // 空值
        Assert.Equal(TimeSpan.Zero, students[12 - 1].StudyHours); // 无效值 invalid-timespan
        Assert.Equal(XmlConvert.ToTimeSpan("PT2H30M"), students[1 - 1].StudyHours); // 正常值

        // 空值
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[13 - 1].CreatedAt);
        // 无效值 invalid-datetime
        Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), students[14 - 1].CreatedAt);
        Assert.Equal(DateTime.Parse("2020/8/15 9:00:00"), students[1 - 1].CreatedAt); // 正常值

        Assert.Null(students[15 - 1].UpdatedAt); // 空值
        Assert.Null(students[14 - 1].UpdatedAt); // 无效值 2023/13/45 29:30
        Assert.Equal("2023/12/1 16:45:30", students[1 - 1].UpdatedAt?.ToString("yyyy/M/d HH:mm:ss")); // 正常值

        Assert.Null(students[14 - 1].GraduationYear); // 空值
        Assert.Equal(-2025, students[15 - 1].GraduationYear); // 无效值 -2025
        Assert.Equal(2024, students[1 - 1].GraduationYear); // 正常值
    }
}