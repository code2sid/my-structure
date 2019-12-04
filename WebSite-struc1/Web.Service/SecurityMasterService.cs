using Common2;
using DataAccessService;
using Model;
using Service.Interfaces.DataAccess;
using Service.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Service
{
    internal class SecurityMasterService : ISecurityMasterService, ICacheInvalidator
    {
        private readonly IRefDataService _refDataService;
        private readonly IConfigurationService _configurationService;
        private const string SecMasterApiUrl = "SecMasterAPIUrl";
        private const string PaasAuthUrl = "PaasAuthUrl";
        private readonly CacheService _cacheService = new CacheService(TimeSpan.FromMinutes(10));

        public CacheName Name => CacheName.SecurityMarketService;
        public void Clear() => _cacheService.Clear();

        public void Delete(string key) => _cacheService.Delete(key);

        public SecurityMasterService(IRefDataService refdataService, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _refDataService = refdataService;
        }

        private async Task<HttpPaasClient> GetRefDataHttpPaasClient()
        {
            var refDataUrl = await _refDataService.GetBasePaasUrl();
            var authUrl = await GetPaasAuthUrl().Caf();

            return new HttpPaasClient(authUrl, refDataUrl);
        }

        private async Task<string> GetPaasAuthUrl()
        {
            var authUrl = await _configurationService.GetConfigValue(PaasAuthUrl).Caf();
            if (string.IsNullOrEmpty(authUrl))
            {
                throw new ArgumentException(
                    $"Configuration for \"{PaasAuthUrl}\" not found. Please contact {Constants.SupportTeamEmailAddress} to add configuration for \"{authUrl}\"");
            }

            return authUrl;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var allCurrencies = await GetCurrencyResultDto().Caf();

            return await _cacheService.GetOrAdd($"allCountries", () =>
            {
                return GetRefDataHttpPaasClient()
                    .PipeAsync(client => client.Get<IEnumerable<CountryResultDto>>("/v1/countries"))
                    .Unwrap()
                    .PipeAsync(countries => countries.Select(c => ToCountry(c, allCurrencies)));
            });
        }

        private Task<IEnumerable<CurrencyResultDto>> GetCurrencyResultDto()
        {
            return GetRefDataHttpPaasClient()
                .PipeAsync(client => client.Get<IEnumerable<CurrencyResultDto>>("/v1/currencies"))
                .Unwrap();
        }

        private static Country ToCountry(CountryResultDto dto, IEnumerable<CurrencyResultDto> allCurrencies)
        {
            if (dto.Currencies == null) return new Country(dto.Name, dto.IsoAlpha3Code, null);

            var currencies = dto.Currencies
                .Select(currencyId => allCurrencies.SingleOrDefault(c => c.RefId == currencyId))
                .Select(currency =>
                {
                    if (currency != null) return new Currency(currency.IsoAlphaCode, currency.Name);
                    else throw new ArgumentException($"Currency not found.");
                }).ToList();

            return new Country(dto.Name, dto.IsoAlpha3Code, currencies);
        }

        private class CurrencyResultDto
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local   
            public string Name { get; set; }
            public string IsoAlphaCode { get; set; }
            public int RefId { get; set; }
        }

        private class CountryResultDto
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local   
            public string Name { get; set; }
            public string IsoAlpha3Code { get; set; }
            public int[] Currencies { get; set; }
        }
    }
}
