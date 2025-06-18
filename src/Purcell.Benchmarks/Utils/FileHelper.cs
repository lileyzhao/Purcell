using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PurcellLibs.Benchmarks;

public static class FileHelper
{
    /// <summary>
    /// 生成导出文件的路径
    /// </summary>
    public static string GenExportFilePath(string? extension, [CallerMemberName] string memberName = "")
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
            $"{memberName}_{DateTime.Now:yyMMddHHmm}_{GenerateCommitStyleHash(memberName + extension)}.{extension}");
    }

    /// <summary>
    /// 获取文件在桌面的路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    public static string GetDesktopFilePath(string fileName, string? extension = null)
    {
        string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Desktop",
            "PurcellTests");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, !string.IsNullOrEmpty(extension) ? fileName + "." + extension : fileName);
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