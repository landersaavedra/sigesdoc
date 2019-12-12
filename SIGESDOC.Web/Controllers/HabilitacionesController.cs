using SIGESDOC.IAplicacionService;
using SIGESDOC.Response;
using SIGESDOC.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGESDOC.Web.Models;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Web.Helpers;
using _Word = Microsoft.Office.Tools.Word;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office;
using System.Reflection;
using Microsoft.VisualStudio.Tools.Applications;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using vml = DocumentFormat.OpenXml.Vml;
using System.Diagnostics;

namespace SIGESDOC.Web.Controllers
{
    public class HabilitacionesController : Controller
    {
        private readonly IHabilitacionesService _HabilitacionesService;
        private readonly IHojaTramiteService _HojaTramiteService;
        private readonly IGeneralService _GeneralService;
        private readonly IOficinaService _OficinaService;

       

        public HabilitacionesController(IHabilitacionesService HabilitacionesService, IGeneralService GeneralService, IOficinaService OficinaService, IHojaTramiteService HojaTramiteService)
        {
            _HabilitacionesService = HabilitacionesService;
            _GeneralService = GeneralService;
            _OficinaService = OficinaService;
            _HojaTramiteService = HojaTramiteService;
        }

        [AllowAnonymous]
        public ActionResult Reporte_TUPA_SDHPA()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[26].Trim() == "1"))) // Acceso a Nuevo Protocolo
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

        [AllowAnonymous]
        public ActionResult variable_Subir_archivo_doc_dhcpa(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_document_id_doc_dhcpa"] = id;
                return RedirectToAction("Adjuntar_archivo_document_dhcpa", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_document_dhcpa()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    int id_doc_dhcpa = 0;

                    try
                    {
                        id_doc_dhcpa = Convert.ToInt32(Session["archivo_document_id_doc_dhcpa"].ToString());
                        Session.Remove("archivo_document_id_doc_dhcpa");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    DocumentoDhcpaResponse doc = new DocumentoDhcpaResponse();
                    doc = _HabilitacionesService.Lista_Documento_dhcpa_x_id_rs(id_doc_dhcpa);

                    ViewBag.Str_documento = "Documento: " + _HojaTramiteService.Consult_tipo_docuemnto(doc.id_tipo_documento ?? 0) + " Nº " + doc.num_doc.ToString() + " " + doc.nom_doc;
                    ViewBag.id_documento_dhcpa = id_doc_dhcpa.ToString();
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
        public ActionResult Adjuntar_archivo_document_dhcpa(HttpPostedFileBase file, int id_doc_dhcpa)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                     (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                     (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                     && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    DocumentoDhcpaRequest doc_rq = new DocumentoDhcpaRequest();
                    doc_rq = _HabilitacionesService.Lista_Documento_dhcpa_x_id_rq(id_doc_dhcpa);

                    doc_rq.pdf = "1";

                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTOS_DHCPA"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, doc_rq.id_doc_dhcpa.ToString() + ".pdf"));
                        _HabilitacionesService.Update_documento_dhcpa(doc_rq);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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


        [AllowAnonymous]
        public ActionResult var_documento_dhcpa_pdf(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTOS_DHCPA"].ToString() + "/" + id.ToString() + ".pdf";
                    return File(ruta_pdf, "application/pdf");
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

        [AllowAnonymous]
        public ActionResult Listado_transportes_hab(string placa = "", string cod_habilitacion = "", int id_tipo_carroceria = 0, int id_tipo_furgon = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[25].Trim() == "1"))) // Acceso a Nuevo Transporte
                {

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_TRANSPORTE");
                    tbl.Columns.Add("PLACA");
                    tbl.Columns.Add("COD_HABILITACION");
                    tbl.Columns.Add("NOMBRE_CARROCERIA");
                    tbl.Columns.Add("NOMBRE_UM");
                    tbl.Columns.Add("CARGA_UTIL");
                    tbl.Columns.Add("ESTADO");
                    tbl.Columns.Add("NOMBRE_FURGON");
                    tbl.Columns.Add("PLACA_ID_TRANSPORTE");

                    var transporte = _HabilitacionesService.Lista_db_general_mae_transporte(placa, cod_habilitacion, id_tipo_carroceria, id_tipo_furgon);

                    foreach (var result in transporte)
                    {
                        tbl.Rows.Add(
                            result.id_transporte,
                            result.placa,
                            result.cod_habilitacion,
                            result.nombre_carroceria,
                            result.siglas_um,
                            result.carga_util,
                            result.nombre_estado,
                            result.nombre_furgon,
                            result.placa + "|" + result.id_transporte.ToString()
                            );
                    };

                    ViewData["Transporte_Tabla"] = tbl;


                    List<SelectListItem> lista_carroceria = new List<SelectListItem>();
                    List<SelectListItem> lista_nuevo_carroceria = new List<SelectListItem>();

                    lista_carroceria.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    foreach (var result in _HabilitacionesService.consulta_todo_activo_tipocarroceria())
                    {
                        lista_carroceria.Add(new SelectListItem()
                        {
                            Text = result.nombre.ToString(),
                            Value = result.id_tipo_carroceria.ToString()
                        });

                        lista_nuevo_carroceria.Add(new SelectListItem()
                        {
                            Text = result.nombre.ToString(),
                            Value = result.id_tipo_carroceria.ToString()
                        });
                    };

                    ViewBag.lst_carroceria = lista_carroceria;
                    ViewBag.lst_nuevo_carroceria = lista_nuevo_carroceria;

                    List<SelectListItem> lista_furgon = new List<SelectListItem>();
                    List<SelectListItem> lista_nuevo_furgon = new List<SelectListItem>();

                    lista_furgon.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    var recupera_furgon = _HabilitacionesService.consulta_todo_activo_tipofurgon(id_tipo_carroceria);

                    foreach (var result2 in recupera_furgon)
                    {
                        lista_furgon.Add(new SelectListItem()
                        {
                            Text = result2.nombre,
                            Value = result2.id_tipo_furgon.ToString()
                        });
                        lista_nuevo_furgon.Add(new SelectListItem()
                        {
                            Text = result2.nombre,
                            Value = result2.id_tipo_furgon.ToString()
                        });
                    };

                    ViewBag.lst_furgon = lista_furgon;
                    ViewBag.lst_tipo_furgon = lista_nuevo_furgon;

                    List<SelectListItem> Lista_unidad_medida = new List<SelectListItem>();

                    var recupera_um = _HabilitacionesService.consulta_todo_activo_unidad_medida();

                    foreach (var result in recupera_um)
                    {
                        Lista_unidad_medida.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.id_um.ToString()
                        }
                        );
                    };

                    ViewBag.lst_nuevo_um = Lista_unidad_medida;

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

        [AllowAnonymous]
        public ActionResult Listado_protocolos(string nombre_protocolo = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1"))) // Acceso a Nuevo Protocolo
                {

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_PROTOCOLO");
                    tbl.Columns.Add("NOMBRE");
                    tbl.Columns.Add("FECHA_EMISION");
                    tbl.Columns.Add("FECHA_INICIO");
                    tbl.Columns.Add("FECHA_FIN");
                    tbl.Columns.Add("ACTIVO");
                    tbl.Columns.Add("NOMBRE_ID_PROTOCOLO_REEM");

                    var transporte = _HabilitacionesService.Lista_mae_protocolo(nombre_protocolo);

                    foreach (var result in transporte)
                    {
                        string var_fecha_fin = "";
                        string var_estado = "NO VIGENTE";
                        int var_id_prot_ree = 0;
                        if (result.fecha_fin != null)
                        {
                            var_fecha_fin = result.fecha_fin.Value.ToShortDateString();
                        }
                        if (result.activo == "1")
                        {
                            var_estado = "VIGENTE";
                        }
                        if (result.id_protocolo_reemplaza != null)
                        {
                            var_id_prot_ree = result.id_protocolo_reemplaza ?? 0;
                        }
                        tbl.Rows.Add(
                       result.id_protocolo,
                       result.nombre,
                       result.fecha_registro.Value.ToShortDateString(),
                       result.fecha_inicio.Value.ToShortDateString(),
                       var_fecha_fin,
                       var_estado,
                       result.nombre + "|" + var_id_prot_ree.ToString()
                       );

                    };

                    ViewData["Protocolo_Tabla"] = tbl;

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

        [AllowAnonymous]
        public ActionResult Imprimir_Protocolo_transporte(int id)
        {
            PtocoloTransporteXIdTransporte2018V1Response prot_res = new PtocoloTransporteXIdTransporte2018V1Response();
            prot_res = _HabilitacionesService.lista_PtocoloTransporteXIdTransporte2018V1Response_x_id(id);

            ViewBag.razon_social = prot_res.razon_social;
            ViewBag.direccion_legal = prot_res.direccion_legal;
            ViewBag.representante_legal = prot_res.representante_legal;
            ViewBag.infra_pesq = prot_res.infra_pesq;
            ViewBag.placa = prot_res.placa;
            ViewBag.cod_habilitacion = prot_res.cod_habilitacion;
            ViewBag.nom_carroceria = prot_res.nombre_carroceria;
            ViewBag.siglas_um = prot_res.siglas_um;
            ViewBag.carga_util = prot_res.carga_util;
            ViewBag.acta_inspeccion = prot_res.acta_inspeccion;
            ViewBag.informe_auditoria = prot_res.informe_auditoria;
            ViewBag.informe_tecnico = prot_res.informe_tecnico_evaluacion;
            ViewBag.fecha_inicio = prot_res.fecha_inicio;
            ViewBag.fecha_fin = prot_res.fecha_fin;
            ViewBag.fecha_emision = prot_res.fecha_emision;
            ViewBag.nombre_protocolo = prot_res.nombre_protocolo;
            ViewBag.expediente = prot_res.expediente;
            ViewBag.nombre_tipo_furgon = prot_res.nombre_tipo_furgon;
            ViewBag.informe_sdhpa = prot_res.informe_sdhpa;

            ProtocoloTransporteRequest protres = new ProtocoloTransporteRequest();
            protres = _HabilitacionesService.lista_protocolo_transporte_x_id_protocolo(id);

            ViewBag.nombre_tipo_carr_tarpro = _HabilitacionesService.consulta_todo_activo_tipocarroceria_x_id(protres.id_tipo_carroceria_tarpro ?? 0).nombre;
            ViewBag.nombre_tipo_atencion = _HabilitacionesService.consulta_tipo_atencion_x_id(protres.id_tipo_atencion ?? 0).nombre;

            return View();
            /*
            ProtocoloTransporteRequest proto_trans_res = new ProtocoloTransporteRequest();
            proto_trans_res = _HabilitacionesService.lista_protocolo_transporte_x_id_protocolo(id);

            if(proto_trans_res.persona_2.Length>153)
            {
                NetworkCredential nwc = new NetworkCredential(ConfigurationManager.AppSettings["MvcReportViewer.Username"].ToString(), ConfigurationManager.AppSettings["MvcReportViewer.Password"].ToString());
                WebClient client = new WebClient();
                client.Credentials = nwc;
                string reportURL = ConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"].ToString() + "/?%2fGesdocReportes/Protocolo_transporte_reducido&ID=" + id.ToString() + "&rs:Command=Render&rs:Format=PDF";
                return File(client.DownloadData(reportURL), "application/pdf");
            }
            else
            {
                NetworkCredential nwc = new NetworkCredential(ConfigurationManager.AppSettings["MvcReportViewer.Username"].ToString(), ConfigurationManager.AppSettings["MvcReportViewer.Password"].ToString());
                WebClient client = new WebClient();
                client.Credentials = nwc;
                string reportURL = ConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"].ToString() + "/?%2fGesdocReportes/Protocolo_Transporte&ID=" + id.ToString() + "&rs:Command=Render&rs:Format=PDF";
                return File(client.DownloadData(reportURL), "application/pdf");
            }   
            */
        }

        [AllowAnonymous]
        public ActionResult Imprimir_solicitud_inspeccion_transporte(int id)
        {
            NetworkCredential nwc = new NetworkCredential(ConfigurationManager.AppSettings["MvcReportViewer.Username"].ToString(), ConfigurationManager.AppSettings["MvcReportViewer.Password"].ToString());
            WebClient client = new WebClient();
            client.Credentials = nwc;
            string persona = (_HabilitacionesService.Lista_solicitud_seguimiento_x_id_solicitud(id).usuario_crea).Split('-')[1].Trim();

            FirmasSdhpaResponse var_firma_resp = _HabilitacionesService.lista_firmas_sdhpa_activas(persona);

            if (var_firma_resp.nombre_reporte != null && var_firma_resp.nombre_reporte != "")
            {
                string reportURL = ConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"].ToString() + "/?%2fGesdocReportes/" + var_firma_resp.nombre_reporte + "&ID=" + id.ToString() + "&rs:Command=Render&rs:Format=PDF";
                return File(client.DownloadData(reportURL), "application/pdf");
            }
            else
            {
                string reportURL = ConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"].ToString() + "/?%2fGesdocReportes/Solicitud_inspeccion_transporte&ID=" + id.ToString() + "&rs:Command=Render&rs:Format=PDF";
                return File(client.DownloadData(reportURL), "application/pdf");
            }
        }

        [AllowAnonymous]
        public ActionResult Editar_var_Protocolo_transporte(int id)
        {
            if (id != null && id != 0)
            {
                Session["editar_proto_id_protocolo"] = id;
                return RedirectToAction("Editar_protocolo_transporte", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Editar_protocolo_transporte()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int id_protocolo = 0;
                    try
                    {
                        id_protocolo = Convert.ToInt32(Session["editar_proto_id_protocolo"].ToString());
                        Session.Remove("editar_proto_id_protocolo");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> lista_sexo = new List<SelectListItem>();

                    lista_sexo.Add(new SelectListItem() { Text = "MASCULINO", Value = "M" });
                    lista_sexo.Add(new SelectListItem() { Text = "FEMENINO", Value = "F" });

                    ViewBag.lst_combo_sexo = lista_sexo;


                    List<SelectListItem> Lista_tipo_doc_iden = new List<SelectListItem>();

                    var recupera_tipo_documento = _GeneralService.llenar_tipo_documento_identidad();

                    foreach (var result in recupera_tipo_documento)
                    {
                        Lista_tipo_doc_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                        );
                    };

                    ViewBag.lst_combo_tipo_identidad = Lista_tipo_doc_iden;

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;

                    List<SelectListItem> Lista_transporte = new List<SelectListItem>();
                    List<SelectListItem> Lista_carroceria = new List<SelectListItem>();
                    List<SelectListItem> Lista_unidad_medida = new List<SelectListItem>();
                    List<SelectListItem> Lista_furgon = new List<SelectListItem>();
                    ViewBag.lst_transporte = Lista_transporte;

                    var recupera_carroceria = _HabilitacionesService.consulta_todo_activo_tipocarroceria();
                    int entra = 0;
                    foreach (var result in recupera_carroceria)
                    {
                        if (entra == 0)
                        {
                            var recupera_furgon = _HabilitacionesService.consulta_todo_activo_tipofurgon(result.id_tipo_carroceria);

                            foreach (var result2 in recupera_furgon)
                            {
                                Lista_furgon.Add(new SelectListItem()
                                {
                                    Text = result2.nombre,
                                    Value = result2.id_tipo_furgon.ToString()
                                }
                                );
                            };
                            entra = 1;
                        }
                        Lista_carroceria.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_carroceria.ToString()
                        }
                        );
                    };

                    ViewBag.lst_tipo_furgon = Lista_furgon;

                    var recupera_um = _HabilitacionesService.consulta_todo_activo_unidad_medida();

                    foreach (var result in recupera_um)
                    {
                        Lista_unidad_medida.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.id_um.ToString()
                        }
                        );
                    };


                    List<SelectListItem> Lista_tipo_atencion = new List<SelectListItem>();
                    var recupera_tipo_atencion = _HabilitacionesService.consulta_todo_tipo_atencion();

                    foreach (var result in recupera_tipo_atencion)
                    {
                        Lista_tipo_atencion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_atencion.ToString()
                        }
                        );
                    };

                    ViewBag.lst_nuevo_carroceria = Lista_carroceria;
                    ViewBag.lst_tipo_carroceria_tarjpro = Lista_carroceria;
                    ViewBag.lst_tipo_atencion = Lista_tipo_atencion;
                    ViewBag.lst_nuevo_um = Lista_unidad_medida;

                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    List<SelectListItem> lista_infraestructura_pesquera = new List<SelectListItem>();

                    var var_lista_tipo_camara_transporte = _HabilitacionesService.consulta_todo_activo_tipoCamaraTransporte();

                    foreach (var result in var_lista_tipo_camara_transporte)
                    {
                        lista_infraestructura_pesquera.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_camara_trans.ToString()
                        }
                        );
                    };

                    ViewBag.lst_tipo_camara_transporte = lista_infraestructura_pesquera;
                    ViewBag.var_id_protocolo = id_protocolo.ToString();
                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    ProtocoloRequest proto_res = new ProtocoloRequest();
                    proto_res = _HabilitacionesService.lista_protocolo_x_id(id_protocolo);
                    ViewBag.var_protocolo_nombre = proto_res.nombre;
                    ViewBag.var_fecha_ini = proto_res.fecha_inicio.ToString().Substring(0, 10);
                    ViewBag.var_fecha_fin = proto_res.fecha_fin.ToString().Substring(0, 10);

                    ProtocoloTransporteRequest proto_trans_res = new ProtocoloTransporteRequest();
                    proto_trans_res = _HabilitacionesService.lista_protocolo_transporte_x_id_protocolo(id_protocolo);

                    ViewBag.var_id_dat_pro_transporte = proto_trans_res.id_dat_pro_transporte.ToString();
                    ViewBag.var_acta_inspeccion = proto_trans_res.acta_inspeccion;
                    ViewBag.var_informe_auditoria = proto_trans_res.informe_auditoria;
                    ViewBag.var_informe_tecnico_evaluacion = proto_trans_res.informe_tecnico_evaluacion;
                    ViewBag.var_txt_persona_2 = proto_trans_res.persona_2;
                    ViewBag.var_info_sdhpa = proto_trans_res.informe_sdhpa;
                    int var_id_ses = proto_res.id_seguimiento ?? 0;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();
                    }
                    else
                    {
                        ViewBag.id_persona_legal = rec_seg.id_dni_persona_legal.ToString();
                    }

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                        ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;
                    }
                    else
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal_DNI;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal_DNI;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal_DNI;
                        ViewBag.Str_Direccion_Legal = rec_seg.str_direccion_persona_natural;
                    }

                    ViewBag.Str_Persona = rec_seg.nom_persona_ext;
                    ViewBag.var_DNI = rec_seg.persona_num_documento;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    DbGeneralMaeTransporteResponse trans_res = new DbGeneralMaeTransporteResponse();
                    if (rec_seg.id_habilitante != 0)
                    {
                        trans_res = _HabilitacionesService.consulta_db_general_transporte_x_id(rec_seg.id_habilitante ?? 0);
                    }
                    if (trans_res.id_transporte != 0 && trans_res.id_transporte != null)
                    {
                        ViewBag.id_transporte = trans_res.id_transporte.ToString();
                        ViewBag.placa = trans_res.placa.ToString();
                        ViewBag.carroceria = trans_res.nombre_carroceria.ToString();
                        ViewBag.furgon = trans_res.nombre_furgon.ToString();
                        ViewBag.carga_util = trans_res.carga_util.ToString() + " " + trans_res.nombre_um.ToString();
                        ViewBag.codigo_hab = trans_res.cod_habilitacion.ToString();
                    }
                    else
                    {
                        ViewBag.id_transporte = "0";
                        ViewBag.placa = "";
                        ViewBag.carroceria = "";
                        ViewBag.furgon = "";
                        ViewBag.carga_util = "";
                        ViewBag.codigo_hab = "";
                    }


                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editar_protocolo_transporte(int txt_id_seguimiento, DateTime txt_fecha_inicio, DateTime txt_fecha_fin,
           int txt_id_nombre_legal, int txt_id_direccion_legal, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add, int txt_id_transporte,
            int cmb_infra_pesq, string lbl_acta_inspeccion, string lbl_inf_auditoria, string lbl_inf_tecnico, int txt_id_protocolo, int txt_id_dat_protocolo_transporte, string txt_persona_2, string lbl_Direccion_legal,
            int cmb_tipo_carro_tarpro, int cmb_tipo_atencion, string txt_nombre, string lbl_info_sdhpa)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo = _HabilitacionesService.lista_protocolo_x_id(txt_id_protocolo);

                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;
                        _HabilitacionesService.actualizar_protocolo(req_protocolo);

                        ProtocoloTransporteRequest req_protocolo_transporte = new ProtocoloTransporteRequest();
                        req_protocolo_transporte = _HabilitacionesService.lista_protocolo_transporte_x_id_protocolo(txt_id_protocolo);

                        req_protocolo_transporte.id_tipo_camara_trans = cmb_infra_pesq;
                        req_protocolo_transporte.id_transporte = txt_id_transporte;
                        req_protocolo_transporte.id_tipo_carroceria_tarpro = cmb_tipo_carro_tarpro;
                        req_protocolo_transporte.id_tipo_atencion = cmb_tipo_atencion;

                        if (txt_id_direccion_legal == null || txt_id_direccion_legal == 0)
                        {
                            req_protocolo_transporte.representante_legal = 0;
                            req_protocolo_transporte.direccion_legal = 0;
                            req_protocolo_transporte.direccion_legal_dni = lbl_Direccion_legal;
                            req_protocolo_transporte.representante_legal_dni = txt_id_nombre_legal;
                        }
                        else
                        {
                            req_protocolo_transporte.representante_legal = txt_id_nombre_legal;
                            req_protocolo_transporte.direccion_legal = txt_id_direccion_legal;
                            req_protocolo_transporte.direccion_legal_dni = "";
                            req_protocolo_transporte.representante_legal_dni = 0;
                        }

                        DbGeneralMaeTransporteResponse tra_res = new DbGeneralMaeTransporteResponse();
                        tra_res = _HabilitacionesService.consulta_db_general_transporte_x_id(txt_id_transporte);

                        req_protocolo_transporte.placa = tra_res.placa;
                        req_protocolo_transporte.cod_habilitacion = tra_res.cod_habilitacion;
                        req_protocolo_transporte.id_tipo_carroceria = tra_res.id_tipo_carroceria;
                        req_protocolo_transporte.id_tipo_furgon = tra_res.id_tipo_furgon;
                        req_protocolo_transporte.id_um = tra_res.id_um;
                        req_protocolo_transporte.carga_util = tra_res.carga_util;
                        req_protocolo_transporte.acta_inspeccion = lbl_acta_inspeccion;
                        req_protocolo_transporte.informe_auditoria = lbl_inf_auditoria;
                        req_protocolo_transporte.informe_tecnico_evaluacion = lbl_inf_tecnico;
                        req_protocolo_transporte.persona_2 = txt_persona_2;
                        req_protocolo_transporte.informe_sdhpa = lbl_info_sdhpa;

                        _HabilitacionesService.Update_Protocolo_Transporte(req_protocolo_transporte);

                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        @ViewBag.Mensaje = "Se modificó el protocolo " + req_protocolo.nombre + " satisfactoriamente |" + req_protocolo.id_protocolo.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult Nuevo_Seguimiento_reistro_OD()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (permiso == 1)
                    {
                        List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();
                        List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                        List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                        List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();
                        List<SelectListItem> Lista_Almacen = new List<SelectListItem>();
                        List<SelectListItem> Lista_Concesion = new List<SelectListItem>();
                        List<SelectListItem> Lista_TUPA = new List<SelectListItem>();
                        List<SelectListItem> lista_tipo_seguimiento = new List<SelectListItem>();

                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = "SELECCIONAR",
                            Value = ""
                        });

                        foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                        {
                            Lista_TUPA.Add(new SelectListItem()
                            {
                                Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                                Value = result.id_tupa.ToString() + "|" + result.id_tipo_procedimiento.ToString() + "|" + result.asunto + "|" + result.dias_tupa.ToString()
                            });
                        };

                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = "RUC",
                            Value = "0"
                        });

                        foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                        {
                            lista_tipo_documento_iden.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.tipo_doc_iden.ToString()
                            }
                                );
                        };

                        Lista_Oficinas_externas.Add(new SelectListItem()
                        {
                            Text = "SELECCIONAR OFICINAS",
                            Value = ""
                        });

                        foreach (var result in _GeneralService.Recupera_oficina_todo())
                        {
                            if (result.ruc != "20565429656")
                            {
                                Lista_Oficinas_externas.Add(new SelectListItem()
                                {
                                    Text = result.nombre,
                                    Value = result.id_oficina.ToString()
                                }
                                );
                            }
                        };


                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = "PROCEDIMIENTOS",
                            Value = "0"
                        });

                        foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                        {
                            lista_tipo_procedimiento.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_procedimiento.ToString()
                            }
                                );
                        }

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add("ID_FACTURA");
                        tbl.Columns.Add("NUM1");
                        tbl.Columns.Add("NUM2");
                        tbl.Columns.Add("FACTURA");
                        tbl.Columns.Add("FECHA");
                        tbl.Columns.Add("IMPORTE");

                        var facturas = _GeneralService.listar_factura();

                        foreach (var result in facturas)
                        {
                            var num1 = "000" + result.num1_fact.ToString();
                            num1 = num1.Substring(num1.Length - 3, 3);
                            var num2 = "000000" + result.num2_fact.ToString();
                            num2 = num2.Substring(num2.Length - 6, 6);
                            tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                        };

                        ViewData["Facturas_Lista"] = tbl;

                        tbl = new DataTable();
                        tbl.Columns.Add("ID_EXPEDIENTE");
                        tbl.Columns.Add("EXPEDIENTE");

                        var expedientes = _GeneralService.llenar_expediente("0");

                        foreach (var result in expedientes)
                        {
                            if (result.id_tipo_expediente == 90) { tbl.Rows.Add(result.id_expediente, result.nom_expediente); }
                            else { tbl.Rows.Add(result.id_expediente, result.nom_expediente + "." + _GeneralService.llenar_tipo_expediente(result.id_tipo_expediente, 0).First().nombre); }

                        };

                        foreach (var result in _GeneralService.recupera_tipo_seguimiento())
                        {
                            lista_tipo_seguimiento.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_seguimiento.ToString()
                            });
                        };

                        ViewBag.lst_tipo_seguimiento = lista_tipo_seguimiento;
                        ViewData["Expediente_Lista"] = tbl;
                        ViewBag.lst_tupa = Lista_TUPA;
                        ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                        ViewBag.lst_embarcacion = Lista_embarcaciones;
                        ViewBag.lst_almacen = Lista_Almacen;
                        ViewBag.lst_concesion = Lista_Concesion;
                        ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;
                        ViewBag.lst_tipo_documento_iden = lista_tipo_documento_iden;
                        ViewBag.lstOficina = Lista_Oficinas_externas;
                        List<SelectListItem> lista_direcciones = new List<SelectListItem>();
                        List<SelectListItem> lista_plantas = new List<SelectListItem>();
                        List<SelectListItem> lista_desembarcaderos = new List<SelectListItem>();
                        List<SelectListItem> lista_oficinas = new List<SelectListItem>();
                        List<SelectListItem> lista_personas = new List<SelectListItem>();
                        ViewBag.lst_direcciones = lista_direcciones;
                        ViewBag.lst_oficinas = lista_oficinas;
                        ViewBag.lst_persona_ext = lista_personas;
                        ViewBag.lst_plantas = lista_plantas;
                        ViewBag.lst_desembarcadero = lista_desembarcaderos;

                        SeguimientoViewModel model = new SeguimientoViewModel();

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Seguimiento_reistro_OD(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (permiso == 1)
                    {
                        try
                        {
                            string todo_expediente = "";

                            if (model.det_seg_exp != null)
                            {
                                foreach (detsegexpViewModel obj in model.det_seg_exp)
                                {
                                    ExpedientesResponse req_exp = new ExpedientesResponse();
                                    req_exp = _HabilitacionesService.GetExpediente_x_id(obj.id_expediente);
                                    if (todo_expediente == "")
                                    {
                                        if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                        else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }

                                    }
                                    else
                                    {
                                        if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                        else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                    }
                                }
                            }

                            DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                            if (request_doc.num_documento == 0)
                            {
                                request_doc.num_documento = null;
                            }
                            request_doc.usuario_od = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            request_doc.usuario_crea = null;
                            request_doc.usuario_recepcion_sdhpa = null;
                            request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                            request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                            request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                            request_doc.fecha_od = DateTime.Now;
                            request_doc.fecha_recibido_evaluador = null;
                            request_doc.fecha_crea = null;
                            request_doc.id_servicio_dhcpa = 0;
                            request_doc.expedientes_relacion = todo_expediente;
                            request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                            request_doc.indicador = "1"; // '1' INICIAL, '2' SECUNDARIO

                            model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);

                            if (model.det_fac_doc != null)
                            {
                                request_doc.det_doc_fact = new List<DetDocFactRequest>();

                                foreach (DetDocFactViewModel obj in model.det_fac_doc)
                                {
                                    DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                    req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                    req_det_doc_fac.activo = "1";
                                    _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                                }
                            }

                            if (model.id_ofi_dir == 0)
                            {
                                model.id_ofi_dir = null;
                            }

                            SeguimientoDhcpaRequest request_seguimiento = ModelToRequest.Seguimiento_dhcpa(model);
                            request_seguimiento.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                            request_seguimiento.nom_oficina_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                            request_seguimiento.persona_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            request_seguimiento.fecha_inicio = DateTime.Now;
                            request_seguimiento.estado = "0"; //'0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO

                            if (model.det_seg_exp != null)
                            {
                                request_doc.det_seg_doc = new List<DetSegDocRequest>();

                                foreach (detsegexpViewModel obj in model.det_seg_exp)
                                {
                                    request_seguimiento.id_expediente = obj.id_expediente;
                                    ExpedientesRequest req_exp = new ExpedientesRequest();
                                    req_exp = _HabilitacionesService.GetExpediente(obj.id_expediente);
                                    if (req_exp.indicador_seguimiento == "0")
                                    {
                                        int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);
                                        req_exp.indicador_seguimiento = "1"; // con seguimiento
                                        _HabilitacionesService.Update_mae_expediente(req_exp);
                                        DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                                        req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                                        req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                                        req_det_seg_doc.activo = "1";

                                        _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                                    }

                                }
                            }
                            else
                            {
                                int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);

                                DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                                req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                                req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                                req_det_seg_doc.activo = "1";

                                _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                            }

                            @ViewBag.Mensaje = model.id_doc_seg.ToString();
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }
                        return PartialView("_Success_NS");
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AllowAnonymous]
        public ActionResult Agregar_Seguimiento_registro_OD()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (permiso == 1)
                    {
                        List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();

                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = "PROCEDIMIENTOS",
                            Value = "0"
                        });

                        foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                        {
                            lista_tipo_procedimiento.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_procedimiento.ToString()
                            }
                                );
                        }

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add("ID_FACTURA");
                        tbl.Columns.Add("NUM1");
                        tbl.Columns.Add("NUM2");
                        tbl.Columns.Add("FACTURA");
                        tbl.Columns.Add("FECHA");
                        tbl.Columns.Add("IMPORTE");

                        var facturas = _GeneralService.listar_factura();

                        foreach (var result in facturas)
                        {
                            var num1 = "000" + result.num1_fact.ToString();
                            num1 = num1.Substring(num1.Length - 3, 3);
                            var num2 = "000000" + result.num2_fact.ToString();
                            num2 = num2.Substring(num2.Length - 6, 6);
                            tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                        };

                        ViewData["Facturas_Lista"] = tbl;

                        tbl = new DataTable();
                        tbl.Columns.Add("ID_SEGUIMIENTO");
                        tbl.Columns.Add("FECHA_SEGUIMIENTO");
                        tbl.Columns.Add("SEGUIMIENTO");
                        tbl.Columns.Add("EXTERNO");
                        tbl.Columns.Add("EMBARCACION");

                        var list_seguimiento = _HabilitacionesService.GetAllSeguimiento("");

                        foreach (var result in list_seguimiento)
                        {
                            string seguimiento = "";
                            if (result.Expediente != null)
                            { seguimiento = result.Expediente + "." + result.nom_tipo_expediente + "(" + result.nom_tipo_procedimiento + ")"; }
                            else
                            { seguimiento = result.nom_tipo_procedimiento; }
                            string externo = "";
                            if (result.nom_oficina_ext != null)
                            { externo = result.ruc + " - " + result.nom_oficina_ext; }
                            else
                            { externo = result.persona_num_documento + " - " + result.nom_persona_ext; }
                            tbl.Rows.Add(result.id_seguimiento, result.fecha_inicio.ToShortDateString(), seguimiento, externo, result.nom_embarcacion);
                        };

                        List<SelectListItem> lista_servicio = new List<SelectListItem>();

                        lista_servicio.Add(new SelectListItem()
                        {
                            Text = "SELECCIONAR SERVICIO",
                            Value = "0"
                        });

                        foreach (var result in _GeneralService.llenar_servicio_dhcpa())
                        {
                            lista_servicio.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_servicio_dhcpa.ToString()
                            });
                        };

                        ViewData["Seguimiento_Lista"] = tbl;
                        ViewBag.lst_servicio_dhcpa = lista_servicio;

                        ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                        ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;

                        SeguimientoViewModel model = new SeguimientoViewModel();

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Agregar_Seguimiento_registro_OD(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (permiso == 1)
                    {
                        try
                        {
                            string todo_expediente = "";
                            foreach (detsegpadreViewModel obj in model.det_seg_padre)
                            {
                                int id_expediente = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento).id_expediente ?? 0;
                                if (id_expediente != 0)
                                {
                                    ExpedientesResponse req_exp = new ExpedientesResponse();
                                    req_exp = _HabilitacionesService.GetExpediente_x_id(id_expediente);
                                    if (todo_expediente == "")
                                    {
                                        if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                        else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }

                                    }
                                    else
                                    {
                                        if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                        else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                    }
                                }
                            }

                            string evaluador_recp = null;
                            foreach (detsegpadreViewModel obj in model.det_seg_padre)
                            {
                                var recup_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                                evaluador_recp = recup_seg_dhcpa.evaluador;
                            }


                            DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                            if (request_doc.num_documento == 0)
                            {
                                request_doc.num_documento = null;
                            }

                            request_doc.usuario_od = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            request_doc.usuario_crea = null;
                            request_doc.usuario_recepcion_sdhpa = null;
                            request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                            request_doc.fecha_recepcion_sdhpa = null;
                            request_doc.fecha_asignacion_evaluador = null;
                            request_doc.fecha_od = DateTime.Now;
                            request_doc.fecha_recibido_evaluador = null;
                            request_doc.fecha_crea = null;
                            request_doc.expedientes_relacion = todo_expediente;
                            request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                            request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                            request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                            request_doc.indicador = "2"; // '1' INICIAL, '2' SECUNDARIO

                            if (evaluador_recp != null)
                            {
                                request_doc.usuario_recepcion_sdhpa = "20565429656 - " + evaluador_recp;
                                request_doc.fecha_recepcion_sdhpa = DateTime.Now;
                                request_doc.estado = "1"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                                request_doc.fecha_asignacion_evaluador = DateTime.Now;
                                request_doc.evaluador = evaluador_recp;
                            }

                            model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);

                            if (model.det_fac_doc != null)
                            {
                                request_doc.det_doc_fact = new List<DetDocFactRequest>();

                                foreach (DetDocFactViewModel obj in model.det_fac_doc)
                                {
                                    DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                    req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                    req_det_doc_fac.activo = "1";
                                    _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                                }
                            }

                            request_doc.det_seg_doc = new List<DetSegDocRequest>();

                            foreach (detsegpadreViewModel obj in model.det_seg_padre)
                            {
                                DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                                req_det_seg_doc.id_seguimiento = obj.id_seguimiento;
                                req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                                req_det_seg_doc.activo = "1";
                                _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);

                                SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                                req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                                //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO
                                if (req_seg_dhcpa.estado == "3")
                                {
                                    req_seg_dhcpa.estado = "2";
                                    _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);
                                }
                            }


                            @ViewBag.Mensaje = model.id_doc_seg.ToString();
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }
                        return PartialView("_Success_NS");
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AllowAnonymous]
        public ActionResult Consulta_seguimiento_habilitaciones(int page = 1, string expediente = "", string externo = "", string habilitante = "", string cmbestado = "", int cmbtupa = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || HttpContext.User.Identity.Name.Split('|')[9].ToString().Split(',')[32].Trim() == "1")
                {

                    List<SelectListItem> Lista_estado_seguimiento_dhcpa = new List<SelectListItem>();

                    Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });

                    var recupera_estado_seguimiento = _HabilitacionesService.Lista_estado_seguimiento_dhcpa();
                    foreach (var result in recupera_estado_seguimiento)
                    {
                        if (result.id_estado != "4")
                        {
                            Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_estado
                            }
                            );
                        }
                    };

                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_TUPA;
                    ViewBag.lst_estado = Lista_estado_seguimiento_dhcpa;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Fecha Inicio");
                    tbl.Columns.Add("Expediente");
                    tbl.Columns.Add("TUPA/SERV");
                    tbl.Columns.Add("Procedimiento");
                    tbl.Columns.Add("Externo");
                    tbl.Columns.Add("Habilitante");
                    tbl.Columns.Add("Evaluador");
                    tbl.Columns.Add("Estado");
                    tbl.Columns.Add("Expediente_Id_seguimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_cond_finalizar");


                    var seguimiento = _HabilitacionesService.GetAllSeguimiento_Consulta_sin_paginado(expediente, "", externo, habilitante, cmbestado, 0, cmbtupa);

                    foreach (var result in seguimiento)
                    {
                        if (result.num_tupa == null)
                        {
                            tbl.Rows.Add(

                            result.fecha_inicio,
                            result.Expediente,
                            "",
                            result.nom_tipo_procedimiento,
                            result.nom_oficina_ext,
                            result.cod_habilitante,
                            result.nom_evaluador,
                            result.nom_estado,
                            result.Expediente + "|" + result.id_seguimiento.ToString(),
                            result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                            );
                        }
                        else
                        {
                            tbl.Rows.Add(

                            result.fecha_inicio,
                            result.Expediente,
                            result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                            result.nom_tipo_procedimiento,
                            result.nom_oficina_ext,
                            result.cod_habilitante,
                            result.nom_evaluador,
                            result.nom_estado,
                            result.Expediente + "|" + result.id_seguimiento.ToString(),
                            result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                            );
                        }

                    };

                    ViewData["Seguimiento_Tabla"] = tbl;

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


        [AllowAnonymous]
        public ActionResult Consulta_seguimiento_x_registro_OD(int page = 1, string expediente = "", string externo = "", string habilitante = "", string cmbestado = "", int cmbtupa = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "15" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "52")
                    {
                        permiso = 1;
                    }
                    if (permiso == 1)
                    {
                        List<SelectListItem> Lista_estado_seguimiento_dhcpa = new List<SelectListItem>();

                        Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                        {
                            Text = "TODO",
                            Value = ""
                        });

                        var recupera_estado_seguimiento = _HabilitacionesService.Lista_estado_seguimiento_dhcpa();
                        foreach (var result in recupera_estado_seguimiento)
                        {
                            if (result.id_estado != "4")
                            {
                                Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                                {
                                    Text = result.nombre,
                                    Value = result.id_estado
                                }
                                );
                            }
                        };

                        List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                        Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                        foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                        {
                            Lista_TUPA.Add(new SelectListItem()
                            {
                                Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                                Value = result.id_tupa.ToString()
                            });
                        };

                        ViewBag.lst_tupa = Lista_TUPA;
                        ViewBag.lst_estado = Lista_estado_seguimiento_dhcpa;

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add("Fecha Inicio");
                        tbl.Columns.Add("Expediente");
                        tbl.Columns.Add("TUPA/SERV");
                        tbl.Columns.Add("Procedimiento");
                        tbl.Columns.Add("Externo");
                        tbl.Columns.Add("Habilitante");
                        tbl.Columns.Add("Evaluador");
                        tbl.Columns.Add("Estado");
                        tbl.Columns.Add("Expediente_Id_seguimiento");
                        tbl.Columns.Add("Expediente_Id_seguimiento_cond_finalizar");


                        var seguimiento = _HabilitacionesService.GetAllSeguimiento_Consulta_sin_paginado(expediente, "", externo, habilitante, cmbestado, 0, cmbtupa);

                        foreach (var result in seguimiento)
                        {
                            if (result.num_tupa == null)
                            {
                                tbl.Rows.Add(

                                result.fecha_inicio,
                                result.Expediente,
                                "",
                                result.nom_tipo_procedimiento,
                                result.nom_oficina_ext,
                                result.cod_habilitante,
                                result.nom_evaluador,
                                result.nom_estado,
                                result.Expediente + "|" + result.id_seguimiento.ToString(),
                                result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                );
                            }
                            else
                            {
                                tbl.Rows.Add(

                                result.fecha_inicio,
                                result.Expediente,
                                result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                result.nom_tipo_procedimiento,
                                result.nom_oficina_ext,
                                result.cod_habilitante,
                                result.nom_evaluador,
                                result.nom_estado,
                                result.Expediente + "|" + result.id_seguimiento.ToString(),
                                result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                );
                            }

                        };

                        ViewData["Seguimiento_Tabla"] = tbl;

                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AllowAnonymous]
        public ActionResult Nuevo_Seguimiento()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {

                    List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();
                    List<SelectListItem> Lista_Almacen = new List<SelectListItem>();
                    List<SelectListItem> Lista_Concesion = new List<SelectListItem>();
                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_seguimiento = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "" });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString() + "|" + result.id_tipo_procedimiento.ToString() + "|" + result.asunto + "|" + result.dias_tupa.ToString()
                        });
                    };

                    foreach (var result in _GeneralService.recupera_tipo_seguimiento())
                    {
                        lista_tipo_seguimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_seguimiento.ToString()
                        });
                    };

                    lista_tipo_documento_iden.Add(new SelectListItem() { Text = "RUC", Value = "0" });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        });
                    };

                    Lista_Oficinas_externas.Add(new SelectListItem() { Text = "SELECCIONAR OFICINAS", Value = "" });

                    foreach (var result in _GeneralService.Recupera_oficina_todo())
                    {
                        if (result.ruc != "20565429656")
                        {
                            Lista_Oficinas_externas.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_oficina.ToString()
                            });
                        }
                    };

                    lista_tipo_procedimiento.Add(new SelectListItem() { Text = "PROCEDIMIENTOS", Value = "0" });

                    foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                    {
                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_procedimiento.ToString()
                        });
                    }

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_FACTURA");
                    tbl.Columns.Add("NUM1");
                    tbl.Columns.Add("NUM2");
                    tbl.Columns.Add("FACTURA");
                    tbl.Columns.Add("FECHA");
                    tbl.Columns.Add("IMPORTE");

                    var facturas = _GeneralService.listar_factura();

                    foreach (var result in facturas)
                    {
                        var num1 = "000" + result.num1_fact.ToString();
                        num1 = num1.Substring(num1.Length - 3, 3);
                        var num2 = "000000" + result.num2_fact.ToString();
                        num2 = num2.Substring(num2.Length - 6, 6);
                        tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                    };

                    ViewData["Facturas_Lista"] = tbl;

                    tbl = new DataTable();
                    tbl.Columns.Add("ID_EXPEDIENTE");
                    tbl.Columns.Add("EXPEDIENTE");

                    var expedientes = _GeneralService.llenar_expediente("0");

                    foreach (var result in expedientes)
                    {

                        if (result.id_tipo_expediente == 90) { tbl.Rows.Add(result.id_expediente, result.nom_expediente); }
                        else { tbl.Rows.Add(result.id_expediente, result.nom_expediente + "." + _GeneralService.llenar_tipo_expediente(result.id_tipo_expediente, 0).First().nombre); }
                    };

                    ViewData["Expediente_Lista"] = tbl;

                    ViewBag.lst_tupa = Lista_TUPA;
                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    ViewBag.lst_embarcacion = Lista_embarcaciones;
                    ViewBag.lst_almacen = Lista_Almacen;
                    ViewBag.lst_concesion = Lista_Concesion;
                    /*ViewBag.lst_evaluador = Lista_Evaluador;*/
                    ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;
                    ViewBag.lst_tipo_documento_iden = lista_tipo_documento_iden;
                    ViewBag.lst_tipo_seguimiento = lista_tipo_seguimiento;
                    ViewBag.lstOficina = Lista_Oficinas_externas;
                    List<SelectListItem> lista_direcciones = new List<SelectListItem>();
                    List<SelectListItem> lista_plantas = new List<SelectListItem>();
                    List<SelectListItem> lista_desembarcaderos = new List<SelectListItem>();
                    List<SelectListItem> lista_oficinas = new List<SelectListItem>();
                    List<SelectListItem> lista_personas = new List<SelectListItem>();
                    ViewBag.lst_direcciones = lista_direcciones;
                    ViewBag.lst_oficinas = lista_oficinas;
                    ViewBag.lst_persona_ext = lista_personas;
                    ViewBag.lst_plantas = lista_plantas;
                    ViewBag.lst_desembarcadero = lista_desembarcaderos;

                    SeguimientoViewModel model = new SeguimientoViewModel();

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Seguimiento(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {
                    try
                    {
                        string todo_expediente = "";
                        if (model.det_seg_exp != null)
                        {
                            foreach (detsegexpViewModel obj in model.det_seg_exp)
                            {
                                ExpedientesResponse req_exp = new ExpedientesResponse();
                                req_exp = _HabilitacionesService.GetExpediente_x_id(obj.id_expediente);
                                if (todo_expediente == "")
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                    else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }

                                }
                                else
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                    else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                }
                            }
                        }

                        DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                        if (request_doc.num_documento == 0)
                        {
                            request_doc.num_documento = null;
                        }
                        request_doc.fecha_crea = DateTime.Now;
                        request_doc.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                        request_doc.fecha_recibido_evaluador = null;
                        request_doc.id_servicio_dhcpa = 0;
                        request_doc.expedientes_relacion = todo_expediente;
                        request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        request_doc.indicador = "1"; // '1' INICIAL, '2' SECUNDARIO

                        model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);


                        if (model.det_fac_doc != null)
                        {
                            request_doc.det_doc_fact = new List<DetDocFactRequest>();

                            foreach (DetDocFactViewModel obj in model.det_fac_doc)
                            {
                                DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                req_det_doc_fac.activo = "1";
                                _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                            }
                        }

                        if (model.id_ofi_dir == 0)
                        {
                            model.id_ofi_dir = null;
                        }

                        SeguimientoDhcpaRequest request_seguimiento = ModelToRequest.Seguimiento_dhcpa(model);
                        request_seguimiento.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_seguimiento.nom_oficina_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_seguimiento.persona_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_seguimiento.fecha_inicio = DateTime.Now;
                        request_seguimiento.estado = "0"; //'0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO

                        if (model.det_seg_exp != null)
                        {
                            request_doc.det_seg_doc = new List<DetSegDocRequest>();

                            foreach (detsegexpViewModel obj in model.det_seg_exp)
                            {
                                request_seguimiento.id_expediente = obj.id_expediente;
                                ExpedientesRequest req_exp = new ExpedientesRequest();
                                req_exp = _HabilitacionesService.GetExpediente(obj.id_expediente);
                                if (req_exp.indicador_seguimiento == "0")
                                {
                                    int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);
                                    req_exp.indicador_seguimiento = "1"; // con seguimiento
                                    _HabilitacionesService.Update_mae_expediente(req_exp);
                                    DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                                    req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                                    req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                                    req_det_seg_doc.activo = "1";

                                    _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                                }
                            }
                        }
                        else
                        {
                            int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);

                            DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                            req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                            req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                            req_det_seg_doc.activo = "1";

                            _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                        }

                        @ViewBag.Mensaje = model.id_doc_seg.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult Ver_documento(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                string ruta = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO"].ToString() + "/" + id.ToString() + ".pdf";
                return File(ruta, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Ver_doc_adj(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                string ruta = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO_ADJUNTO"].ToString() + "/" + id.ToString() + ".pdf";
                return File(ruta, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult ver_acta_adjunta_si(string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                string ruta = ConfigurationManager.AppSettings["RUTA_PDF_ACTA_INSPECCION_SI"].ToString() + "/" + id.ToString() + ".pdf";
                return File(ruta, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }
        [AllowAnonymous]
        public ActionResult ver_info_adjunto_si(string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                string ruta = ConfigurationManager.AppSettings["RUTA_PDF_INFORME_INSPECCION_SI"].ToString() + "/" + id.ToString() + ".pdf";
                return File(ruta, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult ver_acta_chk_lis_si(string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                string ruta = ConfigurationManager.AppSettings["RUTA_PDF_CHECK_LIST_INSPECCION_SI"].ToString() + "/" + id.ToString() + ".pdf";
                return File(ruta, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult ver_pruebas_adjunto_si(string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                string cadena = "";
                var result = _HabilitacionesService.GetAllpruebas_inspeccion_req_x_id_sol_insp_sdhpa(Convert.ToInt32(id));
                foreach (var x in result)
                {
                    if (cadena == "")
                    {
                        cadena = x.id_prueba_insp.ToString();
                    }
                    else
                    {
                        cadena = cadena + "," + x.id_prueba_insp.ToString();
                    }

                }
                ViewBag.Catalogo_imagenes = cadena;
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Vista_imagenes_pruebas_si(string id = "")
        {
            string pth = ConfigurationManager.AppSettings["RUTA_PDF_PRUEBAS_INSPECCION_SI"].ToString() + "/" + id + ".jpg";
            return File(pth, "image/jpeg");
        }

        [AllowAnonymous]
        public ActionResult variable_archivo_nuevo_seguimiento_OD(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_nuevo_seguimiento"] = id;
                Session["archivo_tipo_adjunto"] = 0;
                return RedirectToAction("Adjuntar_archivo_nuevo_seguimiento_OD", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }


        [AllowAnonymous]
        public ActionResult variable_adjuntar_informe_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_informe_id_sol_in"] = id;
                return RedirectToAction("Adjuntar_archivo_informe_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult variable_adjuntar_chk_lis_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_chk_list_id_sol_in"] = id;
                return RedirectToAction("Adjuntar_archivo_chk_list_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult variable_adjuntar_pruebas_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_pruebas_id_sol_in"] = id;
                return RedirectToAction("Adjuntar_archivo_pruebas_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }


        [AllowAnonymous]
        public ActionResult variable_actualizar_chck_lst_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_chk_list_id_chck_lst"] = id;
                return RedirectToAction("actualizar_archivo_chck_lst_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult actualizar_archivo_chck_lst_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_chck_lst = 0;

                    try
                    {
                        id_chck_lst = Convert.ToInt32(Session["archivo_chk_list_id_chck_lst"].ToString());
                        Session.Remove("archivo_chk_list_id_chck_lst");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    CheckListInspeccionDsfpaRequest chck_lst_insp_si_sdhpa_req = new CheckListInspeccionDsfpaRequest();
                    chck_lst_insp_si_sdhpa_req = _HabilitacionesService.GetAllchk_list_inspeccion_req(id_chck_lst);

                    ViewBag.Str_Chck_lst = "Check List de Inspección: " + chck_lst_insp_si_sdhpa_req.nombre_check_list;
                    ViewBag.id_chck_lst_si = id_chck_lst.ToString();
                    ViewBag.nombre_chck_lst_si = chck_lst_insp_si_sdhpa_req.nombre_check_list;
                    ViewBag.lst_personal_od = Lista_personal;

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
        public ActionResult actualizar_archivo_chck_lst_si(HttpPostedFileBase file, int id_chck_lst_si, string txt_nombre_chck_lst, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_CHECK_LIST_INSPECCION_SI"].ToString();

                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, id_chck_lst_si.ToString() + ".pdf"));
                        CheckListInspeccionDsfpaRequest chck_lst_insp_si_sdhpa_req = new CheckListInspeccionDsfpaRequest();
                        chck_lst_insp_si_sdhpa_req = _HabilitacionesService.GetAllchk_list_inspeccion_req(id_chck_lst_si);
                        chck_lst_insp_si_sdhpa_req.nombre_check_list = txt_nombre_chck_lst;
                        chck_lst_insp_si_sdhpa_req.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        chck_lst_insp_si_sdhpa_req.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        chck_lst_insp_si_sdhpa_req.fecha_carga = DateTime.Now;
                        chck_lst_insp_si_sdhpa_req.inspector = CMBINSPECTOR;
                        chck_lst_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + id_chck_lst_si.ToString() + ".pdf";
                        _HabilitacionesService.Update_chk_list_insp_dsfpa(chck_lst_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_chk_list_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_sol_in = 0;

                    try
                    {
                        id_sol_in = Convert.ToInt32(Session["archivo_chk_list_id_sol_in"].ToString());
                        Session.Remove("archivo_chk_list_id_sol_in");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.Str_Solicitud = "Solicitud de Inspección: " + _HabilitacionesService.Consultar_solicitud_inspeccion_sdhpa_x_id(id_sol_in).numero_documento;
                    ViewBag.Id_solicitud_sdhpa = id_sol_in;
                    ViewBag.lst_personal_od = Lista_personal;
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
        public ActionResult Adjuntar_archivo_chk_list_si(HttpPostedFileBase file, int Id_solicitud_sdhpa, string txt_nombre_chk_list, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    CheckListInspeccionDsfpaRequest chk_list_insp_si_sdhpa = new CheckListInspeccionDsfpaRequest();
                    chk_list_insp_si_sdhpa.id_sol_ins = Id_solicitud_sdhpa;
                    chk_list_insp_si_sdhpa.activo = "1";
                    chk_list_insp_si_sdhpa.nombre_check_list = txt_nombre_chk_list;
                    chk_list_insp_si_sdhpa.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    chk_list_insp_si_sdhpa.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                    chk_list_insp_si_sdhpa.fecha_carga = DateTime.Now;
                    chk_list_insp_si_sdhpa.ruta_pdf = "";
                    chk_list_insp_si_sdhpa.inspector = CMBINSPECTOR;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_CHECK_LIST_INSPECCION_SI"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        chk_list_insp_si_sdhpa.id_chk_list_insp = _HabilitacionesService.Create_ChecklistInspeccionDsfpa(chk_list_insp_si_sdhpa);
                        file.SaveAs(Path.Combine(ruta_pdf, chk_list_insp_si_sdhpa.id_chk_list_insp.ToString() + ".pdf"));
                        CheckListInspeccionDsfpaRequest Chk_list_insp_si_sdhpa_req = new CheckListInspeccionDsfpaRequest();
                        Chk_list_insp_si_sdhpa_req = _HabilitacionesService.GetAllchk_list_inspeccion_req(chk_list_insp_si_sdhpa.id_chk_list_insp);
                        Chk_list_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + chk_list_insp_si_sdhpa.id_chk_list_insp.ToString() + ".pdf";
                        _HabilitacionesService.Update_chk_list_insp_dsfpa(Chk_list_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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


        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_pruebas_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_sol_in = 0;

                    try
                    {
                        id_sol_in = Convert.ToInt32(Session["archivo_pruebas_id_sol_in"].ToString());
                        Session.Remove("archivo_pruebas_id_sol_in");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.Str_Solicitud = "Solicitud de Inspección: " + _HabilitacionesService.Consultar_solicitud_inspeccion_sdhpa_x_id(id_sol_in).numero_documento;
                    ViewBag.Id_solicitud_sdhpa = id_sol_in;
                    ViewBag.lst_personal_od = Lista_personal;
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
        public ActionResult Adjuntar_archivo_pruebas_si(List<HttpPostedFileBase> files, int Id_solicitud_sdhpa, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    PruebaInspeccionDsfpaRequest pruebas_insp_si_sdhpa = new PruebaInspeccionDsfpaRequest();
                    pruebas_insp_si_sdhpa.id_sol_ins = Id_solicitud_sdhpa;
                    pruebas_insp_si_sdhpa.activo = "1";
                    pruebas_insp_si_sdhpa.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    pruebas_insp_si_sdhpa.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                    pruebas_insp_si_sdhpa.fecha_carga = DateTime.Now;
                    pruebas_insp_si_sdhpa.ruta_pdf = "";
                    pruebas_insp_si_sdhpa.inspector = CMBINSPECTOR;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_PRUEBAS_INSPECCION_SI"].ToString();
                    int cant_car = 0;
                    foreach (var item in files)
                    {
                        if (item != null && item.ContentLength > 0 && Path.GetExtension(item.FileName).ToLower() == ".jpg")
                        {
                            WebImage img = new WebImage(item.InputStream);
                            img.Resize(1024, 768);
                            cant_car += 1;
                            pruebas_insp_si_sdhpa.id_prueba_insp = _HabilitacionesService.Create_pruebaInspeccionDsfpa(pruebas_insp_si_sdhpa);
                            img.Save(Path.Combine(ruta_pdf, pruebas_insp_si_sdhpa.id_prueba_insp.ToString() + ".jpg"));
                            PruebaInspeccionDsfpaRequest prueb_insp_si_sdhpa_req = new PruebaInspeccionDsfpaRequest();
                            prueb_insp_si_sdhpa_req = _HabilitacionesService.GetAllpruebas_inspeccion_req(pruebas_insp_si_sdhpa.id_prueba_insp);
                            prueb_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + pruebas_insp_si_sdhpa.id_prueba_insp.ToString() + ".jpg";
                            _HabilitacionesService.Update_prueba_insp_dsfpa(prueb_insp_si_sdhpa_req);
                        }
                    }

                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardaron " + cant_car.ToString() + " imagenes correctamente";
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

        [AllowAnonymous]
        public ActionResult variable_actualizar_info_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_informe_id_info"] = id;
                return RedirectToAction("actualizar_archivo_info_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult actualizar_archivo_info_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_info = 0;

                    try
                    {
                        id_info = Convert.ToInt32(Session["archivo_informe_id_info"].ToString());
                        Session.Remove("archivo_informe_id_info");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    InformeInspeccionDsfpaRequest info_insp_si_sdhpa_req = new InformeInspeccionDsfpaRequest();
                    info_insp_si_sdhpa_req = _HabilitacionesService.GetAllinforme_inspeccion_req(id_info);

                    ViewBag.Str_Info = "Informe de Inspección: " + info_insp_si_sdhpa_req.nombre_informe;
                    ViewBag.id_info_si = id_info.ToString();
                    ViewBag.nombre_info_si = info_insp_si_sdhpa_req.nombre_informe;
                    ViewBag.lst_personal_od = Lista_personal;

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
        public ActionResult actualizar_archivo_info_si(HttpPostedFileBase file, int id_info_si, string txt_nombre_info, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_INFORME_INSPECCION_SI"].ToString();

                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, id_info_si.ToString() + ".pdf"));
                        InformeInspeccionDsfpaRequest info_insp_si_sdhpa_req = new InformeInspeccionDsfpaRequest();
                        info_insp_si_sdhpa_req = _HabilitacionesService.GetAllinforme_inspeccion_req(id_info_si);
                        info_insp_si_sdhpa_req.nombre_informe = txt_nombre_info;
                        info_insp_si_sdhpa_req.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        info_insp_si_sdhpa_req.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        info_insp_si_sdhpa_req.fecha_carga = DateTime.Now;
                        info_insp_si_sdhpa_req.inspector = CMBINSPECTOR;
                        info_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + id_info_si.ToString() + ".pdf";
                        _HabilitacionesService.Update_informe_insp_dsfpa(info_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_informe_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_sol_in = 0;

                    try
                    {
                        id_sol_in = Convert.ToInt32(Session["archivo_informe_id_sol_in"].ToString());
                        Session.Remove("archivo_informe_id_sol_in");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.Str_Solicitud = "Solicitud de Inspección: " + _HabilitacionesService.Consultar_solicitud_inspeccion_sdhpa_x_id(id_sol_in).numero_documento;
                    ViewBag.Id_solicitud_sdhpa = id_sol_in;
                    ViewBag.lst_personal_od = Lista_personal;
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
        public ActionResult Adjuntar_archivo_informe_si(HttpPostedFileBase file, int Id_solicitud_sdhpa, string txt_nombre_informe, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    InformeInspeccionDsfpaRequest informe_insp_si_sdhpa = new InformeInspeccionDsfpaRequest();
                    informe_insp_si_sdhpa.id_sol_ins = Id_solicitud_sdhpa;
                    informe_insp_si_sdhpa.activo = "1";
                    informe_insp_si_sdhpa.nombre_informe = txt_nombre_informe;
                    informe_insp_si_sdhpa.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    informe_insp_si_sdhpa.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                    informe_insp_si_sdhpa.fecha_carga = DateTime.Now;
                    informe_insp_si_sdhpa.ruta_pdf = "";
                    informe_insp_si_sdhpa.inspector = CMBINSPECTOR;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_INFORME_INSPECCION_SI"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        informe_insp_si_sdhpa.id_informe_insp = _HabilitacionesService.Create_InformeInspeccionDsfpa(informe_insp_si_sdhpa);
                        file.SaveAs(Path.Combine(ruta_pdf, informe_insp_si_sdhpa.id_informe_insp.ToString() + ".pdf"));
                        InformeInspeccionDsfpaRequest informe_insp_si_sdhpa_req = new InformeInspeccionDsfpaRequest();
                        informe_insp_si_sdhpa_req = _HabilitacionesService.GetAllinforme_inspeccion_req(informe_insp_si_sdhpa.id_informe_insp);
                        informe_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + informe_insp_si_sdhpa.id_informe_insp.ToString() + ".pdf";
                        _HabilitacionesService.Update_informe_insp_dsfpa(informe_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult variable_actualizar_acta_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_acta_id_acta"] = id;
                return RedirectToAction("actualizar_archivo_acta_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult actualizar_archivo_acta_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_acta = 0;

                    try
                    {
                        id_acta = Convert.ToInt32(Session["archivo_acta_id_acta"].ToString());
                        Session.Remove("archivo_acta_id_acta");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ActaInspeccionDsfpaRequest acta_insp_si_sdhpa_req = new ActaInspeccionDsfpaRequest();
                    acta_insp_si_sdhpa_req = _HabilitacionesService.GetAllacta_inspeccion_req(id_acta);

                    ViewBag.Str_Acta = "Acta de Inspección: " + acta_insp_si_sdhpa_req.nombre_acta;
                    ViewBag.id_acta_si = id_acta.ToString();
                    ViewBag.nombre_acta_si = acta_insp_si_sdhpa_req.nombre_acta;
                    ViewBag.lst_personal_od = Lista_personal;

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
        public ActionResult actualizar_archivo_acta_si(HttpPostedFileBase file, int id_acta_si, string txt_nombre_acta, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_ACTA_INSPECCION_SI"].ToString();

                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, id_acta_si.ToString() + ".pdf"));
                        ActaInspeccionDsfpaRequest acta_insp_si_sdhpa_req = new ActaInspeccionDsfpaRequest();
                        acta_insp_si_sdhpa_req = _HabilitacionesService.GetAllacta_inspeccion_req(id_acta_si);
                        acta_insp_si_sdhpa_req.nombre_acta = txt_nombre_acta;
                        acta_insp_si_sdhpa_req.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        acta_insp_si_sdhpa_req.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        acta_insp_si_sdhpa_req.fecha_carga = DateTime.Now;
                        acta_insp_si_sdhpa_req.inspector = CMBINSPECTOR;
                        acta_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + id_acta_si.ToString() + ".pdf";
                        _HabilitacionesService.Update_acta_insp_dsfpa(acta_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult variable_adjuntar_acta_si(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_acta_id_sol_in"] = id;
                return RedirectToAction("Adjuntar_archivo_acta_si", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_acta_si()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_sol_in = 0;

                    try
                    {
                        id_sol_in = Convert.ToInt32(Session["archivo_acta_id_sol_in"].ToString());
                        Session.Remove("archivo_acta_id_sol_in");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.Str_Solicitud = "Solicitud de Inspección: " + _HabilitacionesService.Consultar_solicitud_inspeccion_sdhpa_x_id(id_sol_in).numero_documento;
                    ViewBag.Id_solicitud_sdhpa = id_sol_in;
                    ViewBag.lst_personal_od = Lista_personal;
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
        public ActionResult Adjuntar_archivo_acta_si(HttpPostedFileBase file, int Id_solicitud_sdhpa, string txt_nombre_acta, string CMBINSPECTOR)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    ActaInspeccionDsfpaRequest acta_insp_si_sdhpa = new ActaInspeccionDsfpaRequest();
                    acta_insp_si_sdhpa.id_sol_ins = Id_solicitud_sdhpa;
                    acta_insp_si_sdhpa.activo = "1";
                    acta_insp_si_sdhpa.nombre_acta = txt_nombre_acta;
                    acta_insp_si_sdhpa.usuario_carga = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    acta_insp_si_sdhpa.usuario_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                    acta_insp_si_sdhpa.fecha_carga = DateTime.Now;
                    acta_insp_si_sdhpa.ruta_pdf = "";
                    acta_insp_si_sdhpa.inspector = CMBINSPECTOR;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_ACTA_INSPECCION_SI"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        acta_insp_si_sdhpa.id_acta_insp = _HabilitacionesService.Create_ActaInspeccionDsfpa(acta_insp_si_sdhpa);
                        file.SaveAs(Path.Combine(ruta_pdf, acta_insp_si_sdhpa.id_acta_insp.ToString() + ".pdf"));
                        ActaInspeccionDsfpaRequest acta_insp_si_sdhpa_req = new ActaInspeccionDsfpaRequest();
                        acta_insp_si_sdhpa_req = _HabilitacionesService.GetAllacta_inspeccion_req(acta_insp_si_sdhpa.id_acta_insp);
                        acta_insp_si_sdhpa_req.ruta_pdf = ruta_pdf + "/" + acta_insp_si_sdhpa.id_acta_insp.ToString() + ".pdf";
                        _HabilitacionesService.Update_acta_insp_dsfpa(acta_insp_si_sdhpa_req);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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


        [AllowAnonymous]
        public ActionResult variable_archivo_editar_seguimiento_OD(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_nuevo_seguimiento"] = id;
                Session["archivo_tipo_adjunto"] = 1;
                return RedirectToAction("Adjuntar_archivo_nuevo_seguimiento_OD", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_nuevo_seguimiento_OD()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {

                    int id_documento_seguimiento = 0;
                    int tipo_adjunto = 0;
                    int id_tipo_seguimiento = 0;
                    IEnumerable<TipoDocumentoSeguimientoAdjuntoResponse> tipo_doc_adj = new List<TipoDocumentoSeguimientoAdjuntoResponse>();
                    try
                    {
                        id_documento_seguimiento = Convert.ToInt32(Session["archivo_nuevo_seguimiento"].ToString());
                        tipo_adjunto = Convert.ToInt32(Session["archivo_tipo_adjunto"].ToString());
                        id_tipo_seguimiento = _HabilitacionesService.GetAllSeguimiento_x_id(_HabilitacionesService.GetAllDet_seg_doc(id_documento_seguimiento).First().id_seguimiento).id_tipo_seguimiento ?? 0;
                        tipo_doc_adj = _HabilitacionesService.Lista_tipo_documento_seguimiento_adjunto_x_tipo_seguimiento(id_tipo_seguimiento);
                        Session.Remove("archivo_nuevo_seguimiento");
                        Session.Remove("archivo_tipo_adjunto");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }
                    if (tipo_adjunto == 0) //NUEVO
                    {
                        ViewBag.Str_HT = "Se creo correctamente el Documento: " + id_documento_seguimiento.ToString();
                    }
                    else
                    {
                        ViewBag.Str_HT = "Documento: " + id_documento_seguimiento.ToString();
                    }
                    int cantidad_tipo = 0;

                    ViewBag.texto_uno = "";
                    ViewBag.texto_dos = "";
                    ViewBag.texto_tres = "";
                    ViewBag.texto_cuatro = "";
                    ViewBag.id_uno = 0;
                    ViewBag.id_dos = 0;
                    ViewBag.id_tres = 0;
                    ViewBag.id_cuatro = 0;

                    foreach (var resultado in tipo_doc_adj)
                    {
                        cantidad_tipo += 1;
                        if (cantidad_tipo == 1) { ViewBag.texto_uno = resultado.nombre; ViewBag.id_uno = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 2) { ViewBag.texto_dos = resultado.nombre; ViewBag.id_dos = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 3) { ViewBag.texto_tres = resultado.nombre; ViewBag.id_tres = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 4) { ViewBag.texto_cuatro = resultado.nombre; ViewBag.id_cuatro = resultado.id_tipo_doc_seg_adjunto; }
                    }
                    ViewBag.cantidad_archivo = cantidad_tipo;
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
        public ActionResult Adjuntar_archivo_nuevo_seguimiento_OD(HttpPostedFileBase file, string id_documento, int id_adjunto_uno, HttpPostedFileBase file2, int id_adjunto_dos, HttpPostedFileBase file3, int id_adjunto_tres, HttpPostedFileBase file4, int id_adjunto_cuatro, HttpPostedFileBase file5)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    DocumentoSeguimientoAdjuntoRequest doc_adjunt = new DocumentoSeguimientoAdjuntoRequest();
                    doc_adjunt.id_documento_seg = Convert.ToInt32(id_documento.Split(':')[1].Trim());
                    doc_adjunt.activo = "1";
                    doc_adjunt.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    doc_adjunt.fecha_crea = DateTime.Now;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO"].ToString();
                    string ruta_pdf2 = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO_ADJUNTO"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, id_documento.Split(':')[1].Trim() + ".pdf"));
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(Convert.ToInt32(id_documento.Split(':')[1].Trim()));
                        doc_seg_req.ruta_pdf = ruta_pdf + "/" + id_documento.Split(':')[1].Trim() + ".pdf";
                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);
                    }
                    if (file2 != null && file2.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_uno;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_uno).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file2.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file3 != null && file3.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_dos;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_dos).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file3.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file4 != null && file4.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_tres;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_tres).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file4.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file5 != null && file5.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_cuatro;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_cuatro).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file5.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult variable_archivo_nuevo_seguimiento(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_nuevo_seguimiento"] = id;
                Session["archivo_tipo_adjunto"] = 0;
                return RedirectToAction("Adjuntar_archivo_nuevo_seguimiento", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult variable_archivo_editar_seguimiento(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_nuevo_seguimiento"] = id;
                Session["archivo_tipo_adjunto"] = 1;
                return RedirectToAction("Adjuntar_archivo_nuevo_seguimiento", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_nuevo_seguimiento()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {

                    int id_documento_seguimiento = 0;
                    int tipo_adjunto = 0;
                    int id_tipo_seguimiento = 0;
                    IEnumerable<TipoDocumentoSeguimientoAdjuntoResponse> tipo_doc_adj = new List<TipoDocumentoSeguimientoAdjuntoResponse>();
                    try
                    {
                        id_documento_seguimiento = Convert.ToInt32(Session["archivo_nuevo_seguimiento"].ToString());
                        tipo_adjunto = Convert.ToInt32(Session["archivo_tipo_adjunto"].ToString());
                        id_tipo_seguimiento = _HabilitacionesService.GetAllSeguimiento_x_id(_HabilitacionesService.GetAllDet_seg_doc(id_documento_seguimiento).First().id_seguimiento).id_tipo_seguimiento ?? 0;
                        tipo_doc_adj = _HabilitacionesService.Lista_tipo_documento_seguimiento_adjunto_x_tipo_seguimiento(id_tipo_seguimiento);
                        Session.Remove("archivo_nuevo_seguimiento");
                        Session.Remove("archivo_tipo_adjunto");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }
                    if (tipo_adjunto == 0) //NUEVO
                    {
                        ViewBag.Str_HT = "Se creo correctamente el Documento: " + id_documento_seguimiento.ToString();
                    }
                    else
                    {
                        ViewBag.Str_HT = "Documento: " + id_documento_seguimiento.ToString();
                    }
                    int cantidad_tipo = 0;

                    ViewBag.texto_uno = "";
                    ViewBag.texto_dos = "";
                    ViewBag.texto_tres = "";
                    ViewBag.texto_cuatro = "";
                    ViewBag.id_uno = 0;
                    ViewBag.id_dos = 0;
                    ViewBag.id_tres = 0;
                    ViewBag.id_cuatro = 0;

                    foreach (var resultado in tipo_doc_adj)
                    {
                        cantidad_tipo += 1;
                        if (cantidad_tipo == 1) { ViewBag.texto_uno = resultado.nombre; ViewBag.id_uno = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 2) { ViewBag.texto_dos = resultado.nombre; ViewBag.id_dos = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 3) { ViewBag.texto_tres = resultado.nombre; ViewBag.id_tres = resultado.id_tipo_doc_seg_adjunto; }
                        if (cantidad_tipo == 4) { ViewBag.texto_cuatro = resultado.nombre; ViewBag.id_cuatro = resultado.id_tipo_doc_seg_adjunto; }
                    }
                    ViewBag.cantidad_archivo = cantidad_tipo;


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
        public ActionResult Adjuntar_archivo_nuevo_seguimiento(HttpPostedFileBase file, string id_documento, int id_adjunto_uno, HttpPostedFileBase file2, int id_adjunto_dos, HttpPostedFileBase file3, int id_adjunto_tres, HttpPostedFileBase file4, int id_adjunto_cuatro, HttpPostedFileBase file5)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {
                    DocumentoSeguimientoAdjuntoRequest doc_adjunt = new DocumentoSeguimientoAdjuntoRequest();
                    doc_adjunt.id_documento_seg = Convert.ToInt32(id_documento.Split(':')[1].Trim());
                    doc_adjunt.activo = "1";
                    doc_adjunt.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    doc_adjunt.fecha_crea = DateTime.Now;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO"].ToString();
                    string ruta_pdf2 = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTO_SEGUIMIENTO_ADJUNTO"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, id_documento.Split(':')[1].Trim() + ".pdf"));
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(Convert.ToInt32(id_documento.Split(':')[1].Trim()));
                        doc_seg_req.ruta_pdf = ruta_pdf + "/" + id_documento.Split(':')[1].Trim() + ".pdf";
                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);
                    }
                    if (file2 != null && file2.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_uno;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_uno).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file2.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file3 != null && file3.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_dos;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_dos).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file3.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file4 != null && file4.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_tres;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_tres).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file4.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    if (file5 != null && file5.ContentLength > 0)
                    {
                        doc_adjunt.id_tipo_doc_seg_adjunto = id_adjunto_cuatro;
                        int id_documento_seguimiento_adjunto = 0;
                        id_documento_seguimiento_adjunto = _HabilitacionesService.documento_seguimiento_x_tipo_documento_adjunto(Convert.ToInt32(id_documento.Split(':')[1].Trim()), id_adjunto_cuatro).id_doc_seg_adjunto;
                        if (id_documento_seguimiento_adjunto == 0)
                        {
                            id_documento_seguimiento_adjunto = _HabilitacionesService.Create_documento_seguimiento_adjunto(doc_adjunt);
                        }
                        file5.SaveAs(Path.Combine(ruta_pdf2, id_documento_seguimiento_adjunto + ".pdf"));
                    }

                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        [AllowAnonymous]
        public ActionResult Nuevo_Seguimiento_OD()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de atención al ciudadano
                {
                    List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                    List<SelectListItem> Lista_Concesion = new List<SelectListItem>();
                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();
                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR",
                        Value = ""
                    });
                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString() + "|" + result.id_tipo_procedimiento.ToString() + "|" + result.asunto + "|" + result.dias_tupa.ToString()
                        });

                    };


                    lista_tipo_documento_iden.Add(new SelectListItem()
                    {
                        Text = "RUC",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                            );
                    };

                    Lista_Oficinas_externas.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINAS",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.Recupera_oficina_todo())
                    {
                        if (result.ruc != "20565429656")
                        {
                            Lista_Oficinas_externas.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_oficina.ToString()
                            }
                            );
                        }
                    };


                    lista_tipo_procedimiento.Add(new SelectListItem()
                    {
                        Text = "PROCEDIMIENTOS",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                    {
                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_procedimiento.ToString()
                        }
                            );
                    }
                    /*
                    List<SelectListItem> Lista_Evaluador = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(18);
                    foreach (var result in recupera_persona)
                    {
                        Lista_Evaluador.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };*/

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_FACTURA");
                    tbl.Columns.Add("NUM1");
                    tbl.Columns.Add("NUM2");
                    tbl.Columns.Add("FACTURA");
                    tbl.Columns.Add("FECHA");
                    tbl.Columns.Add("IMPORTE");

                    var facturas = _GeneralService.listar_factura();

                    foreach (var result in facturas)
                    {
                        var num1 = "000" + result.num1_fact.ToString();
                        num1 = num1.Substring(num1.Length - 3, 3);
                        var num2 = "000000" + result.num2_fact.ToString();
                        num2 = num2.Substring(num2.Length - 6, 6);
                        tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                    };

                    ViewData["Facturas_Lista"] = tbl;

                    tbl = new DataTable();
                    tbl.Columns.Add("ID_EXPEDIENTE");
                    tbl.Columns.Add("EXPEDIENTE");

                    var expedientes = _GeneralService.llenar_expediente("0");

                    foreach (var result in expedientes)
                    {

                        if (result.id_tipo_expediente == 90) { tbl.Rows.Add(result.id_expediente, result.nom_expediente); }
                        else { tbl.Rows.Add(result.id_expediente, result.nom_expediente + "." + _GeneralService.llenar_tipo_expediente(result.id_tipo_expediente, 0).First().nombre); }
                    };

                    List<SelectListItem> Lista_Almacen = new List<SelectListItem>();

                    List<SelectListItem> lista_tipo_seguimiento = new List<SelectListItem>();

                    foreach (var result in _GeneralService.recupera_tipo_seguimiento())
                    {
                        lista_tipo_seguimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_seguimiento.ToString()
                        });
                    };

                    ViewBag.lst_tipo_seguimiento = lista_tipo_seguimiento;

                    ViewData["Expediente_Lista"] = tbl;
                    ViewBag.lst_tupa = Lista_TUPA;
                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    ViewBag.lst_embarcacion = Lista_embarcaciones;
                    ViewBag.lst_almacen = Lista_Almacen;
                    /*ViewBag.lst_evaluador = Lista_Evaluador;*/
                    ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;
                    ViewBag.lst_tipo_documento_iden = lista_tipo_documento_iden;
                    ViewBag.lst_concesion = Lista_Concesion;
                    ViewBag.lstOficina = Lista_Oficinas_externas;
                    List<SelectListItem> lista_direcciones = new List<SelectListItem>();
                    List<SelectListItem> lista_plantas = new List<SelectListItem>();
                    List<SelectListItem> lista_desembarcaderos = new List<SelectListItem>();
                    List<SelectListItem> lista_oficinas = new List<SelectListItem>();
                    List<SelectListItem> lista_personas = new List<SelectListItem>();
                    ViewBag.lst_direcciones = lista_direcciones;
                    ViewBag.lst_oficinas = lista_oficinas;
                    ViewBag.lst_persona_ext = lista_personas;
                    ViewBag.lst_plantas = lista_plantas;
                    ViewBag.lst_desembarcadero = lista_desembarcaderos;

                    SeguimientoViewModel model = new SeguimientoViewModel();

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Seguimiento_OD(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de atención al ciudadano
                {
                    try
                    {
                        string todo_expediente = "";
                        if (model.det_seg_exp != null)
                        {
                            foreach (detsegexpViewModel obj in model.det_seg_exp)
                            {
                                ExpedientesResponse req_exp = new ExpedientesResponse();
                                req_exp = _HabilitacionesService.GetExpediente_x_id(obj.id_expediente);
                                if (todo_expediente == "")
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                    else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }

                                }
                                else
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                    else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                }
                            }
                        }

                        DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                        if (request_doc.num_documento == 0)
                        {
                            request_doc.num_documento = null;
                        }
                        request_doc.usuario_recepcion_sdhpa = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                        request_doc.fecha_recepcion_sdhpa = DateTime.Now;
                        request_doc.fecha_od = Convert.ToDateTime(model.fecha_recibido_od);
                        request_doc.fecha_recibido_evaluador = null;
                        request_doc.fecha_crea = null;
                        request_doc.id_servicio_dhcpa = 0;
                        request_doc.expedientes_relacion = todo_expediente;
                        request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        request_doc.indicador = "1"; // '1' INICIAL, '2' SECUNDARIO

                        model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);

                        if (model.det_fac_doc != null)
                        {
                            request_doc.det_doc_fact = new List<DetDocFactRequest>();

                            foreach (DetDocFactViewModel obj in model.det_fac_doc)
                            {
                                DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                req_det_doc_fac.activo = "1";
                                _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                            }
                        }

                        if (model.id_ofi_dir == 0)
                        {
                            model.id_ofi_dir = null;
                        }

                        SeguimientoDhcpaRequest request_seguimiento = ModelToRequest.Seguimiento_dhcpa(model);
                        request_seguimiento.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_seguimiento.nom_oficina_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_seguimiento.persona_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_seguimiento.fecha_inicio = DateTime.Now;
                        request_seguimiento.estado = "0"; //'0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO

                        if (model.det_seg_exp != null)
                        {
                            request_doc.det_seg_doc = new List<DetSegDocRequest>();

                            foreach (detsegexpViewModel obj in model.det_seg_exp)
                            {
                                request_seguimiento.id_expediente = obj.id_expediente;
                                ExpedientesRequest req_exp = new ExpedientesRequest();
                                req_exp = _HabilitacionesService.GetExpediente(obj.id_expediente);
                                if (req_exp.indicador_seguimiento == "0")
                                {
                                    int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);
                                    req_exp.indicador_seguimiento = "1"; // con seguimiento
                                    _HabilitacionesService.Update_mae_expediente(req_exp);
                                    DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                                    req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                                    req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                                    req_det_seg_doc.activo = "1";

                                    _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                                }
                            }
                        }
                        else
                        {
                            int var_id_seguimiento = _HabilitacionesService.Create_Seguimiento(request_seguimiento);

                            DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                            req_det_seg_doc.id_seguimiento = var_id_seguimiento;
                            req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                            req_det_seg_doc.activo = "1";

                            _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);
                        }

                        @ViewBag.Mensaje = model.id_doc_seg.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult Agregar_Seguimiento()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {

                    List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();

                    lista_tipo_documento_iden.Add(new SelectListItem()
                    {
                        Text = "RUC",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                            );
                    };

                    Lista_Oficinas_externas.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINAS",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.Recupera_oficina_todo())
                    {
                        if (result.ruc != "20565429656")
                        {
                            Lista_Oficinas_externas.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_oficina.ToString()
                            }
                            );
                        }
                    };


                    lista_tipo_procedimiento.Add(new SelectListItem()
                    {
                        Text = "PROCEDIMIENTOS",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                    {
                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_procedimiento.ToString()
                        }
                            );
                    }
                    /*
                    List<SelectListItem> Lista_Evaluador = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(18);
                    foreach (var result in recupera_persona)
                    {
                        Lista_Evaluador.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };*/

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_FACTURA");
                    tbl.Columns.Add("NUM1");
                    tbl.Columns.Add("NUM2");
                    tbl.Columns.Add("FACTURA");
                    tbl.Columns.Add("FECHA");
                    tbl.Columns.Add("IMPORTE");

                    var facturas = _GeneralService.listar_factura();

                    foreach (var result in facturas)
                    {
                        var num1 = "000" + result.num1_fact.ToString();
                        num1 = num1.Substring(num1.Length - 3, 3);
                        var num2 = "000000" + result.num2_fact.ToString();
                        num2 = num2.Substring(num2.Length - 6, 6);
                        tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                    };

                    ViewData["Facturas_Lista"] = tbl;

                    tbl = new DataTable();
                    tbl.Columns.Add("ID_SEGUIMIENTO");
                    tbl.Columns.Add("FECHA_SEGUIMIENTO");
                    tbl.Columns.Add("SEGUIMIENTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("EMBARCACION");

                    var list_seguimiento = _HabilitacionesService.GetAllSeguimiento("");

                    foreach (var result in list_seguimiento)
                    {
                        string seguimiento = "";
                        if (result.Expediente != null)
                        { seguimiento = result.Expediente + "." + result.nom_tipo_expediente + "(" + result.nom_tipo_procedimiento + ")"; }
                        else
                        { seguimiento = result.nom_tipo_procedimiento; }
                        string externo = "";
                        if (result.nom_oficina_ext != null)
                        { externo = result.ruc + " - " + result.nom_oficina_ext; }
                        else
                        { externo = result.persona_num_documento + " - " + result.nom_persona_ext; }
                        tbl.Rows.Add(result.id_seguimiento, result.fecha_inicio.ToShortDateString(), seguimiento, externo, result.nom_embarcacion);
                    };

                    List<SelectListItem> lista_servicio = new List<SelectListItem>();

                    lista_servicio.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SERVICIO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_servicio_dhcpa())
                    {
                        lista_servicio.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_servicio_dhcpa.ToString()
                        });
                    };

                    ViewData["Seguimiento_Lista"] = tbl;
                    ViewBag.lst_servicio_dhcpa = lista_servicio;

                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    /*ViewBag.lst_evaluador = Lista_Evaluador;*/
                    ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;

                    SeguimientoViewModel model = new SeguimientoViewModel();

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Agregar_Seguimiento(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {
                    try
                    {
                        string todo_expediente = "";
                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            int id_expediente = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento).id_expediente ?? 0;
                            if (id_expediente != 0)
                            {
                                ExpedientesResponse req_exp = new ExpedientesResponse();
                                req_exp = _HabilitacionesService.GetExpediente_x_id(id_expediente);
                                if (todo_expediente == "")
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                    else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }

                                }
                                else
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                    else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                }
                            }
                        }

                        string evaluador_recp = null;
                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            var recup_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                            evaluador_recp = recup_seg_dhcpa.evaluador;
                        }

                        DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                        if (request_doc.num_documento == 0)
                        {
                            request_doc.num_documento = null;
                        }
                        request_doc.fecha_crea = DateTime.Now;
                        request_doc.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                        request_doc.fecha_recibido_evaluador = null;
                        request_doc.expedientes_relacion = todo_expediente;
                        request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        request_doc.indicador = "2"; // '1' INICIAL, '2' SECUNDARIO

                        if (evaluador_recp != null)
                        {
                            request_doc.usuario_recepcion_sdhpa = "20565429656 - " + evaluador_recp;
                            request_doc.fecha_recepcion_sdhpa = DateTime.Now;
                            request_doc.estado = "1"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                            request_doc.fecha_asignacion_evaluador = DateTime.Now;
                            request_doc.evaluador = evaluador_recp;
                        }

                        model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);

                        if (model.det_fac_doc != null)
                        {
                            request_doc.det_doc_fact = new List<DetDocFactRequest>();

                            foreach (DetDocFactViewModel obj in model.det_fac_doc)
                            {
                                DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                req_det_doc_fac.activo = "1";
                                _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                            }
                        }

                        request_doc.det_seg_doc = new List<DetSegDocRequest>();

                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                            req_det_seg_doc.id_seguimiento = obj.id_seguimiento;
                            req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                            req_det_seg_doc.activo = "1";
                            _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);

                            SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                            req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                            //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO
                            req_seg_dhcpa.estado = "2";
                            _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                        }

                        @ViewBag.Mensaje = model.id_doc_seg.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult Agregar_Seguimiento_OD()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de atención al ciudadano
                {

                    List<SelectListItem> lista_tipo_procedimiento = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();

                    lista_tipo_documento_iden.Add(new SelectListItem()
                    {
                        Text = "RUC",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                            );
                    };

                    Lista_Oficinas_externas.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINAS",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.Recupera_oficina_todo())
                    {
                        if (result.ruc != "20565429656")
                        {
                            Lista_Oficinas_externas.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_oficina.ToString()
                            }
                            );
                        }
                    };


                    lista_tipo_procedimiento.Add(new SelectListItem()
                    {
                        Text = "PROCEDIMIENTOS",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_procedimiento(0))
                    {
                        lista_tipo_procedimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_procedimiento.ToString()
                        }
                            );
                    }
                    /*
                    List<SelectListItem> Lista_Evaluador = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(18);
                    foreach (var result in recupera_persona)
                    {
                        Lista_Evaluador.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };*/

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_FACTURA");
                    tbl.Columns.Add("NUM1");
                    tbl.Columns.Add("NUM2");
                    tbl.Columns.Add("FACTURA");
                    tbl.Columns.Add("FECHA");
                    tbl.Columns.Add("IMPORTE");

                    var facturas = _GeneralService.listar_factura();

                    foreach (var result in facturas)
                    {
                        var num1 = "000" + result.num1_fact.ToString();
                        num1 = num1.Substring(num1.Length - 3, 3);
                        var num2 = "000000" + result.num2_fact.ToString();
                        num2 = num2.Substring(num2.Length - 6, 6);
                        tbl.Rows.Add(result.id_factura, result.num1_fact.ToString(), result.num2_fact.ToString(), num1 + "-" + num2, result.fecha_fact.Value.ToShortDateString(), result.importe_total.ToString());
                    };

                    ViewData["Facturas_Lista"] = tbl;

                    tbl = new DataTable();
                    tbl.Columns.Add("ID_SEGUIMIENTO");
                    tbl.Columns.Add("FECHA_SEGUIMIENTO");
                    tbl.Columns.Add("SEGUIMIENTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("EMBARCACION");

                    var list_seguimiento = _HabilitacionesService.GetAllSeguimiento("");

                    foreach (var result in list_seguimiento)
                    {
                        string seguimiento = "";
                        if (result.Expediente != null)
                        { seguimiento = result.Expediente + "." + result.nom_tipo_expediente + "(" + result.nom_tipo_procedimiento + ")"; }
                        else
                        { seguimiento = result.nom_tipo_procedimiento; }
                        string externo = "";
                        if (result.nom_oficina_ext != null)
                        { externo = result.ruc + " - " + result.nom_oficina_ext; }
                        else
                        { externo = result.persona_num_documento + " - " + result.nom_persona_ext; }
                        tbl.Rows.Add(result.id_seguimiento, result.fecha_inicio.ToShortDateString(), seguimiento, externo, result.nom_embarcacion);
                    };

                    List<SelectListItem> lista_servicio = new List<SelectListItem>();

                    lista_servicio.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SERVICIO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_servicio_dhcpa())
                    {
                        lista_servicio.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_servicio_dhcpa.ToString()
                        });
                    };

                    ViewData["Seguimiento_Lista"] = tbl;
                    ViewBag.lst_servicio_dhcpa = lista_servicio;

                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    /*ViewBag.lst_evaluador = Lista_Evaluador;*/
                    ViewBag.lst_tipo_procedimiento = lista_tipo_procedimiento;

                    SeguimientoViewModel model = new SeguimientoViewModel();

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Agregar_Seguimiento_OD(SeguimientoViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de atención al ciudadano
                {
                    try
                    {

                        string todo_expediente = "";
                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            int id_expediente = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento).id_expediente ?? 0;
                            if (id_expediente != 0)
                            {
                                ExpedientesResponse req_exp = new ExpedientesResponse();
                                req_exp = _HabilitacionesService.GetExpediente_x_id(id_expediente);
                                if (todo_expediente == "")
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = req_exp.nom_expediente; }
                                    else { todo_expediente = req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                }
                                else
                                {
                                    if (req_exp.id_tipo_expediente == 90) { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente; }
                                    else { todo_expediente = todo_expediente + ", " + req_exp.nom_expediente + "." + req_exp.tipo_expediente.nombre; }
                                }
                            }

                        }


                        string evaluador_recp = null;
                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            var recup_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                            evaluador_recp = recup_seg_dhcpa.evaluador;
                        }


                        DocumentoSeguimientoRequest request_doc = ModelToRequest.Documento_Seguimiento(model);

                        if (request_doc.num_documento == 0)
                        {
                            request_doc.num_documento = null;
                        }
                        request_doc.usuario_recepcion_sdhpa = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request_doc.fecha_documento = Convert.ToDateTime(model.fecha_documento);
                        request_doc.fecha_recepcion_sdhpa = DateTime.Now;
                        request_doc.fecha_od = Convert.ToDateTime(model.fecha_recibido_od);
                        request_doc.fecha_recibido_evaluador = null;
                        request_doc.fecha_crea = null;
                        request_doc.expedientes_relacion = todo_expediente;
                        request_doc.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request_doc.nom_ofi_crea = _GeneralService.recupera_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).nombre;
                        request_doc.estado = "0"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        request_doc.indicador = "2"; // '1' INICIAL, '2' SECUNDARIO


                        if (evaluador_recp != null)
                        {
                            request_doc.usuario_recepcion_sdhpa = "20565429656 - " + evaluador_recp;
                            request_doc.fecha_recepcion_sdhpa = DateTime.Now;
                            request_doc.estado = "1"; // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                            request_doc.fecha_asignacion_evaluador = DateTime.Now;
                            request_doc.evaluador = evaluador_recp;
                        }


                        model.id_doc_seg = _HabilitacionesService.Create_documento_sdhcp(request_doc);

                        if (model.det_fac_doc != null)
                        {
                            request_doc.det_doc_fact = new List<DetDocFactRequest>();

                            foreach (DetDocFactViewModel obj in model.det_fac_doc)
                            {
                                DetDocFactRequest req_det_doc_fac = ModelToRequest.Documento_Factura(obj);
                                req_det_doc_fac.id_documento_seg = model.id_doc_seg;
                                req_det_doc_fac.activo = "1";
                                _HabilitacionesService.Create_det_doc_fac(req_det_doc_fac);
                            }
                        }

                        request_doc.det_seg_doc = new List<DetSegDocRequest>();

                        foreach (detsegpadreViewModel obj in model.det_seg_padre)
                        {
                            DetSegDocRequest req_det_seg_doc = new DetSegDocRequest();
                            req_det_seg_doc.id_seguimiento = obj.id_seguimiento;
                            req_det_seg_doc.id_documento_seg = model.id_doc_seg;
                            req_det_seg_doc.activo = "1";
                            _HabilitacionesService.Create_det_doc_seg(req_det_seg_doc);

                            SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                            req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(obj.id_seguimiento);
                            //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO
                            req_seg_dhcpa.estado = "2";
                            _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                        }

                        @ViewBag.Mensaje = model.id_doc_seg.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult Documentos_enviados(int page = 1, string expediente = "", string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Atención al Usuario
                {

                    /*
                     * POR RECIBIR : ESTADO = 0
                     * RECIBIDO: ESTADO = 1
                    */

                    /*
                     * INICIAL: INDICADOR = 1
                     * SECUNDARIO: INDICADOR = 2
                    */

                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR TIPO DOCUMENTO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                    {
                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_documento.ToString()
                        });
                    };

                    if (cmbtipo_documento == "0")
                    {
                        cmbtipo_documento = "";
                    }

                    ViewBag.lst_tipo_documento = lista_documentos;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DOCUMENTO_SEG");
                    tbl.Columns.Add("HABILITANTE");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("NOM_DOCUMENTO");
                    tbl.Columns.Add("NOM_EXTERNO");
                    tbl.Columns.Add("FECHA_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EVALUADOR");
                    tbl.Columns.Add("GROUP_EXPEDIENTE");
                    tbl.Columns.Add("VER_PDF");

                    var documento = _HabilitacionesService.GetAllDocumentos("", "", "", asunto, externo, cmbtipo_documento, num_documento, nom_documento, 28, expediente);

                    foreach (var result in documento)
                    {
                        if (result.ruta_pdf == null || result.ruta_pdf == "")
                        {
                            tbl.Rows.Add(
                                result.id_documento_seg,
                                result.documento_codigo_habilitacion,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.fecha_documento.Value.ToShortDateString(),
                                result.asunto,
                                result.evaluador,
                                result.group_expedientes,
                                false);
                        }
                        else
                        {
                            tbl.Rows.Add(
                                result.id_documento_seg,
                                result.documento_codigo_habilitacion,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.fecha_documento.Value.ToShortDateString(),
                                result.asunto,
                                result.evaluador,
                                result.group_expedientes,
                                true);
                        }
                    };

                    ViewData["Documento_Seg_Tabla"] = tbl;

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

        [AllowAnonymous]
        public ActionResult Documentos_enviados_registrado_x_OD(int page = 1, string expediente = "", string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    )))
                {
                    /*
                     * POR RECIBIR : ESTADO = 0
                     * RECIBIDO: ESTADO = 1
                     
                     * INICIAL: INDICADOR = 1
                     * SECUNDARIO: INDICADOR = 2
                    */

                    // var_id_oficina HttpContext.User.Identity.Name.Split('|')[4].Trim()
                    var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                    int permiso = 0;

                    for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                    {
                        if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                        {
                            permiso = 1;
                        }
                    }
                    if (permiso == 1)
                    {
                        List<SelectListItem> lista_documentos = new List<SelectListItem>();

                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = "SELECCIONAR TIPO DOCUMENTO",
                            Value = "0"
                        });

                        foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                        {
                            lista_documentos.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                        };

                        if (cmbtipo_documento == "0")
                        {
                            cmbtipo_documento = "";
                        }

                        ViewBag.lst_tipo_documento = lista_documentos;

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add("ID_DOCUMENTO_SEG");
                        tbl.Columns.Add("HABILITANTE");
                        tbl.Columns.Add("FECHA_CREA");
                        tbl.Columns.Add("NOM_DOCUMENTO");
                        tbl.Columns.Add("NOM_EXTERNO");
                        tbl.Columns.Add("FECHA_DOCUMENTO");
                        tbl.Columns.Add("ASUNTO");
                        tbl.Columns.Add("EVALUADOR");
                        tbl.Columns.Add("GROUP_EXPEDIENTE");
                        tbl.Columns.Add("VER_PDF");

                        var documento = _HabilitacionesService.GetAllDocumentos("", "", "", asunto, externo, cmbtipo_documento, num_documento, nom_documento, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), expediente);

                        foreach (var result in documento)
                        {
                            if (result.ruta_pdf == null || result.ruta_pdf == "")
                            {
                                tbl.Rows.Add(
                                    result.id_documento_seg,
                                    result.documento_codigo_habilitacion,
                                    result.fecha_od,
                                    result.nom_documento,
                                    result.nom_externo,
                                    result.fecha_documento.Value.ToShortDateString(),
                                    result.asunto,
                                    result.evaluador,
                                    result.group_expedientes,
                                    false);
                            }
                            else
                            {
                                tbl.Rows.Add(
                                    result.id_documento_seg,
                                    result.documento_codigo_habilitacion,
                                    result.fecha_od,
                                    result.nom_documento,
                                    result.nom_externo,
                                    result.fecha_documento.Value.ToShortDateString(),
                                    result.asunto,
                                    result.evaluador,
                                    result.group_expedientes,
                                    true);
                            }
                        };

                        ViewData["Documento_Seg_Tabla"] = tbl;

                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Error_Logeo", "Account");
                    }
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

        [AllowAnonymous]
        public ActionResult Documentos_por_recibir_x_evaluador(int page = 1, string indicador = "", string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[13].Trim() == "1" // Acceso a Nuevo Seguimiento Evaluador
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {

                    /*
                     // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                    */

                    /*
                     * INICIAL: INDICADOR = 1
                     * SECUNDARIO: INDICADOR = 2
                    */


                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR TIPO DOCUMENTO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                    {
                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_documento.ToString()
                        });
                    };

                    if (cmbtipo_documento == "0")
                    {
                        cmbtipo_documento = "";
                    }

                    ViewBag.lst_tipo_documento = lista_documentos;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DOCUMENTO_SEG");
                    tbl.Columns.Add("HABILITANTE");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("NOM_DOCUMENTO");
                    tbl.Columns.Add("NOM_EXTERNO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("GROUP_EXPEDIENTE");
                    tbl.Columns.Add("VER_PDF");

                    var documento = _HabilitacionesService.GetAllDocumentos_x_rec("1", "", HttpContext.User.Identity.Name.Split('|')[1].Trim(), asunto, externo, cmbtipo_documento, num_documento, nom_documento, 0, "");

                    foreach (var result in documento)
                    {
                        if (result.ruta_pdf == "" || result.ruta_pdf == null)
                        {
                            tbl.Rows.Add(
                            result.id_documento_seg,
                            result.documento_codigo_habilitacion,
                            result.fecha_crea,
                            result.nom_documento,
                            result.nom_externo,
                            result.asunto,
                            result.group_expedientes, false);
                        }
                        else
                        {
                            tbl.Rows.Add(
                            result.id_documento_seg,
                            result.documento_codigo_habilitacion,
                            result.fecha_crea,
                            result.nom_documento,
                            result.nom_externo,
                            result.asunto,
                            result.group_expedientes, true);
                        }

                    };

                    ViewData["Documento_Seg_Tabla"] = tbl;

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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Doc_Por_Recibir(string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[13].Trim() == "1" // Acceso a Nuevo Seguimiento Evaluador
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int id_documento_seg = 0;
                    for (int i = 0; i < id.Split('|').Count(); i++)
                    {
                        id_documento_seg = Convert.ToInt32(id.Split('|')[i].Trim());
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(id_documento_seg);
                        doc_seg_req.fecha_recibido_evaluador = DateTime.Now;
                        // '0' POR RECIBIR, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        doc_seg_req.estado = "2";

                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);

                        if (doc_seg_req.indicador == "1")
                        {
                            foreach (var res_det_seg in _HabilitacionesService.GetAllDet_seg_doc(id_documento_seg))
                            {
                                DetSegEvaluadorRequest det_seg_eval = new DetSegEvaluadorRequest();
                                det_seg_eval = _HabilitacionesService.GetAlldet_seg_evaluador(res_det_seg.id_seguimiento).First();
                                det_seg_eval.fecha_recibido = DateTime.Now;
                                _HabilitacionesService.Update_det_seg_evalua(det_seg_eval);

                                SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                                req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(res_det_seg.id_seguimiento);
                                //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO
                                req_seg_dhcpa.estado = "2";
                                _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);
                            }
                        }
                    }
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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
        public ActionResult Recupera_embarcacion_seguimiento(int id_documento_seg = 0)
        {
            string embarcacion = "";
            foreach (var x in _HabilitacionesService.GetAllEmbarcacion_x_documento(id_documento_seg))
            {
                if (embarcacion != "")
                {
                    embarcacion = embarcacion + ", ";
                }
                embarcacion = embarcacion + x.matricula + "(" + x.nombre + ")";
            }

            return Json(embarcacion, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Recupera_planta_seguimiento(int id_documento_seg = 0)
        {
            string planta = "";
            foreach (var x in _HabilitacionesService.GetAllPlanta_x_seguimiento(id_documento_seg))
            {
                if (planta != "")
                {
                    planta = planta + ", ";
                }
                planta = planta + x.siglas_tipo_planta + " " + x.numero_planta.ToString() + " " + x.nombre_planta;
            }

            return Json(planta, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Recupera_expediente(int id_documento_seg = 0)
        {
            string expediente = "";
            foreach (var x in _HabilitacionesService.GetAllExpediente_x_Documento(id_documento_seg))
            {
                if (expediente != "")
                {
                    expediente = expediente + ", ";
                }
                expediente = expediente + x.nom_expediente + "." + x.tipo_expediente.nombre;
            }

            return Json(expediente, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Recupera_facturas(int id_documento_seg = 0)
        {

            string facturas = "";
            foreach (var x in _HabilitacionesService.GetAllfacturas_x_Documento(id_documento_seg))
            {
                if (facturas != "")
                {
                    facturas = facturas + ", ";
                }
                var n1 = "000" + x.num1_fact.ToString();
                var n2 = "000000" + x.num2_fact.ToString();
                facturas = facturas + (n1).Substring(n1.Length - 3, 3) + "-" + (n2).Substring(n2.Length - 6, 6) + " ( S/." + x.importe_total.ToString() + ")";
            }

            return Json(facturas, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documentos_adjuntos_sol_insp_sdhpa(int id_sol_ins = 0)
        {
            Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result resp = new Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result();
            foreach (var x in _HabilitacionesService.Lista_acta_info_pru_por_si(id_sol_ins))
            {
                resp.id_sol_ins = x.id_sol_ins;
                resp.acta_id = x.acta_id;
                resp.acta_nombre = x.acta_nombre;
                resp.acta_fecha_carga = x.acta_fecha_carga;
                resp.acta_ruta_pdf = x.acta_ruta_pdf;

                resp.chkl_id = x.chkl_id;
                resp.chkl_nombre = x.chkl_nombre;
                resp.chkl_fecha_carga = x.chkl_fecha_carga;
                resp.chkl_ruta_pdf = x.chkl_ruta_pdf;

                resp.info_id = x.info_id;
                resp.info_nombre = x.info_nombre;
                resp.info_fecha_carga = x.info_fecha_carga;
                resp.info_ruta_pdf = x.info_ruta_pdf;
                resp.prue_cantidad = x.prue_cantidad;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_destino_emitidos(int id_doc_dhcpa = 0)
        {
            List<SelectListItem> llenar_destino = new List<SelectListItem>();

            foreach (var result in _HabilitacionesService.Lista_destino_documentos_dhcpa(id_doc_dhcpa))
            {
                llenar_destino.Add(new SelectListItem()
                {
                    Text = result.lugar_destino,
                    Value = result.persona_destino
                });
            };

            return Json(llenar_destino, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_expediente_sin_seguimiento()
        {
            List<SelectListItem> llenar_expediente = new List<SelectListItem>();

            foreach (var result in _HabilitacionesService.Lista_expediente_sin_seguimiento())
            {
                if (result.id_tipo_expediente == 90)
                {
                    llenar_expediente.Add(new SelectListItem()
                    {
                        Text = result.nom_expediente,
                        Value = result.id_expediente.ToString()
                    });
                }
                else
                {
                    llenar_expediente.Add(new SelectListItem()
                    {
                        Text = result.nom_expediente + "." + result.tipo_expediente.nombre,
                        Value = result.id_expediente.ToString()
                    });
                }
            };

            return Json(llenar_expediente, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_solicitud_seguimiento(int id_seguimiento = 0)
        {
            IEnumerable<SolicitudInspeccionResponse> llenar_solicitud = new List<SolicitudInspeccionResponse>();

            llenar_solicitud = (from p in _HabilitacionesService.Lista_solicitud_seguimiento(id_seguimiento)
                                select new SolicitudInspeccionResponse
                                {
                                    id_sol_ins = p.id_sol_ins,
                                    numero_documento = p.numero_documento,
                                    fecha_text = p.fecha_crea.Value.ToShortDateString(),
                                    estado_text = p.id_estado == null ? "" : _HojaTramiteService.lista_estado_tramite().Where(x => x.id_est_tramite == p.id_estado).First().nombre
                                }).OrderBy(x => x.id_sol_ins);

            return Json(llenar_solicitud, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_informe_seguimiento(int id_seguimiento = 0)
        {

            List<SelectListItem> llenar_informe = new List<SelectListItem>();

            foreach (var result in _HabilitacionesService.Lista_informe_tecnico_seguimiento(id_seguimiento))
            {
                llenar_informe.Add(new SelectListItem()
                {
                    Text = "Informe N° " + result.numero_documento.ToString(),
                    Value = result.fecha_crea.ToString()
                });
            };

            return Json(llenar_informe, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recuperar_correo_solicitud(int id_solicitud = 0)
        {
            IEnumerable<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result> correo_res = new List<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result>();
            correo_res = (from p in _HabilitacionesService.consulta_correo_x_solicitud(id_solicitud)
                          select new Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result
                             {
                                 correo_responsable = p.correo_responsable,
                                 persona_num_documento = p.persona_num_documento,
                                 id_cargo = p.id_cargo,
                                 nombre_cargo = p.nombre_cargo
                             });

            return Json(correo_res, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult eviar_correo_solicitud_insp(int id_solicitud = 0, string destinos = "")
        {
            return Json(_HabilitacionesService.enviar_correo_notificacion_solicitud_sdhpa(id_solicitud, destinos), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documentos_seguimiento_recibido(int id_seguimiento = 0)
        {
            IEnumerable<DocumentoSeguimientoResponse> documento_res = new List<DocumentoSeguimientoResponse>();
            documento_res = (from p in _HabilitacionesService.lista_documentos_recibidos_x_seguimiento(id_seguimiento)
                             where p.estado != "4"
                             select new DocumentoSeguimientoResponse
                             {
                                 id_documento_seg = p.id_documento_seg,
                                 nom_documento = (p.num_documento == 0 || p.num_documento == null) ? p.nom_tipo_documento + " " + p.nom_documento : p.nom_tipo_documento + " " + p.num_documento.ToString() + " " + p.nom_documento,
                                 asunto = p.asunto,
                                 fecha_documento_text = p.fecha_documento.Value.ToShortDateString(),
                                 fecha_crea_text = (p.fecha_crea == null) ? p.fecha_od.Value.ToShortDateString() : p.fecha_crea.Value.ToShortDateString(),
                                 ruta_pdf = p.ruta_pdf
                             }).OrderBy(x => x.id_documento_seg);

            return Json(documento_res, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documentos_adjuntos(int id_documento_seg = 0)
        {
            IEnumerable<DocumentoSeguimientoAdjuntoResponse> documento_adj_res = new List<DocumentoSeguimientoAdjuntoResponse>();
            documento_adj_res = _HabilitacionesService.lita_documento_seguimiento_x_documento_seg(id_documento_seg);
            foreach (var res in documento_adj_res)
            {
                int i = res.id_doc_seg_adjunto;
            }
            return Json(documento_adj_res, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult inactivar_activar_protocolo(int id_protocolo = 0)
        {

            ProtocoloRequest proto_req = new ProtocoloRequest();
            proto_req = _HabilitacionesService.lista_protocolo_x_id(id_protocolo);

            if (proto_req.activo == "1")
            {
                proto_req.activo = "0";
                _HabilitacionesService.actualizar_protocolo(proto_req);
            }
            else
            {
                proto_req.activo = "1";
                _HabilitacionesService.actualizar_protocolo(proto_req);
            }
            string result = "OK";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documentos_emitidos_dhcpa_seguimiento(int id_seguimiento = 0)
        {
            IEnumerable<DocumentoDhcpaResponse> document_response = new List<DocumentoDhcpaResponse>();

            document_response = (from p in _HabilitacionesService.lista_documentos_emitidos_dhcpa_x_seguimiento(id_seguimiento)
                                 select new DocumentoDhcpaResponse
                                 {
                                     id_doc_dhcpa = p.id_doc_dhcpa,
                                     nom_doc = p.num_doc == null ? p.nom_tipo_documento + " " + p.nom_doc : p.nom_tipo_documento + " N° " + p.num_doc.ToString() + " " + p.nom_doc,
                                     asunto = p.asunto,
                                     pdf = p.pdf,
                                     fecha_doc_text = p.fecha_doc.Value.ToShortDateString()
                                 }).OrderBy(x => x.id_doc_dhcpa);

            return Json(document_response, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_protocolos_transporte(int id_transporte = 0) /// ME QUEDE ACA
        {
            IEnumerable<ProtocoloResponse> Protocolo = new List<ProtocoloResponse>();
            string RUTA_SERVER = ConfigurationManager.AppSettings["RUTA_FTP_VER"].ToString();
            string ruta_pdf = "habilitaciones/transporte";

            Protocolo = (from p in _HabilitacionesService.lista_protocolo_x_id_transporte(id_transporte)
                         select new ProtocoloResponse
                         {
                             id_protocolo = p.id_protocolo,
                             nombre = p.nombre,
                             activo = p.activo,
                             cadena_fecha_inicio = Convert.ToDateTime(p.fecha_inicio).ToShortDateString(),
                             cadena_fecha_fin = p.fecha_fin == null ? "---" : Convert.ToDateTime(p.fecha_fin).ToShortDateString(),
                             ruta_archivo = RUTA_SERVER + ruta_pdf + "/" + p.id_protocolo.ToString() + ".pdf"
                         });

            return Json(Protocolo, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult consulta_protocolo_reemplaza(int id_protocolo = 0)
        {
            ProtocoloRequest Protocolo = new ProtocoloRequest();
            Protocolo.nombre = "";
            if (id_protocolo != 0)
            {
                Protocolo = _HabilitacionesService.lista_protocolo_x_id(id_protocolo);
            }

            return Json(Protocolo.nombre, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_protocolos_seguimiento(int id_seguimiento = 0) /// ME QUEDE ACA
        {
            IEnumerable<ProtocoloResponse> Protocolo = new List<ProtocoloResponse>();

            string RUTA_SERVER = ConfigurationManager.AppSettings["RUTA_FTP_VER"].ToString();

            SeguimientoDhcpaResponse resp_seg = new SeguimientoDhcpaResponse();
            resp_seg = _HabilitacionesService.GetAllSeguimiento_x_id(id_seguimiento);
            int id_tipo_seguimiento = resp_seg.id_tipo_seguimiento ?? 0;

            string ruta_pdf = "";
            if (id_tipo_seguimiento == 1)
            {
                int id_planta = _HabilitacionesService.GetAllSeguimiento_x_id(id_seguimiento).id_habilitante ?? 0;
                int id_tipo_actividad = _GeneralService.recupera_planta_x_id(id_planta).id_tipo_actividad ?? 0;
                ruta_pdf = _GeneralService.recupera_toda_tipo_actividad_planta_x_id(id_tipo_actividad).ruta_ftp;
            }
            else
            {
                if (id_tipo_seguimiento == 2)
                {
                    int var_id_tipo_embarcacion = _HabilitacionesService.Recupera_Embarcacion(id_seguimiento, 0).id_tipo_embarcacion ?? 0;
                    ruta_pdf = _GeneralService.recupera_tipo_embarcacion(var_id_tipo_embarcacion).First().ruta_ftp;
                }
                else
                {
                    if (id_tipo_seguimiento == 3)
                    {
                        ruta_pdf = _GeneralService.recupera_tipo_desembarcadero_x_id_desembarcadero(resp_seg.id_habilitante ?? 0).ruta_pdf;
                    }
                    else
                    {
                        if (id_tipo_seguimiento == 4)
                        {
                            int id_concesion = _HabilitacionesService.GetAllSeguimiento_x_id(id_seguimiento).id_habilitante ?? 0;
                            ruta_pdf = _GeneralService.recupera_mae_concesion_x_id(id_concesion).ruta_pdf;
                        }
                        else
                        {

                            if (id_tipo_seguimiento == 5)
                            {
                                ruta_pdf = "habilitaciones/transporte";
                            }
                            else
                            {
                                if (id_tipo_seguimiento == 6)
                                {
                                    int id_almacen = _HabilitacionesService.GetAllSeguimiento_x_id(id_seguimiento).id_habilitante ?? 0;
                                    ruta_pdf = _GeneralService.recupera_almacen_x_id(id_almacen).ruta_pdf;
                                }
                            }
                        }
                    }
                }
            }
            if (id_tipo_seguimiento == 7)
            {
                Protocolo = (from p in _HabilitacionesService.lista_protocolo_ai_x_seguimiento(id_seguimiento)
                             select new ProtocoloResponse
                             {
                                 id_protocolo = p.id_protocolo,
                                 nombre = p.nombre,
                                 activo = p.activo,
                                 cadena_fecha_inicio = Convert.ToDateTime(p.fecha_inicio).ToShortDateString(),
                                 cadena_fecha_fin = p.fecha_fin == null ? "---" : Convert.ToDateTime(p.fecha_fin).ToShortDateString(),
                                 ruta_archivo = RUTA_SERVER + p.ruta_pdf + "/" + p.id_protocolo.ToString() + ".pdf"
                             });
            }
            else
            {
                if (id_tipo_seguimiento == 8)
                {
                    Protocolo = (from p in _HabilitacionesService.lista_protocolo_lo_x_seguimiento(id_seguimiento)
                                 select new ProtocoloResponse
                                 {
                                     id_protocolo = p.id_protocolo,
                                     nombre = p.nombre,
                                     activo = p.activo,
                                     cadena_fecha_inicio = Convert.ToDateTime(p.fecha_inicio).ToShortDateString(),
                                     cadena_fecha_fin = p.fecha_fin == null ? "---" : Convert.ToDateTime(p.fecha_fin).ToShortDateString(),
                                     ruta_archivo = RUTA_SERVER + p.ruta_pdf + "/" + p.id_protocolo.ToString() + ".pdf"
                                 });
                }
                else
                {
                    Protocolo = (from p in _HabilitacionesService.lista_protocolo_x_seguimiento(id_seguimiento)
                                 select new ProtocoloResponse
                                 {
                                     id_protocolo = p.id_protocolo,
                                     nombre = p.nombre,
                                     activo = p.activo,
                                     cadena_fecha_inicio = Convert.ToDateTime(p.fecha_inicio).ToShortDateString(),
                                     cadena_fecha_fin = p.fecha_fin == null ? "---" : Convert.ToDateTime(p.fecha_fin).ToShortDateString(),
                                     ruta_archivo = RUTA_SERVER + ruta_pdf + "/" + p.id_protocolo.ToString() + ".pdf"
                                 });
                }
            }


            return Json(Protocolo, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_haccp_seguimiento(int id_seguimiento = 0) /// ME QUEDE ACA
        {
            IEnumerable<ConstanciaHaccpResponse> constancia = new List<ConstanciaHaccpResponse>();

            constancia = _HabilitacionesService.lista_haccp_x_seguimiento(id_seguimiento);

            return Json(constancia, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listar_Observacion_x_seguimiento(int id_seguimiento = 0)
        {
            IEnumerable<SeguimientoDhcpaObservacionesResponse> observaciones = new List<SeguimientoDhcpaObservacionesResponse>();

            observaciones = _HabilitacionesService.Listar_Observacion_x_seguimiento(id_seguimiento).OrderBy(x => x.id_seg_dhcpa_observacion);

            return Json(observaciones, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listar_historial_evaluador(int id_seguimiento = 0)
        {
            IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> historial_evaluador = new List<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result>();

            historial_evaluador = _HabilitacionesService.CONSULTA_HISTORIAL_EVALUADOR(id_seguimiento);

            return Json(historial_evaluador, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Documento_dhcpa()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {

                    List<SelectListItem> lista_sedes_externo = new List<SelectListItem>();
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    lista_sedes_externo.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_personal = new List<SelectListItem>();

                    lista_sedes.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    int id_ofi_ruc = 0;

                    foreach (var result in _GeneralService.Recupera_oficina_all_x_ruc("20565429656"))
                    {
                        if (result.id_ofi_padre == null)
                        {
                            id_ofi_ruc = result.id_oficina;
                        }
                    };


                    foreach (var result in _GeneralService.Recupera_sede_all(id_ofi_ruc))
                    {
                        lista_sedes.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_sede.ToString()
                        }
                        );
                    };

                    Lista_Oficina_destino.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINA",
                        Value = "0"
                    });

                    lista_personal.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR PERSONAL",
                        Value = ""
                    });

                    List<SelectListItem> lista_tipo_documento = new List<SelectListItem>();

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("", "0"))
                    {
                        lista_tipo_documento.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                    };

                    List<SelectListItem> lista_archivadores = new List<SelectListItem>();

                    lista_archivadores.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR ARCHIVADOR",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Archivador())
                    {
                        lista_archivadores.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_archivador.ToString()
                            });
                    };

                    List<SelectListItem> lista_filiales = new List<SelectListItem>();

                    lista_filiales.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR FILIALES",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Filial())
                    {
                        lista_filiales.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_filial.ToString()
                            });
                    };

                    List<SelectListItem> Lista_per_crea = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_per_crea.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_SEGUIMIENTO");
                    tbl.Columns.Add("SEGUIMIENTO");
                    tbl.Columns.Add("EXTERNO");

                    string var_per_num_doc = "";
                    if (Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[5].Trim()) == 18)
                    {
                        var_per_num_doc = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    }
                    var oficinas = _HabilitacionesService.GetAllSeguimiento(var_per_num_doc);

                    foreach (var result in oficinas)
                    {
                        string seguimiento = "";
                        if (result.Expediente != null)
                        { seguimiento = result.Expediente + "." + result.nom_tipo_expediente + "(" + result.nom_tipo_procedimiento + ")"; }
                        else
                        { seguimiento = result.nom_tipo_procedimiento; }
                        string externo = "";
                        if (result.nom_oficina_ext != null)
                        { externo = result.ruc + " - " + result.nom_oficina_ext; }
                        else
                        { externo = result.persona_num_documento + " - " + result.nom_persona_ext; }
                        tbl.Rows.Add(result.id_seguimiento, seguimiento, externo);
                    };

                    ViewData["TabSeguimiento"] = tbl;

                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina = lista_personal;

                    ViewBag.lstsede_destino_externo = lista_sedes_externo;
                    ViewBag.lstOficina_destino_externo = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina_externo = lista_personal;

                    ViewBag.lst_persona_crea = Lista_per_crea;
                    ViewBag.list_tip_documento_dhcpa = lista_tipo_documento;
                    ViewBag.list_archivador = lista_archivadores;
                    ViewBag.lista_filiales = lista_filiales;

                    ViewBag.user_document = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    ViewBag.user_perfil = HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim();

                    DocumentodhcpaViewModel model_doc_dhcpa = new DocumentodhcpaViewModel();

                    return View(model_doc_dhcpa);

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Documento_dhcpa(DocumentodhcpaViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    
                    //model.id_oficina_direccion = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                    model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo(model.id_tipo_documento) + 1;
                    model.nom_doc = "-" + DateTime.Now.Year.ToString() + "- DHCPA/SANIPES";
                    DocumentoDhcpaRequest req_documento_dhcpa = ModelToRequest.Documento_dhcpa(model);
                    req_documento_dhcpa.fecha_registro = DateTime.Now;
                    req_documento_dhcpa.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa);
                    req_documento_dhcpa.id_doc_dhcpa = model.id_doc_dhcpa;

                    if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                    {
                        try
                        {
                            DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                            req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                            req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_seguimiento.activo = "1";
                            req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                        }
                        catch (Exception) { }
                    }

                    if (model.documento_dhcpa_detalle != null)
                    {
                        foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                        {
                            DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                            req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_detalle.activo = "1";
                            req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                            obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                        }
                    }

                    if (model.documento_dhcpa_seguimiento != null)
                    {
                        foreach (detdocdhcpasegViewModel obj in model.documento_dhcpa_seguimiento)
                        {
                            DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = ModelToRequest.Documento_dhcpa_seguimiento(obj);
                            req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_seguimiento.activo = "1";
                            req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                        }
                    }

                    if (model.ind_agregar_celula == 1)
                    {
                        string mensaje = "";
                        mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        
                        if (model.id_tipo_documento == 136)
                        {
                            model.doc_notificar_cdl_notif= model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }
                        // 21 : CEDULA DE NOTIFICACION
                        model.id_tipo_documento = 21;
                        model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo(model.id_tipo_documento) + 1;


                        DocumentoDhcpaRequest req_documento_dhcpa2 = ModelToRequest.Documento_dhcpa(model);
                        req_documento_dhcpa2.fecha_registro = DateTime.Now;
                        req_documento_dhcpa2.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();

                        model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa2);
                        req_documento_dhcpa2.id_doc_dhcpa = model.id_doc_dhcpa;

                        if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                        {
                            try
                            {
                                DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                                req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                                req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_seguimiento.activo = "1";
                                req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                            }
                            catch (Exception) { }
                        }

                        if (model.documento_dhcpa_detalle != null)
                        {
                            foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                            {
                                DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                                req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_detalle.activo = "1";
                                req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                                obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                            }
                        }

                        try
                        {
                            mensaje = mensaje + ", Se creó el Documento : CEDULA DE NOTIFICACION N° " + model.num_doc.ToString() + model.nom_doc;
                            @ViewBag.Mensaje = mensaje;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }

                    }
                    else
                    {
                        try
                        {
                            @ViewBag.Mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }
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

        [AllowAnonymous]
        public ActionResult publicar_protocolos()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Generar_data_planta()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        var result = _HabilitacionesService.genera_protocolo_planta();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_plantas" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "externo" + '\u0022' + ":" + '\u0022' + busc.genera_data_externo + '\u0022' + "," + '\u0022' +
                                    "codigo_planta" + '\u0022' + ":" + '\u0022' + busc.genera_data_codigo_planta + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "direccion" + '\u0022' + ":" + '\u0022' + busc.genera_data_direccion + '\u0022' + "," + '\u0022' +
                                    "archivos" + '\u0022' + ":" + '\u0022' + busc.genera_data_archivos + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_PLANTA"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_PLANTA_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        List<SelectListItem> lista_combo_actividad = new List<SelectListItem>();

                        int ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_actividad)
                            {
                                if (actividad.genera_data_actividad == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_actividad.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_actividad,
                                    Value = actividad.genera_data_actividad
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        string CONTENIDO_COMBO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_actividad)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ACT_PLANTA"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ACT_PLANTA_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_autorizacion_instalacion()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        #region principal inicio
                        var result = _HabilitacionesService.genera_protocolo_autorizacion_instalacion();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_autorizacion_instalacion" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "protocolo" + '\u0022' + ":" + '\u0022' + busc.genera_data_protocolo + '\u0022' + "," + '\u0022' +
                                    "fecha" + '\u0022' + ":{" + '\u0022' + "display" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha + '\u0022' + "," + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha_id + '\u0022' + "}," + '\u0022' +
                                    "proposito" + '\u0022' + ":" + '\u0022' + busc.genera_data_proposito + '\u0022' + "," + '\u0022' +
                                    "establecimiento" + '\u0022' + ":" + '\u0022' + busc.genera_data_establecimiento + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "ubicacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_ubicacion + '\u0022' + "," + '\u0022' +
                                    "departamento" + '\u0022' + ":" + '\u0022' + busc.genera_data_departamento + '\u0022' + "," + '\u0022' +
                                    "provincia" + '\u0022' + ":" + '\u0022' + busc.genera_data_provincia + '\u0022' + "," + '\u0022' +
                                    "distrito" + '\u0022' + ":" + '\u0022' + busc.genera_data_distrito + '\u0022' + "," + '\u0022' +
                                    "pdf" + '\u0022' + ":" + '\u0022' + "<a doc='" + busc.genera_data_ruta + "/" + busc.genera_data_pdf + "' class='icon_pdf' ></a>" + '\u0022' + "," + '\u0022' +
                                    "annio" + '\u0022' + ":" + '\u0022' + busc.genera_data_annio + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_AUTORIZACION_INSTALACION"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_AUTORIZACION_INSTALACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);
                        #endregion

                        #region combo_proposito
                        List<SelectListItem> lista_combo_proposito = new List<SelectListItem>();

                        lista_combo_proposito.Add(new SelectListItem()
                        {
                            Text = "todos",
                            Value = ""
                        });

                        int ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_proposito)
                            {
                                if (actividad.genera_data_proposito == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_proposito.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_proposito,
                                    Value = actividad.genera_data_proposito
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        string CONTENIDO_COMBO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_proposito)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROPOSITO_AUTORIZACION_INSTALACION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROPOSITO_AUTORIZACION_INSTALACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        #region annio

                        List<SelectListItem> lista_combo_annio = new List<SelectListItem>();

                        lista_combo_annio.Add(new SelectListItem()
                        {
                            Text = "todos",
                            Value = ""
                        }
                                );

                        ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_annio)
                            {
                                if (actividad.genera_data_annio.ToString() == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_annio.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_annio.ToString(),
                                    Value = actividad.genera_data_annio.ToString()
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        entra = 0;
                        CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_annio)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ANNIO_AUTORIZACION_INSTALACION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ANNIO_AUTORIZACION_INSTALACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_licencia_operacion()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        #region principal inicio
                        var result = _HabilitacionesService.genera_protocolo_licencia_operacion();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_licencia_operacion" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "protocolo" + '\u0022' + ":" + '\u0022' + busc.genera_data_protocolo + '\u0022' + "," + '\u0022' +
                                    "fecha" + '\u0022' + ":{" + '\u0022' + "display" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha + '\u0022' + "," + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha_id + '\u0022' + "}," + '\u0022' +
                                    "proposito" + '\u0022' + ":" + '\u0022' + busc.genera_data_proposito + '\u0022' + "," + '\u0022' +
                                    "establecimiento" + '\u0022' + ":" + '\u0022' + busc.genera_data_establecimiento + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "ubicacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_ubicacion + '\u0022' + "," + '\u0022' +
                                    "departamento" + '\u0022' + ":" + '\u0022' + busc.genera_data_departamento + '\u0022' + "," + '\u0022' +
                                    "provincia" + '\u0022' + ":" + '\u0022' + busc.genera_data_provincia + '\u0022' + "," + '\u0022' +
                                    "distrito" + '\u0022' + ":" + '\u0022' + busc.genera_data_distrito + '\u0022' + "," + '\u0022' +
                                    "pdf" + '\u0022' + ":" + '\u0022' + "<a doc='" + busc.genera_data_ruta + "/" + busc.genera_data_pdf + "' class='icon_pdf' ></a>" + '\u0022' + "," + '\u0022' +
                                    "annio" + '\u0022' + ":" + '\u0022' + busc.genera_data_annio + '\u0022';
                            entra = 1;
                        };


                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_LICENCIA_OPERACION"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_LICENCIA_OPERACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);
                        #endregion

                        #region combo_proposito
                        List<SelectListItem> lista_combo_proposito = new List<SelectListItem>();

                        lista_combo_proposito.Add(new SelectListItem()
                        {
                            Text = "todos",
                            Value = ""
                        });

                        int ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_proposito)
                            {
                                if (actividad.genera_data_proposito == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_proposito.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_proposito,
                                    Value = actividad.genera_data_proposito
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        string CONTENIDO_COMBO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_proposito)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROPOSITO_LICENCIA_OPERACION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROPOSITO_LICENCIA_OPERACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        #region annio

                        List<SelectListItem> lista_combo_annio = new List<SelectListItem>();

                        lista_combo_annio.Add(new SelectListItem()
                        {
                            Text = "todos",
                            Value = ""
                        }
                                );

                        ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_annio)
                            {
                                if (actividad.genera_data_annio.ToString() == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_annio.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_annio.ToString(),
                                    Value = actividad.genera_data_annio.ToString()
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        entra = 0;
                        CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_annio)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ANNIO_LICENCIA_OPERACION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ANNIO_LICENCIA_OPERACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_embarcacion()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        var result = _HabilitacionesService.genera_protocolo_embarcacion();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_embarcacion" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "matricula" + '\u0022' + ":" + '\u0022' + busc.genera_data_matricula + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.genera_data_nombre + '\u0022' + "," + '\u0022' +
                                    "tipo_embarcacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_tipo_embarcacion + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "codigo_embarcacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_codigo_habilitacion + '\u0022' + "," + '\u0022' +
                                    "archivos" + '\u0022' + ":" + '\u0022' + busc.genera_data_archivos + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_EMBARCACION"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_EMBARCACION_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_transporte()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {
                        var result = _HabilitacionesService.genera_protocolo_transporte();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_transporte" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "externo" + '\u0022' + ":" + '\u0022' + busc.genera_data_externo + '\u0022' + "," + '\u0022' +
                                    "placa" + '\u0022' + ":" + '\u0022' + busc.genera_data_Placa + '\u0022' + "," + '\u0022' +
                                    "codigo_habilitacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_cod_Habilitacion + '\u0022' + "," + '\u0022' +
                                    "protocolo" + '\u0022' + ":" + '\u0022' + busc.genera_data_protocolo + '\u0022' + "," + '\u0022' +
                                    "fecha" + '\u0022' + ":{" + '\u0022' + "display" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha + '\u0022' + "," + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.genera_data_fecha_id + '\u0022' + "}," + '\u0022' +
                                    "pdf" + '\u0022' + ":" + '\u0022' + "<a doc='" + busc.genera_data_ruta + "/" + busc.genera_data_pdf + "' class='icon_pdf' ></a>" + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_TRANSPORTE"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_TRANSPORTE_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_almacen()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        var result = _HabilitacionesService.genera_protocolo_almacen();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_almacen" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "externo" + '\u0022' + ":" + '\u0022' + busc.genera_data_externo + '\u0022' + "," + '\u0022' +
                                    "direccion" + '\u0022' + ":" + '\u0022' + busc.genera_data_direccion + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "codigo_habilitacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_codigo_habilitacion + '\u0022' + "," + '\u0022' +
                                    "archivos" + '\u0022' + ":" + '\u0022' + busc.genera_data_archivo + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_ALMACEN"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_ALMACEN_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_concesiones()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {
                        var result = _HabilitacionesService.genera_protocolo_concesion();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_concesiones" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP = CONTENIDO_PHP + '\u0022' + "externo" + '\u0022' + ":" + '\u0022' + busc.genera_data_externo + '\u0022' + "," + '\u0022' +
                                    "actividad" + '\u0022' + ":" + '\u0022' + busc.genera_data_actividad + '\u0022' + "," + '\u0022' +
                                    "codigo_concesion" + '\u0022' + ":" + '\u0022' + busc.genera_data_codigo_concesion + '\u0022' + "," + '\u0022' +
                                    "departamento" + '\u0022' + ":" + '\u0022' + busc.genera_data_departamento + '\u0022' + "," + '\u0022' +
                                    "provincia" + '\u0022' + ":" + '\u0022' + busc.genera_data_provincia + '\u0022' + "," + '\u0022' +
                                    "distrito" + '\u0022' + ":" + '\u0022' + busc.genera_data_distrito + '\u0022' + "," + '\u0022' +
                                    "ubicacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_ubicacion + '\u0022' + "," + '\u0022' +
                                    "archivos" + '\u0022' + ":" + '\u0022' + busc.genera_data_archivos + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_CONCESION"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_CONCESION_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        List<SelectListItem> lista_combo_actividad = new List<SelectListItem>();
                        List<SelectListItem> lista_combo_departamento = new List<SelectListItem>();
                        List<SelectListItem> lista_combo_provincia = new List<SelectListItem>();
                        List<SelectListItem> lista_combo_distrito = new List<SelectListItem>();

                        int ing1 = 0;
                        int ing2 = 0;
                        int ing3 = 0;
                        foreach (var llena_combo in result)
                        {

                            foreach (var encuentra in lista_combo_distrito)
                            {
                                if (llena_combo.genera_data_departamento + "_" + llena_combo.genera_data_provincia + "_" + llena_combo.genera_data_distrito == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                #region combo_departamento

                                foreach (var encuentra_departamento in lista_combo_departamento)
                                {
                                    if (llena_combo.genera_data_departamento == encuentra_departamento.Value)
                                    {
                                        ing2 = 1;
                                        break;
                                    }
                                }
                                if (ing2 == 0)
                                {
                                    lista_combo_departamento.Add(new SelectListItem()
                                    {
                                        Text = llena_combo.genera_data_departamento,
                                        Value = llena_combo.genera_data_departamento
                                    });
                                }
                                #endregion

                                #region combo_provincia
                                foreach (var encuentra_provincia in lista_combo_provincia)
                                {
                                    if (llena_combo.genera_data_departamento + "_" + llena_combo.genera_data_provincia == encuentra_provincia.Text)
                                    {
                                        ing3 = 1;
                                        break;
                                    }
                                }
                                if (ing3 == 0)
                                {
                                    lista_combo_provincia.Add(new SelectListItem()
                                    {
                                        Text = llena_combo.genera_data_departamento + "_" + llena_combo.genera_data_provincia,
                                        Value = llena_combo.genera_data_departamento + "|" + llena_combo.genera_data_provincia
                                    });
                                }
                                #endregion

                                lista_combo_distrito.Add(new SelectListItem()
                                {
                                    Text = llena_combo.genera_data_departamento + "_" + llena_combo.genera_data_provincia + "_" + llena_combo.genera_data_distrito,
                                    Value = llena_combo.genera_data_departamento + "_" + llena_combo.genera_data_provincia + "|" + llena_combo.genera_data_distrito
                                });
                            }

                            ing3 = 0;
                            ing2 = 0;
                            ing1 = 0;

                            #region actividad
                            foreach (var encuentra in lista_combo_actividad)
                            {
                                if (llena_combo.genera_data_actividad == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_actividad.Add(new SelectListItem()
                                {
                                    Text = llena_combo.genera_data_actividad,
                                    Value = llena_combo.genera_data_actividad
                                }
                                );
                            }
                            ing1 = 0;
                            #endregion
                        }

                        #region crea_archivo_ftp_combo_actividad

                        string CONTENIDO_COMBO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_actividad)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PHP_FIN = "}]";

                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ACT_CONCESION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_ACT_CONCESION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        #region crea_archivo_ftp_combo_departamento

                        string CONTENIDO_COMBO_DEPARTAMENTO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_DEPARTAMENTO_PHP = "";
                        foreach (var busc in lista_combo_departamento)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_DEPARTAMENTO_PHP = CONTENIDO_COMBO_DEPARTAMENTO_PHP + "}, {"; }
                            CONTENIDO_COMBO_DEPARTAMENTO_PHP = CONTENIDO_COMBO_DEPARTAMENTO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_DEPARTAMENTO_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_DEPARTAMENTO_CONCESION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_DEPARTAMENTO_CONCESION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_DEPARTAMENTO_PHP_INI, CONTENIDO_COMBO_DEPARTAMENTO_PHP, CONTENIDO_COMBO_DEPARTAMENTO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        #region crea archivo ftp combo provincia

                        string CONTENIDO_COMBO_PROVINCIA_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PROVINCIA_PHP = "";
                        foreach (var busc in lista_combo_provincia)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PROVINCIA_PHP = CONTENIDO_COMBO_PROVINCIA_PHP + "}, {"; }

                            CONTENIDO_COMBO_PROVINCIA_PHP = CONTENIDO_COMBO_PROVINCIA_PHP + '\u0022' + "id_padre" + '\u0022' + ":" + '\u0022' + busc.Value.Split('|')[0].Trim() + '\u0022' + "," + '\u0022' +
                                    "id" + '\u0022' + ":" + '\u0022' + busc.Text + "|" + busc.Value.Split('|')[1].Trim() + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Value.Split('|')[1].Trim() + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PROVINCIA_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROVINCIA_CONCESION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_PROVINCIA_CONCESION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PROVINCIA_PHP_INI, CONTENIDO_COMBO_PROVINCIA_PHP, CONTENIDO_COMBO_PROVINCIA_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        #region crea archivo ftp combo distrito

                        string CONTENIDO_COMBO_DISTRITO_PHP_INI = "[{";
                        entra = 0;
                        string CONTENIDO_COMBO_DISTRITO_PHP = "";

                        foreach (var busc in lista_combo_distrito)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_DISTRITO_PHP = CONTENIDO_COMBO_DISTRITO_PHP + "}, {"; }
                            CONTENIDO_COMBO_DISTRITO_PHP = CONTENIDO_COMBO_DISTRITO_PHP + '\u0022' + "id_padre" + '\u0022' + ":" + '\u0022' + busc.Value.Split('|')[0].Trim() + '\u0022' + "," + '\u0022' +
                                    "id" + '\u0022' + ":" + '\u0022' + busc.Text + "|" + busc.Value.Split('|')[1].Trim() + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Value.Split('|')[1].Trim() + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_DISTRITO_PHP_FIN = "}]";

                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_DISTRITO_CONCESION"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_DISTRITO_CONCESION_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_DISTRITO_PHP_INI, CONTENIDO_COMBO_DISTRITO_PHP, CONTENIDO_COMBO_DISTRITO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        #endregion

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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
        public ActionResult Generar_data_desembarcadero()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[20].Trim() == "1" // Acceso a publicar protocolos
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub dirección de Habilitaciones 
                {
                    try
                    {

                        var result = _HabilitacionesService.genera_protocolo_desembarcadero();

                        string CONTENIDO_PHP_INI = "<?php	if($_POST[" + '\u0022' + "page" + '\u0022' + "]!=" + '\u0022' + "proto_desembarcaderos" + '\u0022' + "){ echo '{" + '\u0022' + "data" + '\u0022' + ":[]}'; exit;} ?> {   " + '\u0022' + "data" + '\u0022' + ": [" + " {";

                        int entra = 0;
                        string CONTENIDO_PHP = "";
                        foreach (var busc in result)
                        {
                            if (entra != 0) { CONTENIDO_PHP = CONTENIDO_PHP + "}, {"; }
                            CONTENIDO_PHP =
                                    CONTENIDO_PHP + '\u0022' + "tipo_desembarcadero" + '\u0022' + ":" + '\u0022' + busc.genera_data_tipo_desembarcadero + '\u0022' + "," + '\u0022' +
                                    "codigo_desembarcadero" + '\u0022' + ":" + '\u0022' + busc.genera_data_codigo_desembarcadero + '\u0022' + "," + '\u0022' +
                                    "denominacion" + '\u0022' + ":" + '\u0022' + busc.genera_data_denominacion + '\u0022' + "," + '\u0022' +
                                    "externo" + '\u0022' + ":" + '\u0022' + busc.genera_data_externo + '\u0022' + "," + '\u0022' +
                                    "direccion" + '\u0022' + ":" + '\u0022' + busc.genera_data_direccion + '\u0022' + "," + '\u0022' +
                                    "pesca_acuicultura" + '\u0022' + ":" + '\u0022' + busc.genera_data_pesca_acuicultura + '\u0022' + "," + '\u0022' +
                                    "archivos" + '\u0022' + ":" + '\u0022' + busc.genera_data_archivos + '\u0022';

                            entra = 1;
                        };

                        string CONTENIDO_PHP_FIN = "} ]}";

                        string PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_DESEMBARCADERO"].ToString();
                        string PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_DESEMBARCADERO_FTP"].ToString();

                        crear_archivo(CONTENIDO_PHP_INI, CONTENIDO_PHP, CONTENIDO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        List<SelectListItem> lista_combo_tipo_desembarcadero = new List<SelectListItem>();

                        int ing1 = 0;
                        foreach (var actividad in result)
                        {
                            foreach (var encuentra in lista_combo_tipo_desembarcadero)
                            {
                                if (actividad.genera_data_tipo_desembarcadero == encuentra.Text)
                                {
                                    ing1 = 1;
                                    break;
                                }
                            }
                            if (ing1 == 0)
                            {
                                lista_combo_tipo_desembarcadero.Add(new SelectListItem()
                                {
                                    Text = actividad.genera_data_tipo_desembarcadero,
                                    Value = actividad.genera_data_tipo_desembarcadero
                                }
                                );
                            }
                            ing1 = 0;
                        }

                        string CONTENIDO_COMBO_PHP_INI = "[{";

                        entra = 0;
                        string CONTENIDO_COMBO_PHP = "";
                        foreach (var busc in lista_combo_tipo_desembarcadero)
                        {
                            if (entra != 0) { CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + "}, {"; }
                            CONTENIDO_COMBO_PHP = CONTENIDO_COMBO_PHP + '\u0022' + "id" + '\u0022' + ":" + '\u0022' + busc.Value + '\u0022' + "," + '\u0022' +
                                    "nombre" + '\u0022' + ":" + '\u0022' + busc.Text + '\u0022';
                            entra = 1;
                        };

                        string CONTENIDO_COMBO_PHP_FIN = "}]";


                        PHP_RUTA_ARCHIVO = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_TIPO_DESEMBARCADERO"].ToString();
                        PHP_RUTA_ARCHIVO_FTP = ConfigurationManager.AppSettings["PHP_RUTA_ARCHIVO_COMBO_TIPO_DESEMBARCADERO_FTP"].ToString();

                        crear_archivo(CONTENIDO_COMBO_PHP_INI, CONTENIDO_COMBO_PHP, CONTENIDO_COMBO_PHP_FIN, PHP_RUTA_ARCHIVO, PHP_RUTA_ARCHIVO_FTP);

                        @ViewBag.Mensaje = "Se Guardo y se publicó Satisfactoriamente";
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

        private void crear_archivo(string CONTENIDO_PHP_INI, string CONTENIDO_PHP, string CONTENIDO_PHP_FIN, string RUTA_ARCHIVO_LOCAL, string RUTA_ARCHIVO_FTP)
        {
            string RUTA_SERVER = ConfigurationManager.AppSettings["RUTA_FTP"].ToString();
            string USU_DATA_FTP = ConfigurationManager.AppSettings["USU_DATA_FTP"].ToString();
            string CONT_DATA_FTP = ConfigurationManager.AppSettings["CONTRA_DATA_FTP"].ToString();

            System.IO.StreamWriter z_varocioStreamWriter = new System.IO.StreamWriter(@"" + RUTA_ARCHIVO_LOCAL, false, System.Text.Encoding.UTF8);
            z_varocioStreamWriter.Write(CONTENIDO_PHP_INI + CONTENIDO_PHP + CONTENIDO_PHP_FIN);
            z_varocioStreamWriter.Close();

            string path = @"" + RUTA_ARCHIVO_LOCAL;

            // Open the stream and read it back.
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RUTA_SERVER + RUTA_ARCHIVO_FTP);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // FTP credenciales
                request.Credentials = new NetworkCredential(USU_DATA_FTP, CONT_DATA_FTP);

                //archivo que se va a subir
                StreamReader sourceStream = new StreamReader(fs);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
            }
        }

        public ActionResult llenar_furgon_x_carroceria(int id_tipo_carroceria)
        {
            List<SelectListItem> Lista_furgon = new List<SelectListItem>();

            var recupera_furgon = _HabilitacionesService.consulta_todo_activo_tipofurgon(id_tipo_carroceria);

            foreach (var result2 in recupera_furgon)
            {
                Lista_furgon.Add(new SelectListItem()
                {
                    Text = result2.nombre,
                    Value = result2.id_tipo_furgon.ToString()
                }
                );
            };

            return Json(Lista_furgon, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_transporte(string expediente, string id_seguimiento)
        {
            if (id_seguimiento != null && id_seguimiento != "")
            {
                Session["Habilitaciones_nuevo_protocolo_transporte_id_seguimiento"] = id_seguimiento;
                return RedirectToAction("Nuevo_Protocolo_transporte", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_transporte()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_transporte_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_transporte_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> lista_sexo = new List<SelectListItem>();

                    lista_sexo.Add(new SelectListItem() { Text = "MASCULINO", Value = "M" });
                    lista_sexo.Add(new SelectListItem() { Text = "FEMENINO", Value = "F" });

                    ViewBag.lst_combo_sexo = lista_sexo;


                    List<SelectListItem> Lista_tipo_doc_iden = new List<SelectListItem>();

                    var recupera_tipo_documento = _GeneralService.llenar_tipo_documento_identidad();

                    foreach (var result in recupera_tipo_documento)
                    {
                        Lista_tipo_doc_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                        );
                    };

                    ViewBag.lst_combo_tipo_identidad = Lista_tipo_doc_iden;

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;

                    List<SelectListItem> Lista_transporte = new List<SelectListItem>();
                    List<SelectListItem> Lista_carroceria = new List<SelectListItem>();
                    List<SelectListItem> Lista_unidad_medida = new List<SelectListItem>();
                    List<SelectListItem> Lista_furgon = new List<SelectListItem>();
                    ViewBag.lst_transporte = Lista_transporte;

                    var recupera_carroceria = _HabilitacionesService.consulta_todo_activo_tipocarroceria();
                    int entra = 0;
                    foreach (var result in recupera_carroceria)
                    {
                        if (entra == 0)
                        {
                            var recupera_furgon = _HabilitacionesService.consulta_todo_activo_tipofurgon(result.id_tipo_carroceria);

                            foreach (var result2 in recupera_furgon)
                            {
                                Lista_furgon.Add(new SelectListItem()
                                {
                                    Text = result2.nombre,
                                    Value = result2.id_tipo_furgon.ToString()
                                }
                                );
                            };
                            entra = 1;
                        }
                        Lista_carroceria.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_carroceria.ToString()
                        }
                        );
                    };

                    ViewBag.lst_tipo_furgon = Lista_furgon;

                    var recupera_um = _HabilitacionesService.consulta_todo_activo_unidad_medida();

                    foreach (var result in recupera_um)
                    {
                        Lista_unidad_medida.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.id_um.ToString()
                        }
                        );
                    };

                    List<SelectListItem> Lista_tipo_atencion = new List<SelectListItem>();
                    var recupera_tipo_atencion = _HabilitacionesService.consulta_todo_tipo_atencion();

                    foreach (var result in recupera_tipo_atencion)
                    {
                        Lista_tipo_atencion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_atencion.ToString()
                        }
                        );
                    };

                    ViewBag.lst_nuevo_carroceria = Lista_carroceria;
                    ViewBag.lst_tipo_carroceria_tarjpro = Lista_carroceria;
                    ViewBag.lst_tipo_atencion = Lista_tipo_atencion;
                    ViewBag.lst_nuevo_um = Lista_unidad_medida;

                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        });
                    };

                    List<SelectListItem> lista_infraestructura_pesquera = new List<SelectListItem>();

                    var var_lista_tipo_camara_transporte = _HabilitacionesService.consulta_todo_activo_tipoCamaraTransporte();

                    foreach (var result in var_lista_tipo_camara_transporte)
                    {
                        lista_infraestructura_pesquera.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_camara_trans.ToString()
                        });
                    };

                    ViewBag.lst_tipo_camara_transporte = lista_infraestructura_pesquera;

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();
                    }
                    else
                    {
                        ViewBag.id_persona_legal = rec_seg.id_dni_persona_legal.ToString();
                    }

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                        ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;
                    }
                    else
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal_DNI;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal_DNI;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal_DNI;
                        ViewBag.Str_Direccion_Legal = rec_seg.str_direccion_persona_natural;
                    }

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;

                    ViewBag.Str_Persona = rec_seg.nom_persona_ext;
                    ViewBag.var_DNI = rec_seg.persona_num_documento;


                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    DbGeneralMaeTransporteResponse trans_res = new DbGeneralMaeTransporteResponse();
                    if (rec_seg.id_habilitante != 0)
                    {
                        trans_res = _HabilitacionesService.consulta_db_general_transporte_x_id(rec_seg.id_habilitante ?? 0);
                    }
                    if (trans_res.id_transporte != 0 && trans_res.id_transporte != null)
                    {
                        ViewBag.id_transporte = trans_res.id_transporte.ToString();
                        ViewBag.placa = trans_res.placa.ToString();
                        ViewBag.carroceria = trans_res.nombre_carroceria.ToString();
                        ViewBag.furgon = trans_res.nombre_furgon.ToString();
                        ViewBag.carga_util = trans_res.carga_util.ToString() + " " + trans_res.nombre_um.ToString();
                        ViewBag.codigo_hab = trans_res.cod_habilitacion.ToString();
                    }
                    else
                    {
                        ViewBag.id_transporte = "0";
                        ViewBag.placa = "";
                        ViewBag.carroceria = "";
                        ViewBag.furgon = "";
                        ViewBag.carga_util = "";
                        ViewBag.codigo_hab = "";
                    }
                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_transporte(int txt_id_seguimiento, DateTime txt_fecha_inicio, DateTime txt_fecha_fin,
           int txt_id_nombre_legal, int txt_id_direccion_legal, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add, int txt_id_transporte,
            int cmb_infra_pesq, string lbl_acta_inspeccion, string lbl_inf_auditoria, string lbl_inf_tecnico, string txt_persona_2, string lbl_Direccion_legal,
            int cmb_tipo_carro_tarpro, int cmb_tipo_atencion, string txt_nombre, string lbl_info_sdhpa)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        /*
                        int numero_proto = _HabilitacionesService.Generar_numero_protocolo_transporte(DateTime.Now.Year);
                        */
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        /*
                        req_protocolo.nombre = "PTH-"+numero_proto.ToString("000")+"-"+DateTime.Now.Year.ToString()+"-SANIPES";
                         * */
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;
                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        SeguimientoDhcpaRequest seg_response = new SeguimientoDhcpaRequest();
                        seg_response = _HabilitacionesService.Recupera_seguimiento_x_id(txt_id_seguimiento).First();

                        ProtocoloTransporteRequest req_protocolo_transporte = new ProtocoloTransporteRequest();

                        if (seg_response.persona_num_documento != null && seg_response.persona_num_documento != "")
                        {
                            req_protocolo_transporte.representante_legal = 0;
                            req_protocolo_transporte.direccion_legal = 0;
                            req_protocolo_transporte.direccion_legal_dni = lbl_Direccion_legal;
                            req_protocolo_transporte.representante_legal_dni = txt_id_nombre_legal;
                        }
                        else
                        {
                            req_protocolo_transporte.representante_legal = txt_id_nombre_legal;
                            req_protocolo_transporte.direccion_legal = txt_id_direccion_legal;
                            req_protocolo_transporte.direccion_legal_dni = "";
                            req_protocolo_transporte.representante_legal_dni = 0;
                        }

                        req_protocolo_transporte.id_tipo_atencion = cmb_tipo_atencion;
                        req_protocolo_transporte.id_tipo_carroceria_tarpro = cmb_tipo_carro_tarpro;
                        req_protocolo_transporte.id_protocolo = req_protocolo.id_protocolo;
                        //req_protocolo_transporte.numero = numero_proto;
                        req_protocolo_transporte.anno = DateTime.Now.Year;
                        req_protocolo_transporte.id_tipo_camara_trans = cmb_infra_pesq;
                        req_protocolo_transporte.id_transporte = txt_id_transporte;
                        req_protocolo_transporte.persona_2 = txt_persona_2;

                        DbGeneralMaeTransporteResponse tra_res = new DbGeneralMaeTransporteResponse();
                        tra_res = _HabilitacionesService.consulta_db_general_transporte_x_id(txt_id_transporte);

                        req_protocolo_transporte.placa = tra_res.placa;
                        req_protocolo_transporte.cod_habilitacion = tra_res.cod_habilitacion;
                        req_protocolo_transporte.id_tipo_carroceria = tra_res.id_tipo_carroceria;
                        req_protocolo_transporte.id_tipo_furgon = tra_res.id_tipo_furgon;
                        req_protocolo_transporte.id_um = tra_res.id_um;
                        req_protocolo_transporte.carga_util = tra_res.carga_util;
                        req_protocolo_transporte.acta_inspeccion = lbl_acta_inspeccion;
                        req_protocolo_transporte.informe_auditoria = lbl_inf_auditoria;
                        req_protocolo_transporte.informe_tecnico_evaluacion = lbl_inf_tecnico;
                        req_protocolo_transporte.informe_sdhpa = lbl_info_sdhpa;

                        req_protocolo_transporte.id_dat_pro_transporte = _HabilitacionesService.Create_Protocolo_Transporte(req_protocolo_transporte);

                        if (seg_response.id_habilitante == 0)
                        {
                            seg_response.id_habilitante = txt_id_transporte;
                            seg_response.cod_habilitante = tra_res.placa + " / " + tra_res.nombre_carroceria;
                            _HabilitacionesService.Update_seguimiento_dhcpa(seg_response);
                        }

                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        @ViewBag.Mensaje = "Se creo el protocolo " + req_protocolo.nombre + " satisfactoriamente |" + req_protocolo.id_protocolo.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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
        public ActionResult Actualizar_Transporte(int id_transporte, string codigo_habilitacion, int id_carroceria, int id_furgon, decimal cantidad, int id_um)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[25].Trim() == "1")
                {
                    try
                    {
                        DbGeneralMaeTransporteResponse trans_res = new DbGeneralMaeTransporteResponse();
                        trans_res = _HabilitacionesService.consulta_db_general_transporte_x_id(id_transporte);
                        if (cantidad == 0)
                        {
                            int x = _HabilitacionesService.actualizar_nuevo_transporte(id_transporte, codigo_habilitacion, id_carroceria, id_furgon, trans_res.carga_util ?? 0, trans_res.id_um ?? 0, HttpContext.User.Identity.Name.Split('|')[1].Trim()).id_transporte;
                        }
                        else
                        {
                            int x = _HabilitacionesService.actualizar_nuevo_transporte(id_transporte, codigo_habilitacion, id_carroceria, id_furgon, cantidad, id_um, HttpContext.User.Identity.Name.Split('|')[1].Trim()).id_transporte;
                        }
                        @ViewBag.Mensaje = id_transporte.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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
        public ActionResult Grabar_Nuevo_Transporte(string nueva_placa, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[25].Trim() == "1")
                {
                    try
                    {
                        int id_transporte = _HabilitacionesService.registrar_nuevo_transporte(nueva_placa, nueva_codigo_habilitacion, nueva_carroceria, tipo_furgon, nueva_carga_util, nueva_unidad_medida, HttpContext.User.Identity.Name.Split('|')[1].Trim()).id_transporte;
                        @ViewBag.Mensaje = id_transporte.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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
        public ActionResult Consultar_placa_transporte(string placa)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[25].Trim() == "1")
                {
                    try
                    {
                        var transpo = _GeneralService.listar_transporte_x_placa(placa);
                        int id_transporte = 0;
                        if (transpo.Count() > 0)
                        {
                            id_transporte = transpo.First().id_transporte;
                        }
                        @ViewBag.Mensaje = id_transporte.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_Success_NS");
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

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_autorizacion_instalacion(string expediente, string id_seguimiento)
        {
            if (id_seguimiento != null && id_seguimiento != "")
            {
                Session["Habilitaciones_nuevo_protocolo_autorizacion_instalacion_id_seguimiento"] = id_seguimiento;
                return RedirectToAction("Nuevo_Protocolo_autorizacion_instalacion", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_autorizacion_instalacion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_autorizacion_instalacion_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_autorizacion_instalacion_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    List<SelectListItem> lista_tipo_autorizacion_instalacion = new List<SelectListItem>();

                    var var_lista_tipo_ai = _HabilitacionesService.Lista_tipo_autorizacion(0);

                    foreach (var result in var_lista_tipo_ai)
                    {
                        lista_tipo_autorizacion_instalacion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_autorizacion_instalacion.ToString()
                        }
                        );
                    };

                    ViewBag.lista_tipo_autorizacion = lista_tipo_autorizacion_instalacion;

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.ruc_seg = rec_seg.ruc;
                    ViewBag.id_sede_ext = rec_seg.id_sede_ext.ToString();
                    ViewBag.nom_direccion_ext = rec_seg.nom_direccion_ext.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                    ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                    ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                    ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_autorizacion_instalacion(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio,
            int txt_id_nombre_legal, int txt_id_sede_ext, int cmb_tipo_autorizacion, string txt_ruc_seg, string txt_actividad)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = 1;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloAutorizacionInstalacionRequest req_protocolo_autorizacion_instalacion = new ProtocoloAutorizacionInstalacionRequest();

                        req_protocolo_autorizacion_instalacion.id_representante_legal = txt_id_nombre_legal;
                        req_protocolo_autorizacion_instalacion.id_sede = txt_id_sede_ext;
                        req_protocolo_autorizacion_instalacion.id_tipo_autorizacion_instalacion = cmb_tipo_autorizacion;
                        req_protocolo_autorizacion_instalacion.ruc = txt_ruc_seg;
                        req_protocolo_autorizacion_instalacion.actividad = txt_actividad;

                        req_protocolo_autorizacion_instalacion.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_autorizacion_instalacion.id_pro_autorizacion_instalacion = _HabilitacionesService.Create_Protocolo_Autorizacion_Instalacion(req_protocolo_autorizacion_instalacion);


                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);

                        if (file != null && file.ContentLength > 0)
                        {
                            string ruta_pdf = _HabilitacionesService.Lista_tipo_autorizacion(cmb_tipo_autorizacion).First().ruta_pdf;
                            subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                        }

                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_licencia_operacion(string expediente, string id_seguimiento)
        {
            if (id_seguimiento != null && id_seguimiento != "")
            {
                Session["Habilitaciones_nuevo_protocolo_licencia_operacion_id_seguimiento"] = id_seguimiento;
                return RedirectToAction("Nuevo_Protocolo_licencia_operacion", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_licencia_operacion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_licencia_operacion_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_licencia_operacion_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    List<SelectListItem> lista_tipo_licencia_operacion = new List<SelectListItem>();

                    var var_lista_tipo_lo = _HabilitacionesService.Lista_tipo_licencia_operacion(0);

                    foreach (var result in var_lista_tipo_lo)
                    {
                        lista_tipo_licencia_operacion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_licencia_operacion.ToString()
                        }
                        );
                    };

                    ViewBag.lista_tipo_licencia_operacion = lista_tipo_licencia_operacion;

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.ruc_seg = rec_seg.ruc;
                    ViewBag.id_sede_ext = rec_seg.id_sede_ext.ToString();
                    ViewBag.nom_direccion_ext = rec_seg.nom_direccion_ext.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                    ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                    ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                    ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_licencia_operacion(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio, string txt_ruc_seg, string txt_resolucion,
            DateTime txt_fecha_resolucion, int txt_id_nombre_legal, int txt_id_sede_ext, int cmb_tipo_licencia_operacion, string txt_actividad)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = 1;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloLicenciaOperacionRequest req_protocolo_licencia_operacion = new ProtocoloLicenciaOperacionRequest();

                        req_protocolo_licencia_operacion.id_representante_legal = txt_id_nombre_legal;
                        req_protocolo_licencia_operacion.id_sede = txt_id_sede_ext;
                        req_protocolo_licencia_operacion.id_tipo_licencia_operacion = cmb_tipo_licencia_operacion;
                        req_protocolo_licencia_operacion.ruc = txt_ruc_seg;
                        req_protocolo_licencia_operacion.resolucion_autorizacion_instalacion = txt_resolucion;
                        req_protocolo_licencia_operacion.fecha_resolucion = txt_fecha_resolucion;
                        req_protocolo_licencia_operacion.actividad = txt_actividad;

                        req_protocolo_licencia_operacion.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_licencia_operacion.id_pro_licencia_operacion = _HabilitacionesService.Create_Protocolo_Licencia_Operacion(req_protocolo_licencia_operacion);

                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);

                        if (file != null && file.ContentLength > 0)
                        {
                            string ruta_pdf = _HabilitacionesService.Lista_tipo_licencia_operacion(cmb_tipo_licencia_operacion).First().ruta_pdf;
                            subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                        }

                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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





        [AllowAnonymous]
        public ActionResult Subir_var_Protocolo_transporte(int id)
        {
            if (id != null && id != 0)
            {
                Session["Habilitaciones_nuevo_archivo_protocolo_transporte"] = id;
                return RedirectToAction("Subir_Protocolo_transporte", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Subir_Protocolo_transporte()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_pro = 0;
                    try
                    {
                        var_id_pro = Convert.ToInt32(Session["Habilitaciones_nuevo_archivo_protocolo_transporte"].ToString());
                        Session.Remove("Habilitaciones_nuevo_archivo_protocolo_transporte");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }
                    ViewBag.id_protocolo = var_id_pro.ToString();
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


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Subir_Protocolo_transporte(HttpPostedFileBase file, int id_protocolo)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            ProtocoloRequest proto_res = new ProtocoloRequest();
                            proto_res = _HabilitacionesService.lista_protocolo_x_id(id_protocolo);
                            proto_res.id_est_pro = 4;
                            _HabilitacionesService.actualizar_protocolo(proto_res);
                            _HabilitacionesService.Insertar_actividad_estado_protocolo(4, id_protocolo);

                            subir_pdf(id_protocolo.ToString(), "habilitaciones/transporte", file);
                        }

                    }
                    catch (Exception)
                    {
                    }
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




        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_concesion(string expediente, string id_seguimiento)
        {
            if (id_seguimiento != null && id_seguimiento != "")
            {
                Session["Habilitaciones_nuevo_protocolo_concesion_id_seguimiento"] = id_seguimiento;
                return RedirectToAction("Nuevo_Protocolo_Concesion", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_Concesion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_concesion_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_concesion_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }


                    List<SelectListItem> lista_sexo = new List<SelectListItem>();

                    lista_sexo.Add(new SelectListItem() { Text = "MASCULINO", Value = "M" });
                    lista_sexo.Add(new SelectListItem() { Text = "FEMENINO", Value = "F" });

                    ViewBag.lst_combo_sexo = lista_sexo;

                    List<SelectListItem> Lista_tipo_doc_iden = new List<SelectListItem>();

                    var recupera_tipo_documento = _GeneralService.llenar_tipo_documento_identidad();

                    foreach (var result in recupera_tipo_documento)
                    {
                        Lista_tipo_doc_iden.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.tipo_doc_iden.ToString()
                        }
                        );
                    };

                    ViewBag.lst_combo_tipo_identidad = Lista_tipo_doc_iden;

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;



                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();
                    }
                    else
                    {
                        ViewBag.id_persona_legal = rec_seg.id_dni_persona_legal.ToString();
                    }

                    if (rec_seg.nom_persona_ext == "")
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                        ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;
                    }
                    else
                    {
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal_DNI;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal_DNI;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal_DNI;
                        ViewBag.Str_Direccion_Legal = rec_seg.str_direccion_persona_natural;
                    }

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;

                    ViewBag.Str_Persona = rec_seg.nom_persona_ext;
                    ViewBag.var_DNI = rec_seg.persona_num_documento;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_Concesion(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio, DateTime txt_fecha_fin, DateTime txt_fecha_resolucion,
            int? cmb_actividad, string txt_resolucion, decimal? txt_area_ha, decimal? txt_total_ha, decimal? txt_lote, decimal? txt_espejo_agua, decimal? txt_area, decimal? txt_capacidad_produccion,
            int txt_id_nombre_legal, int txt_id_direccion_legal, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add, string lbl_Direccion_legal)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloConcesionRequest req_protocolo_concesion = new ProtocoloConcesionRequest();

                        req_protocolo_concesion.representante_legal = txt_id_nombre_legal;
                        req_protocolo_concesion.direccion_legal = txt_id_direccion_legal;

                        SeguimientoDhcpaRequest seg_response = new SeguimientoDhcpaRequest();
                        seg_response = _HabilitacionesService.Recupera_seguimiento_x_id(txt_id_seguimiento).First();

                        if (seg_response.persona_num_documento != null && seg_response.persona_num_documento != "")
                        {
                            req_protocolo_concesion.representante_legal = 0;
                            req_protocolo_concesion.direccion_legal = 0;
                            req_protocolo_concesion.direccion_legal_dni = lbl_Direccion_legal;
                            req_protocolo_concesion.representante_legal_dni = txt_id_nombre_legal;
                        }
                        else
                        {
                            req_protocolo_concesion.representante_legal = txt_id_nombre_legal;
                            req_protocolo_concesion.direccion_legal = txt_id_direccion_legal;
                            req_protocolo_concesion.direccion_legal_dni = "";
                            req_protocolo_concesion.representante_legal_dni = 0;
                        }

                        req_protocolo_concesion.resolucion = txt_resolucion;
                        req_protocolo_concesion.fecha_resolucion = txt_fecha_resolucion;

                        req_protocolo_concesion.id_tip_act_conce = cmb_actividad;
                        req_protocolo_concesion.area_ha = txt_area_ha;
                        req_protocolo_concesion.total_ha = txt_total_ha;
                        req_protocolo_concesion.lote = txt_lote;
                        req_protocolo_concesion.espejo_agua = txt_espejo_agua;
                        req_protocolo_concesion.area = txt_area;
                        req_protocolo_concesion.capacidad_produccion = txt_capacidad_produccion;

                        req_protocolo_concesion.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_concesion.id_det_pro_conce = _HabilitacionesService.Create_Protocolo_Concesion(req_protocolo_concesion);

                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);


                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        if (file != null && file.ContentLength > 0)
                        {
                            int id_concesion = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento).id_habilitante ?? 0;

                            if (id_concesion != 0)
                            {
                                string ruta_pdf = _GeneralService.recupera_mae_concesion_x_id(id_concesion).ruta_pdf;
                                subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                            }
                        }

                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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

        public void subir_pdf(string ID_PROTOCOLO, string RUTA_PDF, HttpPostedFileBase file)
        {
            string RUTA_SERVER = ConfigurationManager.AppSettings["RUTA_FTP"].ToString();
            string USU_PDF_FTP = ConfigurationManager.AppSettings["USU_PDF_FTP"].ToString();
            string CONT_PDF_FTP = ConfigurationManager.AppSettings["CONTRA_PDF_FTP"].ToString();

            string total_ruta = RUTA_SERVER + RUTA_PDF + "/" + ID_PROTOCOLO + ".pdf";

            // se asigna la dirección ip o dominio a subir el archivo y ruta
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(total_ruta);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // FTP credenciales
            request.Credentials = new NetworkCredential(USU_PDF_FTP, CONT_PDF_FTP);

            //subir archivo
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            request.ContentLength = fileBytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileBytes, 0, fileBytes.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_almacen(string expediente, string id_seguimiento)
        {
            if (id_seguimiento != null && id_seguimiento != "")
            {
                Session["Habilitaciones_nuevo_protocolo_almacen_id_seguimiento"] = id_seguimiento;
                return RedirectToAction("Nuevo_Protocolo_Almacen", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_Almacen()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_almacen_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_almacen_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;


                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);
                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();
                    ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                    ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                    ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                    ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                    ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_Almacen(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio, DateTime txt_fecha_fin,
            int? cmb_conhum, string txt_licencia, int txt_id_nombre_legal, int txt_id_direccion_legal, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloAlmacenRequest req_protocolo_almacen = new ProtocoloAlmacenRequest();

                        req_protocolo_almacen.representante_legal = txt_id_nombre_legal;
                        req_protocolo_almacen.direccion_legal = txt_id_direccion_legal;
                        req_protocolo_almacen.licencia = txt_licencia;

                        req_protocolo_almacen.id_tipo_ch = cmb_conhum;
                        req_protocolo_almacen.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_almacen.id_dat_pro_almacen = _HabilitacionesService.Create_Protocolo_Almacen(req_protocolo_almacen);


                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);


                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        if (file != null && file.ContentLength > 0)
                        {

                            int id_almacen = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento).id_habilitante ?? 0;

                            if (id_almacen != 0)
                            {
                                string ruta_pdf = _GeneralService.recupera_almacen_x_id(id_almacen).ruta_pdf;
                                subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                            }
                        }



                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_desembarcadero(string id)
        {
            if (id != null && id != "")
            {
                Session["Habilitaciones_nuevo_protocolo_desembarcadero_id_seguimiento"] = id;
                return RedirectToAction("Nuevo_Protocolo_Desembarcadero", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_Desembarcadero()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_desembarcadero_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_desembarcadero_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;


                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();
                    ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                    ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                    ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                    ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;
                    ViewBag.Str_Correo_Legal = rec_seg.correo_legal;

                    ViewBag.id_seguimiento = var_id_ses.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_Desembarcadero(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio, DateTime txt_fecha_fin, string txt_derecho_uso_area_acuatica,
            int txt_id_nombre_legal, int txt_id_direccion_legal, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {

                        SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                        rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento);

                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloDesembarcaderoRequest req_protocolo_desembarcadero = new ProtocoloDesembarcaderoRequest();

                        req_protocolo_desembarcadero.representante_legal = txt_id_nombre_legal;
                        req_protocolo_desembarcadero.direccion_legal = txt_id_direccion_legal;
                        req_protocolo_desembarcadero.derecho_uso_area_acuatica = txt_derecho_uso_area_acuatica;
                        req_protocolo_desembarcadero.id_desembarcadero = rec_seg.id_habilitante;
                        req_protocolo_desembarcadero.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_desembarcadero.id_det_pro_desemb = _HabilitacionesService.Create_Protocolo_Desembarcadero(req_protocolo_desembarcadero);

                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);


                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        if (file != null && file.ContentLength > 0)
                        {
                            if (rec_seg.id_habilitante != 0)
                            {
                                string ruta_pdf = _GeneralService.recupera_tipo_desembarcadero_x_id_desembarcadero(rec_seg.id_habilitante ?? 0).ruta_pdf;
                                subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                            }
                        }


                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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


        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo_embarcacion(string id)
        {
            if (id != null && id != "")
            {
                Session["Habilitaciones_nuevo_protocolo_id_seguimiento"] = id;
                return RedirectToAction("Nuevo_Protocolo_Embarcacion", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo_Embarcacion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }


                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;


                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    List<SelectListItem> lista_tipo_proto_emb = new List<SelectListItem>();

                    var var_lista_protocolo_emb = _GeneralService.Lista_tipo_protocolo_emb();

                    foreach (var result in var_lista_protocolo_emb)
                    {
                        lista_tipo_proto_emb.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tip_pro_emb.ToString()
                        }
                        );
                    };

                    if (rec_seg.id_ofi_dir != null && rec_seg.id_ofi_dir != 0)
                    {
                        ViewBag.var_RUC = rec_seg.ruc.ToString();
                        ViewBag.id_condicion_seguimiento = "1";
                        ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();
                        ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();
                        ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                        ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                        ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                        ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;
                        ViewBag.Str_Correo_Legal = rec_seg.correo_legal;

                        ViewBag.Str_Direccion_persona_natural = "";
                        ViewBag.Str_Persona_natural = "";
                        ViewBag.id_persona_natural_telefono = "";
                        ViewBag.var_lbl_telefono_Persona_natural = "";
                    }
                    else
                    {
                        ViewBag.id_condicion_seguimiento = "0";
                        ViewBag.id_direccion_legal = "0";
                        ViewBag.id_persona_legal = "0";
                        ViewBag.Str_Empresa = rec_seg.nom_persona_ext;
                        ViewBag.Str_Telefono_Legal = "";
                        ViewBag.Str_Persona_Legal = "";
                        ViewBag.Str_Direccion_Legal = "";
                        ViewBag.Str_Correo_Legal = "";

                        ViewBag.Str_Direccion_persona_natural = rec_seg.str_direccion_persona_natural;
                        ViewBag.Str_Persona_natural = rec_seg.nom_persona_ext;

                        IEnumerable<ConsultarPersonaTelefonoResponse> per_telef = new List<ConsultarPersonaTelefonoResponse>();
                        per_telef = _HabilitacionesService.consulta_persona_natural_telefono(rec_seg.persona_num_documento);

                        if (per_telef.Count() > 0)
                        {
                            ViewBag.id_persona_natural_telefono = per_telef.First().id_persona_telefono.ToString();
                            ViewBag.var_lbl_telefono_Persona_natural = per_telef.First().telefono1;
                        }
                        else
                        {
                            ViewBag.id_persona_natural_telefono = "0";
                            ViewBag.var_lbl_telefono_Persona_natural = "";
                        }
                    }

                    ViewBag.lst_tipo_protocolo_emb = lista_tipo_proto_emb;
                    ViewBag.id_seguimiento = var_id_ses.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo_Embarcacion(HttpPostedFileBase file, int txt_id_seguimiento, string txt_nombre, DateTime txt_fecha_inicio,
            string txt_resolucion, int txt_id_nombre_legal, int txt_id_direccion_legal, int cmbtipo_protocolo_emb, int cmb_lst_indicadorprotocoloespecie, string txt_especie_add, string txt_id_condicion,
            string lbl_Direccion_persona_natural, string txt_id_persona_natural_telefono, string txt_telefono_persona_natural)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {

                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloEmbarcacionRequest req_protocolo_embarcacion = new ProtocoloEmbarcacionRequest();

                        req_protocolo_embarcacion.resolucion = txt_resolucion;
                        req_protocolo_embarcacion.nom_embarcacion = _GeneralService.buscar_embarcacion_x_seguimiento(txt_id_seguimiento).nombre;
                        req_protocolo_embarcacion.id_tip_pro_emb = cmbtipo_protocolo_emb;

                        req_protocolo_embarcacion.id_protocolo = req_protocolo.id_protocolo;

                        if (txt_id_condicion == "1")
                        {
                            req_protocolo_embarcacion.representante_legal = txt_id_nombre_legal;
                            req_protocolo_embarcacion.direccion_legal = txt_id_direccion_legal;
                            req_protocolo_embarcacion.id_det_pro_hab = _HabilitacionesService.Create_Protocolo_Embarcacion(req_protocolo_embarcacion);
                        }
                        else
                        {
                            if (txt_id_persona_natural_telefono == "0")
                            {
                                if (txt_telefono_persona_natural.Trim() != "")
                                {
                                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento);
                                    req_protocolo_embarcacion.id_persona_telefono = _HabilitacionesService.Create_Persona_telefono(rec_seg.persona_num_documento, txt_telefono_persona_natural, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                                }
                            }
                            else
                            {
                                req_protocolo_embarcacion.id_persona_telefono = Convert.ToInt32(txt_id_persona_natural_telefono);
                            }
                            req_protocolo_embarcacion.direccion_persona_natural = lbl_Direccion_persona_natural;
                            req_protocolo_embarcacion.id_det_pro_hab = _HabilitacionesService.Create_Protocolo_Embarcacion(req_protocolo_embarcacion);
                        }

                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);

                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }

                        if (file != null && file.ContentLength > 0)
                        {

                            int var_id_embarcacion = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento).id_habilitante ?? 0;

                            if (var_id_embarcacion != 0)
                            {
                                int var_id_tipo_embarcacion = _HabilitacionesService.Recupera_Embarcacion(txt_id_seguimiento, 0).id_tipo_embarcacion ?? 0;
                                string ruta_pdf = _GeneralService.recupera_tipo_embarcacion(var_id_tipo_embarcacion).First().ruta_ftp;
                                subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                            }
                        }

                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult variable_Nuevo_Protocolo(string id)
        {
            if (id != null && id != "")
            {
                Session["Habilitaciones_nuevo_protocolo_id_seguimiento"] = id;
                return RedirectToAction("Nuevo_Protocolo", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Protocolo()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int var_id_ses = 0;
                    try
                    {
                        var_id_ses = Convert.ToInt32(Session["Habilitaciones_nuevo_protocolo_id_seguimiento"].ToString());
                        Session.Remove("Habilitaciones_nuevo_protocolo_id_seguimiento");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();

                    Lista_departamento.Add(new SelectListItem() { Text = "SELECCIONAR DEPARTAMENTO", Value = "" });
                    Lista_provincia.Add(new SelectListItem() { Text = "SELECCIONAR PROVINCIA", Value = "" });
                    Lista_distrito.Add(new SelectListItem() { Text = "SELECCIONAR DISTRITO", Value = "" });

                    var recupera_departamento = _GeneralService.llenar_departamento();

                    foreach (var result in recupera_departamento)
                    {
                        Lista_departamento.Add(new SelectListItem()
                        {
                            Text = result.departamento,
                            Value = result.codigo_departamento.ToString()
                        }
                        );
                    };
                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;

                    List<SelectListItem> lista_indicador_especie = new List<SelectListItem>();

                    var var_lista_indicador_especie = _HabilitacionesService.Lista_indicadorprotocoloespecie();

                    foreach (var result in var_lista_indicador_especie)
                    {
                        lista_indicador_especie.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_ind_pro_esp.ToString()
                        }
                        );
                    };

                    ViewBag.lst_indicador_especie = lista_indicador_especie;

                    SeguimientoDhcpaResponse rec_seg = new SeguimientoDhcpaResponse();
                    rec_seg = _HabilitacionesService.GetAllSeguimiento_x_id(var_id_ses);

                    ViewBag.var_RUC = rec_seg.ruc.ToString();
                    ViewBag.id_seguimiento = var_id_ses.ToString();
                    ViewBag.id_direccion_legal = rec_seg.id_direccion_legal.ToString();
                    ViewBag.id_persona_legal = rec_seg.id_persona_legal.ToString();

                    ViewBag.Str_Expediente = rec_seg.Expediente + "." + rec_seg.nom_tipo_expediente;
                    ViewBag.Str_Empresa = rec_seg.nom_oficina_ext;
                    ViewBag.Str_Correo_Legal = rec_seg.correo_legal;
                    ViewBag.Str_Telefono_Legal = rec_seg.telefono_legal;
                    ViewBag.Str_Persona_Legal = rec_seg.Nom_persona_legal;
                    ViewBag.Str_Direccion_Legal = rec_seg.Nom_direccion_legal;

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
                    ProtocoloViewModel model_protocolo = new ProtocoloViewModel();

                    return View(model_protocolo);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Protocolo(HttpPostedFileBase file, HttpPostedFileBase file2, int txt_id_seguimiento, string txt_hid_conch_abanico, string txt_hid_otros, string txt_hid_peces, string txt_hid_crusta,
            string txt_nombre, string txt_nombre_segundo_protocolo, DateTime txt_fecha_inicio, DateTime txt_fecha_fin, int? cmb_conhum, string txt_licencia_operacion, int txt_id_nombre_legal, int txt_id_direccion_legal,
            int cmb_lst_indicadorprotocoloespecie, string txt_especie_add)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[15].Trim() == "1" // Acceso a Nuevo Protocolo
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    try
                    {
                        ProtocoloRequest req_protocolo = new ProtocoloRequest();
                        req_protocolo.id_seguimiento = txt_id_seguimiento;
                        req_protocolo.nombre = txt_nombre;
                        req_protocolo.fecha_inicio = txt_fecha_inicio;
                        req_protocolo.fecha_fin = txt_fecha_fin;
                        req_protocolo.activo = "1";
                        req_protocolo.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_protocolo.fecha_registro = DateTime.Now;
                        req_protocolo.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                        req_protocolo.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo);

                        ProtocoloPlantaRequest req_protocolo_planta = new ProtocoloPlantaRequest();

                        req_protocolo_planta.representante_legal = txt_id_nombre_legal;
                        req_protocolo_planta.direccion_legal = txt_id_direccion_legal;
                        req_protocolo_planta.licencia_operacion = txt_licencia_operacion;

                        req_protocolo_planta.ind_concha_abanico = txt_hid_conch_abanico;
                        req_protocolo_planta.ind_crustaceos = txt_hid_crusta;
                        req_protocolo_planta.ind_otros = txt_hid_otros;
                        req_protocolo_planta.ind_peces = txt_hid_peces;
                        req_protocolo_planta.id_tipo_ch = cmb_conhum;
                        req_protocolo_planta.activo = "1";
                        req_protocolo_planta.id_protocolo = req_protocolo.id_protocolo;

                        req_protocolo_planta.id_dat_pro_pla = _HabilitacionesService.Create_Protocolo_Planta(req_protocolo_planta);

                        int var_id_planta = _HabilitacionesService.Recupera_Planta(txt_id_seguimiento, 0).id_planta;

                        _HabilitacionesService.Actualiza_habilitacion_planta(txt_fecha_fin, var_id_planta);

                        ProtocoloRequest proto_res = new ProtocoloRequest();
                        proto_res = _HabilitacionesService.lista_protocolo_x_id(req_protocolo.id_protocolo);
                        proto_res.id_est_pro = 4;
                        _HabilitacionesService.actualizar_protocolo(proto_res);
                        _HabilitacionesService.Insertar_actividad_estado_protocolo(4, proto_res.id_protocolo);

                        if (txt_especie_add != "")
                        {
                            var esp_add = txt_especie_add.Split('|');
                            foreach (var result in esp_add)
                            {
                                ProtocoloEspecieRequest rea_protocolo_especie = new ProtocoloEspecieRequest();
                                rea_protocolo_especie.activo = "1";
                                rea_protocolo_especie.id_det_espec_hab = Convert.ToInt32(result);
                                rea_protocolo_especie.id_protocolo = req_protocolo.id_protocolo;
                                rea_protocolo_especie.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie);
                            }
                        }
                        int id_planta = _HabilitacionesService.GetAllSeguimiento_x_id(txt_id_seguimiento).id_habilitante ?? 0;
                        int id_tipo_actividad = 0;
                        string ruta_pdf = "";

                        if (id_planta != 0)
                        {
                            id_tipo_actividad = _GeneralService.recupera_planta_x_id(id_planta).id_tipo_actividad ?? 0;
                            ruta_pdf = _GeneralService.recupera_toda_tipo_actividad_planta_x_id(id_tipo_actividad).ruta_ftp;
                        }

                        if (file != null && file.ContentLength > 0)
                        {
                            if (id_planta != 0)
                            {
                                subir_pdf(req_protocolo.id_protocolo.ToString(), ruta_pdf, file);
                            }
                        }

                        if (file2 != null && file2.ContentLength > 0 && txt_nombre_segundo_protocolo.Trim() != "")
                        {
                            ProtocoloRequest req_protocolo2 = new ProtocoloRequest();
                            req_protocolo2.id_seguimiento = txt_id_seguimiento;
                            req_protocolo2.nombre = txt_nombre_segundo_protocolo;
                            req_protocolo2.fecha_inicio = txt_fecha_inicio;
                            req_protocolo2.fecha_fin = txt_fecha_fin;
                            req_protocolo2.activo = "1";
                            req_protocolo2.evaluador = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            req_protocolo2.fecha_registro = DateTime.Now;
                            req_protocolo2.id_ind_pro_esp = cmb_lst_indicadorprotocoloespecie;

                            req_protocolo2.id_protocolo = _HabilitacionesService.Create_Protocolo(req_protocolo2);

                            ProtocoloPlantaRequest req_protocolo_planta2 = new ProtocoloPlantaRequest();

                            req_protocolo_planta2.representante_legal = txt_id_nombre_legal;
                            req_protocolo_planta2.direccion_legal = txt_id_direccion_legal;
                            req_protocolo_planta2.licencia_operacion = txt_licencia_operacion;

                            req_protocolo_planta2.ind_concha_abanico = txt_hid_conch_abanico;
                            req_protocolo_planta2.ind_crustaceos = txt_hid_crusta;
                            req_protocolo_planta2.ind_otros = txt_hid_otros;
                            req_protocolo_planta2.ind_peces = txt_hid_peces;
                            req_protocolo_planta2.id_tipo_ch = cmb_conhum;
                            req_protocolo_planta2.activo = "1";
                            req_protocolo_planta2.id_protocolo = req_protocolo2.id_protocolo;

                            req_protocolo_planta2.id_dat_pro_pla = _HabilitacionesService.Create_Protocolo_Planta(req_protocolo_planta2);


                            ProtocoloRequest proto_res2 = new ProtocoloRequest();
                            proto_res2 = _HabilitacionesService.lista_protocolo_x_id(req_protocolo2.id_protocolo);
                            proto_res2.id_est_pro = 4;
                            _HabilitacionesService.actualizar_protocolo(proto_res2);
                            _HabilitacionesService.Insertar_actividad_estado_protocolo(4, req_protocolo2.id_protocolo);


                            if (txt_especie_add != "")
                            {
                                var esp_add = txt_especie_add.Split('|');
                                foreach (var result in esp_add)
                                {
                                    ProtocoloEspecieRequest rea_protocolo_especie2 = new ProtocoloEspecieRequest();
                                    rea_protocolo_especie2.activo = "1";
                                    rea_protocolo_especie2.id_det_espec_hab = Convert.ToInt32(result);
                                    rea_protocolo_especie2.id_protocolo = req_protocolo2.id_protocolo;
                                    rea_protocolo_especie2.id_pro_espe = _HabilitacionesService.Create_Protocolo_Especie(rea_protocolo_especie2);
                                }
                            }

                            if (id_planta != 0)
                            {
                                subir_pdf(req_protocolo2.id_protocolo.ToString(), ruta_pdf, file2);
                            }
                        }



                        @ViewBag.Mensaje = "Se guardo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult Consulta_seguimiento(int page = 1, string expediente = "", string externo = "", string habilitante = "", string cmbestado = "", int cmbtupa = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "15" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "52"))))
                // Oficina 28: Atención al usuario
                {
                    List<SelectListItem> Lista_estado_seguimiento_dhcpa = new List<SelectListItem>();

                    Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });

                    var recupera_estado_seguimiento = _HabilitacionesService.Lista_estado_seguimiento_dhcpa();
                    foreach (var result in recupera_estado_seguimiento)
                    {
                        if (result.id_estado != "4")
                        {
                            Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_estado
                            });
                        }
                    };

                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_estado = Lista_estado_seguimiento_dhcpa;
                    ViewBag.lst_tupa = Lista_TUPA;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Fecha Inicio");
                    tbl.Columns.Add("Expediente");
                    tbl.Columns.Add("TUPA/SERV");
                    tbl.Columns.Add("Procedimiento");
                    tbl.Columns.Add("Externo");
                    tbl.Columns.Add("Habilitante");
                    tbl.Columns.Add("Evaluador");
                    tbl.Columns.Add("Estado");
                    tbl.Columns.Add("Expediente_Id_seguimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_cond_finalizar");

                    var seguimiento = _HabilitacionesService.GetAllSeguimiento_Consulta_sin_paginado(expediente, "", externo, habilitante, cmbestado, 0, cmbtupa);

                    foreach (var result in seguimiento)
                    {
                        if (result.num_tupa == null)
                        {
                            tbl.Rows.Add(

                                result.fecha_inicio,
                                result.Expediente,
                                "",
                                result.nom_tipo_procedimiento,
                                result.nom_oficina_ext,
                                result.cod_habilitante,
                                result.nom_evaluador,
                                result.nom_estado,
                                result.Expediente + "|" + result.id_seguimiento.ToString(),
                                result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                );
                        }
                        else
                        {
                            tbl.Rows.Add(

                                result.fecha_inicio,
                                result.Expediente,
                                result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                result.nom_tipo_procedimiento,
                                result.nom_oficina_ext,
                                result.cod_habilitante,
                                result.nom_evaluador,
                                result.nom_estado,
                                result.Expediente + "|" + result.id_seguimiento.ToString(),
                                result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                );
                        }
                    };

                    ViewData["Seguimiento_Tabla"] = tbl;

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

        [AllowAnonymous]
        public ActionResult Documentos_por_recibir_sdhpa(int page = 1, string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "", string expediente = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    /*
                    ESTADO : '0' POR RECIBIR SDHPA, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                   */

                    /*
                     * INICIAL: INDICADOR = 1
                     * SECUNDARIO: INDICADOR = 2
                    */


                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR TIPO DOCUMENTO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                    {
                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_documento.ToString()
                        });
                    };

                    if (cmbtipo_documento == "0")
                    {
                        cmbtipo_documento = "";
                    }

                    ViewBag.lst_tipo_documento = lista_documentos;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DOCUMENTO_SEG");
                    tbl.Columns.Add("HABILITANTE");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("NOM_DOCUMENTO");
                    tbl.Columns.Add("NOM_EXTERNO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("GROUP_EXPEDIENTE");
                    tbl.Columns.Add("VER_PDF");

                    var documento = _HabilitacionesService.GetAllDocumentos_x_rec("0", "", "", asunto, externo, cmbtipo_documento, num_documento, nom_documento, 0, expediente);

                    foreach (var result in documento)
                    {
                        if (result.ruta_pdf == "" || result.ruta_pdf == null)
                        {
                            tbl.Rows.Add(
                            result.id_documento_seg,
                            result.documento_codigo_habilitacion,
                            result.fecha_crea,
                            result.nom_documento,
                            result.nom_externo,
                            result.asunto,
                            result.group_expedientes, false);
                        }
                        else
                        {
                            tbl.Rows.Add(
                            result.id_documento_seg,
                            result.documento_codigo_habilitacion,
                            result.fecha_crea,
                            result.nom_documento,
                            result.nom_externo,
                            result.asunto,
                            result.group_expedientes, true);
                        }

                    };

                    ViewData["Documento_Seg_Tabla"] = tbl;

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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Doc_Por_Recibir_habilitaciones(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    int id_documento_seg = 0;
                    for (int i = 0; i < id.Split('|').Count(); i++)
                    {
                        id_documento_seg = Convert.ToInt32(id.Split('|')[i].Trim());
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(id_documento_seg);
                        doc_seg_req.fecha_recepcion_sdhpa = DateTime.Now;
                        doc_seg_req.usuario_recepcion_sdhpa = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                        doc_seg_req.estado = "1";
                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);

                        if (doc_seg_req.indicador == "1")
                        {
                            foreach (var res_det_seg in _HabilitacionesService.GetAllDet_seg_doc(id_documento_seg))
                            {
                                SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                                req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(res_det_seg.id_seguimiento);
                                //ESTADO '0' POR RECIBIR SDHPA, '1' RECIBIDO SDHPA, '2' EN PROCESO, '3' FINALIZADO
                                req_seg_dhcpa.estado = "1";
                                _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);
                            }
                        }
                    }
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult Doc_Asignar_Evaluador(int page = 1, string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "", string expediente = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {

                    /*
                     ESTADO : '0' POR RECIBIR SDHPA, '1' RECIBIDO_SDHPA, '2' RECIBIDO_EVALUADOR
                    */

                    /*
                     * INICIAL: INDICADOR = 1
                     * SECUNDARIO: INDICADOR = 2
                    */

                    List<SelectListItem> Lista_Evaluador = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(18);
                    foreach (var result in recupera_persona)
                    {
                        Lista_Evaluador.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };



                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR TIPO DOCUMENTO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                    {
                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_documento.ToString()
                        });
                    };

                    if (cmbtipo_documento == "0")
                    {
                        cmbtipo_documento = "";
                    }

                    ViewBag.lst_tipo_documento = lista_documentos;

                    ViewBag.lst_evaluador = Lista_Evaluador;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DOCUMENTO_SEG");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("NOM_DOCUMENTO");
                    tbl.Columns.Add("NOM_EXTERNO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EVALUADOR");
                    tbl.Columns.Add("GROUP_EXPEDIENTE");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("NOM_OFI_CREA");
                    tbl.Columns.Add("NOM_USU_CREA");

                    var documento = _HabilitacionesService.GetAllDocumentos("1", "", "", asunto, externo, cmbtipo_documento, num_documento, nom_documento, 0, expediente);

                    foreach (var result in documento)
                    {
                        if (result.ruta_pdf == "" || result.ruta_pdf == null)
                        {
                            tbl.Rows.Add(
                                result.id_documento_seg,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.asunto,
                                result.evaluador,
                                result.group_expedientes,
                                false,
                                result.nom_ofi_crea,
                                result.usu_crea
                                );
                        }
                        else
                        {
                            tbl.Rows.Add(
                                result.id_documento_seg,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.asunto,
                                result.evaluador,
                                result.group_expedientes,
                                true,
                                result.nom_ofi_crea,
                                result.usu_crea
                                );
                        }
                    };

                    ViewData["Documento_Seg_Tabla"] = tbl;


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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Asignar_Evaluador(int id_documento, string evaluador)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                    doc_seg_req = _HabilitacionesService.GetAllDocumento_req(id_documento);
                    doc_seg_req.evaluador = evaluador;
                    doc_seg_req.fecha_asignacion_evaluador = DateTime.Now;
                    bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);

                    foreach (var res_det_seg in _HabilitacionesService.GetAllDet_seg_doc(id_documento))
                    {
                        SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                        req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(res_det_seg.id_seguimiento);

                        if (doc_seg_req.indicador == "1")
                        {
                            req_seg_dhcpa.evaluador = evaluador;
                            _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                            var result = _HabilitacionesService.GetAlldet_seg_evaluador(res_det_seg.id_seguimiento);

                            if (result.Count() != 0)
                            {
                                DetSegEvaluadorRequest det_seg_ev_req = new DetSegEvaluadorRequest();
                                det_seg_ev_req.id_det_exp_eva = result.First().id_det_exp_eva;
                                det_seg_ev_req.id_seguimiento = result.First().id_seguimiento;
                                det_seg_ev_req.evaluador = evaluador;
                                det_seg_ev_req.indicador = "1";
                                _HabilitacionesService.Update_det_seg_evalua(det_seg_ev_req);
                            }
                            else
                            {
                                DetSegEvaluadorRequest det_seg_ev = new DetSegEvaluadorRequest();
                                det_seg_ev.id_seguimiento = res_det_seg.id_seguimiento;
                                det_seg_ev.evaluador = evaluador;
                                det_seg_ev.indicador = "1";
                                _HabilitacionesService.Create_det_seg_evaluador(det_seg_ev);

                            }
                        }
                        else
                        {
                            if (req_seg_dhcpa.evaluador != evaluador)
                            {
                                req_seg_dhcpa.evaluador = evaluador;
                                _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                                var result = _HabilitacionesService.GetAlldet_seg_evaluador(res_det_seg.id_seguimiento);

                                DetSegEvaluadorRequest det_seg_ev_req = new DetSegEvaluadorRequest();
                                det_seg_ev_req.id_det_exp_eva = result.First().id_det_exp_eva;
                                det_seg_ev_req.id_seguimiento = result.First().id_seguimiento;
                                det_seg_ev_req.evaluador = result.First().evaluador;
                                det_seg_ev_req.indicador = "0";
                                det_seg_ev_req.fecha_recibido = result.First().fecha_recibido;
                                det_seg_ev_req.fecha_derivado = result.First().fecha_derivado;
                                _HabilitacionesService.Update_det_seg_evalua(det_seg_ev_req);

                                DetSegEvaluadorRequest det_seg_ev = new DetSegEvaluadorRequest();
                                det_seg_ev.id_seguimiento = res_det_seg.id_seguimiento;
                                det_seg_ev.evaluador = evaluador;
                                det_seg_ev.indicador = "1";
                                _HabilitacionesService.Create_det_seg_evaluador(det_seg_ev);

                            }
                        }
                    }

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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
        public ActionResult Asignar_Evaluador_seguimiento(int id_seguimiento, string evaluador)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                    req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(id_seguimiento);

                    if (req_seg_dhcpa.evaluador != evaluador)
                    {
                        req_seg_dhcpa.evaluador = evaluador;
                        _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                        var result = _HabilitacionesService.GetAlldet_seg_evaluador(id_seguimiento);

                        DetSegEvaluadorRequest det_seg_ev_req = new DetSegEvaluadorRequest();
                        det_seg_ev_req.id_det_exp_eva = result.First().id_det_exp_eva;
                        det_seg_ev_req.id_seguimiento = result.First().id_seguimiento;
                        det_seg_ev_req.evaluador = result.First().evaluador;
                        det_seg_ev_req.indicador = "0";
                        det_seg_ev_req.fecha_recibido = result.First().fecha_recibido;
                        det_seg_ev_req.fecha_derivado = result.First().fecha_derivado;
                        _HabilitacionesService.Update_det_seg_evalua(det_seg_ev_req);

                        DetSegEvaluadorRequest det_seg_ev = new DetSegEvaluadorRequest();
                        det_seg_ev.id_seguimiento = id_seguimiento;
                        det_seg_ev.evaluador = evaluador;
                        det_seg_ev.indicador = "1";
                        det_seg_ev.fecha_recibido = DateTime.Now;
                        _HabilitacionesService.Create_det_seg_evaluador(det_seg_ev);

                    }

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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
        public ActionResult Asignar_Expediente_seguimiento(int id_seguimiento, int id_expediente)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones
                {
                    SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                    req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(id_seguimiento);
                    req_seg_dhcpa.id_expediente = id_expediente;
                    _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                    ExpedientesRequest req_exp = new ExpedientesRequest();
                    req_exp = _HabilitacionesService.GetExpediente(id_expediente);
                    req_exp.indicador_seguimiento = "1"; // con seguimiento
                    _HabilitacionesService.Update_mae_expediente(req_exp);

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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

        [AllowAnonymous]
        public ActionResult Consulta_seguimiento_evaluador(int page = 1, string expediente = "", string externo = "", string habilitante = "", string cmbestado = "", int cmbtupa = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[13].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones Pesqueras y Acuicolas
                {

                    List<SelectListItem> Lista_estado_seguimiento_dhcpa = new List<SelectListItem>();
                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();

                    Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });

                    var recupera_estado_seguimiento = _HabilitacionesService.Lista_estado_seguimiento_dhcpa();
                    foreach (var result in recupera_estado_seguimiento)
                    {
                        if (result.id_estado == "2" || result.id_estado == "3")
                        {
                            Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_estado
                            });
                        }
                    };


                    List<SelectListItem> Lista_tipo_servicio_habilitaciones = new List<SelectListItem>();

                    var recupera_tipo_servicio_hab = _HabilitacionesService.Lista_tipo_servicio_habilitaciones();
                    foreach (var result in recupera_tipo_servicio_hab)
                    {
                        Lista_tipo_servicio_habilitaciones.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_ser_hab.ToString()
                            });
                    };

                    ViewBag.lst_si_tipo_servicio_habilitaciones = Lista_tipo_servicio_habilitaciones;

                    List<SelectListItem> Lista_Filial_sol_ins = new List<SelectListItem>();

                    var recupera_filial = _HabilitacionesService.GetAll_Filial();
                    foreach (var result in recupera_filial)
                    {
                        if (result.sol_insp == 1)
                        {
                            Lista_Filial_sol_ins.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_filial.ToString()
                            });
                        }
                    };

                    ViewBag.lst_si_filial = Lista_Filial_sol_ins;

                    List<SelectListItem> lista_combo = new List<SelectListItem>();

                    lista_combo.Add(new SelectListItem()
                    {
                        Text = "SELECCIONA",
                        Value = ""
                    });

                    List<SelectListItem> Lista_manuales = new List<SelectListItem>();

                    Lista_manuales.Add(new SelectListItem() { Text = "SI", Value = "1" });
                    Lista_manuales.Add(new SelectListItem() { Text = "NO", Value = "0" });

                    ViewBag.lst_si_manuales = Lista_manuales;

                    List<SelectListItem> Lista_destinos = new List<SelectListItem>();

                    foreach (var result in _GeneralService.recupera_destino_si())
                    {
                        Lista_destinos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_dest_sol_ins.ToString()
                        });
                    };

                    ViewBag.lst_si_destino = Lista_destinos;


                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_TUPA;
                    ViewBag.lst_embarcacion = Lista_embarcaciones;
                    ViewBag.lst_estado = Lista_estado_seguimiento_dhcpa;
                    ViewBag.lst_combo = lista_combo;

                    List<SelectListItem> Lista_carroceria = new List<SelectListItem>();
                    List<SelectListItem> Lista_unidad_medida = new List<SelectListItem>();
                    List<SelectListItem> Lista_furgon = new List<SelectListItem>();

                    var recupera_carroceria = _HabilitacionesService.consulta_todo_activo_tipocarroceria();
                    int entra = 0;
                    foreach (var result in recupera_carroceria)
                    {
                        if (entra == 0)
                        {
                            var recupera_furgon = _HabilitacionesService.consulta_todo_activo_tipofurgon(result.id_tipo_carroceria);

                            foreach (var result2 in recupera_furgon)
                            {
                                Lista_furgon.Add(new SelectListItem()
                                {
                                    Text = result2.nombre,
                                    Value = result2.id_tipo_furgon.ToString()
                                }
                                );
                            };
                            entra = 1;
                        }
                        Lista_carroceria.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_carroceria.ToString()
                        }
                        );
                    };

                    ViewBag.lst_tipo_furgon = Lista_furgon;

                    var recupera_um = _HabilitacionesService.consulta_todo_activo_unidad_medida();

                    foreach (var result in recupera_um)
                    {
                        Lista_unidad_medida.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.id_um.ToString()
                        }
                        );
                    };

                    ViewBag.lst_nuevo_carroceria = Lista_carroceria;
                    ViewBag.lst_nuevo_um = Lista_unidad_medida;

                    /*
                    IEnumerable<SeguimientoDhcpaResponse> model = new List<SeguimientoDhcpaResponse>();
                    ViewBag.TotalRows = _HabilitacionesService.CountSeguimiento_Consulta(expediente, HttpContext.User.Identity.Name.Split('|')[1].Trim(),externo,matricula,cmbestado);
                    model = _HabilitacionesService.GetAllSeguimiento_Consulta(page, 10, expediente, HttpContext.User.Identity.Name.Split('|')[1].Trim(), externo, matricula, cmbestado);
                    */

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Fecha Inicio");
                    tbl.Columns.Add("Expediente");
                    tbl.Columns.Add("TUPA/SERV");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("Procedimiento");
                    tbl.Columns.Add("Externo");
                    tbl.Columns.Add("Habilitante");
                    tbl.Columns.Add("Evaluador");
                    tbl.Columns.Add("Estado");
                    tbl.Columns.Add("Expediente_Id_seguimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_Id_ofi_dir");
                    tbl.Columns.Add("Expediente_Id_seguimiento_documento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_duracion_sdhpa_duracion_tramite_observacion");
                    tbl.Columns.Add("cond_expediente");
                    /*
                    tbl.Columns.Add("cond_planta");
                    tbl.Columns.Add("cond_embarcacion");
                     * */
                    tbl.Columns.Add("id_tipo_seguimiento");
                    tbl.Columns.Add("cond_habilitante");

                    tbl.Columns.Add("cond_finalizar");
                    tbl.Columns.Add("Id_seguimiento");
                    tbl.Columns.Add("Id_expediente");
                    tbl.Columns.Add("Id_procedimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_cond_finalizar");

                    var seguimiento = _HabilitacionesService.GetAllSeguimiento_Consulta_sin_paginado(expediente, HttpContext.User.Identity.Name.Split('|')[1].Trim(), externo, habilitante, cmbestado, 0, cmbtupa);

                    foreach (var result in seguimiento)
                    {
                        if (result.num_tupa == null)
                        {
                            if (result.persona_num_documento == null)
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    "",
                                    "",
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.ruc,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),/*
                            result.cond_planta.ToString(),
                            result.cond_embarcacion.ToString(),*/

                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),

                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                            else
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    "",
                                    "",
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.persona_num_documento,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),/*
                            result.cond_planta.ToString(),
                            result.cond_embarcacion.ToString(),*/

                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),

                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                        }
                        else
                        {
                            if (result.persona_num_documento == null)
                            {

                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                    result.num_tupa,
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.ruc,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),/*
                            result.cond_planta.ToString(),
                            result.cond_embarcacion.ToString(),*/

                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),

                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                            else
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                    result.num_tupa,
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.persona_num_documento,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),/*
                            result.cond_planta.ToString(),
                            result.cond_embarcacion.ToString(),*/

                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),

                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );

                            }
                        }

                    };

                    ViewData["Seguimiento_Tabla"] = tbl;

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

        public ActionResult Imprimir_masivo_evaluador(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    ViewBag.id = id;
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Asignar_masivo_evaluador(string id, string evaluador)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string[] id_doc_str = id.Split(',');
                    foreach (var id_doc in id_doc_str)
                    {
                        int id_documento = Convert.ToInt32(id_doc);
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(id_documento);
                        doc_seg_req.evaluador = evaluador;
                        doc_seg_req.fecha_asignacion_evaluador = DateTime.Now;
                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);

                        foreach (var res_det_seg in _HabilitacionesService.GetAllDet_seg_doc(id_documento))
                        {
                            SeguimientoDhcpaRequest req_seg_dhcpa = new SeguimientoDhcpaRequest();
                            req_seg_dhcpa = _HabilitacionesService.recupera_todo_seguimiento_dhcpa(res_det_seg.id_seguimiento);

                            if (doc_seg_req.indicador == "1")
                            {
                                req_seg_dhcpa.evaluador = evaluador;
                                _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                                var result = _HabilitacionesService.GetAlldet_seg_evaluador(res_det_seg.id_seguimiento);

                                if (result.Count() != 0)
                                {
                                    DetSegEvaluadorRequest det_seg_ev_req = new DetSegEvaluadorRequest();
                                    det_seg_ev_req.id_det_exp_eva = result.First().id_det_exp_eva;
                                    det_seg_ev_req.id_seguimiento = result.First().id_seguimiento;
                                    det_seg_ev_req.evaluador = evaluador;
                                    det_seg_ev_req.indicador = "1";
                                    _HabilitacionesService.Update_det_seg_evalua(det_seg_ev_req);
                                }
                                else
                                {
                                    DetSegEvaluadorRequest det_seg_ev = new DetSegEvaluadorRequest();
                                    det_seg_ev.id_seguimiento = res_det_seg.id_seguimiento;
                                    det_seg_ev.evaluador = evaluador;
                                    det_seg_ev.indicador = "1";
                                    _HabilitacionesService.Create_det_seg_evaluador(det_seg_ev);

                                }
                            }
                            else
                            {
                                if (req_seg_dhcpa.evaluador != evaluador)
                                {
                                    req_seg_dhcpa.evaluador = evaluador;
                                    _HabilitacionesService.Update_seguimiento_dhcpa(req_seg_dhcpa);

                                    var result = _HabilitacionesService.GetAlldet_seg_evaluador(res_det_seg.id_seguimiento);

                                    DetSegEvaluadorRequest det_seg_ev_req = new DetSegEvaluadorRequest();
                                    det_seg_ev_req.id_det_exp_eva = result.First().id_det_exp_eva;
                                    det_seg_ev_req.id_seguimiento = result.First().id_seguimiento;
                                    det_seg_ev_req.evaluador = result.First().evaluador;
                                    det_seg_ev_req.indicador = "0";
                                    det_seg_ev_req.fecha_recibido = result.First().fecha_recibido;
                                    det_seg_ev_req.fecha_derivado = result.First().fecha_derivado;
                                    _HabilitacionesService.Update_det_seg_evalua(det_seg_ev_req);

                                    DetSegEvaluadorRequest det_seg_ev = new DetSegEvaluadorRequest();
                                    det_seg_ev.id_seguimiento = res_det_seg.id_seguimiento;
                                    det_seg_ev.evaluador = evaluador;
                                    det_seg_ev.indicador = "1";
                                    _HabilitacionesService.Create_det_seg_evaluador(det_seg_ev);

                                }
                            }
                        }
                    }
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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

        public ActionResult Imprimir_masivo(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    ViewBag.id = id;
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Seguimiento_finalizar(int id, int tiempo_tramite, int tiempo_sdhpa, string observacion_final, string inspector_designado, string fecha_auditoria, string fecha_acta, string fecha_oficio, string con_proceso)
        {
            SeguimientoDhcpaRequest seg_request = new SeguimientoDhcpaRequest();
            seg_request = _HabilitacionesService.Recupera_seguimiento_x_id(id).First();
            seg_request.estado = "3";
            seg_request.fecha_fin = DateTime.Now;
            seg_request.duracion_tramite = tiempo_tramite;
            seg_request.duracion_sdhpa = tiempo_sdhpa;
            seg_request.observaciones = observacion_final;
            seg_request.inspecto_designado = inspector_designado;
            if (fecha_auditoria != "") { seg_request.fecha_auditoria = Convert.ToDateTime(fecha_auditoria); } else { seg_request.fecha_auditoria = null; }
            if (fecha_acta != "") { seg_request.fecha_envio_acta = Convert.ToDateTime(fecha_acta); } else { seg_request.fecha_envio_acta = null; }
            if (fecha_oficio != "") { seg_request.fecha_envio_oficio_sdhpa = Convert.ToDateTime(fecha_oficio); } else { seg_request.fecha_envio_oficio_sdhpa = null; }
            seg_request.con_proceso = con_proceso;
            _HabilitacionesService.Update_seguimiento_dhcpa(seg_request);
            @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
            return PartialView("_Success");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Cambiar_Tipo_seguimiento(int id_seguimiento, int id_tipo_seguimiento)
        {
            SeguimientoDhcpaRequest seg_response = new SeguimientoDhcpaRequest();
            seg_response = _HabilitacionesService.Recupera_seguimiento_x_id(id_seguimiento).First();
            seg_response.id_tipo_seguimiento = id_tipo_seguimiento;
            _HabilitacionesService.Update_seguimiento_dhcpa(seg_response);
            @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
            return PartialView("_Success");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Asignar_habilitante(int id_seguimiento, int id_habilitante, string codigo_habilitante)
        {
            SeguimientoDhcpaRequest seg_response = new SeguimientoDhcpaRequest();
            seg_response = _HabilitacionesService.Recupera_seguimiento_x_id(id_seguimiento).First();
            seg_response.id_habilitante = id_habilitante;
            seg_response.cod_habilitante = codigo_habilitante;
            _HabilitacionesService.Update_seguimiento_dhcpa(seg_response);
            @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
            return PartialView("_Success");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Guardar_Observacion_x_seguimiento(int id_seguimiento, string observacion)
        {
            SeguimientoDhcpaObservacionesRequest observacion_seguimiento = new SeguimientoDhcpaObservacionesRequest();
            observacion_seguimiento.activo = "1";
            observacion_seguimiento.fecha_crea = DateTime.Now;
            observacion_seguimiento.id_seguimiento = id_seguimiento;
            observacion_seguimiento.observacion = observacion;
            observacion_seguimiento.usuario_crea = HttpContext.User.Identity.Name.Split('|')[1].Trim();
            _HabilitacionesService.Guardar_Observacion_seguimiento(observacion_seguimiento);
            observacion_seguimiento.usuario_crea = HttpContext.User.Identity.Name.Split('|')[3].Trim();
            return Json(observacion_seguimiento, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult crear_constancia_seguimiento(int id_seguimiento, string constancia)
        {
            ConstanciaHaccpRequest const_request = new ConstanciaHaccpRequest();
            const_request.id_seguimiento = id_seguimiento;
            const_request.nombre = constancia;
            const_request.activo = "1";
            const_request.fecha_registro = DateTime.Now;
            const_request.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
            _HabilitacionesService.insert_constancia(const_request);
            @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
            return PartialView("_Success");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Generar_solicitud_inspeccion(int id_seguimiento, string resolucion, string persona_contacto, string telefono_oficina, string telefono_planta, string correo, string observacion, int serv_habilitacion, int filial)
        {
            int var_año = DateTime.Now.Year;
            int var_oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
            int num_solicitud = _HabilitacionesService.recupera_cantidad_solicitud_inspeccion(var_oficina_crea, var_año);

            SolicitudInspeccionRequest req_soli_insp = new SolicitudInspeccionRequest();
            req_soli_insp.id_seguimiento = id_seguimiento;
            req_soli_insp.numero_documento = num_solicitud + 1;
            req_soli_insp.fecha_crea = DateTime.Now;
            req_soli_insp.oficina_crea = var_oficina_crea;
            req_soli_insp.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
            req_soli_insp.año_crea = DateTime.Now.Year;
            req_soli_insp.resolucion = resolucion;
            req_soli_insp.persona_contacto = persona_contacto;
            req_soli_insp.telefono_oficina = telefono_oficina;
            req_soli_insp.telefono_planta = telefono_planta;
            req_soli_insp.correo = correo;
            req_soli_insp.observaciones = observacion;
            req_soli_insp.id_tipo_ser_hab = serv_habilitacion;
            req_soli_insp.id_filial = filial;

            int insert = _HabilitacionesService.Create_solicitud_inspeccion(req_soli_insp);

            @ViewBag.Mensaje = "Se creo la solicitud N° " + (num_solicitud + 1).ToString();
            return PartialView("_Success_NS");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Generar_solicitud_inspeccion_transporte(int id_seguimiento, string resolucion, string persona_contacto, string telefono_oficina, string telefono_planta, string correo, string observacion, int serv_habilitacion, int filial,
            int destino, string cond_manual, string norma_aplicar)
        {
            int var_año = DateTime.Now.Year;
            int var_oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
            int num_solicitud = _HabilitacionesService.recupera_cantidad_solicitud_inspeccion(var_oficina_crea, var_año);
            FilialDhcpaResponse var_filial = _HabilitacionesService.GetAll_Filial().Where(x => x.id_filial == filial).First();

            SolicitudInspeccionRequest req_soli_insp = new SolicitudInspeccionRequest();
            req_soli_insp.id_seguimiento = id_seguimiento;
            req_soli_insp.numero_documento = num_solicitud + 1;
            req_soli_insp.fecha_crea = DateTime.Now;
            req_soli_insp.oficina_crea = var_oficina_crea;
            req_soli_insp.nom_ofi_crea = _GeneralService.recupera_oficina(var_oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(var_oficina_crea).nombre;
            req_soli_insp.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
            req_soli_insp.año_crea = DateTime.Now.Year;
            req_soli_insp.resolucion = resolucion;
            req_soli_insp.persona_contacto = persona_contacto;
            req_soli_insp.telefono_oficina = telefono_oficina;
            req_soli_insp.telefono_planta = telefono_planta;
            req_soli_insp.correo = correo;
            req_soli_insp.observaciones = observacion;
            req_soli_insp.id_tipo_ser_hab = serv_habilitacion;
            req_soli_insp.id_filial = filial;
            req_soli_insp.id_oficina_destino = var_filial.id_od_insp;
            req_soli_insp.nom_oficina_destino = _GeneralService.recupera_oficina(req_soli_insp.id_oficina_destino ?? 0).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(req_soli_insp.id_oficina_destino ?? 0).nombre;
            req_soli_insp.id_dest_sol_ins = destino;
            req_soli_insp.cond_manuales = cond_manual;
            req_soli_insp.norma_aplica = norma_aplicar;
            req_soli_insp.id_estado = 1;

            int insert = _HabilitacionesService.Create_solicitud_inspeccion(req_soli_insp);

            @ViewBag.Mensaje = insert.ToString();
            return PartialView("_Success_NS");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Generar_informe_tecnico(int id_seguimiento, string observaciones)
        {
            int var_año = DateTime.Now.Year;
            int var_oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
            int num_informe = _HabilitacionesService.recupera_cantidad_informe_tecnico(var_oficina_crea, var_año);

            InformeTecnicoEvalRequest req_soli_insp = new InformeTecnicoEvalRequest();
            req_soli_insp.id_seguimiento = id_seguimiento;
            req_soli_insp.numero_documento = num_informe + 1;
            req_soli_insp.observaciones = observaciones;
            req_soli_insp.fecha_crea = DateTime.Now;
            req_soli_insp.oficina_crea = var_oficina_crea;
            req_soli_insp.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
            req_soli_insp.año_crea = DateTime.Now.Year;

            bool insert = _HabilitacionesService.Create_informe_tecnico(req_soli_insp);

            @ViewBag.Mensaje = "Se creó el Informe N° " + (num_informe + 1).ToString();
            return PartialView("_Success");
        }

        [AllowAnonymous]
        public ActionResult Consulta_seguimiento_asitente(int page = 1, string expediente = "", string externo = "", string habilitante = "", string cmbestado = "", string cbo_cons_evaluador = "", int cmbtupa = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[9].Trim() == "1" // Acceso a Nuevo Seguimiento
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones Pesqueras y Acuicolas
                {

                    List<SelectListItem> Lista_estado_seguimiento_dhcpa = new List<SelectListItem>();

                    Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });

                    var recupera_estado_seguimiento = _HabilitacionesService.Lista_estado_seguimiento_dhcpa();
                    foreach (var result in recupera_estado_seguimiento)
                    {
                        if (result.id_estado != "4")
                        {
                            Lista_estado_seguimiento_dhcpa.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_estado
                            }
                            );
                        }
                    };



                    List<SelectListItem> Lista_tipo_seguimiento = new List<SelectListItem>();
                    foreach (var result in _GeneralService.recupera_tipo_seguimiento())
                    {
                        Lista_tipo_seguimiento.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_seguimiento.ToString()
                        });
                    };

                    ViewBag.lst_combo_mts = Lista_tipo_seguimiento;

                    List<SelectListItem> Lista_Evaluador = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(18);

                    foreach (var result in recupera_persona)
                    {
                        Lista_Evaluador.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };



                    List<SelectListItem> Lista_Evaluador_consulta = new List<SelectListItem>();

                    var recupera_persona_consulta = _GeneralService.Recupera_personal_oficina(18);

                    Lista_Evaluador_consulta.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });

                    foreach (var result in recupera_persona_consulta)
                    {
                        Lista_Evaluador_consulta.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    List<SelectListItem> Lista_expediente = new List<SelectListItem>();

                    Lista_expediente.Add(new SelectListItem()
                    {
                        Text = "",
                        Value = ""
                    });

                    List<SelectListItem> lista_combo = new List<SelectListItem>();

                    lista_combo.Add(new SelectListItem()
                    {
                        Text = "SELECCIONA",
                        Value = ""
                    });

                    List<SelectListItem> Lista_embarcaciones = new List<SelectListItem>();

                    List<SelectListItem> Lista_TUPA = new List<SelectListItem>();

                    Lista_TUPA.Add(new SelectListItem() { Text = "SELECCIONAR", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina == 18))
                    {
                        Lista_TUPA.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString(),
                            Value = result.id_tupa.ToString()
                        });
                    };


                    ViewBag.lst_tupa = Lista_TUPA;
                    ViewBag.lst_embarcacion = Lista_embarcaciones;
                    ViewBag.lst_combo = lista_combo;
                    ViewBag.lst_evaluador = Lista_Evaluador;
                    ViewBag.lst_evaluador_consulta = Lista_Evaluador_consulta;
                    ViewBag.lst_expediente = Lista_expediente;
                    ViewBag.lst_estado = Lista_estado_seguimiento_dhcpa;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Fecha Inicio");
                    tbl.Columns.Add("Expediente");
                    tbl.Columns.Add("TUPA/SERV");
                    tbl.Columns.Add("Procedimiento");
                    tbl.Columns.Add("Externo");
                    tbl.Columns.Add("Habilitante");
                    tbl.Columns.Add("Evaluador");
                    tbl.Columns.Add("Estado");
                    tbl.Columns.Add("Expediente_Id_seguimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_Id_ofi_dir");
                    tbl.Columns.Add("Expediente_Id_seguimiento_documento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_duracion_sdhpa_duracion_tramite_observacion");
                    tbl.Columns.Add("cond_expediente");

                    tbl.Columns.Add("id_tipo_seguimiento");
                    tbl.Columns.Add("cond_habilitante");

                    tbl.Columns.Add("cond_finalizar");
                    tbl.Columns.Add("Id_seguimiento");
                    tbl.Columns.Add("Id_expediente");
                    tbl.Columns.Add("Id_procedimiento");
                    tbl.Columns.Add("Expediente_Id_seguimiento_cond_finalizar");


                    var seguimiento = _HabilitacionesService.GetAllSeguimiento_Consulta_sin_paginado(expediente, cbo_cons_evaluador, externo, habilitante, cmbestado, 0, cmbtupa);

                    foreach (var result in seguimiento)
                    {
                        if (result.num_tupa == null)
                        {
                            if (result.persona_num_documento == null)
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    "",
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.ruc,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),
                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),
                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                            else
                            {

                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    "",
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.persona_num_documento,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),
                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),
                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                        }
                        else
                        {

                            if (result.persona_num_documento == null)
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.ruc,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),
                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),
                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );
                            }
                            else
                            {
                                tbl.Rows.Add(

                                    result.fecha_inicio,
                                    result.Expediente,
                                    result.nom_tipo_tupa + " : " + result.num_tupa_cadena,
                                    result.nom_tipo_procedimiento,
                                    result.nom_oficina_ext,
                                    result.cod_habilitante,
                                    result.nom_evaluador,
                                    result.nom_estado,
                                    result.Expediente + "|" + result.id_seguimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.id_ofi_dir.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.persona_num_documento,
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.duracion_sdhpa.ToString() + "|" + result.duracion_tramite.ToString() + "|" + result.observaciones,
                                    result.cond_expediente.ToString(),
                                    result.id_tipo_seguimiento.ToString(),
                                    result.cond_habilitante.ToString(),
                                    result.cond_finalizar.ToString(),
                                    result.id_seguimiento.ToString(),
                                    result.id_expediente.ToString(),
                                    result.id_tipo_procedimiento.ToString(),
                                    result.Expediente + "|" + result.id_seguimiento.ToString() + "|" + result.cond_finalizar.ToString()
                                    );

                            }
                        }
                    };

                    ViewData["Seguimiento_Tabla"] = tbl;
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
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Buscar_expediente_documento_externo(string expediente) /// ME QUEDE ACA
        {
            return Json(_HabilitacionesService.Consulta_expediente_x_expediente(expediente), JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        public ActionResult Nuevo_Documento_dhcpa_Certificaciones()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    List<SelectListItem> lista_sedes_externo = new List<SelectListItem>();
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    lista_sedes_externo.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_personal = new List<SelectListItem>();

                    lista_sedes.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    int id_ofi_ruc = 0;

                    foreach (var result in _GeneralService.Recupera_oficina_all_x_ruc("20565429656"))
                    {
                        if (result.id_ofi_padre == null)
                        {
                            id_ofi_ruc = result.id_oficina;
                        }
                    };


                    foreach (var result in _GeneralService.Recupera_sede_all(id_ofi_ruc))
                    {
                        lista_sedes.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_sede.ToString()
                        }
                        );
                    };

                    Lista_Oficina_destino.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINA",
                        Value = "0"
                    });

                    lista_personal.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR PERSONAL",
                        Value = ""
                    });

                    List<SelectListItem> lista_tipo_documento = new List<SelectListItem>();

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("", "0"))
                    {
                        lista_tipo_documento.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                    };

                    List<SelectListItem> lista_archivadores = new List<SelectListItem>();

                    lista_archivadores.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR ARCHIVADOR",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Archivador())
                    {
                        lista_archivadores.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_archivador.ToString()
                            });
                    };

                    List<SelectListItem> lista_filiales = new List<SelectListItem>();

                    lista_filiales.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR FILIALES",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Filial())
                    {
                        lista_filiales.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_filial.ToString()
                            });
                    };

                    List<SelectListItem> Lista_per_crea = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_per_crea.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina = lista_personal;

                    ViewBag.lstsede_destino_externo = lista_sedes_externo;
                    ViewBag.lstOficina_destino_externo = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina_externo = lista_personal;

                    ViewBag.lst_persona_crea = Lista_per_crea;
                    ViewBag.list_tip_documento_dhcpa = lista_tipo_documento;
                    ViewBag.list_archivador = lista_archivadores;
                    ViewBag.lista_filiales = lista_filiales;

                    ViewBag.user_document = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    ViewBag.user_perfil = HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim();

                    DocumentodhcpaViewModel model_doc_dhcpa = new DocumentodhcpaViewModel();

                    return View(model_doc_dhcpa);

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

        [AcceptVerbs(HttpVerbs.Post)]   //Aqui me quede Lander 
        public ActionResult Nuevo_Documento_dhcpa_Certificaciones(DocumentodhcpaViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28 : Atención al Cliente
                {
                    model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo(model.id_tipo_documento) + 1;
                    model.nom_doc = "-" + DateTime.Now.Year.ToString() + "- DHCPA/SANIPES";
                    DocumentoDhcpaRequest req_documento_dhcpa = ModelToRequest.Documento_dhcpa(model);
                    req_documento_dhcpa.fecha_registro = DateTime.Now;
                    req_documento_dhcpa.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();

                    model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa);
                    req_documento_dhcpa.id_doc_dhcpa = model.id_doc_dhcpa;

                    if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                    {
                        try
                        {
                            DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                            req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                            req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_seguimiento.activo = "1";
                            req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                        }
                        catch (Exception){ }
                    }

                    if (model.documento_dhcpa_detalle != null)
                    {
                        foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                        {
                            DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                            req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_detalle.activo = "1";
                            req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                            obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                        }
                    }

                    if (model.ind_agregar_celula == 1)
                    {
                        string mensaje = "";
                        mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;

                        if (model.id_tipo_documento == 136)
                        {
                            model.doc_notificar_cdl_notif = model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }

                        // 21 : CEDULA DE NOTIFICACION
                        model.id_tipo_documento = 21;
                        model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo(model.id_tipo_documento) + 1;
                        
                        DocumentoDhcpaRequest req_documento_dhcpa2 = ModelToRequest.Documento_dhcpa(model);
                        req_documento_dhcpa2.fecha_registro = DateTime.Now;
                        req_documento_dhcpa2.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();

                        model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa2);
                        req_documento_dhcpa2.id_doc_dhcpa = model.id_doc_dhcpa;

                        if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                        {
                            try
                            {
                                DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                                req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                                req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_seguimiento.activo = "1";
                                req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                            }
                            catch (Exception) { }
                        }

                        if (model.documento_dhcpa_detalle != null)
                        {
                            foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                            {
                                DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                                req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_detalle.activo = "1";
                                req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                                obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                            }
                        }

                        try
                        {
                            mensaje = mensaje + ", Se creó el Documento : CEDULA DE NOTIFICACION N° " + model.num_doc.ToString() + model.nom_doc;
                            @ViewBag.Mensaje = mensaje;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }

                    }
                    else
                    {
                        try
                        {
                            @ViewBag.Mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }
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

        [AllowAnonymous]
        public ActionResult Consulta_Documentos_emitidos_dhcpa(int page = 1, int cmbtipo_documento = 0, string asunto = "", int cmbanno_documento = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento
                    ))
                {

                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    if (cmbanno_documento == 0)
                    {
                        cmbanno_documento = DateTime.Now.Year;
                    }

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("", "0"))
                    {
                        lista_documentos.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                    };

                    List<SelectListItem> anno_Documento = new List<SelectListItem>();

                    for (int i = DateTime.Now.Year; i >= 2015; i--)
                    {

                        anno_Documento.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }
                    ViewBag.lst_anno_documento = anno_Documento;

                    ViewBag.lst_tipo_documento = lista_documentos;

                    string var_evaluador = "";
                    if (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "18")
                    {
                        var_evaluador = "20565429656 - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    }

                    DataTable tbl = new DataTable();
                    
                    tbl.Columns.Add("NOM_TIPO_DOCUMENTO");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("FECHA_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("ANEXO");
                    tbl.Columns.Add("ID_DOC_DHCPA");
                    tbl.Columns.Add("PDF");
                    tbl.Columns.Add("RUTA_PDF");

                    //Add by HM - 13/11/2019
                    tbl.Columns.Add("RUC");
                    tbl.Columns.Add("NUM_DOC");
                    tbl.Columns.Add("EVALUADOR_CDL_NOTIF");
                    tbl.Columns.Add("DIRECCION_CDL_NOTIF");
                    tbl.Columns.Add("EMPRESA_CDL_NOTIF");
                    tbl.Columns.Add("FOLIA_CDL_NOTIF");
                    tbl.Columns.Add("DOC_NOTIFICAR_CDL_NOTIF");
                    tbl.Columns.Add("EXP_O_HT_CDL_NOTIF");
                    tbl.Columns.Add("EXP_O_HT_N_CDL_NOTIF");



                    var documento_dhcpa = _HabilitacionesService.Lista_Documentos_dhcpa(var_evaluador, cmbtipo_documento, asunto, cmbanno_documento);

                    foreach (var result in documento_dhcpa)
                    {
                        string ruta_x = "";
                        if (result.pdf == "1")
                        {
                            ruta_x = "/Habilitaciones/var_documento_dhcpa_pdf/" + result.id_doc_dhcpa.ToString();
                        }
                        tbl.Rows.Add(
                            result.nom_tipo_documento,
                            result.num_doc.ToString() + result.nom_doc,
                            result.fecha_doc.Value.ToShortDateString(),
                            result.asunto,
                            result.anexos,
                            result.id_doc_dhcpa,
                            result.pdf,
                            ruta_x,

                            //Add by HM - 13/11/2019
                            result.ruc,
                            result.num_doc,
                            result.evaluador_cdl_notif,
                            result.direccion_cdl_notif,
                            result.empresa_cdl_notif,
                            result.folia_cdl_notif,
                            result.doc_notificar_cdl_notif,
                            result.exp_o_ht_cdl_notif,
                            result.exp_o_ht_n_cdl_notif
                            );
                    };

                    ViewData["documentos_tabla"] = tbl;

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

        [AllowAnonymous]
        public ActionResult Consulta_Solicitudes_dhcpa(int page = 1)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[17].Trim() == "1" // Acceso a Consulta solicitud
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18")))
                // Oficina 18: Sub Dirección de Habilitaciones Pesqueras y Acuicolas 
                {
                    DataTable tbl = new DataTable();

                    tbl.Columns.Add("FECHA_INICIO");
                    tbl.Columns.Add("EXPEDIENTE");
                    tbl.Columns.Add("NRO_SOLICITUD");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("PROCEDIMIENTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("HABILITANTE");
                    tbl.Columns.Add("EVALUADOR");
                    tbl.Columns.Add("ESTADO");
                    tbl.Columns.Add("EXPEDIENTE_ID_SEGUIMIENTO");

                    var documentos_od = _HabilitacionesService.Lista_Solicitudes_dhcpa();

                    foreach (var result in documentos_od)
                    {
                        tbl.Rows.Add(
                            result.fecha_inicio,
                            result.Expediente,
                            result.num_solicitud_dhcpa,
                            result.fecha_solicitud_dhcpa,
                            result.num_tupa,
                            result.nom_tipo_procedimiento,
                            result.nom_oficina_ext,
                            result.cod_habilitante,
                            result.nom_evaluador,
                            result.nom_estado,
                            result.Expediente + "|" + result.id_seguimiento.ToString()
                            );
                    };

                    ViewData["solicitudes_tabla"] = tbl;


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

        [AllowAnonymous]
        public ActionResult Consulta_Documentos_OD_Por_Recibir(int page = 1, string expediente = "", string asunto = "", string externo = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[18].Trim() == "1" // Acceso a Consulta Documento OD POR RECIBIR
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de atención al ciudadano
                {


                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR TIPO DOCUMENTO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("T", "0"))
                    {
                        lista_documentos.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_documento.ToString()
                        });
                    };

                    if (cmbtipo_documento == "0")
                    {
                        cmbtipo_documento = "";
                    }

                    ViewBag.lst_tipo_documento = lista_documentos;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DOCUMENTO_SEG");
                    tbl.Columns.Add("FECHA_ENVIADO");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("FECHA_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EXPEDIENTES");
                    tbl.Columns.Add("VER_PDF");

                    var documentos_od = _HabilitacionesService.Lista_Documento_OD_pendientes_x_recibir("", "", "", asunto, externo, cmbtipo_documento, num_documento, nom_documento, expediente);

                    foreach (var result in documentos_od)
                    {
                        if (result.ruta_pdf == null || result.ruta_pdf == "")
                        {
                            tbl.Rows.Add(
                               result.id_documento_seg,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.fecha_documento.Value.ToShortDateString(),
                                result.asunto,
                                result.group_expedientes, false);
                        }
                        else
                        {

                            tbl.Rows.Add(
                               result.id_documento_seg,
                                result.fecha_crea,
                                result.nom_documento,
                                result.nom_externo,
                                result.fecha_documento.Value.ToShortDateString(),
                                result.asunto,
                                result.group_expedientes, true);
                        }
                    };

                    ViewData["documentos_od_tabla"] = tbl;

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

        public ActionResult Recibir_pendientes_od(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[18].Trim() == "1" // Acceso a Consulta Documento OD POR RECIBIR
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28")))
                // Oficina 28: Oficina de Atención del ciudadano
                {

                    int id_recibir = 0;
                    for (int i = 0; i < id.Split(',').Count(); i++)
                    {
                        id_recibir = Convert.ToInt32(id.Split(',')[i].Trim());
                        DocumentoSeguimientoRequest doc_seg_req = new DocumentoSeguimientoRequest();
                        doc_seg_req = _HabilitacionesService.GetAllDocumento_req(id_recibir);
                        doc_seg_req.fecha_crea = DateTime.Now;
                        doc_seg_req.usuario_crea = "20565429656 - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        bool document_seg = _HabilitacionesService.Update_mae_documento_seg(doc_seg_req);
                    }
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
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
        public ActionResult recupera_RUC_NOM_vista_seguimiento(string NOM = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _HabilitacionesService.Consultar_RUC_X_NOM_Seguimiento(NOM))
            {
                if (x.ruc == "99999999999" || x.ruc == "99999999998")
                {
                    lista_oficinas.Add(new SelectListItem()
                    {
                        Text = x.nombre,
                        Value = x.id_oficina.ToString()
                    });
                }
                else
                {
                    lista_oficinas.Add(new SelectListItem()
                    {
                        Text = x.ruc + " - " + x.nombre,
                        Value = x.id_oficina.ToString()
                    });
                }

            }

            if (lista_oficinas.Count() <= 0)
            {
                lista_oficinas.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_oficinas, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_RUC_vista_seguimiento(string RUC = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _HabilitacionesService.Consultar_RUC_seguimiento(RUC))
            {
                lista_oficinas.Add(new SelectListItem()
                {
                    Text = x.ruc + " - " + x.nombre,
                    Value = x.id_oficina.ToString()
                });
            }

            if (lista_oficinas.Count() <= 0)
            {
                lista_oficinas.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_oficinas, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_Especie_habilitaciones(string nombre_comun, string nombre_cientifico) /// ME QUEDE ACA
        {
            IEnumerable<EspeciesHabilitacionesResponse> Especie_habilitaciones = new List<EspeciesHabilitacionesResponse>();

            nombre_comun = nombre_comun.Trim();
            nombre_cientifico = nombre_cientifico.Trim();

            Especie_habilitaciones = _HabilitacionesService.lista_especies_habilitaciones(nombre_comun, nombre_cientifico);

            //id_det_espec_hab ,nombre_comun ,nombre_cientifico ,especie_categoria 

            return Json(Especie_habilitaciones, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_DESEMBARCADERO_vista(int ID_DIRECCION = 1)
        {

            List<SelectListItem> lista_desembarcadero = new List<SelectListItem>();

            lista_desembarcadero.Add(new SelectListItem()
            {
                Text = "SELECCIONAR DESEMBARCADERO",
                Value = ""
            });

            foreach (var x in _GeneralService.lista_desembarcadero_x_sede(ID_DIRECCION))
            {
                lista_desembarcadero.Add(new SelectListItem()
                {
                    Text = x.codigo_desembarcadero,
                    Value = x.id_desembarcadero.ToString()
                });
            }

            return Json(lista_desembarcadero, JsonRequestBehavior.AllowGet);
        }



        #region Exportar_Excel

        public ActionResult Export_Excel_Plantas_Protocolo()
        {

            DataTable tbl_plantas = new DataTable();
            tbl_plantas.Columns.Add("FILIAL");
            tbl_plantas.Columns.Add("RAZON SOCIAL");
            tbl_plantas.Columns.Add("CODIGO PLANTA");
            tbl_plantas.Columns.Add("ACTIVIDAD");
            tbl_plantas.Columns.Add("TIPO CHD / CHI");
            tbl_plantas.Columns.Add("LICENCIA");
            tbl_plantas.Columns.Add("CAPACIDAD");
            tbl_plantas.Columns.Add("PLANTA - DIRECCION");
            tbl_plantas.Columns.Add("PLANTA - DEPARTAMENTO");
            tbl_plantas.Columns.Add("PLANTA - PROVINCIA");
            tbl_plantas.Columns.Add("PLANTA - DISTRITO");
            tbl_plantas.Columns.Add("DIRECCION LEGAL - DIRECCION");
            tbl_plantas.Columns.Add("DIRECCION LEGAL - DEPARTAMENTO");
            tbl_plantas.Columns.Add("DIRECCION LEGAL - PROVINCIA");
            tbl_plantas.Columns.Add("DIRECCION LEGAL - DISTRITO");
            tbl_plantas.Columns.Add("PROTOCOLO - NOMBRE");
            tbl_plantas.Columns.Add("PROTOCOLO - EMISION");
            tbl_plantas.Columns.Add("PROTOCOLO - INICIO VIGENCIA");
            tbl_plantas.Columns.Add("PROTOCOLO - FIN VIGENCIA");
            tbl_plantas.Columns.Add("PROTOCOLO - AÑO");
            tbl_plantas.Columns.Add("PROTOCOLO - MES");

            var list = _HabilitacionesService.Lista_plantas_excel();

            DataRow tbl_row_plantas;
            foreach (var solici in list)
            {
                tbl_row_plantas = tbl_plantas.NewRow();
                tbl_row_plantas["FILIAL"] = solici.excel_filial;
                tbl_row_plantas["RAZON SOCIAL"] = solici.excel_razon_social;
                tbl_row_plantas["CODIGO PLANTA"] = solici.excel_codigo_planta;
                tbl_row_plantas["ACTIVIDAD"] = solici.excel_actividad;
                tbl_row_plantas["TIPO CHD / CHI"] = solici.excel_tch;
                tbl_row_plantas["LICENCIA"] = solici.excel_licencia_operacion;
                tbl_row_plantas["CAPACIDAD"] = "";
                tbl_row_plantas["PLANTA - DIRECCION"] = solici.excel_direccion_planta;
                tbl_row_plantas["PLANTA - DEPARTAMENTO"] = solici.excel_departamento_planta;
                tbl_row_plantas["PLANTA - PROVINCIA"] = solici.excel_provincia_planta;
                tbl_row_plantas["PLANTA - DISTRITO"] = solici.excel_distrito_planta;
                tbl_row_plantas["DIRECCION LEGAL - DIRECCION"] = solici.excel_direccion_legal;
                tbl_row_plantas["DIRECCION LEGAL - DEPARTAMENTO"] = solici.excel_departamento_legal;
                tbl_row_plantas["DIRECCION LEGAL - PROVINCIA"] = solici.excel_provincia_legal;
                tbl_row_plantas["DIRECCION LEGAL - DISTRITO"] = solici.excel_distrito_legal;
                var list_proto = _HabilitacionesService.Lista_protocolo_seguimiento_planta(solici.excel_id_planta);
                tbl_row_plantas["PROTOCOLO - NOMBRE"] = list_proto.excel_documento_resolutivo;
                tbl_row_plantas["PROTOCOLO - INICIO VIGENCIA"] = list_proto.excel_ini_vigencia;
                tbl_row_plantas["PROTOCOLO - FIN VIGENCIA"] = list_proto.excel_fin_vigencia;
                tbl_row_plantas["PROTOCOLO - EMISION"] = list_proto.excel_fecha_emision;

                tbl_plantas.Rows.Add(tbl_row_plantas);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_plantas;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_planta_protocolo.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }

        public ActionResult Export_Excel_solicitud(string para1 = "", string para2 = "")
        {

            DataTable tbl_solicitud = new DataTable();
            tbl_solicitud.Columns.Add("Fecha de Recepcion OTD");
            tbl_solicitud.Columns.Add("Nro de Expediente");
            tbl_solicitud.Columns.Add("Evaluador");
            tbl_solicitud.Columns.Add("Fecha recepción Evaluador");
            tbl_solicitud.Columns.Add("Nro Solicitud");
            tbl_solicitud.Columns.Add("Fecha de Solicitud");
            tbl_solicitud.Columns.Add("TUPA");
            tbl_solicitud.Columns.Add("Asunto");
            tbl_solicitud.Columns.Add("Nombre de la Infraestructura");
            tbl_solicitud.Columns.Add("Matricula");
            tbl_solicitud.Columns.Add("Código de la Infraestructura");
            tbl_solicitud.Columns.Add("Actividad");
            tbl_solicitud.Columns.Add("OD");
            tbl_solicitud.Columns.Add("Situación");
            tbl_solicitud.Columns.Add("N° Documento Resolutivo");
            tbl_solicitud.Columns.Add("Inicio de vigencia");
            tbl_solicitud.Columns.Add("Fin de vigencia");
            tbl_solicitud.Columns.Add("Fecha de emisión");
            tbl_solicitud.Columns.Add("Duración de Trámite");
            tbl_solicitud.Columns.Add("Duración de SDHPA");
            tbl_solicitud.Columns.Add("Observación");

            tbl_solicitud.Columns.Add("Inspector Designado");
            tbl_solicitud.Columns.Add("Fecha Auditoria");
            tbl_solicitud.Columns.Add("Fecha envio acta");
            tbl_solicitud.Columns.Add("Fecha envio de oficio - sdhpa");
            tbl_solicitud.Columns.Add("Con Proceso");


            var list = _HabilitacionesService.Lista_Solicitudes_excel();

            DataRow tbl_row_solicitud;
            foreach (var solici in list)
            {
                tbl_row_solicitud = tbl_solicitud.NewRow();
                tbl_row_solicitud["Fecha de Recepcion OTD"] = solici.fecha_inicio.ToShortDateString();
                tbl_row_solicitud["Nro de Expediente"] = solici.Expediente;

                var select_evaluador = _HabilitacionesService.Lista_datos_evaluador(solici.id_seguimiento);
                tbl_row_solicitud["Evaluador"] = select_evaluador.nom_evaluador;
                tbl_row_solicitud["Fecha recepción Evaluador"] = select_evaluador.fecha_recepcion_evaluador;

                tbl_row_solicitud["Nro Solicitud"] = solici.num_solicitud_dhcpa;
                if (solici.fecha_solicitud_dhcpa != null || solici.fecha_solicitud_dhcpa.ToString() != "")
                {
                    tbl_row_solicitud["Fecha de Solicitud"] = solici.fecha_solicitud_dhcpa.Value.ToShortDateString();
                }
                else
                {
                    tbl_row_solicitud["Fecha de Solicitud"] = "";
                }
                tbl_row_solicitud["TUPA"] = solici.num_tupa_cadena;
                tbl_row_solicitud["Asunto"] = solici.asunto;
                tbl_row_solicitud["Nombre de la Infraestructura"] = solici.nom_oficina_ext;
                tbl_row_solicitud["Matricula"] = solici.matricula;  // INGRESAR LA MATRICULA SI ES EMBARCACION O TRANSPORTE

                if (solici.id_tipo_ser_hab == 1)
                {
                    tbl_row_solicitud["Código de la Infraestructura"] = "NUEVO";
                }
                else
                {
                    tbl_row_solicitud["Código de la Infraestructura"] = solici.codigo_habilitacion;
                }

                tbl_row_solicitud["Actividad"] = solici.nom_actividad;
                tbl_row_solicitud["OD"] = solici.nom_filial;

                var list_proto = _HabilitacionesService.Lista_protocolo_solicitud(solici.id_seguimiento);

                if (list_proto.excel_documento_resolutivo == "")
                {
                    tbl_row_solicitud["Situación"] = "PENDIENTE";
                }
                else
                {
                    tbl_row_solicitud["Situación"] = "ATENDIDO";
                }

                tbl_row_solicitud["N° Documento Resolutivo"] = list_proto.excel_documento_resolutivo;
                tbl_row_solicitud["Inicio de vigencia"] = list_proto.excel_ini_vigencia;
                tbl_row_solicitud["Fin de vigencia"] = list_proto.excel_fin_vigencia;
                tbl_row_solicitud["Fecha de emisión"] = list_proto.excel_fecha_emision;

                tbl_row_solicitud["Duración de Trámite"] = solici.duracion_tramite;
                tbl_row_solicitud["Duración de SDHPA"] = solici.duracion_sdhpa;
                tbl_row_solicitud["Observación"] = solici.observaciones;

                tbl_row_solicitud["Inspector Designado"] = solici.inspecto_designado;
                tbl_row_solicitud["Fecha Auditoria"] = solici.fecha_auditoria;
                tbl_row_solicitud["Fecha envio acta"] = solici.fecha_envio_acta;
                tbl_row_solicitud["Fecha envio de oficio - sdhpa"] = solici.fecha_envio_oficio_sdhpa;
                tbl_row_solicitud["Con Proceso"] = solici.con_proceso;

                tbl_solicitud.Rows.Add(tbl_row_solicitud);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_solicitud;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_solicitud.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }

        public ActionResult Export_Excel_documentos_dhcpa_emitidos(string para1 = "", string para2 = "")
        {

            DataTable tbl_documento = new DataTable();
            tbl_documento.Columns.Add("Nro Documento");
            tbl_documento.Columns.Add("Fecha Documento");
            tbl_documento.Columns.Add("Destinatario");
            tbl_documento.Columns.Add("Destino");
            tbl_documento.Columns.Add("Tipo Documento");
            tbl_documento.Columns.Add("Asunto");
            tbl_documento.Columns.Add("Anexo");
            tbl_documento.Columns.Add("OD");
            tbl_documento.Columns.Add("EVALUADOR");

            var list = _HabilitacionesService.Lista_Documentos_x_tipo_documento(Convert.ToInt32(para1), Convert.ToInt32(para2));

            DataRow tbl_row_documento;
            foreach (var document in list)
            {

                string lugar_destino = "";
                string persona_destino = "";

                var var_destin = _HabilitacionesService.Lista_Destino_Documentos_x_tipo_documento(document.id_doc_dhcpa);

                foreach (var destinity in var_destin)
                {
                    if (lugar_destino == "")
                    {
                        lugar_destino = destinity.lugar_destino;
                        persona_destino = destinity.persona_destino;
                    }
                    else
                    {
                        lugar_destino = lugar_destino + "/" + destinity.lugar_destino;
                        persona_destino = persona_destino + "/" + destinity.persona_destino;
                    }
                }

                tbl_row_documento = tbl_documento.NewRow();
                tbl_row_documento["Nro Documento"] = document.num_doc.ToString();
                tbl_row_documento["Fecha Documento"] = document.fecha_doc.Value.ToShortDateString();
                tbl_row_documento["Destinatario"] = persona_destino;
                tbl_row_documento["Destino"] = lugar_destino;
                tbl_row_documento["Tipo Documento"] = document.nom_tipo_documento;
                tbl_row_documento["Asunto"] = document.asunto;
                tbl_row_documento["Anexo"] = document.anexos;
                tbl_row_documento["OD"] = document.nom_filial;
                tbl_row_documento["EVALUADOR"] = document.usuario_registro;
                tbl_documento.Rows.Add(tbl_row_documento);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_documento;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Documentos.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }

        public ActionResult Exportar_Excel_Expediente_Evaluador()
        {

            DataTable tbl_seguimiento = new DataTable();
            tbl_seguimiento.Columns.Add("Oficina que crea");
            tbl_seguimiento.Columns.Add("Sede que crea");
            tbl_seguimiento.Columns.Add("Usuario que crea");
            tbl_seguimiento.Columns.Add("Fecha Inicio");
            tbl_seguimiento.Columns.Add("Fecha Fin");
            tbl_seguimiento.Columns.Add("Expediente");
            tbl_seguimiento.Columns.Add("Tipo Tupa");
            tbl_seguimiento.Columns.Add("Tupa");
            tbl_seguimiento.Columns.Add("Procedimiento");
            tbl_seguimiento.Columns.Add("Externo");
            tbl_seguimiento.Columns.Add("Tipo Habilitante");
            tbl_seguimiento.Columns.Add("Habilitante");
            tbl_seguimiento.Columns.Add("Evaluador");
            tbl_seguimiento.Columns.Add("Estado");

            var list = _HabilitacionesService.GetAllSeguimiento_Consulta_excel(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));

            DataRow tbl_row_seguimiento;
            foreach (var seguimiento in list)
            {
                tbl_row_seguimiento = tbl_seguimiento.NewRow();
                tbl_row_seguimiento["Oficina que crea"] = seguimiento.excel_oficina_crea;
                tbl_row_seguimiento["Sede que crea"] = seguimiento.excel_sede_crea;
                tbl_row_seguimiento["Usuario que crea"] = seguimiento.excel_usuario_crea;
                tbl_row_seguimiento["Fecha Inicio"] = seguimiento.fecha_inicio;
                tbl_row_seguimiento["Fecha Fin"] = seguimiento.fecha_fin;
                tbl_row_seguimiento["Expediente"] = seguimiento.Expediente;
                tbl_row_seguimiento["Tipo Tupa"] = seguimiento.nom_tipo_tupa;
                tbl_row_seguimiento["Tupa"] = seguimiento.num_tupa;
                tbl_row_seguimiento["Procedimiento"] = seguimiento.nom_tipo_procedimiento;
                tbl_row_seguimiento["Externo"] = seguimiento.nom_oficina_ext;
                tbl_row_seguimiento["Tipo Habilitante"] = seguimiento.nom_tipo_seguimiento;
                tbl_row_seguimiento["Habilitante"] = seguimiento.cod_habilitante;
                tbl_row_seguimiento["Evaluador"] = seguimiento.nom_evaluador;
                tbl_row_seguimiento["Estado"] = seguimiento.nom_estado;
                tbl_seguimiento.Rows.Add(tbl_row_seguimiento);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_seguimiento;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Documentos.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }


        public ActionResult Export_Excel_transportes_habilitados()
        {

            DataTable tbl_transporte = new DataTable();
            tbl_transporte.Columns.Add("Administrado");
            tbl_transporte.Columns.Add("Código de Habilitación");
            tbl_transporte.Columns.Add("Placa");
            tbl_transporte.Columns.Add("Carroceria");
            tbl_transporte.Columns.Add("Furgón");
            tbl_transporte.Columns.Add("Nombre del Protocolo");
            tbl_transporte.Columns.Add("Fecha de emisión");
            tbl_transporte.Columns.Add("Fecha de inicio de Vigencia");
            tbl_transporte.Columns.Add("Fecha de fin de Vigencia");

            var list = _HabilitacionesService.lista_transportes_con_protocolo_habilitado();

            DataRow tbl_row_transporte;
            foreach (var trans in list)
            {
                tbl_row_transporte = tbl_transporte.NewRow();
                tbl_row_transporte["Administrado"] = trans.externo;
                tbl_row_transporte["Código de Habilitación"] = trans.cod_habilitacion;
                tbl_row_transporte["Placa"] = trans.placa;
                tbl_row_transporte["Carroceria"] = trans.nombre_carroceria;
                tbl_row_transporte["Furgón"] = trans.nombre_furgon;
                tbl_row_transporte["Nombre del Protocolo"] = trans.nombre;
                tbl_row_transporte["Fecha de emisión"] = trans.fec_emi;
                tbl_row_transporte["Fecha de inicio de Vigencia"] = trans.fec_ini;
                tbl_row_transporte["Fecha de fin de Vigencia"] = trans.fec_fin;
                tbl_transporte.Rows.Add(tbl_row_transporte);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_transporte;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Listado_Transportes_habilitados.xls");
            Response.ContentType = "application/excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }



        #endregion

        #region Cedula de Notificacion
        public void CedulaNotificacionWord(CargaWordCedulaNotificacion tableData)
        {
            DateTime fecha_PATH = DateTime.Now;
            //DESARROLLO
            // string path = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";
            
            //alterar en web.config para pre-produccion o/u produccion
            string path = ConfigurationManager.AppSettings["cedula"];

            byte[] byteArray = System.IO.File.ReadAllBytes(path+@"/CÉDULANOTIFICACIÓN.docx");

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, (int)byteArray.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
                {

                    IDictionary<String, BookmarkStart> bookmarkMaps = new Dictionary<String, BookmarkStart>();

                    foreach (BookmarkStart bookmarkStart in wordDocument.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                    {
                        bookmarkMaps[bookmarkStart.Name] = bookmarkStart;
                    }

                    Run NOM_DOC_TITULO = bookmarkMaps["A_NON_DOC"].NextSibling<Run>();
                    NOM_DOC_TITULO.GetFirstChild<Text>().Text = tableData.NON_DOC;

                    Run NUM_DOC = bookmarkMaps["NUM_DOC"].NextSibling<Run>();
                    NUM_DOC.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                    Run DIRECCION_CDL_NOTIF = bookmarkMaps["DIRECCION_CDL_NOTIF"].NextSibling<Run>();
                    DIRECCION_CDL_NOTIF.GetFirstChild<Text>().Text = tableData.DIRECCION_CDL_NOTIF;

                    Run EMPRESA_CDL_NOTIF = bookmarkMaps["EMPRESA_CDL_NOTIF"].NextSibling<Run>();
                    EMPRESA_CDL_NOTIF.GetFirstChild<Text>().Text = tableData.EMPRESA_CDL_NOTIF;

                    Run FOLIA_CDL_NOTIF = bookmarkMaps["FOLIA_CDL_NOTIF"].NextSibling<Run>();
                    FOLIA_CDL_NOTIF.GetFirstChild<Text>().Text = tableData.FOLIA_CDL_NOTIF;

                    Run DOC_NOTIFICAR_CDL_NOTIF = bookmarkMaps["DOC_NOTIFICAR_CDL_NOTIF"].NextSibling<Run>();
                    DOC_NOTIFICAR_CDL_NOTIF.GetFirstChild<Text>().Text = tableData.DOC_NOTIFICAR_CDL_NOTIF;

                    //Run EXP_O_HT_N_CDL_NOTIF = bookmarkMaps["EXP_O_HT_N_CDL_NOTIF"].NextSibling<Run>();
                    //EXP_O_HT_N_CDL_NOTIF.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;

                    #region Acta de CD notificacion 1
                    Run A_NON_DOC1 = bookmarkMaps["A_NON_DOC1"].NextSibling<Run>();
                    A_NON_DOC1.GetFirstChild<Text>().Text = tableData.NON_DOC;

                    Run A_DIRECCION_CDL_NOTIF1 = bookmarkMaps["A_DIRECCION_CDL_NOTIF1"].NextSibling<Run>();
                    A_DIRECCION_CDL_NOTIF1.GetFirstChild<Text>().Text = tableData.DIRECCION_CDL_NOTIF;

                    Run A_DOC_NOTIFICAR_CDL_NOTIF1 = bookmarkMaps["A_DOC_NOTIFICAR_CDL_NOTIF1"].NextSibling<Run>();
                    A_DOC_NOTIFICAR_CDL_NOTIF1.GetFirstChild<Text>().Text = tableData.DOC_NOTIFICAR_CDL_NOTIF;

                    Run A_EXP_O_HT_N_CDL_NOTIF1 = bookmarkMaps["A_EXP_O_HT_N_CDL_NOTIF1"].NextSibling<Run>();
                    A_EXP_O_HT_N_CDL_NOTIF1.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;

                    #endregion

                    #region Acta de CD notificacion 2
                    Run A_NON_DOC2 = bookmarkMaps["A_NON_DOC2"].NextSibling<Run>();
                    A_NON_DOC2.GetFirstChild<Text>().Text = tableData.NON_DOC;

                    Run A_DIRECCION_CDL_NOTIF2 = bookmarkMaps["A_DIRECCION_CDL_NOTIF2"].NextSibling<Run>();
                    A_DIRECCION_CDL_NOTIF2.GetFirstChild<Text>().Text = tableData.DIRECCION_CDL_NOTIF;

                    Run A_DOC_NOTIFICAR_CDL_NOTIF2 = bookmarkMaps["A_DOC_NOTIFICAR_CDL_NOTIF2"].NextSibling<Run>();
                    A_DOC_NOTIFICAR_CDL_NOTIF2.GetFirstChild<Text>().Text = tableData.DOC_NOTIFICAR_CDL_NOTIF;

                    Run A_EXP_O_HT_N_CDL_NOTIF2 = bookmarkMaps["A_EXP_O_HT_N_CDL_NOTIF2"].NextSibling<Run>();
                    A_EXP_O_HT_N_CDL_NOTIF2.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;

                    #endregion

                    #region Acta de CD notificacion 3
                    Run A_NON_DOC3 = bookmarkMaps["A_NON_DOC3"].NextSibling<Run>();
                    A_NON_DOC3.GetFirstChild<Text>().Text = tableData.NON_DOC;

                    Run A_DIRECCION_CDL_NOTIF3 = bookmarkMaps["A_DIRECCION_CDL_NOTIF3"].NextSibling<Run>();
                    A_DIRECCION_CDL_NOTIF3.GetFirstChild<Text>().Text = tableData.DIRECCION_CDL_NOTIF;

                    Run A_DOC_NOTIFICAR_CDL_NOTIF3 = bookmarkMaps["A_DOC_NOTIFICAR_CDL_NOTIF3"].NextSibling<Run>();
                    A_DOC_NOTIFICAR_CDL_NOTIF3.GetFirstChild<Text>().Text = tableData.DOC_NOTIFICAR_CDL_NOTIF;

                    Run A_EXP_O_HT_N_CDL_NOTIF3 = bookmarkMaps["A_EXP_O_HT_N_CDL_NOTIF3"].NextSibling<Run>();
                    A_EXP_O_HT_N_CDL_NOTIF3.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;

                    #endregion 

                    #region  Acta de CD notificacion 4
                    Run A_NON_DOC4 = bookmarkMaps["A_NON_DOC4"].NextSibling<Run>();
                    A_NON_DOC4.GetFirstChild<Text>().Text = tableData.NON_DOC;

                    Run A_DIRECCION_CDL_NOTIF4 = bookmarkMaps["A_DIRECCION_CDL_NOTIF4"].NextSibling<Run>();
                    A_DIRECCION_CDL_NOTIF4.GetFirstChild<Text>().Text = tableData.DIRECCION_CDL_NOTIF;

                    Run A_DOC_NOTIFICAR_CDL_NOTIF4 = bookmarkMaps["A_DOC_NOTIFICAR_CDL_NOTIF4"].NextSibling<Run>();
                    A_DOC_NOTIFICAR_CDL_NOTIF4.GetFirstChild<Text>().Text = tableData.DOC_NOTIFICAR_CDL_NOTIF;

                    Run A_EXP_O_HT_N_CDL_NOTIF4 = bookmarkMaps["A_EXP_O_HT_N_CDL_NOTIF4"].NextSibling<Run>();
                    A_EXP_O_HT_N_CDL_NOTIF4.GetFirstChild<Text>().Text = tableData.EXP_O_HT_N_CDL_NOTIF;


                    #endregion

                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                }
                string nuevopath = Path.Combine(path, "/CEDULA_NOTIFICACION_" + fecha_PATH.ToString("ddMMyy")+".docx");
                stream.Close();
                System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());
                
                //Process process = new Process();
               // process.StartInfo.FileName = Server.MapPath(nuevopath);
               // process.Start();
                Process.Start("WINWORD.EXE", nuevopath);
                
            }
        }

        #endregion

        #region RESOLUCION DIRECTORAL
        [HttpGet]
        public void ResolucionDirectoralWord(CargaWordResolucionDirectoral tableData)
        {
            DateTime fecha_PATH = DateTime.Now;
            
             DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["resoluciondirectoral"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/RESOLUCION_DIRECTORAL.docx");

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, (int)byteArray.Length);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
                {

                    IDictionary<String, BookmarkStart> bookmarkMaps = new Dictionary<String, BookmarkStart>();

                    foreach (BookmarkStart bookmarkStart in wordDocument.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                    {
                        bookmarkMaps[bookmarkStart.Name] = bookmarkStart;
                    }

                    Run EMPRESA = bookmarkMaps["EMPRESA"].NextSibling<Run>();
                    EMPRESA.GetFirstChild<Text>().Text = tableData.EMPRESA_CDL_NOTIF;

                    Run EMPRESA_1 = bookmarkMaps["EMPRESA_1"].NextSibling<Run>();
                    EMPRESA_1.GetFirstChild<Text>().Text = tableData.EMPRESA_CDL_NOTIF;

                    Run EMPRESA_2 = bookmarkMaps["EMPRESA_2"].NextSibling<Run>();
                    EMPRESA_2.GetFirstChild<Text>().Text = tableData.EMPRESA_CDL_NOTIF;

                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    Run RUC = bookmarkMaps["RUC"].NextSibling<Run>();
                    RUC.GetFirstChild<Text>().Text = tableData.RUC;

                    Run RUC_1 = bookmarkMaps["RUC_1"].NextSibling<Run>();
                    RUC_1.GetFirstChild<Text>().Text = tableData.RUC;

                    Run RUC_2 = bookmarkMaps["RUC_2"].NextSibling<Run>();
                    RUC_2.GetFirstChild<Text>().Text = tableData.RUC;

                    Run EXPEDIENTE = bookmarkMaps["EXPEDIENTE"].NextSibling<Run>();
                    EXPEDIENTE.GetFirstChild<Text>().Text = tableData.EXPEDIENTE;

                    Run EXPEDIENTE_1 = bookmarkMaps["EXPEDIENTE_1"].NextSibling<Run>();
                    EXPEDIENTE_1.GetFirstChild<Text>().Text = tableData.EXPEDIENTE;

                    Run EXPEDIENTE_2 = bookmarkMaps["EXPEDIENTE_2"].NextSibling<Run>();
                    EXPEDIENTE_2.GetFirstChild<Text>().Text = tableData.EXPEDIENTE;
                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                }

                // string nuevopath = path + @"\RESOLUCION_DIRECTORAL_"+fecha_PATH.ToString("ddMMyy")+".docx";
                string nuevopath = Path.Combine(path, "RESOLUCION_DIRECTORAL_" + fecha_PATH.ToString("ddMMyy") + ".docx");

                System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());
                //Process process = new Process();
                //process.StartInfo.FileName = nuevopath;
                //process.Start();
                Process.Start("WINWORD.EXE", nuevopath);
            }
         }
        #endregion

        #region Informe
        public void informeUTIWord(CargaWordInformeUTI informe)
        {
            object missing = System.Reflection.Missing.Value;
            Word.Application application = new Word.Application();
            application.Visible = true;
            Word.Document document = application.Documents.Open(@"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc\SIGESDOC.INFORMEUTI\bin\Debug\INFORME_UTI.docx", ref missing, false);

        }

        #endregion
        #region OFICIO

        [HttpGet]
        public void OficioWord(CargaOficioWord oficioWord)
        {
            object missing = System.Reflection.Missing.Value;
            Word.Application application = new Word.Application();
            Word.Document document = application.Documents.Open(@"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc\SIGESDOC.OFICIO\bin\Debug\OFICIO.docx", ref missing);

        }

        #endregion

        //Add by HM - 27/11/2019
        [AllowAnonymous]
        public ActionResult Consulta_Documentos_emitidos_Externos(int page = 1, int cmbtipo_documento = 0, string asunto = "", int cmbanno_documento = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento
                    ))
                {

                    List<SelectListItem> lista_documentos = new List<SelectListItem>();

                    lista_documentos.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    if (cmbanno_documento == 0)
                    {
                        cmbanno_documento = DateTime.Now.Year;
                    }

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("", "0"))
                    {
                        lista_documentos.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                    };

                    List<SelectListItem> anno_Documento = new List<SelectListItem>();

                    for (int i = DateTime.Now.Year; i >= 2015; i--)
                    {

                        anno_Documento.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }
                    ViewBag.lst_anno_documento = anno_Documento;

                    ViewBag.lst_tipo_documento = lista_documentos;

                    //Enviamos la Oficina Para Verificacion en JAVASCRIPT
                    ViewBag.ID_OFICINA_DIRECCION = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

                    string var_evaluador = "";
                    if (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "18")
                    {
                        var_evaluador = "20565429656 - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    }

                    DataTable tbl = new DataTable();

                    tbl.Columns.Add("NOM_TIPO_DOCUMENTO");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("FECHA_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("ANEXO");
                    tbl.Columns.Add("ID_DOC_DHCPA");
                    tbl.Columns.Add("PDF");
                    tbl.Columns.Add("RUTA_PDF");

                    //Add by HM - 13/11/2019
                    tbl.Columns.Add("RUC");
                    tbl.Columns.Add("NUM_DOC");
                    tbl.Columns.Add("EVALUADOR_CDL_NOTIF");
                    tbl.Columns.Add("DIRECCION_CDL_NOTIF");
                    tbl.Columns.Add("EMPRESA_CDL_NOTIF");
                    tbl.Columns.Add("FOLIA_CDL_NOTIF");
                    tbl.Columns.Add("DOC_NOTIFICAR_CDL_NOTIF");
                    tbl.Columns.Add("EXP_O_HT_CDL_NOTIF");
                    tbl.Columns.Add("EXP_O_HT_N_CDL_NOTIF");


                    var Oficina_Global = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

                    var documento_dhcpa = _HabilitacionesService.Lista_Documentos_externos(var_evaluador, cmbtipo_documento, asunto, cmbanno_documento, Oficina_Global);

                    foreach (var result in documento_dhcpa)
                    {
                        string ruta_x = "";
                        if (result.pdf == "1")
                        {
                            ruta_x = "/Habilitaciones/var_documento_dhcpa_pdf/" + result.id_doc_dhcpa.ToString();
                        }
                        tbl.Rows.Add(
                            result.nom_tipo_documento,
                            result.num_doc.ToString() + result.nom_doc,
                            result.fecha_doc.Value.ToShortDateString(),
                            result.asunto,
                            result.anexos,
                            result.id_doc_dhcpa,
                            result.pdf,
                            ruta_x,

                            //Add by HM - 13/11/2019
                            result.ruc,
                            result.num_doc,
                            result.evaluador_cdl_notif,
                            result.direccion_cdl_notif,
                            result.empresa_cdl_notif,
                            result.folia_cdl_notif,
                            result.doc_notificar_cdl_notif,
                            result.exp_o_ht_cdl_notif,
                            result.exp_o_ht_n_cdl_notif
                            );
                    };

                    ViewData["documentos_tabla"] = tbl;

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

        //Add by HM - 28/11/2019
        [AllowAnonymous]
        public ActionResult Nuevo_Documento_Externos()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    List<SelectListItem> lista_sedes_externo = new List<SelectListItem>();
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    lista_sedes_externo.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_personal = new List<SelectListItem>();

                    lista_sedes.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR SEDE",
                        Value = "0"
                    });

                    int id_ofi_ruc = 0;

                    foreach (var result in _GeneralService.Recupera_oficina_all_x_ruc("20565429656"))
                    {
                        if (result.id_ofi_padre == null)
                        {
                            id_ofi_ruc = result.id_oficina;
                        }
                    };


                    foreach (var result in _GeneralService.Recupera_sede_all(id_ofi_ruc))
                    {
                        lista_sedes.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_sede.ToString()
                        }
                        );
                    };

                    Lista_Oficina_destino.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR OFICINA",
                        Value = "0"
                    });

                    lista_personal.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR PERSONAL",
                        Value = ""
                    });

                    List<SelectListItem> lista_tipo_documento = new List<SelectListItem>();

                    foreach (var result in _GeneralService.Recupera_tipo_documento_todo("", "0"))
                    {
                        lista_tipo_documento.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_tipo_documento.ToString()
                            });
                    };

                    List<SelectListItem> lista_archivadores = new List<SelectListItem>();

                    lista_archivadores.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR ARCHIVADOR",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Archivador())
                    {
                        lista_archivadores.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_archivador.ToString()
                            });
                    };

                    List<SelectListItem> lista_filiales = new List<SelectListItem>();

                    lista_filiales.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR FILIALES",
                        Value = ""
                    });

                    foreach (var result in _HabilitacionesService.GetAll_Filial())
                    {
                        lista_filiales.Add
                            (new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_filial.ToString()
                            });
                    };

                    List<SelectListItem> Lista_per_crea = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_per_crea.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina = lista_personal;

                    ViewBag.lstsede_destino_externo = lista_sedes_externo;
                    ViewBag.lstOficina_destino_externo = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina_externo = lista_personal;

                    ViewBag.lst_persona_crea = Lista_per_crea;
                    ViewBag.list_tip_documento_dhcpa = lista_tipo_documento;
                    ViewBag.list_archivador = lista_archivadores;
                    ViewBag.lista_filiales = lista_filiales;

                    ViewBag.user_document = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    ViewBag.user_perfil = HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim();

                    //Enviamos la Oficina Para Verificacion en JAVASCRIPT
                    ViewBag.ID_OFICINA_DIRECCION = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

                    DocumentodhcpaViewModel model_doc_dhcpa = new DocumentodhcpaViewModel();

                    return View(model_doc_dhcpa);

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

        //Add by HM - 28/11/2019
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Documento_Externos(DocumentodhcpaViewModel model)
        {
            var Oficina_Global = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28 : Atención al Cliente
                {
                    model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo_oficina_direccion(model.id_tipo_documento, Oficina_Global) + 1;
                    model.nom_doc = "-" + DateTime.Now.Year.ToString() + "- DHCPA/SANIPES";
                    DocumentoDhcpaRequest req_documento_dhcpa = ModelToRequest.Documento_dhcpa(model);
                    req_documento_dhcpa.fecha_registro = DateTime.Now;
                    req_documento_dhcpa.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                    req_documento_dhcpa.id_oficina_direccion = Oficina_Global;//Nuevo

                    model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa);
                    req_documento_dhcpa.id_doc_dhcpa = model.id_doc_dhcpa;

                    if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                    {
                        try
                        {
                            DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                            req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                            req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_seguimiento.activo = "1";
                            req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                        }
                        catch (Exception) { }
                    }

                    if (model.documento_dhcpa_detalle != null)
                    {
                        foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                        {
                            DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                            req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa.id_doc_dhcpa;
                            req_documento_dhcpa_detalle.activo = "1";
                            req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                            obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                        }
                    }

                    if (model.ind_agregar_celula == 1)
                    {
                        string mensaje = "";
                        mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;

                        if (model.id_tipo_documento == 136)
                        {
                            model.doc_notificar_cdl_notif = model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }

                        // 21 : CEDULA DE NOTIFICACION
                        model.id_tipo_documento = 21;
                        model.num_doc = _HabilitacionesService.CountDocumentos_x_tipo(model.id_tipo_documento) + 1;

                        DocumentoDhcpaRequest req_documento_dhcpa2 = ModelToRequest.Documento_dhcpa(model);
                        req_documento_dhcpa2.fecha_registro = DateTime.Now;
                        req_documento_dhcpa2.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_documento_dhcpa2.id_oficina_direccion = Oficina_Global;//nuevo

                        model.id_doc_dhcpa = _HabilitacionesService.Create_documento_dhcpa(req_documento_dhcpa2);
                        req_documento_dhcpa2.id_doc_dhcpa = model.id_doc_dhcpa;

                        if (model.exp_o_ht_n_cdl_notif != "" && model.exp_o_ht_n_cdl_notif != null)
                        {
                            try
                            {
                                DetSegDocDhcpaRequest req_documento_dhcpa_seguimiento = new DetSegDocDhcpaRequest();
                                req_documento_dhcpa_seguimiento.id_seguimiento = _HabilitacionesService.Consulta_expediente_x_expediente(model.exp_o_ht_n_cdl_notif).id_seguimiento;
                                req_documento_dhcpa_seguimiento.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_seguimiento.activo = "1";
                                req_documento_dhcpa_seguimiento.id_det_dsdhcpa = _HabilitacionesService.Create_documento_dhcpa_seguimiento(req_documento_dhcpa_seguimiento);
                            }
                            catch (Exception) { }
                        }

                        if (model.documento_dhcpa_detalle != null)
                        {
                            foreach (detDocdhcpaViewModel obj in model.documento_dhcpa_detalle)
                            {
                                DocumentoDhcpaDetalleRequest req_documento_dhcpa_detalle = ModelToRequest.Documento_dhcpa_detalle(obj);
                                req_documento_dhcpa_detalle.id_doc_dhcpa = req_documento_dhcpa2.id_doc_dhcpa;
                                req_documento_dhcpa_detalle.activo = "1";
                                req_documento_dhcpa_detalle.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                req_documento_dhcpa_detalle.fecha_registro = DateTime.Now;
                                obj.id_doc_dhcpa_det = _HabilitacionesService.Create_documento_dhcpa_detalle(req_documento_dhcpa_detalle);
                            }
                        }

                        try
                        {
                            mensaje = mensaje + ", Se creó el Documento : CEDULA DE NOTIFICACION N° " + model.num_doc.ToString() + model.nom_doc;
                            @ViewBag.Mensaje = mensaje;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }

                    }
                    else
                    {
                        try
                        {
                            @ViewBag.Mensaje = "Se creó el Documento : " + model.nom_tipo_documento + " N° " + model.num_doc.ToString() + model.nom_doc;
                        }
                        catch (Exception)
                        {
                            @ViewBag.Mensaje = "";
                        }
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

        //Add by HM - 28/11/2019
        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_document_externo()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    int id_doc_dhcpa = 0;

                    try
                    {
                        id_doc_dhcpa = Convert.ToInt32(Session["archivo_document_id_doc_dhcpa"].ToString());
                        Session.Remove("archivo_document_id_doc_dhcpa");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    DocumentoDhcpaResponse doc = new DocumentoDhcpaResponse();
                    doc = _HabilitacionesService.Lista_Documento_dhcpa_x_id_rs(id_doc_dhcpa);

                    ViewBag.Str_documento = "Documento: " + _HojaTramiteService.Consult_tipo_docuemnto(doc.id_tipo_documento ?? 0) + " Nº " + doc.num_doc.ToString() + " " + doc.nom_doc;
                    ViewBag.id_documento_dhcpa = id_doc_dhcpa.ToString();
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

        //Add by HM - 28/11/2019
        [HttpPost]
        public ActionResult Adjuntar_archivo_document_externo(HttpPostedFileBase file, int id_doc_dhcpa)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                     (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                     (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[14].Trim() == "1" // Acceso a Nuevo Documento DHCPA
                     && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "17" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "7" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18"))))
                // Oficina 17: Sub Dirección de Certificaciones ó Oficina 7: Direccion de HyCPA ó Oficina 28: Atención al Cliente
                {

                    DocumentoDhcpaRequest doc_rq = new DocumentoDhcpaRequest();
                    doc_rq = _HabilitacionesService.Lista_Documento_dhcpa_x_id_rq(id_doc_dhcpa);

                    doc_rq.pdf = "1";

                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCUMENTOS_DHCPA"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, doc_rq.id_doc_dhcpa.ToString() + ".pdf"));
                        _HabilitacionesService.Update_documento_dhcpa(doc_rq);
                    }
                    //return File("//Srvdnet/sigesdoc/PE-OCTUBRE15.pdf", "application/pdf");
                    @ViewBag.Mensaje = "Se guardo el archivo correctamente";
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

        //Add by HM - 28/11/2019
        [AllowAnonymous]
        public ActionResult variable_Subir_archivo_doc_externo(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_document_id_doc_dhcpa"] = id;
                return RedirectToAction("Adjuntar_archivo_document_externo", "Habilitaciones");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        //Add by HM - 28/11/2019
        public ActionResult Export_Excel_documentos_emitidos_externo(string para1 = "", string para2 = "", string para3 = "")
        {

            DataTable tbl_documento = new DataTable();
            tbl_documento.Columns.Add("Nro Documento");
            tbl_documento.Columns.Add("Fecha Documento");
            tbl_documento.Columns.Add("Destinatario");
            tbl_documento.Columns.Add("Destino");
            tbl_documento.Columns.Add("Tipo Documento");
            tbl_documento.Columns.Add("Asunto");
            tbl_documento.Columns.Add("Anexo");
            tbl_documento.Columns.Add("OD");
            tbl_documento.Columns.Add("EVALUADOR");

            var list = _HabilitacionesService.Lista_Documentos_x_tipo_documento_oficina_direccion(Convert.ToInt32(para1), Convert.ToInt32(para2), Convert.ToInt32(para3));

            DataRow tbl_row_documento;
            foreach (var document in list)
            {

                string lugar_destino = "";
                string persona_destino = "";

                var var_destin = _HabilitacionesService.Lista_Destino_Documentos_x_tipo_documento(document.id_doc_dhcpa);

                foreach (var destinity in var_destin)
                {
                    if (lugar_destino == "")
                    {
                        lugar_destino = destinity.lugar_destino;
                        persona_destino = destinity.persona_destino;
                    }
                    else
                    {
                        lugar_destino = lugar_destino + "/" + destinity.lugar_destino;
                        persona_destino = persona_destino + "/" + destinity.persona_destino;
                    }
                }

                tbl_row_documento = tbl_documento.NewRow();
                tbl_row_documento["Nro Documento"] = document.num_doc.ToString();
                tbl_row_documento["Fecha Documento"] = document.fecha_doc.Value.ToShortDateString();
                tbl_row_documento["Destinatario"] = persona_destino;
                tbl_row_documento["Destino"] = lugar_destino;
                tbl_row_documento["Tipo Documento"] = document.nom_tipo_documento;
                tbl_row_documento["Asunto"] = document.asunto;
                tbl_row_documento["Anexo"] = document.anexos;
                tbl_row_documento["OD"] = document.nom_filial;
                tbl_row_documento["EVALUADOR"] = document.usuario_registro;
                tbl_documento.Rows.Add(tbl_row_documento);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_documento;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Documentos.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }


    }
}