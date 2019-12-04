using Common2;
using Model;
using Model.Dtos;
using Service.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Service
{
    internal class RefDataService : IRefDataService
    {
        private readonly IConfigurationService _configurationService;
        private const string RefDataApiUrl = "RefDataAPIURL";
        private const string RefDataApiUrlForInvestmentAccount = "RefDataAPIURL2";
        private const string PaasAuthUrl = "PaasAuthUrl";
        private readonly CacheService _cacheService = new CacheService(TimeSpan.FromMinutes(10));

        public RefDataService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<string> GetBasePaasUrl()
        {
            var baseUrl = await _configurationService.GetConfigValue(RefDataApiUrl).Caf();
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException(
                    $"Configuration for \"{RefDataApiUrl}\" not found. Please contact {Constants.SupportTeamEmailAddress} to add configuration for \"{RefDataApiUrl}\"");
            }

            return baseUrl;
        }

        private async Task<string> GetInvestmentAccountBasePaasUrl()
        {
            var baseUrl = await _configurationService.GetConfigValue(RefDataApiUrlForInvestmentAccount).Caf();
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException(
                    $"Configuration for \"{RefDataApiUrl}\" not found. Please contact {Constants.SupportTeamEmailAddress} to add configuration for \"{RefDataApiUrl}\"");
            }

            return baseUrl;
        }

        public async Task<IEnumerable<PiTeamDto>> GetAccountPiTeams(int? refId, DateTime? validDate = null)
        {

            var refData = await _cacheService.GetOrAdd($"{refId}PiTeams", () =>
            {
                var validDateFilter = (validDate ?? DateTime.Now).ToString("yyyy-MM-dd");
                return GetRefDataHttpPaasClient()
                    .PipeAsync(client =>
                        client.Get<RefData>($"/v1/investmentAccounts/{refId}?validDate={validDateFilter}"))
                    .Unwrap();

            }).Caf();

            if (refData.ImplementationOwnerRefIds == null)
            {
                return Enumerable.Empty<PiTeamDto>();
            }

            var allPiTeams = await GetAllPiTeams().Caf();

            var selectedPiTeams = refData.ImplementationOwnerRefIds.Join(
                allPiTeams,
                piTeamId => piTeamId,
                piTeamDto => piTeamDto.RefId,
                (piTeamId, piTeamDto) => piTeamDto);

            return selectedPiTeams;
        }

        public Task<IEnumerable<PiTeamDto>> GetAllPiTeams()
        {
            return _cacheService.GetOrAdd($"AllPiTeams", () =>
            {
                return GetRefDataHttpPaasClient()
                    .PipeAsync(client =>
                        client.Get<IEnumerable<PiTeamDto>>($"/v1/investmentAccounts/validImplementationOwners"))
                    .Unwrap();
            });
        }

        private class RefData
        {
            public RefData(int[] implementationOwnerRefIds)
            {
                ImplementationOwnerRefIds = implementationOwnerRefIds;
            }

            public int[] ImplementationOwnerRefIds { get; }
        }

        private async Task<HttpPaasClient> GetRefDataHttpPaasClient()
        {
            var refDataUrl = await GetInvestmentAccountBasePaasUrl();
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
    }
}
