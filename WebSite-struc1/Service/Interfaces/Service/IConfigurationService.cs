using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Service
{
    public interface IConfigurationService
    {
        Task<string> GetConfigValue(string key);
    }
}
