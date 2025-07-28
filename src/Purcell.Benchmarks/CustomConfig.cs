using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Perfolizer.Horology;
using Perfolizer.Metrology;

namespace PurcellLibs.Benchmarks;

public class QuickTestConfig : TestConfigBase
{
    public QuickTestConfig()
    {
        // 🔥 关键修改：创建快速测试Job
        Job quickJob = Job.Default
            .WithWarmupCount(1) // 只预热1次（默认是多次）
            .WithIterationCount(3) // 运行3次迭代以获得更稳定的结果
            .WithInvocationCount(1) // 每次迭代只调用1次
            .WithUnrollFactor(1) // 不展开循环
            .WithToolchain(InProcessEmitToolchain.Instance); // 使用进程内工具链，更快

        // 添加作业并设置测试配置
        AddJob(quickJob);
        SetTestConfig();
    }
}

public class AccurateTestConfig : TestConfigBase
{
    public AccurateTestConfig()
    {
        // 添加作业 - 使用更精确的配置
        Job accurateJob = Job.Default
            .WithWarmupCount(5) // 标准预热次数
            .WithIterationCount(10) // 更多迭代次数以获得更稳定的结果
            .WithToolchain(InProcessEmitToolchain.Instance); // 使用进程内工具链

        // 添加作业并设置测试配置
        AddJob(accurateJob);
        SetTestConfig();
    }
}

public class TestConfigBase : ManualConfig
{
    protected void SetTestConfig()
    {
        // 添加日志记录器
        AddLogger(ConsoleLogger.Default);

        // 添加默认的列提供者
        AddColumnProvider(DefaultColumnProviders.Instance);

        // 添加诊断器
        AddDiagnoser(MemoryDiagnoser.Default); // 内存诊断

        // 设置摘要样式，使用毫秒作为时间单位
        WithSummaryStyle(SummaryStyle.Default
            .WithTimeUnit(TimeUnit.Millisecond) // 使用毫秒作为时间单位
            .WithSizeUnit(SizeUnit.KB) // 使用 KB 作为大小单位
            .WithRatioStyle(RatioStyle.Percentage)); // 使用百分比显示比率

        // 添加导出器
        AddExporter(MarkdownExporter.GitHub); // 导出为GitHub Markdown
        AddExporter(CsvExporter.Default); // 导出为CSV
        AddExporter(HtmlExporter.Default); // 导出为HTML
        AddExporter(JsonExporter.Default); // 导出为JSON

        // 启用或禁用特定功能
        WithOptions(ConfigOptions.JoinSummary | // 合并摘要
                    ConfigOptions.StopOnFirstError); // 首次错误时停止
    }
}