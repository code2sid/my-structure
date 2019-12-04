using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public sealed class SettingNode
    {
        internal SettingNode(Setting setting)
        {
            this.Setting = setting;
            this.CreatedAt = DateTime.Now;
        }

        public Setting Setting { get; }

        public DateTime CreatedAt { get; }
    }
}
