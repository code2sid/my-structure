using Autofac;
using Service.Interfaces.Authorization;

namespace Authorization
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ApprovalPermissionsService>().As<IApprovalPermissionsService>();
            //builder.RegisterType<PermissionsHelperService>().As<IPermissionsHelperService>();
            builder.RegisterType<RequestDetailsPermissionsService>().As<IRequestDetailsPermissionsService>();
            //builder.RegisterType<SubApprovalPermissionsService>().As<ISubApprovalPermissionsService>();
            //builder.RegisterType<UserPermissionsService>().As<IUserPermissionsService>();
        }
    }
}
