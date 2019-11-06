using System;
using System.Globalization;
using Autofac;
using Autofac.Integration.Wcf;
using SIGESDOC.Host.Modules;


namespace SIGESDOC.Host
{
    public static class DependencyInjectionHelper
    {
        public static void LoadContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RepositorioModule>();
            builder.RegisterModule<AplicacionServiceModule>();

            builder.RegisterInstance(CultureInfo.CurrentCulture).As<IFormatProvider>();

            AutofacHostFactory.Container = builder.Build();
        }
    }
}