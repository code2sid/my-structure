using Common2;
using Common2.Interfaces;
using DataAccess.Helpers;
using DataAccess.Model;
using Model;
using Service.Interfaces.Service;
using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum ConnectionIntent
    {
        ReadOnly,
        ReadWrite
    }

    internal class DbConnectionManager : IDbConnectionManager, IDisposable
    {
        private readonly IConfigurationService _configurationService;
        private readonly bool _storeConnections;
        private readonly ConcurrentStack<DbConnection> _dbConnections;
        private readonly ApplicationNameAndVersion _appVersion;
        private readonly IConnectionStringHelper _connectionStringHelper;
        private readonly ILoggingService<DbConnectionManager> _loggingService;

        public DbConnectionManager(IConfigurationService configurationService, ApplicationNameAndVersion appVersion, IConnectionStringHelper connectionStringHelper, ILoggingService<DbConnectionManager> loggingService, bool storeConnections = true)
        {
            _configurationService = configurationService;
            _appVersion = appVersion;
            _connectionStringHelper = connectionStringHelper;
            _loggingService = loggingService;
            _storeConnections = storeConnections;
            _dbConnections = new ConcurrentStack<DbConnection>();
        }

        private async Task<DbConnection> GetConnection(ConnectionName connectionName, ConnectionIntent intent)
        {
            ApplicationIntent applicationIntent;
            switch (intent)
            {
                case ConnectionIntent.ReadOnly:
                    applicationIntent = ApplicationIntent.ReadOnly;
                    break;
                case ConnectionIntent.ReadWrite:
                    applicationIntent = ApplicationIntent.ReadWrite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(intent), $"{intent} is not a valid value for {typeof(ConnectionIntent)}");
            }
            var conn = await OpenConnection(connectionName, applicationIntent).ConfigureAwait(false);
            if (_storeConnections)
            {
                _dbConnections.Push(conn);
            }
            return conn;
        }

        public async Task InvokeOnConnection(ConnectionName connectionName, ConnectionIntent intent,
            Func<DbConnection, Task> action)
        {
            using (var connection = await GetConnection(connectionName, intent))
            {
                await action(connection);
            }
        }

        public async Task<T> InvokeOnConnection<T>(ConnectionName connectionName, ConnectionIntent intent,
            Func<DbConnection, Task<T>> func)
        {
            using (var connection = await GetConnection(connectionName, intent))
            {
                return await func(connection);
            }
        }

        private async Task<DbConnection> OpenConnection(ConnectionName name, ApplicationIntent applicationIntent)
        {
            _loggingService.Debug($"Getting connection {name}:{applicationIntent}");
            var connStr = await ResolveConnString(name.ToString(), applicationIntent).Caf();
            var cnn = new SqlConnection(connStr);
            cnn.Open();
            return cnn;
        }

        private async Task<string> ResolveConnString(string name, ApplicationIntent intent)
        {
            var connStr = await _configurationService.GetConfigValue(name).Caf();
            if (string.IsNullOrEmpty(connStr))
            {
                throw new ArgumentException($"Connection string named \"{name}\" not found.");
            }

            return _connectionStringHelper.UpdateConnectionString(
                connStr,
                new ConnectionStringDetails
                {
                    AppName = _appVersion.ApplicationName,
                    Version = _appVersion.Version,
                    ApplicationIntent = intent,
                    MultiSubnetFailover = true
                });
        }

        public void Dispose()
        {
            while (_dbConnections.TryPop(out DbConnection connection))
            {
                connection.Dispose();
            }
        }
    }
}
