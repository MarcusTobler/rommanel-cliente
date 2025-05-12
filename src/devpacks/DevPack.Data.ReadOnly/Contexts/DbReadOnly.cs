using System.Data;
using Npgsql;

namespace DevPack.Data.ReadOnly;

public class DbReadOnly : IDbReadOnly 
{
    public IDbConnection Connection { get; protected set; }
    
    public DbReadOnly(string? connectionString = null)
    {
        Connection = new NpgsqlConnection("Server=localhost;Port=5432;Database=ctaclaimstrack;User ID=claimstrack;Password=s4dkL5SALkrj3dZ;");
        Connection.Open();
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}