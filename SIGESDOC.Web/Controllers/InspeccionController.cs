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

// DATOS_USUARIO
// 00 - RUC
// 01 - DNI_CE
// 02 - NOM_EMPRESA
// 03 - NOM_USUARIO
// 04 - ID_OFICINA_DIRECCION
// 05 - ID_PERFIL
// 06 - NOM_PERFIL
// 07 - ID_SISTEMA
// 08 - NOM_SEDE_OFI
// 09 - ACCESO

// DATOS_ACCESO_09

// CREAR_HT_00;
// CONSULTA_GENERAL_01;
// CONSULTA_MIS_HT_02;
// BANDEJA_GENERAL_03;
// REGISTRAR_PERSONAS_04;
// REGISTRAR_ENTIDAD_05;
// REPORTES_06;
// QUITAR_ARCHIVO_07;
// QUITAR_ATENDIDO_08;
// NUEVA_EMBARCACION_10;
// NUEVA_FACTURA_11;
// NUEVO_EXPEDIENTE_12;
// NUEVO_SEGUI_EVALUADOR_13;
// NUEVO_DOCUMENTO_14;
// NUEVO_PROTOCOLO_15;
// NUEVA_PLANTA_16;
// CONSULTA_SOLICITUD_DHCPA_17;
// CONSULTA_DOCUMENTO_OD_POR_RECIBIR_18;
// NUEVO_ALMACEN_19;
// PUBLICAR_PROTOCOLOS_20;
// NUEVA_CONCESION_21;
// NUEVO_DESEMBARCADERO_22;
// PERMISO_REGISTRO_OD_23;
// PERMISO_CONSULTA_PEDIDO_HT_24;
// NUEVO_TRANSPORTE_25;
// REPORTE_TUPA_SDHPA_26;
// CONSULTA_DOC_X_OFICINA_27;
// REPORTE_GENERAL_SANIPES_28;
// PERMISO_DOCU_AUTOMA_29;
// PERMISO_RECEPCION_SS_OD_30;
// PERMISO_RECEPCION_SS_INSPECTOR_31;

namespace SIGESDOC.Web.Controllers
{
    public class InspeccionController : Controller
    {

        private readonly IHabilitacionesService _HabilitacionesService;
        private readonly IInspeccionService _InspeccionService;
        private readonly IGeneralService _GeneralService;
        private readonly IOficinaService _OficinaService;
        private readonly IHojaTramiteService _HojaTramiteService;

        public InspeccionController(IHabilitacionesService HabilitacionesService
            , IGeneralService GeneralService
            , IOficinaService OficinaService
            , IInspeccionService InspeccionService
            , IHojaTramiteService HojaTramiteService)
        {
            _HabilitacionesService = HabilitacionesService;
            _GeneralService = GeneralService;
            _OficinaService = OficinaService;
            _InspeccionService = InspeccionService;
            _HojaTramiteService = HojaTramiteService;
        }

        [AllowAnonymous]
        public ActionResult Consulta_Solicitud_Inspeccion_OD(string estado="",string expediente="", string solicitud="")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" //Administrador
                    || HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[30].Trim() == "1" // JEFE_OD_30
                    ))
                {
                    List<SelectListItem> Lista_Inspector = new List<SelectListItem>();
                    ViewBag.lst_inspector = Lista_Inspector;
                    

                    _InspeccionService.recibe_soli_insp(HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("EXPEDIENTE");
                    tbl.Columns.Add("ID_SEGUIMIENTO");
                    tbl.Columns.Add("ID_SOL_INS");
                    tbl.Columns.Add("ID_ESTADO");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("FECHA_CREA_TEXT");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("NOMBRE_ESTADO");
                    tbl.Columns.Add("NOMBRE_TIPO_SOLICITUD");
                    tbl.Columns.Add("SOLICITUD_INSPECCION");
                    tbl.Columns.Add("ID_OD_INSP");
                    tbl.Columns.Add("PERSONA_CONTACTO");
                    tbl.Columns.Add("TELEFONO_OFICINA");
                    tbl.Columns.Add("TELEFONO_PLANTA");
                    tbl.Columns.Add("CORREO");
                    tbl.Columns.Add("RESOLUCION");
                    tbl.Columns.Add("OBSERVACIONES");
                    tbl.Columns.Add("INSPECTOR");
                    tbl.Columns.Add("FEC_RECEPCION_INSP");
                    tbl.Columns.Add("FEC_INSPECCION");
                    tbl.Columns.Add("EXPEDIENTE_ID_SEGUIMIENTO");
                    tbl.Columns.Add("ID_OD_INSP_ID_SOL_INS");
                    

                    var solicitud_inspeccion = _InspeccionService.Recupera_lista_si_od(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).Where(x => x.expediente.Contains(expediente.Trim().ToUpper()) && x.nombre_estado.Contains(estado.Trim().ToUpper()) && x.solicitud_inspeccion.Contains(solicitud.Trim().ToUpper()));

                    foreach (var result in solicitud_inspeccion)
                    {
                        tbl.Rows.Add(
                        result.expediente,
                        result.id_seguimiento,
                        result.id_sol_ins,
                        result.id_estado,
                        result.fecha_crea,
                        result.fecha_crea_text,
                        result.externo,
                        result.nombre_estado,
                        result.nombre_tipo_solicitud,
                        result.solicitud_inspeccion,
                        result.id_od_insp,
                        result.persona_contacto,
                        result.telefono_oficina,
                        result.telefono_planta,
                        result.correo,
                        result.resolucion,
                        result.observaciones,
                        result.nom_inspector,
                        result.fecha_recepcion_inspector_text,
                        result.fecha_inspeccion_text,
                        result.expediente+"|"+result.id_seguimiento.ToString(),
                        result.id_od_insp.ToString() + "|" + result.id_sol_ins.ToString()
                        );

                    };

                    ViewData["solicit_insp_tabla"] = tbl;

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

        //Consulta_Solicitud_Inspeccion_x_inspector_OD
        [AllowAnonymous]
        public ActionResult c_solicitud_inspeccion_x_inspector_od(string estado = "", string expediente = "", string solicitud = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" //Administrador
                    || HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[31].Trim() == "1" // INSPEC_OD_31
                    ))
                {
                    _InspeccionService.recibe_soli_insp_x_inspector(HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HttpContext.User.Identity.Name.Split('|')[1].Trim());

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("FEC_RECEPCION_INSP");
                    tbl.Columns.Add("FEC_INSPECCION");
                    tbl.Columns.Add("EXPEDIENTE");
                    tbl.Columns.Add("ID_SEGUIMIENTO");
                    tbl.Columns.Add("ID_SOL_INS");
                    tbl.Columns.Add("ID_ESTADO");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("FECHA_CREA_TEXT");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("NOMBRE_ESTADO");
                    tbl.Columns.Add("NOMBRE_TIPO_SOLICITUD");
                    tbl.Columns.Add("SOLICITUD_INSPECCION");
                    tbl.Columns.Add("ID_OD_INSP");
                    tbl.Columns.Add("PERSONA_CONTACTO");
                    tbl.Columns.Add("TELEFONO_OFICINA");
                    tbl.Columns.Add("TELEFONO_PLANTA");
                    tbl.Columns.Add("CORREO");
                    tbl.Columns.Add("RESOLUCION");
                    tbl.Columns.Add("OBSERVACIONES");
                    tbl.Columns.Add("EXPEDIENTE_ID_SEGUIMIENTO");
                    tbl.Columns.Add("SOLICITUD_INSPECCION_ID_SOL_INS");

                    var solicitud_inspeccion = _InspeccionService.Recupera_lista_si_od_x_inspector(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HttpContext.User.Identity.Name.Split('|')[1].Trim()).Where(x => x.expediente.Contains(expediente.Trim().ToUpper()) && x.nombre_estado.Contains(estado.Trim().ToUpper()) && x.solicitud_inspeccion.Contains(solicitud.Trim().ToUpper()));

                    foreach (var result in solicitud_inspeccion)
                    {
                        tbl.Rows.Add(
                        result.fecha_recepcion_inspector_text,
                        result.fecha_inspeccion_text,
                        result.expediente,
                        result.id_seguimiento,
                        result.id_sol_ins,
                        result.id_estado,
                        result.fecha_crea,
                        result.fecha_crea_text,
                        result.externo,
                        result.nombre_estado,
                        result.nombre_tipo_solicitud,
                        result.solicitud_inspeccion,
                        result.id_od_insp,
                        result.persona_contacto,
                        result.telefono_oficina,
                        result.telefono_planta,
                        result.correo,
                        result.resolucion,
                        result.observaciones,
                        result.expediente + "|" + result.id_seguimiento.ToString(),
                        result.solicitud_inspeccion + "|" + result.id_sol_ins.ToString()
                        );

                    };

                    ViewData["solicit_insp_tabla"] = tbl;

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
        public ActionResult Listar_historial_evaluador(int id_seguimiento = 0)
        {
            IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> historial_evaluador = new List<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result>();

            historial_evaluador = _HabilitacionesService.CONSULTA_HISTORIAL_EVALUADOR(id_seguimiento);

            return Json(historial_evaluador, JsonRequestBehavior.AllowGet);
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
        public ActionResult Listar_Observacion_x_seguimiento(int id_seguimiento = 0)
        {
            IEnumerable<SeguimientoDhcpaObservacionesResponse> observaciones = new List<SeguimientoDhcpaObservacionesResponse>();

            observaciones = _HabilitacionesService.Listar_Observacion_x_seguimiento(id_seguimiento).OrderBy(x => x.id_seg_dhcpa_observacion);

            return Json(observaciones, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_inspector_x_si(int id_od_insp)
        {
            List<SelectListItem> Lista_inspector = new List<SelectListItem>();

            Lista_inspector.Add(new SelectListItem()
            {
                Text = "SELECCIONAR INSPECTOR",
                Value = ""
            });

            var recupera_persona = _GeneralService.Recupera_personal_oficina(id_od_insp);

            foreach (var result in recupera_persona)
            {
                Lista_inspector.Add(new SelectListItem()
                {
                    Text = result.nom_persona,
                    Value = result.persona_num_documento.ToString()
                });
            };

            return Json(Lista_inspector, JsonRequestBehavior.AllowGet);
        }
        
        

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Asignar_inspector_ss(int id_sol_insp_od, string inspector, string fec_inspeccion)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" //Administrador
                    || HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[30].Trim() == "1" // JEFE_OD_30
                    ))
                {
                    _InspeccionService.asigna_inspector(id_sol_insp_od, inspector, fec_inspeccion);

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
        public ActionResult Atender_inspector_ss(int id_sol_insp_od)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" //Administrador
                    || HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[31].Trim() == "1" // JEFE_OD_30
                    ))
                {
                    _InspeccionService.atender_inspector(id_sol_insp_od);

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

        

    }
}