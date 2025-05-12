using System.Data;

namespace DevPack.Data.Abstractions;

public interface IRepository
{
    IDbConnection Connection { get;  }
    IDbTransaction? Transaction { get; }
}