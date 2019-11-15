using SIGESDOC.IAplicacionService;
using SIGESDOC.Response;
using SIGESDOC.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGESDOC.Web.Models;
using System.Web.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Text;
using System.Dynamic;

namespace SIGESDOC.Web.Controllers
{
    public class OficinaController : Controller
    {

        private readonly IOficinaService _OficinaService;
        private readonly IGeneralService _GeneralService;
        static string baseUrl = ConfigurationManager.AppSettings["SrvRuc"].ToString();

        public OficinaController(IOficinaService OficinaService, IGeneralService GeneralService)
        {
            _OficinaService = OficinaService;
            _GeneralService = GeneralService;
        }


        public ActionResult Listar_Oficina(int page = 1, string RUC = "", string NOMBRE = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    IEnumerable<ConsultarOficinaResponse> model = new List<ConsultarOficinaResponse>();

                    ViewBag.TotalRows = _OficinaService.CountOficina_x_RUC_NOMBRE(RUC, NOMBRE);
                    model = _OficinaService.GetallOficina_x_RUC_NOMBRE(page, 10, RUC, NOMBRE);
                    ViewBag.vb_ruc = RUC;
                    ViewBag.vb_nombre = NOMBRE;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        public ActionResult Nueva_Oficina(int page = 1, string TXT_RUC = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    IEnumerable<ConsultarDireccionResponse> model = new List<ConsultarDireccionResponse>();


                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    List<SelectListItem> lista_direccion = new List<SelectListItem>();
                    lista_direccion.Add(new SelectListItem() { Text = "SELECCIONAR DIRECCION", Value = "" });

                    if (TXT_RUC != "")
                    {
                        int cuenta = _OficinaService.CountOficina_DIR_x_RUC(TXT_RUC);
                        ViewBag.TotalRows = cuenta;
                        model = _OficinaService.GetallOficina_DIR_x_RUC(page, 10, TXT_RUC);
                        var recupera_departamento = _GeneralService.llenar_departamento();

                        int var_id_oficina = 0;
                        foreach (var result_ofi in _OficinaService.Consultar_Oficina_x_RUC(TXT_RUC))
                        {
                            if (result_ofi.id_ofi_padre == null)
                            {
                                var_id_oficina = result_ofi.id_oficina;
                            }
                        };

                        foreach (var result_sede in _OficinaService.Consultar_direcciones_x_oficina(var_id_oficina))
                        {

                            if (result_sede.nombre.ToString().Trim() == "")
                            {

                                lista_direccion.Add(new SelectListItem()
                                {
                                    Text = result_sede.direccion,
                                    Value = result_sede.id_sede.ToString()
                                }
                                );
                            }
                            else
                            {
                                lista_direccion.Add(new SelectListItem()
                                {
                                    Text = result_sede.nombre + '-' + result_sede.direccion,
                                    Value = result_sede.id_sede.ToString()
                                }
                                );
                            }
                        };

                        foreach (var result in recupera_departamento)
                        {
                            Lista_departamento.Add(new SelectListItem()
                            {
                                Text = result.departamento,
                                Value = result.codigo_departamento.ToString()
                            }
                            );
                        };
                    }
                    else
                    {
                        ViewBag.TotalRows = 0;
                    }
                    ViewBag.lst_new_direccion = lista_direccion;
                    ViewBag.vb_ruc = TXT_RUC;
                    ViewBag.lst_departamento = Lista_departamento;
                    ViewBag.lst_provincia = Lista_provincia;
                    ViewBag.lst_distrito = Lista_distrito;
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Empresa(string ruc, string nombre_empresa, string siglas, string nombre_sede, string direccion, string referencia, string ubigeo)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {

                        if (_OficinaService.crea_empresa(ruc.Trim(), nombre_empresa.ToUpper().Trim(), siglas.ToUpper().Trim(), nombre_sede.ToUpper().Trim(), direccion.ToUpper().Trim(), referencia.ToUpper().Trim(), ubigeo, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim()) == true)
                        {
                            @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                        }
                        else
                        {
                            @ViewBag.Mensaje = "Datos errados";
                        }

                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_datos_del_ruc(string persona_num_documento = "")
        {

            List<SelectListItem> lista_ruc = new List<SelectListItem>();

            foreach (var x in _OficinaService.GetAllEmpresa_RUC(persona_num_documento))
            {
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.id_oficina_direccion.ToString() });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.nom_oficina });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.siglas });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.nom_sede });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.direccion });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.referencia });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.nom_ubigeo });
                lista_ruc.Add(new SelectListItem() { Text = "SI", Value = x.id_oficina.ToString() });
            }

            if (lista_ruc.Count() <= 0)
            {
                lista_ruc.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_ruc, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Oficina(string new_oficina_direccion, string new_oficina_siglas, string new_oficina_id_sede, string new_oficina_dir_condicion, string new_oficina_nom_direccion,
                    string new_oficina_nom_sede, string new_oficina_nom_referencia, string new_oficina_ubigeo, int id_oficina_padre, string new_oficina_ruc)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {
                        if (id_oficina_padre == 0)
                        {
                            id_oficina_padre = _OficinaService.GetAllEmpresa_RUC(new_oficina_ruc).First().id_oficina;
                        }

                        if (new_oficina_dir_condicion == "0")
                        {
                            if (_OficinaService.crea_oficina_secundaria(new_oficina_direccion.ToUpper().Trim(), id_oficina_padre, new_oficina_siglas.ToUpper().Trim(), new_oficina_ruc.Trim(), Convert.ToInt32(new_oficina_id_sede), HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim()) == true)
                            {
                                @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                            }
                            else
                            {
                                @ViewBag.Mensaje = "Datos errados";
                            }
                        }
                        else
                        {
                            if (new_oficina_dir_condicion == "1")
                            {
                                int id_sede = _OficinaService.crea_sede_secundaria(new_oficina_nom_sede.ToUpper().Trim(), new_oficina_nom_direccion.ToUpper().Trim(), new_oficina_nom_referencia.ToUpper().Trim(), new_oficina_ubigeo, id_oficina_padre, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim()).id_sede;

                                if (_OficinaService.crea_oficina_secundaria(new_oficina_direccion.ToUpper().Trim(), id_oficina_padre, new_oficina_siglas.ToUpper().Trim(), new_oficina_ruc.Trim(), id_sede, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim()) == true)
                                {
                                    @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                                }
                                else
                                {
                                    @ViewBag.Mensaje = "Datos errados";
                                }
                            }
                            else
                            {
                                _OficinaService.crea_sede_secundaria(new_oficina_nom_sede.ToUpper().Trim(), new_oficina_nom_direccion.ToUpper().Trim(), new_oficina_nom_referencia.ToUpper().Trim(), new_oficina_ubigeo, id_oficina_padre, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                                @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Direccion_Legal(int id_oficina_direccion_legal, int id_sede, string RUC)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {
                        string res = _OficinaService.insertar_actualizar_direccion_legal(id_oficina_direccion_legal, RUC, id_sede, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                        if (res == "")
                        {
                            @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                        }
                        else
                        {
                            @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                        }
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                    }
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Direccion_Legal_recuera_id(int id_oficina_direccion_legal, int id_sede, string RUC)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {
                        int res = _OficinaService.direccion_legal_id(id_oficina_direccion_legal, RUC, id_sede, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());

                        @ViewBag.Mensaje = res.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                    }
                    return PartialView("_Success_OFICINA");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Persona_Legal(int id_persona_legal, string documento, string telefono, string correo, string RUC)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {
                        string res = _OficinaService.insertar_actualizar_persona_legal(id_persona_legal, documento, telefono, correo, RUC, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                        if (res == "")
                        {
                            @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                        }
                        else
                        {
                            @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                        }
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                    }
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Modificar_direccion_persona(string direccion, string ubigeo, string persona_num_documento)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    ConsultarDniResponse res_dni = new ConsultarDniResponse();
                    res_dni = _GeneralService.actualizar_persona(persona_num_documento, direccion, ubigeo, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    @ViewBag.Mensaje = "Se Modificó Satisfactoriamente";
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Persona_Legal_DNI(int id_dni_persona_legal, string documento, string telefono, string correo, string DNI)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    try
                    {
                        string res = _OficinaService.insertar_actualizar_persona_legal_DNI(id_dni_persona_legal, documento, telefono, correo, DNI, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                        if (res == "")
                        {
                            @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                        }
                        else
                        {
                            @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                        }
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "Ocurrio algo inesperado comunicarse con UTI : Anexo 7063";
                    }
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Asignar_Oficina(int id_oficina_dir, string person_num_doc)
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    _OficinaService.asignar_personal(person_num_doc, id_oficina_dir, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Quitar_Oficina(int id_per_emp)
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    _OficinaService.quita_oficina_persona(id_per_emp, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
                    return PartialView("_Success");
                }
                else
                {
                    return RedirectToAction("Error_Logeo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        //Add by HM
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_datos_del_SUNAT(string persona_num_documento)
        {
            List<EntRuc> listEntRuc = new List<EntRuc>();

            try
            {
                //Entidad Ruc
                EntRuc objEntRuc = new EntRuc();

                //Serializando Numero de DNI
                Ruc_Input ruc_Input = new Ruc_Input();
                ruc_Input.ruc = persona_num_documento;
                string outjson = JsonConvert.SerializeObject(ruc_Input, Formatting.Indented);

                //Configuraciones de llamado de Servicio
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(baseUrl);
                myRequest.KeepAlive = true;
                myRequest.Method = "POST";
                byte[] postBytes = Encoding.UTF8.GetBytes(outjson);
                myRequest.Accept = "application/json";
                myRequest.ContentType = "application/json";
                myRequest.MediaType = "application/json";
                myRequest.ContentLength = postBytes.Length;
                Stream requestStream = myRequest.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                var sr = new StreamReader(response.GetResponseStream());
                string responseText = sr.ReadToEnd();

                var obj = ToObject(responseText) as IDictionary<string, object>;
                foreach (var item in obj)
                {
                    switch (item.Key)
                    {
                        case "datosPrincipales":
                            var respdp = obj[item.Key] as IDictionary<string, object>;
                            objEntRuc.ddp_nombre = respdp["ddp_nombre"].ToString();
                            objEntRuc.ddp_nomzon = respdp["ddp_nomzon"].ToString();
                            objEntRuc.ddp_refer1 = respdp["ddp_refer1"].ToString();
                            objEntRuc.desc_dep = respdp["desc_dep"].ToString().Trim();
                            objEntRuc.desc_prov = respdp["desc_prov"].ToString().Trim();
                            objEntRuc.desc_dist = respdp["desc_dist"].ToString().Trim();
                            break;
                        case "datosSecundarios":
                            break;
                        case "representantesLegales":
                            break;
                        case "domicilioLegal":
                            objEntRuc.domicilioLegal = obj[item.Key].ToString();
                            break;
                    }
                }

                listEntRuc.Add(objEntRuc);
            }
            catch (Exception ex)
            {
                return Json("Ocurrio un error", JsonRequestBehavior.AllowGet);
            }

            return Json(listEntRuc, JsonRequestBehavior.AllowGet);
        }

        //Add by HM
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ObtieneUbigeo(string id_departamento = "", string desc_Provincia = "", string desc_Distrito = "")
        {
            try
            {
                #region Datos Provincia
                List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                var recupera_provincia = _GeneralService.llenar_provincia_x_departamento(id_departamento);

                foreach (var result in recupera_provincia)
                {
                    Lista_provincia.Add(new SelectListItem()
                    {
                        Text = result.provincia,
                        Value = result.codigo_provincia
                    }
                    );
                };
                var DatosProvincia = Lista_provincia.Where(x => x.Text == desc_Provincia);
                var CodProvinciaSeleccionado = DatosProvincia.ToList()[0].Value.ToString();
                #endregion

                #region Datos Distrito
                List<SelectListItem> Lista_distrito = new List<SelectListItem>();
                var recupera_distrito = _GeneralService.llenar_distrito_x_provincia(id_departamento + CodProvinciaSeleccionado);

                foreach (var result in recupera_distrito)
                {
                    Lista_distrito.Add(new SelectListItem()
                    {
                        Text = result.distrito,
                        Value = result.ubigeo
                    }
                    );
                };
                var DatosDistrito = Lista_distrito.Where(x => x.Text == desc_Distrito);
                var CodDistritoSeleccionado = DatosDistrito.ToList()[0].Value.ToString();
                #endregion
                return Json(CodDistritoSeleccionado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ocurrio un Error en ", JsonRequestBehavior.AllowGet);
            }
        }

        //Add by HM
        private class Ruc_Input
        {
            public string ruc { get; set; }
        }

        //Add by HM
        public static object ToObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return ToObject(JToken.Parse(json));
        }

        //Add by HM
        private static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value),
                                              StringComparer.OrdinalIgnoreCase);

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }

    }
}