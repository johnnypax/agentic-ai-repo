using System.Data;
using CompanyDirectoryMcpServer.Models;
using Microsoft.Data.SqlClient;

public class CompanyUserRepository
{
    private readonly string _connectionString;
    public CompanyUserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CompanyDirectory") ??
            throw new InvalidOperationException("Connection string not found");
    }

    private async Task<List<CompanyUser>> ExecuteQueryAsync(string sql, params SqlParameter[] parameters)
    {
        var users = new List<CompanyUser>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new CompanyUser(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("EmployeeCode")),
                reader.GetString(reader.GetOrdinal("FirstName")),
                reader.GetString(reader.GetOrdinal("LastName")),
                reader.GetString(reader.GetOrdinal("Email")),
                reader.GetString(reader.GetOrdinal("JobTitle")),
                reader.GetString(reader.GetOrdinal("DepartmentName")),
                reader.GetString(reader.GetOrdinal("CostCenter")),
                reader.IsDBNull(reader.GetOrdinal("ManagerEmployeeCode")) ? null : reader.GetString(reader.GetOrdinal("ManagerEmployeeCode")),
                reader.GetString(reader.GetOrdinal("Location")),
                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                reader.GetDateTime(reader.GetOrdinal("HireDate"))
            ));
        }

        return users;
    }

    public Task<List<CompanyUser>> GetActiveUsersAsync()
    {
        const string sql = """
        SELECT 
            u.Id,
            u.EmployeeCode,
            u.FirstName,
            u.LastName,
            u.Email,
            u.JobTitle,
            d.Name AS DepartmentName,
            d.CostCenter,
            u.ManagerEmployeeCode,
            u.Location,
            u.IsActive,
            u.HireDate
        FROM CompanyUsers u
        INNER JOIN Departments d ON u.DepartmentId = d.Id
        WHERE u.IsActive = 1
        ORDER BY u.LastName, u.FirstName;
        """;

        return ExecuteQueryAsync(sql);
    }

    public async Task<CompanyUser?> GetUserByEmployeeCodeAsync(string employeeCode)
    {
        const string sql = """
        SELECT 
            u.Id,
            u.EmployeeCode,
            u.FirstName,
            u.LastName,
            u.Email,
            u.JobTitle,
            d.Name AS DepartmentName,
            d.CostCenter,
            u.ManagerEmployeeCode,
            u.Location,
            u.IsActive,
            u.HireDate
        FROM CompanyUsers u
        INNER JOIN Departments d ON u.DepartmentId = d.Id
        WHERE u.EmployeeCode = @EmployeeCode;
        """;

        var users = await ExecuteQueryAsync(sql, new SqlParameter("@EmployeeCode", employeeCode));
        return users.FirstOrDefault();
    }

    public async Task<List<CompanyUser>> SearchUsersAsync(string searchTerm)
    {
        const string sql = """
        SELECT 
            u.Id,
            u.EmployeeCode,
            u.FirstName,
            u.LastName,
            u.Email,
            u.JobTitle,
            d.Name AS DepartmentName,
            d.CostCenter,
            u.ManagerEmployeeCode,
            u.Location,
            u.IsActive,
            u.HireDate
        FROM CompanyUsers u
        INNER JOIN Departments d ON u.DepartmentId = d.Id
        WHERE 
            u.FirstName LIKE @SearchTerm OR 
            u.LastName LIKE @SearchTerm OR 
            u.Email LIKE @SearchTerm OR 
            d.Name LIKE @SearchTerm
        ORDER BY u.LastName, u.FirstName;
        """;

        return await ExecuteQueryAsync(sql, new SqlParameter("@SearchTerm", $"%{searchTerm}%"));
    }

    public async Task<List<CompanyUser>> GetUserByDepartmentAsync(string departmentName)
    {
        const string sql = """
        SELECT 
            u.Id,
            u.EmployeeCode,
            u.FirstName,
            u.LastName,
            u.Email,
            u.JobTitle,
            d.Name AS DepartmentName,
            d.CostCenter,
            u.ManagerEmployeeCode,
            u.Location,
            u.IsActive,
            u.HireDate
        FROM CompanyUsers u
        INNER JOIN Departments d ON u.DepartmentId = d.Id
        WHERE d.Name = @DepartmentName
        ORDER BY u.LastName, u.FirstName;
        """;

        return await ExecuteQueryAsync(sql, new SqlParameter("@DepartmentName", departmentName));
    }

    public async Task<List<CompanyUser>> GetUsersByManagerAsync(string managerEmployeeCode)
    {
        const string sql = """
        SELECT 
            u.Id,
            u.EmployeeCode,
            u.FirstName,
            u.LastName,
            u.Email,
            u.JobTitle,
            d.Name AS DepartmentName,
            d.CostCenter,
            u.ManagerEmployeeCode,
            u.Location,
            u.IsActive,
            u.HireDate
        FROM CompanyUsers u
        INNER JOIN Departments d ON u.DepartmentId = d.Id
        WHERE u.ManagerEmployeeCode = @ManagerEmployeeCode
        ORDER BY u.LastName, u.FirstName;
        """;

        return await ExecuteQueryAsync(sql, new SqlParameter("@ManagerEmployeeCode", managerEmployeeCode));
    }

    public async Task<List<DepartmentSummary>> GetDepartmentSummaryAsync()
    {
        const string sql = """
        SELECT 
            d.Name AS DepartmentName,
            d.CostCenter,
            SUM(CASE WHEN u.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveUsers,
            SUM(CASE WHEN u.IsActive = 0 THEN 1 ELSE 0 END) AS InactiveUsers,
            COUNT(*) AS TotalUsers
        FROM Departments d
        LEFT JOIN CompanyUsers u ON u.DepartmentId = d.Id
        GROUP BY d.name, d.CostCenter
        ORDER BY d.Name;
        """;

        var summaries = new List<DepartmentSummary>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            summaries.Add(new DepartmentSummary(
                reader.GetString(reader.GetOrdinal("DepartmentName")),
                reader.GetString(reader.GetOrdinal("CostCenter")),
                reader.GetInt32(reader.GetOrdinal("ActiveUsers")),
                reader.GetInt32(reader.GetOrdinal("InactiveUsers")),
                reader.GetInt32(reader.GetOrdinal("TotalUsers"))
            ));
        }

        return summaries;
    }
}