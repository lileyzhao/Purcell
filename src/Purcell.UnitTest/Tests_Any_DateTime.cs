// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Any_DateTime
{
    [Fact]
    public void TestDateTime()
    {
        string filePath = FileHelper.GenExportFilePath("xlsx");
        List<object> objects = new();
        for (DateTime i = new(1900, 3, 2, 2, 38, 23); i >= DateTime.MinValue.AddDays(1); i = i.AddDays(-1))
        {
            objects.Add(new { DtString = i.ToString("yyyy-MM-dd HH:mm:ss"), DateTime = i });
        }

        Purcell.Export(objects, filePath);

        File.Delete(filePath); // 删除测试文件
    }
}