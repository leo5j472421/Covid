using System.Data;
using Covid.Enums;

namespace Covid.Repositories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(EnumDbConnection dbConnection);

        IDbConnection CreateDbConnection(string connectionString);
    }
}