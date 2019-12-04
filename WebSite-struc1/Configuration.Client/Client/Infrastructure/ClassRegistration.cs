using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public class ClassRegistration
    {
        public ClassRegistration(ClassName name)
        {
            this.Name = name;
            this.Settings = new Dictionary<string, Setting>((IEqualityComparer<string>)Constants.StringComparer);
        }

        public ClassName Name { get; }

        public ClassName Parent { get; set; }

        public Dictionary<string, Setting> Settings { get; }
    }
}
