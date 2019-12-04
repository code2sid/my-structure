using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.DataAccess
{
    public interface ICacheInvalidator
    {
        CacheName Name { get; }
        void Clear();
        void Delete(string key);
    }

    public enum CacheName
    {
        AccountsInvestmentsRepo,
        ApprovalSettingsRepo,
        RequestRepo,
        CurrenciesRepo,
        CountriesRepo,
        SecurityMarketService
    }
}
