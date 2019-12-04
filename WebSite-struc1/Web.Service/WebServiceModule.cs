using System;
using Autofac;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Interfaces.Service;
using System.Reflection;
using Service.Interfaces.DataAccess;
using Module = Autofac.Module;
using Configuration.Client;

namespace Web.Service
{
    public class WebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpService>().As<IHttpService>().SingleInstance();
            //builder.RegisterType<RmsService>().As<IRmsService>().SingleInstance();
            //builder.RegisterType<EntitlementService>().As<IEntitlementService>().SingleInstance();
            //builder.RegisterType<EntitlementService>().As<IUserService>().SingleInstance();
            //builder.RegisterType<ExternalSystemService>().As<IExternalSystemService>().SingleInstance();
            builder.RegisterType<RefDataService>().As<IRefDataService>().SingleInstance();
            //builder.RegisterType<ClearingBrokersService>().As<IClearingBrokersService>().SingleInstance();
            //builder.RegisterType<PipelineService>().As<IPipelineService>().SingleInstance();
            builder.Register(CreateConfigurationHelper).As<IConfigurationService>().SingleInstance();
            RegisterCacheInvalidators(builder);
        }

        private static void RegisterCacheInvalidators(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<ICacheInvalidator>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static IConfigurationService CreateConfigurationHelper(IComponentContext context)
        {
            string environment;
            var environmentType = EnvironmentSettings.EnvironmetType;
            switch (environmentType)
            {
                case EnvironmentSettings.Environments.DEV:
                    environment = "DEV";
                    break;
                case EnvironmentSettings.Environments.QA:
                    environment = "QA";
                    break;
                case EnvironmentSettings.Environments.PROD:
                    environment = "PROD";
                    break;
                case EnvironmentSettings.Environments.STAGING:
                    environment = "STG";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(environmentType), $"Unknown environment type for configuration: {environmentType}");
            }

            var builder = new ConfigurationProviderBuilder("app name", environment);
            builder.AddDataServiceSource("https://configuration.prd.app.com/dataservice");
            return new ConfigurationService(builder.Build());
        }
    }
}
