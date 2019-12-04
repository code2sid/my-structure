using Dapper;
using Model;
using Service.Interfaces.DataAccess;
using Service.Interfaces.DataAccess.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    internal class CashMarketRepository : BaseRepository, ICashMarketRepository
    {
        public CashMarketRepository(IDbConnectionManager dbConnectionManager) : base(dbConnectionManager)
        {
        }

        public Task<IEnumerable<Currency>> GetAll()
        {
            return CacheService.GetOrAdd("all", () =>
            {
                const string sql = @"SELECT Id, Name
                                 FROM cpty.CustomCashMarkets";
                return DbConnectionManager.InvokeOnConnection(ConnectionName.Db1,
                    ConnectionIntent.ReadOnly,
                    connection => connection.QueryAsync<Currency>(sql));
            });
        }

        public override CacheName Name => CacheName.CurrenciesRepo;
    }
}
