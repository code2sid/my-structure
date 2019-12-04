using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public interface ITimeoutInvalidator
    {
        void StartMonitoring(Func<IEnumerable<ConcurrentDictionary<string, SettingNode>>> getTargetFunc, CancellationToken cancellationToken, TimeSpan pollingInterval);
    }
}
