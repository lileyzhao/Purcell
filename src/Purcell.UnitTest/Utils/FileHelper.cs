using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PurcellLibs.UnitTest;

public static class FileHelper
{
    /// <summary>
    /// 获取导出文件的路径
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static string GenExportFilePath(string extension, [CallerMemberName] string memberName = "")
    {
        string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Desktop",
            "PurcellTests");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath,
            $"{memberName + extension}_{DateTime.Now:yyMMddHHmm}_{GenerateCommitStyleHash(memberName)}.{extension}");
    }

    private static string GenerateCommitStyleHash(string val, int length = 8)
    {
        // 模拟Git提交对象的内容结构
        var commitContent =
            $"blob {val}\0{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\0{Environment.MachineName}\0{Guid.NewGuid()}";

        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(commitContent));
        return Convert.ToHexString(hash)[..length].ToLower();
    }
}