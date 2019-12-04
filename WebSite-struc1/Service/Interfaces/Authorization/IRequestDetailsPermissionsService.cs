using Model;
using System.Collections.Generic;

namespace Service.Interfaces.Authorization
{
    public interface IRequestDetailsPermissionsService
    {
        RequestDetailsPermissions GetDetailsPermissions(RequestDetails details, IEnumerable<Approval> approvals, string userId);
    }
}
