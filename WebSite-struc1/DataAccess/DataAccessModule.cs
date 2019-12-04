using Autofac;
using DataAccess.Helpers;
using DataAccess.Repository;
using System.Reflection;
using Module = Autofac.Module;

namespace DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionStringHelper>().As<IConnectionStringHelper>();
            builder.RegisterType<DbConnectionManager>().As<IDbConnectionManager>().InstancePerLifetimeScope();
            RegisterRepositories(builder);
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<BaseRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
