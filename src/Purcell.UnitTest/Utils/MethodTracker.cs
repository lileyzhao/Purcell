using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PurcellLibs.UnitTest;

public class MethodTracker
{
    /// <summary>
    /// 获取当前方法名
    /// </summary>
    public static string GetMethodName([CallerMemberName] string memberName = "")
    {
        return memberName;
    }
    
    /// <summary>
    /// 获取当前方法的完整信息
    /// </summary>
    public static MethodInfo GetMethodInfo(
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        return new MethodInfo
        {
            MethodName = memberName,
            FilePath = filePath,
            LineNumber = lineNumber,
            FileName = Path.GetFileName(filePath)
        };
    }
    
    /// <summary>
    /// 获取调用者的完整方法名（包括类名）
    /// </summary>
    public static string GetFullMethodName([CallerMemberName] string memberName = "")
    {
        var stackTrace = new StackTrace();
        var frame = stackTrace.GetFrame(1);
        var method = frame?.GetMethod();
        var className = method?.DeclaringType?.Name ?? "Unknown";
        return $"{className}.{memberName}";
    }
    
    /// <summary>
    /// 完整方法信息（包括类名、文件路径和行号）
    /// </summary>
    public class MethodInfo
    {
        public string? MethodName { get; init; }
        
        public string? FilePath { get; init; }
        
        public string? FileName { get; init; }
        
        public int LineNumber { get; init; }

        public override string ToString()
        {
            return $"{MethodName} in {FileName} at line {LineNumber}";
        }
    }
}