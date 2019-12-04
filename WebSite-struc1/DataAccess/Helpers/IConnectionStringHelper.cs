using DataAccess.Model;
using System;

namespace DataAccess.Helpers
{
    public interface IConnectionStringHelper
    {
        string UpdateApplicationName(Version version, string connectionString, int? appId = null, string appName = null);

        string UpdateConnectionString(string connectionString, ConnectionStringDetails details);
    }
}
