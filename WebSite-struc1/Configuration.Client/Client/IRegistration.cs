using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client
{
    public interface IRegistration
    {
        void SetParent(string category, string className);

        void SetParent(ClassName className);

        void AddSetting(string key, string value);

        void Commit();
    }
}
