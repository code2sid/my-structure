using Common2;
using Service.Interfaces.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    internal abstract class BaseRepository : ICacheInvalidator
    {
        protected readonly IDbConnectionManager DbConnectionManager;
        protected readonly CacheService CacheService = new CacheService(TimeSpan.FromMinutes(10));

        protected BaseRepository(IDbConnectionManager dbConnectionManager)
        {
            DbConnectionManager = dbConnectionManager;
        }

        public static void VerifyOneRowAffected(int numberOfRowsAffected, string errorMessage)
        {
            VerifyNumberOfRowsAffected(1, numberOfRowsAffected, errorMessage);
        }

        protected static void VerifyNumberOfRowsAffected(int expectedNumberOfRows, int actualNumberOfRows, string errorMessage)
        {
            if (actualNumberOfRows != expectedNumberOfRows)
            {
                throw new Exception($"Expected {expectedNumberOfRows} row to be affected, but was {actualNumberOfRows}. {errorMessage}");
            }
        }

        public abstract CacheName Name { get; }
        public void Clear() => CacheService.Clear();

        public void Delete(string key) => CacheService.Delete(key);
    }
}
