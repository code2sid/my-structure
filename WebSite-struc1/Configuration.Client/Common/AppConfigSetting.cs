using System.Configuration;

namespace Configuration.Client.Common
{
    public sealed class AppConfigSetting : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
        }
    }
}
