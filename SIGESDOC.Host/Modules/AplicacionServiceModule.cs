using Autofac;
using System;
using System.Reflection;

namespace SIGESDOC.Host.Modules
{
    public class AplicacionServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SIGESDOC.AplicacionService"))
                .Where(type => type.Name.EndsWith("Service", StringComparison.Ordinal))
                .AsImplementedInterfaces();
        }
    }
}