using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces.DataAccessService.Helpers
{
    public interface IDataProvider<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
