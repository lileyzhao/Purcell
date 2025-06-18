using System.Diagnostics;
using BenchmarkDotNet.Engines;
using MiniExcelLibs;
using Spectre.Console;

namespace PurcellLibs.Benchmarks;

public static class SimpleTest
{
    public static async Task Run()
    {
        // 显示标题
        AnsiConsole.Write(new FigletText("Simple Test").Centered().Color(Color.Green));

        // 创建结果存储
        List<(string Name, string Category, long ElapsedMs)> results = new();

        // 设置测试环境
        AnsiConsole.Status()
            .Start("准备测试环境...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                new BenchmarkLarge().Setup();
                ctx.Status("测试环境准备完成");
                Thread.Sleep(1000); // 让用户看到完成信息
            });

        // 运行所有基准测试
        await AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .StartAsync(async ctx =>
            {
                // 定义测试任务
                (string Name, Action Action, string Category)[] benchmarks =
                [
                    (Name: "MiniExcel First CSV", Action: () => TestMiniExcelFirst("csv"), Category: "查询CSV首行"),
                    (Name: "Purcell First CSV", Action: () => TestPurcellFirst("csv"), Category: "查询CSV首行"),
                    (Name: "MiniExcel First Xlsx", Action: () => TestMiniExcelFirst("xlsx"), Category: "查询Xlsx首行"),
                    (Name: "Purcell First Xlsx", Action: () => TestPurcellFirst("xlsx"), Category: "查询Xlsx首行"),
                    (Name: "MiniExcel 100w CSV", Action: () => TestMiniExcelLarge("csv"), Category: "百万CSV数据集"),
                    (Name: "Purcell 100w CSV", Action: () => TestPurcellLarge("csv"), Category: "百万CSV数据集"),
                    (Name: "MiniExcel 100w Xlsx", Action: () => TestMiniExcelLarge("xlsx"), Category: "百万Xlsx数据集"),
                    (Name: "Purcell 100w Xlsx", Action: () => TestPurcellLarge("xlsx"), Category: "百万Xlsx数据集")
                ];

                // 创建进度任务
                ProgressTask task = ctx.AddTask("[green]运行基准测试[/]", maxValue: benchmarks.Length);

                // 执行每个基准测试
                foreach ((string Name, Action Action, string Category) benchmark in benchmarks)
                {
                    // 更新当前任务描述
                    task.Description = $"[cyan]正在测试: {benchmark.Name}[/]";

                    // 执行测试并计时
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    benchmark.Action();
                    stopwatch.Stop();

                    // 存储结果
                    results.Add((benchmark.Name, benchmark.Category, stopwatch.ElapsedMilliseconds));

                    // 更新进度
                    task.Increment(1);

                    // 模拟短暂暂停，让进度变化
                    await Task.Delay(200);
                }
            });

        // 显示结果表格
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[yellow]测试结果[/]").RuleStyle("grey").Centered());
        AnsiConsole.WriteLine();

        // 创建并显示结果表格
        Table table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[bold]Excel 读取性能对比[/]")
            .AddColumn(new TableColumn("[u]测试项目[/]").LeftAligned())
            .AddColumn(new TableColumn("[u]数据集[/]").LeftAligned())
            .AddColumn(new TableColumn("[u]耗时 (ms)[/]").RightAligned());

        // 按类别分组并添加到表格
        foreach (string category in results.Select(r => r.Category).Distinct())
        {
            List<(string Name, string Category, long ElapsedMs)> categoryResults =
                results.Where(r => r.Category == category).ToList();

            // 添加类别分隔行
            table.AddRow(new Markup($"[bold blue]{category}[/]"), new Markup(""), new Markup(""));

            // 添加该类别的所有结果
            foreach ((string Name, string Category, long ElapsedMs) result in categoryResults)
            {
                table.AddRow(result.Name, category, $"[bold]{result.ElapsedMs} ms[/]");
            }

            // 如果不是最后一个类别，添加空行
            if (category != results.Select(r => r.Category).Distinct().Last())
                table.AddEmptyRow();
        }

        // 显示表格
        AnsiConsole.Write(table);

        // 添加结论
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel(
                Align.Center(new Markup("[bold yellow]测试完成！[/] 耗时越少表示性能越好。")))
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Grey));
    }

    private static void TestPurcellFirst(string ext)
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", ext);
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            consumer.Consume(item); // 防止被编译器优化掉
            break;
        }
    }

    private static void TestMiniExcelFirst(string ext)
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", ext);
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in MiniExcel.Query(filePath, true))
        {
            consumer.Consume(item); // 防止被编译器优化掉
            break;
        }
    }

    private static void TestPurcellLarge(string ext)
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", ext);
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in Purcell.Query(filePath))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }

    private static void TestMiniExcelLarge(string ext)
    {
        string filePath = FileHelper.GetDesktopFilePath("100_0000x10", ext);
        Consumer consumer = new();
        foreach (IDictionary<string, object?> item in MiniExcel.Query(filePath))
        {
            consumer.Consume(item); // 防止被编译器优化掉
        }
    }
}