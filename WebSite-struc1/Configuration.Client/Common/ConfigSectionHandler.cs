using System.Configuration;

namespace Configuration.Client.Common
{
    public sealed class ConfigSectionHandler : ConfigurationSection
    {
        public const string ConfigSectionName = "aqrConfig";

        [ConfigurationProperty("invalidationTimeoutSeconds")]
        public int InvalidationTimeoutSeconds
        {
            get
            {
                return (int?)this["invalidationTimeoutSeconds"] ?? 300;
            }
        }

        [ConfigurationProperty("app", IsRequired = true)]
        public AppConfigRoot App
        {
            get
            {
                return (AppConfigRoot)this["app"];
            }
        }
    }
}
