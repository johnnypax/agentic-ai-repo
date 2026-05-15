namespace CompanyDirectoryMcpServer.Models;

public record DepartmentSummary(
    string DepartmentName,
    string CostCenter,
    int ActiveUsers,
    int InactiveUsers,
    int TotalUsers
);