using System.Xml;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest.ReadingTableTests;

public class Tests_Any_PurTable(ITestOutputHelper testHelper)
{
    /// <summary>
    /// 测试 SheetIndex 和 SheetName 正确读取指定工作表，测试数据集正确读取
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_Index_Name_DataSet(string extension, QueryType queryType)
    {
        string domain = "StudentsEnMultiSheets";
        string filePath = $"Resources/{domain}.{extension}";

        var studentSets = new List<List<Student>>
        {
            Purcell.Query<Student>(filePath, PurTable.From(0)).ToList(),
            Purcell.Query<Student>(filePath, PurTable.From("学生信息")).ToList(),
            Purcell.Query<Student>(filePath, new PurTable { SheetIndex = 2 }).ToList(),
            Purcell.Query<Student>(filePath, new PurTable { SheetName = "学生信息Locked" }).ToList(),
            Purcell.Query<Student>(filePath, PurTable.New().WithName("学生信息Space")
                .AddColumn(PurColumn.FromProperty("EnrollmentDate").AddName("Enrollment Date"))
            ).ToList(),
            Purcell.Query<Student>(filePath, PurTable
                .From([PurColumn.FromProperty("EnrollmentDate").AddName("Enrollment Date")], "学生信息Space2")
            ).ToList(),
            Purcell.Query<Student>(filePath, PurTable.New().WithIndex(6)).ToList(),
            Purcell.Query<Student>(filePath, new PurTable
            {
                Columns = [PurColumn.FromProperty("EnrollmentDate").AddName("Enrollment Date")],
                SheetName = "学生信息Space3"
            }).ToList()
        };

        for (int i = 0; i < studentSets.Count; i++)
        {
            var students = studentSets[i];

            testHelper.WriteLine($"Sheet{i + 1}：🚀");

            #region 测试 SheetIndex 和 SheetName 正确读取指定工作表

            for (int j = 0; j < students.Count; j++)
            {
                var student = students[j];
                Assert.Equal(j + 1 + (i * 10), student.Id);
                Assert.True(!string.IsNullOrWhiteSpace(student.Name)); // 判断姓名是否非空
            }

            #endregion 测试 SheetIndex 和 SheetName 正确读取指定工作表

            testHelper.WriteLine($"正确读取工作表：✅");

            #region 测试学生数据集

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

            #endregion 测试学生数据集

            testHelper.WriteLine($"通过数据集测试：✅");
        }
    }

    /// <summary>
    /// 测试 HasHeader = false
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_HasHeader_False(string extension, QueryType queryType)
    {
        string domain = "StudentsEnMultiSheets";
        string filePath = $"Resources/{domain}.{extension}";

        var studentsWithoutHeader = Purcell.Query<Student>(filePath,
            PurTable.From(0).WithoutHeader().AddColumn(PurColumn.FromProperty(nameof(Student.Id)).WithIndex(0))
        ).ToList();

        for (var i = 0; i < studentsWithoutHeader.Count; i++)
        {
            Assert.Equal(i + 1, studentsWithoutHeader[i].Id); // 判断ID是否从1开始递增
            Assert.True(string.IsNullOrWhiteSpace(studentsWithoutHeader[i].Name)); // 因为未映射，则一定为空值
        }

        testHelper.WriteLine($"测试无表头模式：✅");
    }

    /// <summary>
    /// 测试 MaxReadRows
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_MaxReadRows(string extension, QueryType queryType)
    {
        string domain = "StudentsEnMultiSheets";
        string filePath = $"Resources/{domain}.{extension}";

        var studentsRows5 = Purcell.Query<Student>(filePath, PurTable.From(0).WithMaxReadRows(5)).ToList();
        Assert.Equal(5, studentsRows5.Count); // 确认读取了5行数据
        testHelper.WriteLine($"验证MaxReadRows5设置：✅");

        var studentsRows8 = Purcell.Query<Student>(filePath, new PurTable(0) { MaxReadRows = 8 }).ToList();
        Assert.Equal(8, studentsRows8.Count); // 确认读取了8行数据
        testHelper.WriteLine($"验证MaxReadRows8设置：✅");
    }

    /// <summary>
    /// 测试 MaxWriteRows
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_MaxWriteRows(string extension, QueryType queryType)
    {
        string filePath = FileHelper.GenExportFilePath(extension);

        Purcell.Export(PurTable.From(MockData.GetGenericData().Where(t => t != null).ToList()).WithMaxWriteRows(9),
            filePath);
        var studentsRows9 = Purcell.Query(filePath, PurTable.From(0)).ToList();
        Assert.Equal(9, studentsRows9.Count); // 确认读取了9行数据
        testHelper.WriteLine($"验证MaxWriteRows9设置：✅");
        File.Delete(filePath);

        var purTable = new PurTable { MaxWriteRows = 6 }.WithRecords(MockData.GetGenericData().Where(t => t != null).ToList());
        Purcell.Export(purTable, filePath);
        var studentsRows6 = Purcell.Query(filePath, PurTable.From(0)).ToList();
        Assert.Equal(6, studentsRows6.Count); // 确认读取了6行数据
        testHelper.WriteLine($"验证MaxWriteRows6设置：✅");
        File.Delete(filePath);
    }

    /// <summary>
    /// 测试 HeaderStart 和 DataStart 的设置
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_HeaderStart_DataStart(string extension, QueryType queryType)
    {
        string domain = "StudentsEnMultiSheets";
        string filePath = $"Resources/{domain}.{extension}";

        // 设置 HeaderStart，以便测试是否从 2 开始递增
        var studentsWithoutHeaderStart = Purcell.Query<Student>(filePath,
            PurTable.From(0).WithHeaderStart("A2").AddColumn(PurColumn.FromProperty(nameof(Student.Id)).WithIndex(0))
        ).ToList();

        for (var i = 0; i < studentsWithoutHeaderStart.Count; i++)
        {
            Assert.Equal(i + 2, studentsWithoutHeaderStart[i].Id); // 判断ID是否从2开始递增
        }

        // 设置 HeaderStart，因为无法转换，则Id应均为0
        var studentsWithoutHeaderStart2 = Purcell.Query<Student>(filePath,
            PurTable.From(0).WithHeaderStart("B2").WithDataStart(CellLocator.Create("B3"))
                .AddColumn(PurColumn.FromProperty(nameof(Student.Id)).WithIndex(0))
        ).ToList();

        foreach (var t in studentsWithoutHeaderStart2)
        {
            Assert.Equal(0, t.Id); // 判断ID均为0
        }

        testHelper.WriteLine($"验证HeaderStart设置：✅");

        // 设置 DataStart，以便测试是否从 3 开始递增
        var studentsWithoutDataStart = Purcell.Query<Student>(filePath,
            PurTable.From(0).WithDataStart("A4")
        ).ToList();

        for (var i = 0; i < studentsWithoutDataStart.Count; i++)
        {
            Assert.Equal(i + 3, studentsWithoutDataStart[i].Id); // 判断ID是否从3开始递增
        }

        testHelper.WriteLine($"验证DataStart设置：✅");

        // 测试预期的异常抛出
        try
        {
            Assert.Contains(
                "数据区域的起始列必须与表头区域的起始列相同",
                Assert.Throws<InvalidOperationException>(() =>
                {
                    PurTable.New().WithHeaderStart("A2").WithDataStart("B2").GetDataStart();
                }).Message
            );

            Assert.Contains(
                "数据区域的起始行必须大于表头区域的起始行",
                Assert.Throws<InvalidOperationException>(() =>
                {
                    PurTable.New().WithHeaderStart("A2").WithDataStart("A2").GetDataStart();
                }).Message
            );
        }
        catch
        {
            // ignored
        }

        testHelper.WriteLine($"预期的异常抛出：✅");
    }

    /// <summary>
    /// 测试列配置
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_Columns(string extension, QueryType queryType)
    {
    }

    /// <summary>
    /// 测试 IgnoreParseError
    /// </summary>
    [InlineData("xlsx", QueryType.Xlsx), Theory]
    public void Test_IgnoreParseError(string extension, QueryType queryType)
    {
        // var purTable = new PurTable { IgnoreParseError = true };
    }
}