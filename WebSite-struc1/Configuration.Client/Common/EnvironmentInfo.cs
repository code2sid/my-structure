using Configuration.Client.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common
{
    public sealed class EnvironmentInfo
    {
        public EnvironmentInfo(string application)
          : this(application, (string)null)
        {
        }

        public EnvironmentInfo(string application, string environment)
        {
            this.Application = application.CheckNotEmpty(nameof(application));
            this.Environment = environment;
        }

        public string Application { get; }

        public string Environment { get; }

        public bool HasEnvironment
        {
            get
            {
                return !string.IsNullOrEmpty(this.Environment);
            }
        }
    }
}
