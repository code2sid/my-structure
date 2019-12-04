using DataAccess.Model;
using System;
using System.Data.SqlClient;

namespace DataAccess.Helpers
{
    public class ConnectionStringHelper : IConnectionStringHelper
    {
        public string UpdateApplicationName(Version version, string connectionString, int? appId = null, string appName = null)
        {
            return this.UpdateConnectionString(connectionString, new ConnectionStringDetails()
            {
                Version = version,
                AppId = appId,
                AppName = appName
            });
        }

        public string UpdateConnectionString(string connectionString, ConnectionStringDetails details)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            int num = details.AppId ?? details.Version.Major;
            string str = details.AppName ?? connectionStringBuilder.ApplicationName;
            connectionStringBuilder.ApplicationName = string.Format("{0}:{1}:{2}", (object)num, (object)details.Version, (object)str);
            if (details.ApplicationIntent.HasValue)
                connectionStringBuilder.ApplicationIntent = details.ApplicationIntent.Value;
            if (details.MultiSubnetFailover.HasValue)
                connectionStringBuilder.MultiSubnetFailover = details.MultiSubnetFailover.Value;
            return connectionStringBuilder.ToString();
        }
    }
}
