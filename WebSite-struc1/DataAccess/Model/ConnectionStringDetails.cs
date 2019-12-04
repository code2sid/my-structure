using System;

namespace DataAccess.Model
{
    public class ConnectionStringDetails
    {
        public int? AppId { get; set; }

        public Version Version { get; set; }

        public string AppName { get; set; }

        public System.Data.SqlClient.ApplicationIntent? ApplicationIntent { get; set; }

        public bool? MultiSubnetFailover { get; set; }
    }
}
