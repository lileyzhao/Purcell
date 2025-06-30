#if RELEASE
using BenchmarkDotNet.Running;
#endif

namespace PurcellLibs.Benchmarks;

internal static class Program
{
#if DEBUG
    private static async Task Main(string[] args)
    {
        // await SimpleTest.Run();
        // new BenchmarkQuery().QueryDict_Purcell_Xlsx();
        new BenchmarkQuery().QueryDict_PurcellProviders_Csv();
    }
#else
    static void Main(string[] args)
    {
        // 生成的样本数据的表格文件会存放在用户桌面上
        // _ = BenchmarkRunner.Run<BenchmarkLarge>();
        // _ = BenchmarkRunner.Run<BenchmarkExport>();
        _ = BenchmarkRunner.Run<BenchmarkQuery>();
    }
#endif
}