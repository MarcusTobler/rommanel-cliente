using System.Data;

namespace DevPack.Data.ReadOnly.Services;

public interface IDbReadOnlyService
{
    IDbConnection? Connection { get;  }
}