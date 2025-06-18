#if RELEASE
using BenchmarkDotNet.Running;
#endif
namespace PurcellLibs.Benchmarks;

internal static class Program
{
#if DEBUG
    private static async Task Main(string[] args)
    {
        await SimpleTest.Run();
    }
#else
    static void Main(string[] args)
    {
        // 生成的样本数据的表格文件会存放在用户桌面上
        // _ = BenchmarkRunner.Run<BenchmarkLarge>();
        _ = BenchmarkRunner.Run<BenchmarkExport>();
    }
#endif
}