using System.Data;

namespace DevPack.Data.ReadOnly;

public interface IDbReadOnly : IDisposable
{
    IDbConnection Connection { get;  }
}