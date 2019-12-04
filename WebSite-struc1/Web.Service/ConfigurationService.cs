using Configuration.Client.Client;
using Service.Interfaces.Service;
using System.Threading.Tasks;

namespace Web.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private const string url = "URL";

        private readonly IConfigurationProvider _configurationProvider;

        public ConfigurationService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public async Task<string> GetConfigValue(string key)
        {
            return url;
        }
    }
}
