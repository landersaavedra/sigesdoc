using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGESDOC.Web.Seguridad
{
    public static class ServiceConfiguration
    {
        public static int PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
    }
}