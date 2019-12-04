using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    internal class TimeoutInvalidator : ITimeoutInvalidator
    {
        public void StartMonitoring(Func<IEnumerable<ConcurrentDictionary<string, SettingNode>>> getTargetFunc, CancellationToken cancellationToken, TimeSpan pollingInterval)
        {
            ConfigSectionHandler section = (ConfigSectionHandler)ConfigurationManager.GetSection("aqrConfig");
            int num = section != null ? section.InvalidationTimeoutSeconds : 300;
            Task.Factory.StartNew(new Action<object>(TimeoutInvalidator.DoChecks), (object)new TimeoutInvalidator.Context()
            {
                GetSettingsFunc = getTargetFunc,
                Token = cancellationToken,
                SettingAbsoluteExpiration = TimeSpan.FromSeconds((double)num),
                PollingInterval = pollingInterval
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private static void DoChecks(object contextBoxed)
        {
            TimeoutInvalidator.Context context = (TimeoutInvalidator.Context)contextBoxed;
            //ILog logger = LogManager.GetLogger(typeof(TimeoutInvalidator).Name);
            logger.Debug((object)"Starting timeout invalidation checks");
            while (!context.Token.WaitHandle.WaitOne(context.PollingInterval))
            {
                DateTime now = DateTime.Now;
                foreach (ConcurrentDictionary<string, SettingNode> concurrentDictionary in context.GetSettingsFunc())
                {
                    foreach (SettingNode settingNode1 in concurrentDictionary.Values.Where<SettingNode>((Func<SettingNode, bool>)(x => now - x.CreatedAt > context.SettingAbsoluteExpiration)).ToArray<SettingNode>())
                    {
                        SettingNode settingNode2;
                        if (concurrentDictionary.TryRemove(settingNode1.Setting.Key, out settingNode2))
                        {
                            //logger.DebugFormat("Removing from cache: {0}", (object)settingNode2.Setting.Key);
                        }
                    }
                }
            }
            //logger.Debug((object)"Exiting");
        }

        private class Context
        {
            public Func<IEnumerable<ConcurrentDictionary<string, SettingNode>>> GetSettingsFunc { get; set; }

            public CancellationToken Token { get; set; }

            public TimeSpan SettingAbsoluteExpiration { get; set; }

            public TimeSpan PollingInterval { get; set; }
        }
    }
}
