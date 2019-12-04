using Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Service
{
    public interface IRefDataService
    {
        Task<string> GetBasePaasUrl();
        Task<IEnumerable<PiTeamDto>> GetAccountPiTeams(int? refId, DateTime? validDate = null);
        Task<IEnumerable<PiTeamDto>> GetAllPiTeams();

    }
}
