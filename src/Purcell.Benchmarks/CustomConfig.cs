using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Perfolizer.Horology;
using Perfolizer.Metrology;

namespace PurcellLibs.Benchmarks;

public class CustomConfig : ManualConfig
{
    public CustomConfig()
    {
        // 🔥 关键修改：创建快速测试Job
        var quickJob = Job.Default
            .WithWarmupCount(1)      // 只预热1次（默认是多次）
            .WithIterationCount(2)   // 只运行3次迭代（默认是更多）
            .WithInvocationCount(1)  // 每次迭代只调用1次
            .WithUnrollFactor(1)     // 不展开循环
            .WithToolchain(InProcessEmitToolchain.Instance); // 使用进程内工具链，更快
        
        // 添加作业
        // AddJob(Job.Default);
        AddJob(quickJob);

        // 添加诊断器
        AddDiagnoser(MemoryDiagnoser.Default); // 内存诊断

        // 只保留Ratio列
        HideColumns("RatioSD", "Alloc Ratio");

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