using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public interface IAdapterPipeline : IDisposable
    {
        Task<FindClassResult> FindClassAsync(ClassName className, string correlationId);

        Task<LoadSettingResult> LoadSettingAsync(ClassName className, string key, string correlationId);

        Task<LoadSettingResult> LoadEnvSettingAsync(string key, string correlationId);

        Task<LoadSettingResult> LoadAppSettingAsync(string key, string correlationId);

        Task<LoadSettingResult> LoadGlobalSettingAsync(string key, string correlationId);

        Task<IReadOnlyCollection<string>> LoadAllAvailableKeys(string correlationId, IEnumerable<ClassName> classes = null);
    }
}
