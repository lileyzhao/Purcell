using System.Text;
using PurcellLibs.Utilities;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace PurcellLibs.UnitTest;

public class Tests_Any_Encoding(ITestOutputHelper testHelper)
{
    [Fact]
    public void TestEncoding()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Dictionary<string, Encoding> encodingDict = new()
        {
            { "Encoding_Gb2312", Encoding.GetEncoding("GB18030") },
            { "Encoding_Gbk", Encoding.GetEncoding("GB18030") },
            { "Encoding_Utf8", Encoding.UTF8 },
            { "Encoding_Utf8Bom", Encoding.UTF8 },
            { "Encoding_Utf16Be", Encoding.BigEndianUnicode },
            { "Encoding_Utf16Le", Encoding.Unicode }
        };

        int rowIndex = -1;
        foreach (string key in encodingDict.Keys)
        {
            rowIndex++;
            string filePath = $"Resources/{key}.csv";
            Encoding? detectEncoding = EncodingUtils.DetectEncoding(filePath);
            Assert.Equal(encodingDict[key].WebName, detectEncoding?.WebName);
            testHelper.WriteLine($"第 {rowIndex + 1} 行：{key} - {detectEncoding?.WebName ?? "NULL"}");
        }
    }
}