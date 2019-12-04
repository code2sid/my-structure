using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessService
{
    public interface ISecurityMasterService
    {
        Task<IEnumerable<Country>> GetAllCountries();
    }
}