using System.Diagnostics;
using MiniExcelLibs;

namespace PurcellLibs.ConsoleApp;

public class TestMemory
{
    public void Test01()
    {
        long minMemory = long.MaxValue; // 最小用量
        long maxMemory = long.MinValue; // 最大用量

        for (int i = 0; i < 1; i++)
        {
            // 在代码执行前收集内存信息
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryBefore = GC.GetTotalMemory(true);

            // 执行要测量的代码
            string fileName = @"Resources/10_0000x11.xlsx";
            string path = Path.Combine(AppContext.BaseDirectory, fileName);
            foreach (dynamic? unused in MiniExcel.Query(path))
            {
                long memoryAfter = GC.GetTotalMemory(true);
                // 计算内存差异（字节数）
                long memoryUsed = memoryAfter - memoryBefore;
                minMemory = Math.Min(minMemory, memoryUsed);
                maxMemory = Math.Max(maxMemory, memoryUsed);
            }

            // 输出格式化美观的结果报告
            Console.Clear();
            Console.WriteLine($"最小内存: {minMemory / 1024} KB");
            Console.WriteLine($"最大内存: {maxMemory / 1024} KB");
        }
    }

    public void Test02()
    {
        Process currentProcess = Process.GetCurrentProcess();

        long memoryBefore = currentProcess.WorkingSet64;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 内存初始: {memoryBefore / 1024} KB");

        int min = 999999999;
        int max = 0;

        // 创建一个定时器，定期输出内存使用情况
        Timer memoryMonitor = new(_ =>
        {
            // 刷新进程信息
            currentProcess.Refresh();
            Console.Clear();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 内存用量");
            Console.WriteLine($"初始内存: {memoryBefore / 1024} KB");
            Console.WriteLine($"当前内存: {currentProcess.WorkingSet64 / 1024} KB");
            Console.WriteLine($"实际初始: {(currentProcess.WorkingSet64 - memoryBefore) / 1024} KB");
            min = Math.Min(min, (int)((currentProcess.WorkingSet64 - memoryBefore) / 1024));
            max = Math.Max(max, (int)((currentProcess.WorkingSet64 - memoryBefore) / 1024));
            Console.WriteLine($"最小内存: {min} KB");
            Console.WriteLine($"最大内存: {max} KB");
        }, null, 0, 100); // 每5秒输出一次

        // 执行要测量的代码
        new ExcelQuery().MiniExcel_Query_Dynamic();

        // 停止定时器
        memoryMonitor.Dispose();
    }
}