using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SIGESDOC.Web.Models;
using SIGESDOC.Request;
using SIGESDOC.Response;
using SIGESDOC.IAplicacionService;
using System.Configuration;

namespace SIGESDOC.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountService _AccountService;
        private readonly IGeneralService _GeneralService;

        public AccountController(IAccountService AccountService, IGeneralService GeneralService)
        {
            _AccountService = AccountService;
            _GeneralService = GeneralService;
        }
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Login()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Inicio");
            }
            else
            {
                ViewBag.cond_ofi = "0";
                List<SelectListItem> Lista_Oficina = new List<SelectListItem>();

                Lista_Oficina.Add(new SelectListItem()
                {
                    Text = "SELECCIONAR OFICINA",
                    Value = ""
                });

                List<SelectListItem> Lista_sede = new List<SelectListItem>();

                Lista_sede.Add(new SelectListItem()
                {
                    Text = "SELECCIONAR SEDE",
                    Value = "0"
                });

                ViewBag.lstSede = Lista_sede;
                ViewBag.lstOficina = Lista_Oficina;

                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(ConsultarUsuarioViewModel model)
        {

            List<SelectListItem> Lista_sede = new List<SelectListItem>();
            List<SelectListItem> Lista_Oficina = new List<SelectListItem>();

            Lista_sede.Add(new SelectListItem()
            {
                Text = "SELECCIONAR SEDE",
                Value = "0"
            });

            Lista_Oficina.Add(new SelectListItem()
            {
                Text = "SELECCIONAR OFICINA",
                Value = ""
            });

            if (HttpContext.Request.IsAuthenticated)
            {
                int val_perf = _AccountService.RecuperaDatos("20565429656", HttpContext.User.Identity.Name.Split('|')[1].Trim(), Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).id_perfil;
                if (val_perf == 15)
                {
                    return RedirectToAction("Consultar_HT_General", "HojaTramite");
                }
                else
                {
                    if (val_perf == 18)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")
                        {
                            return RedirectToAction("Documentos_por_recibir_x_evaluador", "Habilitaciones");
                        }
                        else
                        {
                            return RedirectToAction("Nuevo_Documento_dhcpa_Certificaciones", "Habilitaciones");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Por_Recibir_Ht", "HojaTramite");
                    }
                }
            }
            else
            {
                if (IsValid(model.persona_num_documento, model.clave))
                {
                    ViewBag.cond_ofi = "1";
                    if (model.persona.Split('|')[0].Trim() != null && model.persona.Split('|')[0].Trim() != "")
                    {
                        if (IsValid_oficina(model.persona_num_documento, model.clave, Convert.ToInt32(model.persona.Split('|')[0].Trim()), model.persona.Split('|')[1].Trim()))
                        {

                            int val_perf = _AccountService.RecuperaDatos("20565429656", model.persona_num_documento, Convert.ToInt32(model.persona.Split('|')[0].Trim())).id_perfil;

                            if (val_perf == 15)
                            {
                                return RedirectToAction("Consultar_HT_General", "HojaTramite");
                            }
                            else
                            {
                                if (val_perf == 18)
                                {
                                    if (Convert.ToInt32(model.persona.Split('|')[0].Trim()) == 18)
                                    {
                                        return RedirectToAction("Documentos_por_recibir_x_evaluador", "Habilitaciones");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Nuevo_Documento_dhcpa_Certificaciones", "Habilitaciones");
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("Por_Recibir_Ht", "HojaTramite");
                                }
                            }

                        }
                        else
                        {
                            return View(model);
                        }
                    }
                    else
                    {
                        int s_ok = 0;
                        var oficina_dir = _GeneralService.Consulta_Usuario("20565429656", model.persona_num_documento);

                        if (oficina_dir.Count() == 1)
                        {

                            if (IsValid_oficina(model.persona_num_documento, model.clave, oficina_dir.First().id_oficina_direccion, oficina_dir.First().nom_sede + "-" + oficina_dir.First().nom_ofi))
                            {
                                int val_perf = _AccountService.RecuperaDatos("20565429656", model.persona_num_documento, oficina_dir.First().id_oficina_direccion).id_perfil;

                                if (val_perf == 15)
                                {
                                    return RedirectToAction("Consultar_HT_General", "HojaTramite");
                                }
                                else
                                {
                                    if (val_perf == 18)
                                    {
                                        if (oficina_dir.First().id_oficina_direccion == 18)
                                        {
                                            return RedirectToAction("Documentos_por_recibir_x_evaluador", "Habilitaciones");
                                        }
                                        else
                                        {
                                            return RedirectToAction("Nuevo_Documento_dhcpa_Certificaciones", "Habilitaciones");
                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("Por_Recibir_Ht", "HojaTramite");
                                    }
                                }

                            }
                            else
                            {
                                return View(model);
                            }
                        }
                        else
                        {

                            foreach (var result in oficina_dir.OrderBy(x => x.nom_ofi))
                            {
                                s_ok = 0;
                                foreach (var result2 in Lista_sede.ToList())
                                {
                                    if (result.id_sede.ToString() == result2.Value.ToString())
                                    {
                                        s_ok = 1;
                                    }
                                }
                                if (s_ok == 0)
                                {
                                    Lista_sede.Add(new SelectListItem()
                                    {
                                        Text = result.nom_sede,
                                        Value = result.id_sede.ToString()
                                    });
                                }
                            };

                            ViewBag.lstSede = Lista_sede;
                            ViewBag.lstOficina = Lista_Oficina;
                            return View(model);
                        }
                    }
                }
                else
                {
                    ViewBag.cond_ofi = "0";
                    ViewBag.lstSede = Lista_sede;
                    ViewBag.lstOficina = Lista_Oficina;
                    ModelState.AddModelError("", "");
                    return View(model);
                }
            }

        }

        [HttpGet]
        public ActionResult Modificar_Clave(ConsultarUsuarioViewModel model, string clave_ini = "", string clave_fin = "", string clave_fin2 = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    return View();
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

        [HttpPost]
        public ActionResult Modificar_Clave(ConsultarUsuarioViewModel model, string clave_ini, string clave_fin)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    if (clave_fin != "123456")
                    {
                        /*
                        string pwd = clave_ini;

                        byte[] bytes = Encoding.UTF8.GetBytes(pwd);
                        var sha1 = SHA1.Create();
                        byte[] hashBytes = sha1.ComputeHash(bytes);
                        string pass = HexStringFromBytes(hashBytes).ToUpper();

                        string pwd2 = clave_fin;

                        byte[] bytes2 = Encoding.UTF8.GetBytes(pwd2);
                        var sha2 = SHA1.Create();
                        byte[] hashBytes2 = sha2.ComputeHash(bytes2);
                        string pass2 = HexStringFromBytes(hashBytes2).ToUpper();
                        */
                        if (_AccountService.Modificar_clave(HttpContext.User.Identity.Name.Split('|')[0].Trim(), HttpContext.User.Identity.Name.Split('|')[1].Trim(), clave_ini, clave_fin) == true)
                        {
                            FormsAuthentication.SignOut();
                            return RedirectToAction("Index", "Inicio");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Datos errados, Intente de Nuevo");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Elija otra contraseña");
                    }

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
        /*
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }*/

        private bool IsValid(string persona_num_documento, string clave)
        {
            /*
            string str_id_perfil = "";
            string str_perfil = "";
            byte[] bytes = Encoding.UTF8.GetBytes(clave);
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            string pass = HexStringFromBytes(hashBytes).ToUpper();
            */
            ConsultarUsuarioViewModel model = new ConsultarUsuarioViewModel();

            bool x = false;
            string entra = "NO";
            try
            {
                entra = _AccountService.valida_usuario("20565429656", persona_num_documento, clave);

            }
            catch (Exception) { }

            if (entra == "SI")
            {
                x = true;
            }

            return x;
        }

        private bool IsValid_oficina(string persona_num_documento, string clave, int id_oficina, string nombre_sede_oficina)
        {

            string str_id_perfil = "";
            string str_perfil = "";
            string str_jefe_od_perfil = "0";
            string str_insp_od_perfil = "0";

            /*
            byte[] bytes = Encoding.UTF8.GetBytes(clave);
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            string pass = HexStringFromBytes(hashBytes).ToUpper();
            */
            ConsultarUsuarioViewModel model = new ConsultarUsuarioViewModel();
            try
            {
                string entra = "NO";

                entra = _AccountService.valida_usuario("20565429656", persona_num_documento, clave);

                if (entra == "SI")
                {
                    var result = _AccountService.RecuperaDatos("20565429656", persona_num_documento, id_oficina);
                    model.ruc = result.ruc;
                    model.persona_num_documento = result.persona_num_documento;
                    model.empresa = result.empresa;
                    model.persona = result.persona;
                    str_id_perfil = result.id_perfil.ToString();
                    str_perfil = result.perfil;
                    if (result.id_perfil_jefe_od != null) { str_jefe_od_perfil = result.id_perfil_jefe_od.ToString(); }
                    if (result.id_perfil_inspector_od != null) { str_insp_od_perfil = result.id_perfil_inspector_od.ToString(); }

                }
            }
            catch (Exception) { }

            var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
            string permiso = "0";

            for (int i = 0; i < oficinas_permiso_od.Count(); i++)
            {
                if (id_oficina.ToString() == oficinas_permiso_od[i])
                {
                    permiso = "1";
                }
            }

            string permiso_ver_reporte_general = "0";

            var personas_permiso_ver_reporte_general = ConfigurationManager.AppSettings["PERMISOS_REPORTE_GENERAL"].ToString().Split(',');

            for (int i = 0; i < personas_permiso_ver_reporte_general.Count(); i++)
            {
                if (persona_num_documento == personas_permiso_ver_reporte_general[i])
                {
                    permiso_ver_reporte_general = "1";
                }
            }


            string permiso_docu_automa = "0";

            var personas_permiso_doc_auto = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_DOCUMENTOS_AUTOMATICO"].ToString().Split(',');

            for (int i = 0; i < personas_permiso_doc_auto.Count(); i++)
            {
                if (id_oficina.ToString() == personas_permiso_doc_auto[i])
                {
                    permiso_docu_automa = "1";
                }
            }

            var personas_permiso_seg_hab = ConfigurationManager.AppSettings["PERSONAS_CONSULTAS_SEG_HAB"].ToString().Split(',');
            string permiso_hab = "0";

            for (int i = 0; i < personas_permiso_seg_hab.Count(); i++)
            {
                if (model.persona_num_documento == personas_permiso_seg_hab[i])
                {
                    permiso_hab = "1";
                }
            }
            for (int i = 0; i < oficinas_permiso_od.Count(); i++)
            {
                if (id_oficina.ToString() == oficinas_permiso_od[i])
                {
                    permiso_hab = "0";
                }
            }

            if (id_oficina.ToString() == "18" || id_oficina.ToString() == "52")
            {
                permiso_hab = "0";
            }



            bool x = false;
            string ver_reporte_tupa_sdhpa = "0";

            if (id_oficina.ToString() == "18" || id_oficina.ToString() == "28")
            {
                ver_reporte_tupa_sdhpa = "1";
            }

            if (model.ruc != null)
            {
                if (id_oficina != 0 && str_id_perfil != "")
                {
                    string access = "";
                    /*
                        8	ASISTENTE
                        9	MESA DE PARTES
                        15	CONSULTA
                        16	ADMINISTRADOR
                        18	EVALUADOR
                        20  USUARIO_SIMPLE
                    */



                    if (str_id_perfil == "8") { access = permiso_pagina("1", "1", "1", "1", "1", "1", "1", "0", "0", "1", "1", "1", "1", "0", "1", "1", "1", "1", "1", "1", "1", "1", "1", permiso, "1", "1", ver_reporte_tupa_sdhpa, "1", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab); }
                    else
                    {
                        if (str_id_perfil == "9") { access = permiso_pagina("1", "1", "1", "1", "1", "1", "1", "0", "0", "1", "1", "1", "1", "0", "1", "1", "1", "1", "1", "1", "1", "1", "1", permiso, "1", "1", ver_reporte_tupa_sdhpa, "1", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab); }
                        else
                        {
                            if (str_id_perfil == "15") { access = permiso_pagina("0", "1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", permiso, "1", "0", ver_reporte_tupa_sdhpa, "1", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab); }
                            else
                            {
                                if (str_id_perfil == "16") { access = permiso_pagina("1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", permiso, "1", "1", ver_reporte_tupa_sdhpa, "1", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab); }
                                else
                                {
                                    if (str_id_perfil == "18") { access = permiso_pagina("1", "1", "1", "1", "1", "1", "0", "0", "0", "0", "1", "0", "0", "1", "1", "1", "1", "1", "1", "1", "0", "1", "1", permiso, "0", "1", ver_reporte_tupa_sdhpa, "0", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab); }
                                    else
                                    {
                                        if (str_id_perfil == "20")
                                        {
                                            access = permiso_pagina("1", "1", "1", "1", "1", "1", "1", "0", "0", "1", "1", "1", "1", "0", "1", "1", "1", "1", "1", "1", "1", "1", "1", permiso, "1", "1", ver_reporte_tupa_sdhpa, "0", permiso_ver_reporte_general, permiso_docu_automa, str_jefe_od_perfil, str_insp_od_perfil, permiso_hab);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    FormsAuthentication.SetAuthCookie(devuelve_usuario(model.ruc, model.persona_num_documento, model.empresa, model.persona, id_oficina.ToString(), str_id_perfil, str_perfil, "2", nombre_sede_oficina, access), false);
                    x = true;
                }
                else
                {
                    x = false;
                }
            }

            return x;
        }

        string devuelve_usuario(string RUC_00, string DNI_CE_01, string NOM_EMPRESA_02, string NOM_USUARIO_03,
            string ID_OFICINA_DIRECCION_04, string ID_PERFIL_05, string NOM_PERFIL_06, string ID_SISTEMA_07,
            string NOM_SEDE_OFICINA_08, string ACCESO_09)
        {
            string usuario = "";
            usuario = RUC_00;
            usuario = usuario + " | " + DNI_CE_01;
            usuario = usuario + " | " + NOM_EMPRESA_02;
            usuario = usuario + " | " + NOM_USUARIO_03;
            usuario = usuario + " | " + ID_OFICINA_DIRECCION_04;
            usuario = usuario + " | " + ID_PERFIL_05;
            usuario = usuario + " | " + NOM_PERFIL_06;
            usuario = usuario + " | " + ID_SISTEMA_07;
            usuario = usuario + " | " + NOM_SEDE_OFICINA_08;
            usuario = usuario + " | " + ACCESO_09;
            return usuario;
        }

        string permiso_pagina(string CREAR_HT_00, string CONSULTA_GENERAL_01, string CONSULTA_MIS_HT_02, string BANDEJA_GENERAL_03,
            string REGISTRAR_PERSONAS_04, string REGISTRAR_ENTIDAD_05, string REPORTES_06, string QUITAR_ARCHIVO_07, string QUITAR_ATENDIDO_08, string NUEVO_SEGUIMIENTO_09, string NUEVA_EMBARCACION_10,
            string NUEVA_FACTURA_11, string NUEVO_EXPEDIENTE_12, string NUEVO_SEGUI_EVALUADOR_13, string NUEVO_DOCUMENTO_14, string NUEVO_PROTOCOLO_15, string NUEVA_PLANTA_16,
            string CONSULTA_SOLICITUD_DHCPA_17, string CONSULTA_DOCUMENTO_OD_POR_RECIBIR_18, string NUEVO_ALMACEN_19, string PUBLICAR_PROTOCOLOS_20, string NUEVA_CONCESION_21, string NUEVO_DESEMBARCADERO_22,
            string PERMISO_REGISTRO_OD_23, string PERMISO_CONSULTA_PEDIDO_HT_24, string NUEVO_TRANSPORTE_25, string REPORTE_TUPA_SDHPA_26, string CONSULTA_DOC_X_OFICINA_27, string REPORTE_GENERAL_SANIPES_28,
            string PERMISO_DOCU_AUTOMA_29, string JEFE_OD_30, string INSPEC_OD_31, string VER_SEGUIMIENTO_HAB_32)
        {

            string PERMISO_RECEPCION_SS_OD_30 = "0";
            string PERMISO_RECEPCION_SS_INSPECTOR_31 = "0";

            if (JEFE_OD_30 == "1")
            {
                PERMISO_RECEPCION_SS_OD_30 = "1";
            }

            if (INSPEC_OD_31 == "1")
            {
                PERMISO_RECEPCION_SS_INSPECTOR_31 = "1";
            }

            string acceso = "";
            acceso = CREAR_HT_00;
            acceso = acceso + " , " + CONSULTA_GENERAL_01;
            acceso = acceso + " , " + CONSULTA_MIS_HT_02;
            acceso = acceso + " , " + BANDEJA_GENERAL_03;
            acceso = acceso + " , " + REGISTRAR_PERSONAS_04;
            acceso = acceso + " , " + REGISTRAR_ENTIDAD_05;
            acceso = acceso + " , " + REPORTES_06;
            acceso = acceso + " , " + QUITAR_ARCHIVO_07;
            acceso = acceso + " , " + QUITAR_ATENDIDO_08;
            acceso = acceso + " , " + NUEVO_SEGUIMIENTO_09;
            acceso = acceso + " , " + NUEVA_EMBARCACION_10;
            acceso = acceso + " , " + NUEVA_FACTURA_11;
            acceso = acceso + " , " + NUEVO_EXPEDIENTE_12;
            acceso = acceso + " , " + NUEVO_SEGUI_EVALUADOR_13;
            acceso = acceso + " , " + NUEVO_DOCUMENTO_14;
            acceso = acceso + " , " + NUEVO_PROTOCOLO_15;
            acceso = acceso + " , " + NUEVA_PLANTA_16;
            acceso = acceso + " , " + CONSULTA_SOLICITUD_DHCPA_17;
            acceso = acceso + " , " + CONSULTA_DOCUMENTO_OD_POR_RECIBIR_18;
            acceso = acceso + " , " + NUEVO_ALMACEN_19;
            acceso = acceso + " , " + PUBLICAR_PROTOCOLOS_20;
            acceso = acceso + " , " + NUEVA_CONCESION_21;
            acceso = acceso + " , " + NUEVO_DESEMBARCADERO_22;
            acceso = acceso + " , " + PERMISO_REGISTRO_OD_23;
            acceso = acceso + " , " + PERMISO_CONSULTA_PEDIDO_HT_24;
            acceso = acceso + " , " + NUEVO_TRANSPORTE_25;
            acceso = acceso + " , " + REPORTE_TUPA_SDHPA_26;
            acceso = acceso + " , " + CONSULTA_DOC_X_OFICINA_27;
            acceso = acceso + " , " + REPORTE_GENERAL_SANIPES_28;
            acceso = acceso + " , " + PERMISO_DOCU_AUTOMA_29;
            acceso = acceso + " , " + PERMISO_RECEPCION_SS_OD_30;
            acceso = acceso + " , " + PERMISO_RECEPCION_SS_INSPECTOR_31;
            acceso = acceso + " , " + VER_SEGUIMIENTO_HAB_32;
            return acceso;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Inicio");
        }

        public ActionResult Error_Logeo()
        {
            return View();
        }

        public ActionResult Llenar_oficina_sede(string dni, int id_sede)
        {
            List<SelectListItem> Lista_Oficina = new List<SelectListItem>();

            Lista_Oficina.Add(new SelectListItem()
            {
                Text = "SELECCIONAR OFICINA",
                Value = ""
            });

            if (id_sede != 0)
            {
                foreach (var result in _GeneralService.Recupera_oficina_dni_y_sede(dni, id_sede).OrderBy(x => x.nom_ofi))
                {
                    Lista_Oficina.Add(new SelectListItem()
                    {
                        Text = result.nom_ofi,
                        Value = result.id_oficina_direccion.ToString()
                    });
                }
            }

            return Json(Lista_Oficina, JsonRequestBehavior.AllowGet);
        }

        public ActionResult consultar_documentos_pendientes_principal()
        {
            string texto_pendiente = "";

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string documento = "";
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20") { documento = HttpContext.User.Identity.Name.Split('|')[1].Trim(); }
                    int cantidad = _GeneralService.Consultar_documentos_pendientes(documento, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).contador ?? 0;
                    if (cantidad == 1)
                    {
                        texto_pendiente = "TIENES " + cantidad.ToString() + " DOCUMENTO PENDIENTE";
                    }
                    else
                    {
                        texto_pendiente = "TIENES " + cantidad.ToString() + " DOCUMENTOS PENDIENTES";
                    }
                }
            }
            return Json(texto_pendiente, JsonRequestBehavior.AllowGet);
        }
        public ActionResult consultar_documentos_pendientes_principal_detalle()
        {
            IEnumerable<SIGESDOC.Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result> resp_det = new List<SIGESDOC.Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result>();
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string documento = "";
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20") { documento = HttpContext.User.Identity.Name.Split('|')[1].Trim(); }
                    resp_det = _GeneralService.Consultar_documentos_pendientes_detalle(documento, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                }
            }
            return Json(resp_det, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Consultar_documentos_pendientes_detalle_desagregado(string fecha)
        {
            IEnumerable<SIGESDOC.Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result> resp_det = new List<SIGESDOC.Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result>();
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string documento = "";
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20") { documento = HttpContext.User.Identity.Name.Split('|')[1].Trim(); }
                    resp_det = _GeneralService.Consultar_documentos_pendientes_detalle_desagregado(documento, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), fecha);
                }
            }
            return Json(resp_det, JsonRequestBehavior.AllowGet);
        }
    }
}