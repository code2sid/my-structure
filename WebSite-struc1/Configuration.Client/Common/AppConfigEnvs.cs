using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common
{
    public sealed class AppConfigEnvs : ConfigurationElementCollection, IEnumerable<AppConfigSetting>, IEnumerable
    {
        public AppConfigEnvs()
        {
            this.AddElementName = "env";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)new AppConfigEnv();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (object)((AppConfigEnv)element).Name;
        }

        IEnumerator<AppConfigEnv> IEnumerable<AppConfigEnv>.GetEnumerator()
        {
            return (IEnumerator<AppConfigEnv>)new EnumeratorWrapper<AppConfigEnv>(this.GetEnumerator());
        }
    }
}
