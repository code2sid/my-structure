using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public sealed class LoadSettingResult
    {
        public LoadSettingResult()
          : this((Setting)null)
        {
        }

        public LoadSettingResult(Setting setting)
        {
            this.TargetSetting = setting;
        }

        public Setting TargetSetting { get; }

        public bool IsSuccess
        {
            get
            {
                return this.TargetSetting != (Setting)null;
            }
        }
    }
}
