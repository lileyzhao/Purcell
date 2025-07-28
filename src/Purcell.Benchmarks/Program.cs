using BenchmarkDotNet.Running;

namespace PurcellLibs.Benchmarks;

public static class Program
{
    private static void Main(string[] args)
    {
# if DEBUG
        var bq = new BenchmarkQuery();
        bq.Setup();
        bq.QueryDict_PurcellProviders_Csv();
        // 退出Debug模式下的程序
        Environment.Exit(0);
#endif

        int choice = ShowMenuWithArrowKeys();

        switch (choice)
        {
            case 1:
                RunQuickQueryBenchmark();
                break;
            case 2:
                RunAccurateQueryBenchmark();
                break;
            case 3:
                RunQuickExportBenchmark();
                break;
            case 4:
                RunAccurateExportBenchmark();
                break;
            case 5:
                Console.WriteLine("👋 退出程序...");
                return;
            default:
                Console.WriteLine("❌ 无效选择。");
                break;
        }
    }

    private static int ShowMenuWithArrowKeys()
    {
        string[] menuItems =
        [
            "🚀 快速测试表格查询",
            "🎯 精确测试表格查询",
            "🚀 快速测试导出表格",
            "🎯 精确测试导出表格",
            "❌ 退出程序"
        ];

        int selectedIndex = 0;

        Console.WriteLine("🚀 Purcell 性能基准测试工具");
        Console.WriteLine("📋 请选择测试模式:");
        Console.WriteLine();

        // 初始显示所有菜单项
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == selectedIndex)
            {
                // 选中项高亮显示（使用绿色背景和黑色文字）
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" ⇨ {menuItems[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {menuItems[i]}");
            }
        }

        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true); // true表示不显示按下的键

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : menuItems.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = selectedIndex < menuItems.Length - 1 ? selectedIndex + 1 : 0;
                    break;
                case ConsoleKey.Enter:
                    break;
                default:
                    continue; // 忽略其他按键
            }

            // 重新绘制整个菜单
            Console.SetCursorPosition(0, Console.CursorTop - menuItems.Length);
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    // 选中项高亮显示（使用绿色背景和黑色文字）
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($" ⇨ {menuItems[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {menuItems[i]}");
                }
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine(); // 换行
        return selectedIndex + 1; // 返回1-5的选项编号
    }

    private static void RunQuickQueryBenchmark()
    {
        Console.WriteLine("\n 🚀 正在运行快速测试表格查询...");
        Console.WriteLine("   注意：快速测试结果仅供参考，不用于精确性能分析。");

        // 使用快速测试配置运行查询基准测试
        _ = BenchmarkRunner.Run<BenchmarkQuery>(new QuickTestConfig());
    }

    private static void RunAccurateQueryBenchmark()
    {
        Console.WriteLine("\n 🎯 正在运行精确测试表格查询...");
        Console.WriteLine("   请耐心等待，这可能需要一些时间...");

        // 使用精确测试配置运行查询基准测试
        _ = BenchmarkRunner.Run<BenchmarkQuery>(new AccurateTestConfig());
    }

    private static void RunQuickExportBenchmark()
    {
        Console.WriteLine("\n 🚀 正在运行快速测试导出表格...");
        Console.WriteLine("   注意：快速测试结果仅供参考，不用于精确性能分析。");

        // 使用快速测试配置运行导出基准测试
        _ = BenchmarkRunner.Run<BenchmarkExport>(new QuickTestConfig());
    }

    private static void RunAccurateExportBenchmark()
    {
        Console.WriteLine("\n 🎯 正在运行精确测试导出表格...");
        Console.WriteLine("   请耐心等待，这可能需要一些时间...");

        // 使用精确测试配置运行导出基准测试
        _ = BenchmarkRunner.Run<BenchmarkExport>(new AccurateTestConfig());
    }
}