using Autofac;
using SIGESDOC.Contexto;
using SIGESDOC.IRepositorio;
using SIGESDOC.Repositorio;
using SIGESDOC.Repositorio.Base;
using System;
using System.Data.Entity;
using System.Reflection;

namespace SIGESDOC.Host.Modules
{
    public class RepositorioModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SIGESDOC.Repositorio"))
                .Where(type => type.Name.EndsWith("Repositorio", StringComparison.Ordinal))
                .AsImplementedInterfaces();

            var method = typeof(RepositorioModule).GetMethod("RegisterRepository");
            var types = Assembly.Load("SIGESDOC.Entidades").GetTypes();
            foreach (var type in types) method.MakeGenericMethod(type).Invoke(null, new[] { builder });

            string nameOrConnectionString = "name=DB_GESDOCEntities";
            builder.RegisterType<DB_GESDOCEntities>().As<DbContext>().WithParameter("nameOrConnectionString", nameOrConnectionString).InstancePerLifetimeScope();

            builder.RegisterType<ContextSIGESDOC>().As<IContext>();
            builder.RegisterType<ContextSIGESDOC>().As<IUnitOfWork>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterRepository<T>(ContainerBuilder builder) where T : class
        {
            builder.RegisterType<BaseRepositorio<T>>().AsImplementedInterfaces();
        }
    }
}