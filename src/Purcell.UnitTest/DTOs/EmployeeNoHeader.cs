namespace PurcellLibs.UnitTest;

internal class EmployeeNoHeader1
{
    [PurColumn("A")] public int EmpId { get; set; }

    [PurColumn("B")] public string? Xing { get; set; }

    [PurColumn("B")] public string? XingMing { get; set; }
}

internal class EmployeeNoHeader2
{
    [PurColumn(0)] public int EmpId { get; set; }

    [PurColumn(1)] public string? Xing { get; set; }

    [PurColumn(2)] public string? XingMing { get; set; }
}