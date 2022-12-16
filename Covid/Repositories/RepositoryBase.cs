using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Covid.Enums;
using Covid.Repositories.Interfaces;
using Covid.Services.Interfaces;

namespace Covid.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILoggerService _loggerService;
        private readonly EnumDbConnection _connectionName;

        protected RepositoryBase(IDbConnectionFactory dbConnectionFactory, ILoggerService loggerService, EnumDbConnection connectionName)
        {
            _loggerService = loggerService;
            _dbConnectionFactory = dbConnectionFactory;
            _connectionName = connectionName;
        }

        protected IEnumerable<T> QuerySP<T>(string spName, object parameters = null)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateDbConnection(_connectionName))
                {
                    connection.Open();
                    return connection.Query<T>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                _loggerService.Error($"connectionString :{_dbConnectionFactory.CreateDbConnection(_connectionName).ConnectionString}");
                _loggerService.Error($"spName:{spName}", e);
                return null;
            }
        }
    }
}