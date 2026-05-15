using System.ComponentModel;
using CompanyDirectoryMcpServer.Models;
using ModelContextProtocol.Server;

[McpServerToolType]
public class CompanyUserTools(CompanyUserRepository companyUserRepository)
{
    [McpServerTool]                 // GetActiveUsers -> get-active-users
    [Description("Retrieves a list of all active company users.")]
    public async Task<List<CompanyUser>> GetActiveUsersAsync()
    {
        return await companyUserRepository.GetActiveUsersAsync();
    }

    [McpServerTool]
    [Description("Retrieve a user by their employee code.")]
    public async Task<CompanyUser?> GetUserByEmployeeCodeAsync(string employeeCode)
    {
        return await companyUserRepository.GetUserByEmployeeCodeAsync(employeeCode);
    }

    [McpServerTool]
    [Description("Search for users by name, job title, or department.")]
    public async Task<List<CompanyUser>> SearchUsersAsync(string searchTerm)
    {
        return await companyUserRepository.SearchUsersAsync(searchTerm);    
    }

    

    [McpServerTool]
    [Description("Return company users  belonging to a specific department.")]
    public async Task<List<CompanyUser>> GetUserByDepartmentAsync(string department)
    {
        return await companyUserRepository.GetUserByDepartmentAsync(department);
    }

    [McpServerTool]
    [Description("Return company users that report to a specific manager.")]
    public async Task<List<CompanyUser>> GetUsersByManagerAsync(string managerEmployeeCode)
    {
        return await companyUserRepository.GetUsersByManagerAsync(managerEmployeeCode); 
    }

    [McpServerTool]
    [Description("Return a summary for depatment, including total users, active users, and average tenure.")]
    public async Task<List<DepartmentSummary>> GetDepartmentSummaryAsync(string department)
    {
        return await companyUserRepository.GetDepartmentSummaryAsync();
    }
}