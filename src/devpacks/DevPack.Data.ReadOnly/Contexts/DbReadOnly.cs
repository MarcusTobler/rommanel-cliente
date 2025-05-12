using System.Data;
using Npgsql;

namespace DevPack.Data.ReadOnly;

public class DbReadOnly : IDbReadOnly 
{
    public IDbConnection Connection { get; protected set; }
    
    public DbReadOnly(string? connectionString = null)
    {
        Connection = new NpgsqlConnection("rommaneldb");
        Connection.Open();
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}