using Configuration.Client.Common;
using System.Collections.Generic;

namespace Configuration.Client.Client.Infrastructure
{
    internal class ConfigurationBuilderContext
    {
        public ConfigurationBuilderContext(EnvironmentInfo environment)
        {
            this.Environment = environment;
            this.Sources = (ICollection<ConfigSourceInfo>)new List<ConfigSourceInfo>();
            this.CustomRegistrations = new Dictionary<ClassName, ClassRegistration>();
        }

        public EnvironmentInfo Environment { get; }

        public ICollection<ConfigSourceInfo> Sources { get; }

        public Dictionary<ClassName, ClassRegistration> CustomRegistrations { get; }
    }
}
