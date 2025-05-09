using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Backend.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string storedProcedure, object? parameters = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> ExecuteAsync(string storedProcedure, object? parameters = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteAsync(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
    }
} 