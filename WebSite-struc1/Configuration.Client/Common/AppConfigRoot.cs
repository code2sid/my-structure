using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common
{
    public sealed class AppConfigRoot : ConfigurationElementCollection, IEnumerable<AppConfigSetting>, IEnumerable
    {
        public AppConfigRoot()
        {
            this.AddElementName = "set";
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("envs")]
        public AppConfigEnvs Environments
        {
            get
            {
                return (AppConfigEnvs)this["envs"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)new AppConfigSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (object)((AppConfigSetting)element).Key;
        }

        IEnumerator<AppConfigSetting> IEnumerable<AppConfigSetting>.GetEnumerator()
        {
            return (IEnumerator<AppConfigSetting>)new EnumeratorWrapper<AppConfigSetting>(this.GetEnumerator());
        }
    }
}
