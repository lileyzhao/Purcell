namespace PurcellLibs.UnitTest;

internal class EmployeeHasHeader
{
    [PurColumn("员工编号", "员工ID")] public int EmpId { get; set; }

    [PurColumn("姓氏")] public string? LastName { get; set; }

    [PurColumn("姓名")] public string? RealName { get; set; }
}