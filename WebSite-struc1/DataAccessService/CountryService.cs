using Common2;
using DataAccessService.Interfaces.DataAccess.Repository;
using Model;
using Service.Interfaces.DataAccessService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessService
{
    internal class CountryService : ICountryService
    {
        private readonly ISecurityMarketRepository _securityMarketRepository;
        private readonly ISecurityMasterService _securityMasterService;

        public CountryService(
            ISecurityMarketRepository securityMarketRepository,
            ISecurityMasterService securityMasterService)
        {
            _securityMarketRepository = securityMarketRepository;
            _securityMasterService = securityMasterService;
        }
        public async Task<IEnumerable<Country>> GetAll()
        {
            var (countries, customSecurityMarkets) = await TaskHelper.RunInParallel(
                _securityMasterService.GetAllCountries(),
                _securityMarketRepository.GetAll()).Caf();

            return customSecurityMarkets
                    .Select(_ => new Country(
                        _.Name,
                        _.Id,
                        Enumerable.Empty<Currency>())).Concat(countries);
        }
    }
}
