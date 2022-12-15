using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Covid.Enums;
using Covid.Repositories.Interfaces;

namespace Covid.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDictionary<EnumDbConnection, string> _connections;

        public DbConnectionFactory(IDictionary<EnumDbConnection, string> connections) => _connections = connections;

        public IDbConnection CreateDbConnection(EnumDbConnection dbConnection)
        {
            if (_connections.TryGetValue(dbConnection, out var connectionString))
            {
                return new SqlConnection(connectionString);
            }

            throw new ArgumentNullException();
        }

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}