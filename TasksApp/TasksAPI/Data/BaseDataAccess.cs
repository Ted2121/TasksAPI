using System.Data.SqlClient;

namespace TasksAPI.Data;

public abstract class BaseDataAccess
{
    private string _connectionstring;

    protected BaseDataAccess(string connectionstring) => _connectionstring = connectionstring;
    
    protected SqlConnection CreateConnection() => new SqlConnection(_connectionstring);
}
