using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client
{
    public interface IConfigurationProvider : IDisposable
    {
        Task<Setting> LoadSettingAsync(string key, params ClassName[] classesToSearch);

        Task<Setting> LoadSettingAsync(string key, string correlationId, params ClassName[] classesToSearch);

        Task<IReadOnlyDictionary<string, Setting>> LoadAllAvailableAppSettings();

        Task<IReadOnlyDictionary<string, Setting>> LoadAllAvailableAppSettings(string correlationId);

        Task<IReadOnlyDictionary<string, Setting>> LoadAllAvailableAppSettings(params ClassName[] classesToSearch);

        Task<IReadOnlyDictionary<string, Setting>> LoadAllAvailableAppSettings(string correlationId, params ClassName[] classesToSearch);

        Task Populate(object obj, string correlationId);

        void Invalidate();
    }
}
