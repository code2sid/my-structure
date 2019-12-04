using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccess
{
    internal interface IDbConnectionManager
    {
        Task InvokeOnConnection(ConnectionName connectionName, ConnectionIntent intent, Func<DbConnection, Task> action);
        Task<T> InvokeOnConnection<T>(ConnectionName connectionName, ConnectionIntent intent, Func<DbConnection, Task<T>> func);
    }
}
