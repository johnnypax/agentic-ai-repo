namespace CompanyDirectoryMcpServer.Models;

public record CompanyUser(
    int Id,
    string EmployeeCode,
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    string DepartmentName,
    string CostCenter,
    string? ManagerEmployeeCode,
    string Location,
    bool IsActive,
    DateTime HireDate
);