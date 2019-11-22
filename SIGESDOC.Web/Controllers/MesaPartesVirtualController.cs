using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGESDOC.Web.Controllers
{
    public class MesaPartesVirtualController : Controller
    {
        // GET: MesaPartesVirtual


        [AllowAnonymous]
        public ActionResult BandejaDespacho()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult BandejaRecepcion()
        {
            return View();
        }

    }
}