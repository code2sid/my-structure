using Dapper;
using DataAccessService.Interfaces.DataAccess.Repository;
using Model;
using Service.Interfaces.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    internal class SecurityMarketRepository : BaseRepository, ISecurityMarketRepository
    {
        public SecurityMarketRepository(IDbConnectionManager dbConnectionManager) :
            base(dbConnectionManager)
        {
        }

        public Task<IEnumerable<SecurityMarket>> GetAll()
        {
            const string sql = @"SELECT Name, Id
                                 FROM cpty.CustomSecurityMarkets";
            return DbConnectionManager.InvokeOnConnection(ConnectionName.Db1, ConnectionIntent.ReadWrite,
                connection => connection.QueryAsync<SecurityMarket>(sql));
        }


        public override CacheName Name => CacheName.CountriesRepo;
    }
}
