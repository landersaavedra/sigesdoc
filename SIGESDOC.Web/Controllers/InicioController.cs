using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SIGESDOC.Web.Models;
using SIGESDOC.Response;
using SIGESDOC.Request;

namespace SIGESDOC.Web.Controllers
{
    public class InicioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Menu()//, string user_dni, int user_id_perfil, string user_empresa, string user_persona, string user_perfil)
        {
            /*Lógica para enviar el area/action/controller activo*/
            var fullUrl = Request.Url.ToString();
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1)
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }

            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            var values = routeData.Values;
            var controllerName = values["controller"];
            var actionName = values["action"];
            var areaName = routeData.DataTokens["area"];

            ViewBag.ControllerActive = areaName == null ? controllerName : string.Format("{0}/{1}", areaName, controllerName);
            ViewBag.ActionActive = actionName;

            return PartialView("_MenuPartial");
        }

        public PartialViewResult LoginMenu()
        {
            return PartialView("_LoginPartial");
        }
    }
}