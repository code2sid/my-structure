using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ApplicationNameAndVersion
    {
        public ApplicationNameAndVersion(Version version, string applicationName)
        {
            Version = version;
            ApplicationName = applicationName;
        }

        public Version Version { get; }
        public string ApplicationName { get; }
    }
}
