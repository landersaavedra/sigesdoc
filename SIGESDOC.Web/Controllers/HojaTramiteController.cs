using SIGESDOC.IAplicacionService;
using SIGESDOC.Response;
using SIGESDOC.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using SIGESDOC.Web.Models;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office;
using System.Reflection;

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

namespace SIGESDOC.Web.Controllers
{
    public class HojaTramiteController : Controller
    {
        private readonly IHojaTramiteService _HojaTramiteService;
        private readonly IGeneralService _GeneralService;
        private readonly IOficinaService _OficinaService;
        private readonly IHabilitacionesService _HabilitacionesService;
        
        public HojaTramiteController(IHojaTramiteService HojaTramiteService, IGeneralService GeneralService, IOficinaService OficinaService, IHabilitacionesService HabilitacionesService)
        {
            _HojaTramiteService = HojaTramiteService;
            _GeneralService = GeneralService;
            _OficinaService = OficinaService;
            _HabilitacionesService = HabilitacionesService;
        }

        [AllowAnonymous]
        public ActionResult Nuevo_Externo()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    HojaTramiteViewModel model = new HojaTramiteViewModel();

                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> Lista_RUC = new List<SelectListItem>();
                    List<SelectListItem> Lista_Oficinas_externas = new List<SelectListItem>();
                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
                    List<SelectListItem> lista_personal = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    List<SelectListItem> lista_tipo_pedido_siga = new List<SelectListItem>();

                    lista_tipo_pedido_siga.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR PEDIDO SIGA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.llenar_tipo_pedido_siga())
                    {
                        lista_tipo_pedido_siga.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_pedido_siga.ToString()
                        }
                            );
                    };

                    List<SelectListItem> lista_año_siga = new List<SelectListItem>();

                    lista_año_siga.Add(new SelectListItem()
                    {
                        Text = "AÑO",
                        Value = ""
                    });

                    for (int i = DateTime.Now.Year; i >= 2015; i--)
                    {
                        lista_año_siga.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        }
                            );
                    }

                    lista_tipo_documento_iden.Add(new SelectListItem()
                    {
                        Text = "RUC",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                            {
                                Text = result.siglas,
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
                        if (result.id_ofi_padre == null)
                        {

                            Lista_RUC.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.ruc
                            }
                            );
                        }
                    };

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

                    /*
                        int entra = 0;
                        foreach (var result in _GeneralService.Recupera_oficina_todo())
                        {
                            // Como la oficina que ingresa es la id_oficina=1 entonces no tiene que aparecer esa oficina como destino
                            if (result.ruc == "20565429656")
                            {

                                if (entra == 0)
                                {
                                    ViewBag.lstpersonal_oficina = _GeneralService.Recupera_personal_oficina(result.id_oficina).Select(c => new SelectListItem() { Text = c.nom_persona, Value = c.persona_num_documento.ToString() }).ToList();
                                    entra = 1;
                                }
                                Lista_Oficina_destino.Add(new SelectListItem()
                                {
                                    Text = result.nombre,
                                    Value = result.id_oficina.ToString()
                                });
                            }
                        };
                    */

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

                    ViewBag.lst_persona_crea = Lista_per_crea;

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).ThenBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.lst_tipo_documento_iden = lista_tipo_documento_iden;
                    ViewBag.lst_ruc_empresas = Lista_RUC;
                    ViewBag.vst_check_externo = "2";
                    ViewBag.lstOficina = Lista_Oficinas_externas;
                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina = lista_personal;
                    ViewBag.id_oficina_crea = HttpContext.User.Identity.Name.Split('|')[4].Trim();
                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("T", "0").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    ViewBag.cond_grabar = "0";
                    ViewBag.lst_tipo_pedido_siga = lista_tipo_pedido_siga;
                    List<SelectListItem> lista_direcciones = new List<SelectListItem>();
                    List<SelectListItem> lista_oficinas = new List<SelectListItem>();
                    List<SelectListItem> lista_personas = new List<SelectListItem>();
                    ViewBag.lst_direcciones = lista_direcciones;
                    ViewBag.lst_oficinas = lista_oficinas;
                    ViewBag.lst_persona_ext = lista_personas;
                    ViewBag.lst_anno_siga = lista_año_siga;

                    if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                    {
                        ViewBag.ver_numero_doc = "0";
                    }
                    else
                    {
                        ViewBag.ver_numero_doc = "1";
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Externo(HojaTramiteViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    try
                    {
                        model.fecha_emision = DateTime.Now;
                        model.fecha_envio = DateTime.Now;
                        model.usuario_emision = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        if (model.persona_crea == "" || model.persona_crea == null)
                        {
                            model.persona_crea = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        }
                        HojaTramiteRequest request = ModelToRequest.HojaTramite(model);
                        if (model.HT_PADRE != "" && model.HT_PADRE != null)
                        {
                            request.numero_padre = _HojaTramiteService.GetAllHT_x_HojaTramite(model.HT_PADRE).First().numero;
                        }
                        int cantidad_ht = _HojaTramiteService.CountHT();
                        //                    request.numero = DateTime.Now.Year.ToString() + (cantidad_ht + 1).ToString().PadLeft(6, '0');
                        request.hoja_tramite = _HojaTramiteService.Create_numero(model.id_tipo_tramite);
                        request.editar = "1";
                        if (model.id_tipo_tramite == 1)
                        {
                            request.clave = _HojaTramiteService.genera_clave_documento_externo();
                            model.clave = request.clave;
                        }
                        model.numero = _HojaTramiteService.Create(request);
                        model.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

                        model.nom_oficina_crea = _GeneralService.recupera_oficina(model.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(model.oficina_crea).nombre;

                        DocumentoRequest request2 = ModelToRequest.documento(model);
                        request2.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request2.id_indicador_documento = 1;
                        request2.num_ext = 1;

                        if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                        {
                            if (_GeneralService.Consulta_Tipo_Documento(model.id_tipo_documento).First().tp_e_i.Trim() != "E")
                            {
                                if (model.id_tipo_documento != 149 && model.id_tipo_documento != 150)
                                {
                                    if (model.id_tipo_documento != 169)
                                    {
                                        var ultimo = _HojaTramiteService.Recupera_Documento(model.oficina_crea, model.id_tipo_documento, DateTime.Now.Year);

                                        if (ultimo.Count() > 0) { request2.numero_documento = ultimo.First().numero_documento + 1; }
                                        else { request2.numero_documento = 1; }

                                        request2.nom_doc = "-" + DateTime.Now.Year.ToString() + "-" + _GeneralService.recupera_oficina(model.oficina_crea).siglas;
                                    }
                                    else
                                    {
                                        request2.nom_doc = "";
                                    }
                                }
                            }
                        }
                        

                        model.id_documento = _HojaTramiteService.Documento_Create(request2);


                        if (model.documento_detalle != null)
                        {
                            request2.documento_detalle = new List<DocumentoDetalleRequest>();

                            foreach (DocumentoDetalleViewModel obj in model.documento_detalle)
                            {
                                DocumentoDetalleRequest request3 = ModelToRequest.DocumentoDetalle(obj);
                                string indic = "";
                                if (request3.ind_01 == true) { indic = "1"; }
                                if (request3.ind_02 == true) { if (indic == "") { indic = "2"; } else { indic = indic + ",2"; } }
                                if (request3.ind_03 == true) { if (indic == "") { indic = "3"; } else { indic = indic + ",3"; } }
                                if (request3.ind_04 == true) { if (indic == "") { indic = "4"; } else { indic = indic + ",4"; } }
                                if (request3.ind_05 == true) { if (indic == "") { indic = "5"; } else { indic = indic + ",5"; } }
                                if (request3.ind_06 == true) { if (indic == "") { indic = "6"; } else { indic = indic + ",6"; } }
                                if (request3.ind_07 == true) { if (indic == "") { indic = "7"; } else { indic = indic + ",7"; } }
                                if (request3.ind_08 == true) { if (indic == "") { indic = "8"; } else { indic = indic + ",8"; } }
                                if (request3.ind_09 == true) { if (indic == "") { indic = "9"; } else { indic = indic + ",9"; } }
                                if (request3.ind_10 == true) { if (indic == "") { indic = "10"; } else { indic = indic + ",10"; } }
                                if (request3.ind_11 == true) { if (indic == "") { indic = "11"; } else { indic = indic + ",11"; } }
                                request3.indicadores = indic;
                                if (request3.id_cab_det_documento == 0)
                                {
                                    request3.id_cab_det_documento = null;
                                }
                                if (request3.observacion == null)
                                {
                                    request3.observacion = "";
                                }
                                request3.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                request3.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                                request3.fecha_crea = DateTime.Now;
                                request3.id_documento = model.id_documento;
                                request3.nom_oficina_crea = model.nom_oficina_crea;
                                request3.id_est_tramite = 1;
                                request3.flag_destino_principal = obj.flag_destino_principal;
                                request3.nom_oficina_destino = _GeneralService.recupera_oficina(request3.oficina_destino).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_destino).nombre;
                                _HojaTramiteService.Documento_detalle_Create(request3);
                            }
                        }
                        @ViewBag.Mensaje = "Hoja de Trámite Nro : " + request.hoja_tramite + " : " + model.numero.ToString() + "  Clave: " + model.clave;

                    }
                    catch (Exception e)
                    {
                        @ViewBag.Mensaje = e.Message;
                    }
                    return PartialView("_SuccessHT");
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
        public ActionResult llenar_datos_HT(int numero) /// ME QUEDE ACA
        {
            var Hoja_Tramite = _HojaTramiteService.GetAllHT_x_Numero_request(numero);
            return Json(Hoja_Tramite, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_Historial_HT(int numero) /// ME QUEDE ACA
        {
            IEnumerable<Response.SP_CONSULTA_HISTORIAL_HT_Result> Hoja_Tramite = new List<Response.SP_CONSULTA_HISTORIAL_HT_Result>();

            Hoja_Tramite = (from p in _HojaTramiteService.recupera_historial_ht(numero)
                            select new Response.SP_CONSULTA_HISTORIAL_HT_Result
                            {
                                est_tramite = p.est_tramite,
                                id_documento = p.id_documento,
                                id_det_documento = p.id_det_documento,
                                id_cab_det_documento = p.id_cab_det_documento,
                                fecha_crea = p.fecha_crea,
                                fecha_recepcion = p.fecha_recepcion,
                                documento = p.documento,
                                ruta_pdf = p.ruta_pdf,
                                designado = p.designado,
                                nom_sede = p.nom_sede,
                                nom_oficina = p.nom_oficina,
                                observacion = p.observacion,
                                observacion_fin = p.observacion_fin,
                                fecha_fin = p.fecha_fin
                            });
            return Json(Hoja_Tramite, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_HT_Padre(string buscador) /// ME QUEDE ACA
        {
            IEnumerable<HojaTramiteResponse> Hoja_Tramite = new List<HojaTramiteResponse>();

            buscador = buscador.ToUpper();

            Hoja_Tramite = (from p in _HojaTramiteService.GetAllHojaTramite_Padre()
                            where p.hoja_tramite.Contains(buscador) || p.asunto.Contains(buscador) || p.fecha_emision.ToShortDateString().Contains(buscador)
                            select new HojaTramiteResponse
                            {
                                hoja_tramite = p.hoja_tramite,
                                asunto = p.asunto,
                                fecha_emision_text = p.fecha_emision.ToShortDateString()
                            }).OrderByDescending(x => x.numero).Take(1000);

            return Json(Hoja_Tramite, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Llenar_oficina_sede_externo(int id_sede)
        {

            List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();

            Lista_Oficina_destino.Add(new SelectListItem()
            {
                Text = "SELECCIONAR OFICINA",
                Value = "0"
            });

            foreach (var result in _GeneralService.Recupera_oficina_all_x_sede(id_sede))
            {
                Lista_Oficina_destino.Add(new SelectListItem()
                {
                    Text = result.nombre,
                    Value = result.id_oficina.ToString()
                });
            };

            return Json(Lista_Oficina_destino, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Editar_HojaTramite(int id = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    List<SelectListItem> lista_tipo_pedido_siga = new List<SelectListItem>();

                    lista_tipo_pedido_siga.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR PEDIDO SIGA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.llenar_tipo_pedido_siga())
                    {
                        lista_tipo_pedido_siga.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_pedido_siga.ToString()
                        });
                    };

                    List<SelectListItem> lista_año_siga = new List<SelectListItem>();

                    lista_año_siga.Add(new SelectListItem()
                    {
                        Text = "AÑO",
                        Value = ""
                    });

                    for (int i = DateTime.Now.Year; i >= 2015; i--)
                    {
                        lista_año_siga.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }

                    ViewBag.lst_anno_siga = lista_año_siga;
                    ViewBag.lst_tipo_pedido_siga = lista_tipo_pedido_siga;
                    HojaTramiteViewModel model = new HojaTramiteViewModel();
                    HojaTramiteRequest ht_reques = new HojaTramiteRequest();
                    ht_reques = _HojaTramiteService.GetAllHT_x_Numero_request(id);
                    if (ht_reques.id_tipo_tramite == 1)
                    {
                        ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("E", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    }
                    else
                    {
                        ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("I", "1").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    }
                    DocumentoResponse doc_respon = new DocumentoResponse();
                    doc_respon = _HojaTramiteService.GetAllDocumento_x_Numero_HT(ht_reques.numero).First();
                    ViewBag.cond_grabar = "0";
                    ViewBag.Str_HT = ht_reques.hoja_tramite;
                    ViewBag.Str_id_documento = doc_respon.id_documento.ToString();
                    ViewBag.Str_id_HT = id.ToString();
                    model.id_tipo_documento = doc_respon.id_tipo_documento;
                    model.numero_documento = doc_respon.numero_documento;
                    model.nom_doc = doc_respon.nom_doc;
                    model.asunto = ht_reques.asunto;
                    model.referencia = ht_reques.referencia;
                    model.anexos = doc_respon.anexos;
                    model.folios = doc_respon.folios;
                    model.id_tipo_pedido_siga = ht_reques.id_tipo_pedido_siga;
                    model.pedido_siga = ht_reques.pedido_siga;
                    model.anno_siga = ht_reques.anno_siga;
                    ViewBag.var_id_tipo_tramite = ht_reques.id_tipo_tramite.ToString();
                    ViewBag.var_persona_crea = doc_respon.persona_crea;

                    if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                    {
                        if (ht_reques.id_tipo_tramite == 1)
                        {
                            ViewBag.ver_numero_doc = "1";
                        }
                        else
                        {
                            ViewBag.ver_numero_doc = "0";
                        }
                    }
                    else
                    {
                        ViewBag.ver_numero_doc = "1";
                    }


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

                    ViewBag.lst_persona_crea = Lista_per_crea;

                    List<SelectListItem> lista_tipo_documento_iden = new List<SelectListItem>();
                    lista_tipo_documento_iden.Add(new SelectListItem()
                    {
                        Text = "RUC",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.llenar_tipo_documento_identidad())
                    {
                        lista_tipo_documento_iden.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.tipo_doc_iden.ToString()
                        }
                            );
                    };

                    ViewBag.lst_tipo_documento_iden = lista_tipo_documento_iden;
                    List<SelectListItem> lista_direcciones = new List<SelectListItem>();
                    List<SelectListItem> lista_oficinas = new List<SelectListItem>();
                    List<SelectListItem> lista_personas = new List<SelectListItem>();
                    ViewBag.lst_direcciones = lista_direcciones;
                    ViewBag.lst_oficinas = lista_oficinas;
                    ViewBag.lst_persona_ext = lista_personas;

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
        public ActionResult Editar_HojaTramite(HojaTramiteViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    try
                    {
                        HojaTramiteRequest ht_req = new HojaTramiteRequest();
                        ht_req = _HojaTramiteService.GetAllHT_x_Numero_request(model.numero);
                        ht_req.asunto = model.asunto;
                        ht_req.referencia = model.referencia;
                        ht_req.usuario_emision = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        ht_req.pedido_siga = model.pedido_siga;
                        ht_req.id_tipo_pedido_siga = model.id_tipo_pedido_siga;
                        ht_req.anno_siga = model.anno_siga;
                        if (model.modifica_persona_externa == "1")
                        {
                            ht_req.nombre_externo = model.nom_externo;
                            ht_req.id_oficina = model.id_oficina;
                            ht_req.persona_num_documento = model.persona_num_documento;
                        }
                        _HojaTramiteService.Update(ht_req);
                        DocumentoRequest doc_req = new DocumentoRequest();
                        doc_req = _HojaTramiteService.GetAllDocumento_x_Numero_HT_request(ht_req.numero).First();
                        doc_req.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        if (model.persona_crea == "" || model.persona_crea == null)
                        {
                            model.persona_crea = HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        }

                        if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                        {
                            if (_GeneralService.Consulta_Tipo_Documento(model.id_tipo_documento).First().tp_e_i.Trim() == "E")
                            {
                                doc_req.numero_documento = model.numero_documento;
                                doc_req.nom_doc = model.nom_doc;
                            }
                        }
                        else
                        {
                            doc_req.numero_documento = model.numero_documento;
                            doc_req.nom_doc = model.nom_doc;
                        }
                        doc_req.persona_crea = model.persona_crea;
                        doc_req.id_tipo_documento = model.id_tipo_documento;
                        doc_req.anexos = model.anexos;
                        doc_req.folios = model.folios;
                        doc_req.nom_oficina_crea = _GeneralService.recupera_oficina(doc_req.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(doc_req.oficina_crea).nombre;
                        _HojaTramiteService.Documento_Update(doc_req);
                        @ViewBag.Mensaje = "Hoja de Trámite Nro : " + ht_req.hoja_tramite + " : " + ht_req.numero.ToString();
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_SuccessHT");
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
        public ActionResult Adjuntar_Documento(string id)
        {
            if (id != null && id != "")
            {
                Session["pdf_document_id_documento"] = id;
                return RedirectToAction("Adjuntar_Documento_ht", "HojaTramite");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_Documento_ht()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {

                    int var_id_documento = Convert.ToInt32(Session["pdf_document_id_documento"].ToString());
                    Session.Remove("pdf_document_id_documento");

                    DocumentoResponse res_doc = _HojaTramiteService.GetAllDocumento_resp(var_id_documento);

                    string documento = "";

                    if (res_doc.numero_documento != null && res_doc.numero_documento != 0)
                    {
                        documento = _GeneralService.Consulta_Tipo_Documento(res_doc.id_tipo_documento).First().nombre + " N." + res_doc.numero_documento.ToString() + res_doc.nom_doc;
                    }
                    else
                    {
                        documento = _GeneralService.Consulta_Tipo_Documento(res_doc.id_tipo_documento).First().nombre + " " + res_doc.nom_doc;
                    }

                    ViewBag.nom_docu = documento;
                    ViewBag.var_id_documento_ext = var_id_documento.ToString();

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
        public ActionResult Adjuntar_Documento_ht(HttpPostedFileBase file, int lbl_id_documento_ext)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCU_HT"].ToString();
                        var path = Path.Combine(ruta_pdf, lbl_id_documento_ext + ".pdf");
                        file.SaveAs(path);

                        /*
                        var fileName = Path.GetFileName(file.FileName);
                        string ruta_scan = ConfigurationManager.AppSettings["RUTA_PDF_SCAN"].ToString();
                        var fileruta = Path.Combine(ruta_scan, fileName);
                        System.IO.File.Delete(fileruta);*/

                        DocumentoRequest docu_request = new DocumentoRequest();
                        docu_request = _HojaTramiteService.GetAllDocumento_req(lbl_id_documento_ext);
                        docu_request.ruta_pdf = path.Replace('\\', '/');
                        bool succes = _HojaTramiteService.Documento_Update(docu_request);
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
        public ActionResult Adjuntar_HT(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    var datos = _HojaTramiteService.GetAllHT_x_HojaTramite(id);
                    if (datos.First().id_tipo_tramite == 1)
                    {
                        ViewBag.Str_HT = "HT: " + id + " Clave: " + datos.First().clave;
                    }
                    else
                    {
                        ViewBag.Str_HT = "HT: " + id;
                    }

                    var resp = _HojaTramiteService.GetAllDocumento_x_Numero_HT(datos.First().numero);
                    string documento = "";
                    foreach (var x in resp)
                    {
                        if (x.id_indicador_documento == 1)
                        {
                            if (x.numero_documento != null && x.numero_documento != 0)
                            {
                                documento = _GeneralService.Consulta_Tipo_Documento(x.id_tipo_documento).First().nombre + " N." + x.numero_documento.ToString() + x.nom_doc;
                            }
                            else
                            {
                                documento = _GeneralService.Consulta_Tipo_Documento(x.id_tipo_documento).First().nombre + " " + x.nom_doc;
                            }
                        }
                    }

                    ViewBag.nom_docu = documento;
                    ViewBag.num_HT = id;

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
        public ActionResult Adjuntar_HT(HttpPostedFileBase file, int lbl_ht)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[0].Trim() == "1")
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        /*
                        string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_HT"].ToString();
                        var path = Path.Combine(ruta_pdf, lbl_ht + ".pdf");
                        file.SaveAs(path);
                        */

                        string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_DOCU_HT"].ToString();

                        HojaTramiteRequest ht_request = new HojaTramiteRequest();
                        var datos = _HojaTramiteService.GetAllHT_x_HojaTramite(lbl_ht.ToString()).First();

                        DocumentoRequest docu_request = new DocumentoRequest();
                        docu_request = _HojaTramiteService.GetAllDocumento_req_x_ht(datos.numero);

                        var path = Path.Combine(ruta_pdf, docu_request.id_documento + ".pdf");
                        file.SaveAs(path);

                        docu_request.ruta_pdf = path.Replace('\\', '/');
                        bool succes = _HojaTramiteService.Documento_Update(docu_request);

                        /*
                        HojaTramiteRequest ht_request = new HojaTramiteRequest();
                        var datos = _HojaTramiteService.GetAllHT_x_HojaTramite(lbl_ht).First();
                        ht_request.numero = datos.numero;
                        ht_request.id_tipo_tramite = datos.id_tipo_tramite;
                        ht_request.id_oficina = datos.id_oficina;
                        ht_request.fecha_emision = datos.fecha_emision;
                        ht_request.usuario_emision = datos.usuario_emision;
                        ht_request.asunto = datos.asunto;
                        ht_request.persona_num_documento = datos.persona_num_documento;
                        ht_request.tipo_per = datos.tipo_per;
                        ht_request.hoja_tramite = datos.hoja_tramite;
                        ht_request.id_expediente = datos.id_expediente;
                        ht_request.numero_padre = datos.numero_padre;
                        ht_request.ruta_pdf = path.Replace('\\', '/');
                        ht_request.editar = datos.editar;
                        ht_request.pedido_siga = datos.pedido_siga;
                        ht_request.id_tipo_pedido_siga = datos.id_tipo_pedido_siga;
                        ht_request.anno_siga = datos.anno_siga;
                        ht_request.clave = datos.clave;
                        ht_request.referencia = datos.referencia;
                        ht_request.id_tupa = datos.id_tupa;
                        bool succes = _HojaTramiteService.Update(ht_request);
                        */
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
        public ActionResult Ver_PDF(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    string ruta = _HojaTramiteService.GetAllHT_x_HojaTramite(id).First().ruta_pdf;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_SIN_PDF"].ToString();
                    if (ruta == null || ruta == "")
                    {
                        ruta = ruta_pdf;
                    }
                    return File(ruta, "application/pdf");
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
        public ActionResult Ver_docu_ht_PDF(int id = 0)
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    Session["pdf_document_id_documento2"] = id;
                    return RedirectToAction("Ver_docu_ht_PDF_sv", "HojaTramite");
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
        public ActionResult Ver_docu_ht_PDF_sv()
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    int var_id_documento = Convert.ToInt32(Session["pdf_document_id_documento2"].ToString());
                    Session.Remove("pdf_document_id_documento2");

                    string ruta = _HojaTramiteService.GetAllDocumento_resp(var_id_documento).ruta_pdf;
                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_SIN_PDF"].ToString();
                    if (ruta == null || ruta == "")
                    {
                        ruta = ruta_pdf;
                    }
                    return File(ruta, "application/pdf");
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
        public ActionResult Listar_Persona_Natural(int page = 1, string persona_num_documento = "", string PATERNO = "", string MATERNO = "", string NOMBRE = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[4].Trim() == "1")
                {
                    IEnumerable<ConsultarDniResponse> model = new List<ConsultarDniResponse>();
                    ViewBag.Crear = true;
                    ViewBag.TotalRows = _HojaTramiteService.CountPersona_Natural(persona_num_documento, PATERNO, MATERNO, NOMBRE);
                    model = _HojaTramiteService.GetAllPersona_Natural(page, 10, persona_num_documento, PATERNO, MATERNO, NOMBRE);
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

        [AllowAnonymous]
        public ActionResult Editar_Persona(int page = 1, string id = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[4].Trim() == "1")
                {

                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    ConsultarDniResponse resp_dni = new ConsultarDniResponse();
                    resp_dni = _HojaTramiteService.Recupera_persona_x_documento(id);

                    ViewBag.Doc_iden = id;
                    ViewBag.Nom_completo = resp_dni.paterno + " " + resp_dni.materno + " " + resp_dni.nombres;
                    ViewBag.nombres_per = resp_dni.nombres;
                    ViewBag.paterno_per = resp_dni.paterno;
                    ViewBag.materno_per = resp_dni.materno;
                    ViewBag.direccion_per = resp_dni.direccion;
                    ViewBag.ubigeo_per = resp_dni.ubigeo;

                    IEnumerable<ConsultarPersonalResponse> view_oficina = new List<ConsultarPersonalResponse>();
                    ViewBag.TotalRows = _HojaTramiteService.Count_oficina_x_persona(id);

                    view_oficina = _HojaTramiteService.Recupera_oficina_x_persona(page, 10, id);

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

                    ViewBag.lst_departamento = Lista_departamento;
                    ViewBag.lst_provincia = Lista_provincia;
                    ViewBag.lst_distrito = Lista_distrito;

                    return View(view_oficina);

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
          "persona_num_documento": $('#doc_iden').val(),
                    "nombres": $('#txt_nombres_edit').val(),
                    "paterno": $('#txt_paterno_edit').val(),
                    "materno": $('#txt_materno_edit').val(),
                    "direccion": $('#txt_direccion_edit').val(),
                    "ubigeo": $('#cmblista_distrito_edit').val(),
         */

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Ht_editar_persona(string persona_num_documento = "", string nombres = "", string paterno = "", string materno = "", string direccion = "", string ubigeo = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    var success = _HojaTramiteService.editar_persona(persona_num_documento, paterno, materno, nombres, direccion, ubigeo);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");

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
        public ActionResult Nueva_Natural()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[4].Trim() == "1")
                {
                    ConsultarDniViewModel model = new ConsultarDniViewModel();

                    List<SelectListItem> Lista_departamento = new List<SelectListItem>();
                    List<SelectListItem> Lista_provincia = new List<SelectListItem>();
                    List<SelectListItem> Lista_distrito = new List<SelectListItem>();
                    List<SelectListItem> Lista_tipo_doc_iden = new List<SelectListItem>();
                    List<SelectListItem> lista_sexo = new List<SelectListItem>();

                    lista_sexo.Add(new SelectListItem() { Text = "SELECCIONAR SEXO", Value = "" });
                    lista_sexo.Add(new SelectListItem() { Text = "MASCULINO", Value = "M" });
                    lista_sexo.Add(new SelectListItem() { Text = "FEMENINO", Value = "F" });

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
                    ViewBag.cond_grabar = "0";
                    ViewBag.lst_tipo_doc_iden = Lista_tipo_doc_iden;
                    ViewBag.lst_departamento = Lista_departamento;
                    ViewBag.lst_provincia = Lista_provincia;
                    ViewBag.lst_distrito = Lista_distrito;
                    ViewBag.lstsexo = lista_sexo;

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
        public ActionResult Grabar_Nueva_Natural(string persona_num_documento, int tipo_doc_iden, string paterno, string materno, string nombres, string fecha_nacimiento, string ubigeo, string sexo, string direccion, string ruc)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[4].Trim() == "1")
                {
                    try
                    {
                        if (ruc == null)
                        {
                            ruc = "";
                        }
                        if (_OficinaService.Crea_Persona(persona_num_documento, Convert.ToByte(tipo_doc_iden), paterno.ToUpper(), materno.ToUpper(), nombres.ToUpper(), Convert.ToDateTime(fecha_nacimiento), ubigeo, sexo, direccion.ToUpper(), ruc, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim()) == true)
                        {
                            @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
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
        public ActionResult Agregar_Destino(string id_documento_detalle, string id_documento, string oficina_destino, string encargado, string var_observacion, bool v_ind_01, bool v_ind_02, bool v_ind_03,
                               bool v_ind_04, bool v_ind_05, bool v_ind_06, bool v_ind_07, bool v_ind_08, bool v_ind_09, bool v_ind_10, bool v_ind_11)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    try
                    {
                        DocumentoDetalleRequest request3 = new DocumentoDetalleRequest();
                        if (id_documento_detalle == "0")
                        {
                            id_documento_detalle = null;
                        }
                        else
                        {
                            request3.id_cab_det_documento = Convert.ToInt32(id_documento_detalle);
                        }
                        request3.ind_01 = v_ind_01;
                        request3.ind_02 = v_ind_02;
                        request3.ind_03 = v_ind_03;
                        request3.ind_04 = v_ind_04;
                        request3.ind_05 = v_ind_05;
                        request3.ind_06 = v_ind_06;
                        request3.ind_07 = v_ind_07;
                        request3.ind_08 = v_ind_08;
                        request3.ind_09 = v_ind_09;
                        request3.ind_10 = v_ind_10;
                        request3.ind_11 = v_ind_11;
                        request3.oficina_destino = Convert.ToInt32(oficina_destino);
                        request3.persona_num_documento = encargado;
                        request3.id_documento = Convert.ToInt32(id_documento);
                        string indic = "";
                        if (request3.ind_01 == true) { indic = "1"; }
                        if (request3.ind_02 == true) { if (indic == "") { indic = "2"; } else { indic = indic + ",2"; } }
                        if (request3.ind_03 == true) { if (indic == "") { indic = "3"; } else { indic = indic + ",3"; } }
                        if (request3.ind_04 == true) { if (indic == "") { indic = "4"; } else { indic = indic + ",4"; } }
                        if (request3.ind_05 == true) { if (indic == "") { indic = "5"; } else { indic = indic + ",5"; } }
                        if (request3.ind_06 == true) { if (indic == "") { indic = "6"; } else { indic = indic + ",6"; } }
                        if (request3.ind_07 == true) { if (indic == "") { indic = "7"; } else { indic = indic + ",7"; } }
                        if (request3.ind_08 == true) { if (indic == "") { indic = "8"; } else { indic = indic + ",8"; } }
                        if (request3.ind_09 == true) { if (indic == "") { indic = "9"; } else { indic = indic + ",9"; } }
                        if (request3.ind_10 == true) { if (indic == "") { indic = "10"; } else { indic = indic + ",10"; } }
                        if (request3.ind_11 == true) { if (indic == "") { indic = "11"; } else { indic = indic + ",11"; } }
                        request3.indicadores = indic;

                        if (request3.observacion == null)
                        {
                            request3.observacion = "";
                        }

                        request3.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request3.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request3.fecha_crea = DateTime.Now;
                        request3.id_documento = Convert.ToInt32(id_documento);
                        request3.id_est_tramite = 1;
                        request3.nom_oficina_crea = _GeneralService.recupera_oficina(request3.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_crea).nombre;
                        request3.nom_oficina_destino = _GeneralService.recupera_oficina(request3.oficina_destino).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_destino).nombre;
                        _HojaTramiteService.Documento_detalle_Create(request3);
                        @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
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
        public ActionResult Editar_Destino(string id_documento_detalle, string oficina_destino, string encargado, string var_observacion, bool v_ind_01, bool v_ind_02, bool v_ind_03,
                               bool v_ind_04, bool v_ind_05, bool v_ind_06, bool v_ind_07, bool v_ind_08, bool v_ind_09, bool v_ind_10, bool v_ind_11)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    try
                    {
                        DocumentoDetalleResponse respodet_doc = new DocumentoDetalleResponse();
                        respodet_doc = _HojaTramiteService.Consultar_Doc_detalle(Convert.ToInt32(id_documento_detalle)).First();
                        DocumentoDetalleRequest request3 = new DocumentoDetalleRequest();
                        request3.id_det_documento = Convert.ToInt32(id_documento_detalle);
                        request3.id_cab_det_documento = respodet_doc.id_cab_det_documento;
                        request3.ind_01 = v_ind_01;
                        request3.ind_02 = v_ind_02;
                        request3.ind_03 = v_ind_03;
                        request3.ind_04 = v_ind_04;
                        request3.ind_05 = v_ind_05;
                        request3.ind_06 = v_ind_06;
                        request3.ind_07 = v_ind_07;
                        request3.ind_08 = v_ind_08;
                        request3.ind_09 = v_ind_09;
                        request3.ind_10 = v_ind_10;
                        request3.ind_11 = v_ind_11;
                        request3.oficina_destino = Convert.ToInt32(oficina_destino);
                        request3.persona_num_documento = encargado;
                        request3.id_documento = respodet_doc.id_documento;
                        string indic = "";
                        if (request3.ind_01 == true) { indic = "1"; }
                        if (request3.ind_02 == true) { if (indic == "") { indic = "2"; } else { indic = indic + ",2"; } }
                        if (request3.ind_03 == true) { if (indic == "") { indic = "3"; } else { indic = indic + ",3"; } }
                        if (request3.ind_04 == true) { if (indic == "") { indic = "4"; } else { indic = indic + ",4"; } }
                        if (request3.ind_05 == true) { if (indic == "") { indic = "5"; } else { indic = indic + ",5"; } }
                        if (request3.ind_06 == true) { if (indic == "") { indic = "6"; } else { indic = indic + ",6"; } }
                        if (request3.ind_07 == true) { if (indic == "") { indic = "7"; } else { indic = indic + ",7"; } }
                        if (request3.ind_08 == true) { if (indic == "") { indic = "8"; } else { indic = indic + ",8"; } }
                        if (request3.ind_09 == true) { if (indic == "") { indic = "9"; } else { indic = indic + ",9"; } }
                        if (request3.ind_10 == true) { if (indic == "") { indic = "10"; } else { indic = indic + ",10"; } }
                        if (request3.ind_11 == true) { if (indic == "") { indic = "11"; } else { indic = indic + ",11"; } }
                        request3.indicadores = indic;
                        request3.observacion = var_observacion;
                        request3.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request3.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        request3.fecha_crea = respodet_doc.fecha_crea;
                        request3.id_est_tramite = 1;
                        request3.nom_oficina_crea = respodet_doc.nom_oficina_crea;
                        request3.nom_oficina_destino = _GeneralService.recupera_oficina(request3.oficina_destino).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_destino).nombre;
                        _HojaTramiteService.Documento_detalle_Update(request3);
                        @ViewBag.Mensaje = "Se Guardo Satisfactoriamente";
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
        public ActionResult Guardar_empresa(string ruc = "", string nombre = "", string siglas = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[5].Trim() == "1")
                {
                    _HojaTramiteService.Crear_Empresa(ruc, nombre, siglas, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");
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
        public ActionResult Por_Recibir_Ht(int page = 1, string HT = "", string Asunto = "", string Empresa = "", int id_ofi_dir = 0, string ofi_dir = "", string cmbtupa = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    //IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();

                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_sedes = new List<SelectListItem>();

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

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.Crear = true;
                    //ViewBag.TotalRows = _HojaTramiteService.CountNoRecibidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa);
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("TIPO_HOJA_TRAMITE");
                    tbl.Columns.Add("FECHA_DERIVADO");
                    tbl.Columns.Add("DIRIGIDO_A");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("OFICINA_DERIVA");
                    tbl.Columns.Add("FOLIOS");
                    tbl.Columns.Add("ID_DET_DOCUMENTO");
                    tbl.Columns.Add("NRO_HOJA_TRAMITE");
                    tbl.Columns.Add("OBSERVACION_INDICADORES");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("NUMERO_ID_HT_TEXTO");
                    tbl.Columns.Add("ID_TIPO_DOCUMENTO");
                    tbl.Columns.Add("VER_DOCU_PDF");
                    tbl.Columns.Add("ID_DOCUMENTO");
                    tbl.Columns.Add("TUPA");

                    IEnumerable<DocumentoDetalleResponse> var_ht_x_recibir = new List<DocumentoDetalleResponse>();

                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20" || HttpContext.User.Identity.Name.Split('|')[5].Trim() == "18")
                    {
                        var_ht_x_recibir = _HojaTramiteService.GetAllNoRecibidos_x_persona(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, id_ofi_dir, HttpContext.User.Identity.Name.Split('|')[1].Trim(), cmbtupa);
                    }
                    else
                    {
                        var_ht_x_recibir = _HojaTramiteService.GetAllNoRecibidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, id_ofi_dir, cmbtupa);
                    }


                    foreach (var result in var_ht_x_recibir)
                    {

                        string ver_docu_pdf = "0";
                        if (result.documento.ruta_pdf != null)
                        {
                            ver_docu_pdf = "1";
                        }
                        tbl.Rows.Add(
                            result.documento.hoja_tramite.hoja_tramite,
                            result.documento.hoja_tramite.nombre_tipo_tramite,
                            result.fecha_derivado,
                            result.nombre_encargado,
                            result.documento.nom_doc,
                            result.documento.hoja_tramite.asunto,
                            result.documento.hoja_tramite.nombre_oficina,
                            result.documento.siglas_oficina,
                            result.documento.folios.ToString(),
                            result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.numero.ToString(),
                            result.observacion.ToString() + "|" + result.indicadores.ToString(),
                            result.documento.hoja_tramite.ver_pdf,
                            result.documento.hoja_tramite.numero + "|" + result.documento.hoja_tramite.hoja_tramite,
                            result.documento.id_tipo_documento.ToString(),
                            ver_docu_pdf,
                            result.documento.id_documento.ToString(),
                            result.documento.hoja_tramite.nom_tupa
                            );
                    };

                    ViewData["HT_POR_RECIBIR"] = tbl;

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
        public ActionResult Lista_Hojas_Tramite_x_pedido(int id_tipo_pedido_siga = 1, int pedido_siga = 0, int anno_siga = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[24].Trim() == "1")
                {
                    if (anno_siga == 0)
                    {
                        anno_siga = DateTime.Now.Year;
                    }
                    List<SelectListItem> lista_tipo_pedido_siga = new List<SelectListItem>();

                    foreach (var result in _GeneralService.llenar_tipo_pedido_siga())
                    {
                        lista_tipo_pedido_siga.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_pedido_siga.ToString()
                        }
                            );
                    };

                    List<SelectListItem> lista_año_siga = new List<SelectListItem>();

                    for (int i = DateTime.Now.Year; i >= 2015; i--)
                    {
                        lista_año_siga.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        }
                            );
                    }

                    ViewBag.lista_anno = lista_año_siga;
                    ViewBag.lista_tipo_siga = lista_tipo_pedido_siga;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("PERSONA_FINAL");
                    tbl.Columns.Add("OFICINA_FINAL");
                    tbl.Columns.Add("FECHA_ENVIO");
                    tbl.Columns.Add("FECHA_RECEPCION");
                    tbl.Columns.Add("ESTADO");
                    string asunto_siga = "";
                    string centro_costo = "";
                    if (pedido_siga != 0)
                    {
                        var var_ht_x_pedido = _HojaTramiteService.GetAllHoja_Tramite_x_PEDIDO_SIGA(id_tipo_pedido_siga, pedido_siga, anno_siga, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                        if (var_ht_x_pedido.Count() > 0)
                        {
                            asunto_siga = "Asunto: " + var_ht_x_pedido.First().documento.hoja_tramite.siga_asunto;
                            centro_costo = "Oficina: " + var_ht_x_pedido.First().documento.hoja_tramite.siga_centro_costo;
                        }

                        foreach (var result in var_ht_x_pedido)
                        {
                            tbl.Rows.Add(
                                result.documento.hoja_tramite.hoja_tramite,
                                result.documento.hoja_tramite.asunto,
                                result.nombre_encargado,
                                result.nombre_oficina_destino,
                                result.fecha_crea,
                                result.fecha_recepcion,
                                result.estado_tramite.nombre
                                );
                        };
                    }

                    ViewBag.STR_ASUNTO_SIGA = asunto_siga;
                    ViewBag.STR_CENTRO_DE_COSTO = centro_costo;
                    ViewData["HT_PEDIDO"] = tbl;

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
        public ActionResult Consultar_Documentos_x_oficina(int page = 1, string val_txtfechainicio = "", string val_txtfechafin = "", string HT = "",
            string Asunto = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "", string cmbtupa = "", string anexos = "", string Empresa = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                // 07 - ID_SISTEMA = "2" - GESDOC
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2"
                    // 09 - ACCESO : CONSULTA_DOC_X_OFICINA_27;
                    && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[27].Trim() == "1")
                {

                    int ival_txtfechainicio = 0;
                    int ival_txtfechafin = 0;

                    if (val_txtfechainicio != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechainicio = val_txtfechainicio.Substring(6, 4) + val_txtfechainicio.Substring(3, 2) + val_txtfechainicio.Substring(0, 2);
                        ival_txtfechainicio = Convert.ToInt32(val_txtfechainicio);
                    }
                    if (val_txtfechafin != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechafin = val_txtfechafin.Substring(6, 4) + val_txtfechafin.Substring(3, 2) + val_txtfechafin.Substring(0, 2);
                        ival_txtfechafin = Convert.ToInt32(val_txtfechafin);
                    }

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

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.Crear = true;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("NOMBRE_DOCUMENTO");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("NOMBRE_TIPO_TRAMITE");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("NOMBRE_OFICINA");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("REFERENCIA");
                    tbl.Columns.Add("ANEXOS");
                    tbl.Columns.Add("NUMERO_HOJA_TRAMITE");
                    tbl.Columns.Add("ID_DOCUMENTO");
                    tbl.Columns.Add("RUTA_PDF");
                    tbl.Columns.Add("PDF");

                    // User.Identity.Name.Split
                    // 04 - ID_OFICINA_DIRECCION

                    var documento = _HojaTramiteService.GetmisDoc(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, cmbtupa, anexos, Empresa);

                    foreach (var result in documento)
                    {
                        if (result.ruta_pdf == "" || result.ruta_pdf == null)
                        {
                            tbl.Rows.Add(
                            result.nom_doc,
                            result.fecha_envio,
                            result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nombre_tipo_tramite,
                            result.hoja_tramite.nom_tupa,
                            result.hoja_tramite.nombre_oficina,
                            result.hoja_tramite.asunto,
                            result.hoja_tramite.referencia,
                            result.anexos,
                            result.hoja_tramite.numero,
                            result.id_documento,
                            result.ruta_pdf,
                            "0"
                            );
                        }
                        else
                        {
                            tbl.Rows.Add(
                            result.nom_doc,
                            result.fecha_envio,
                            result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nombre_tipo_tramite,
                            result.hoja_tramite.nom_tupa,
                            result.hoja_tramite.nombre_oficina,
                            result.hoja_tramite.asunto,
                            result.hoja_tramite.referencia,
                            result.anexos,
                            result.hoja_tramite.numero,
                            result.id_documento,
                            result.ruta_pdf,
                            "1"
                            );
                        }

                    };

                    ViewData["HT_General_Tabla"] = tbl;

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
        public ActionResult UploadFiles()
        {
            DocumentoAnexoRequest docu_anexo = new DocumentoAnexoRequest();

            string ruta_archivo = ConfigurationManager.AppSettings["RUTA_PDF_DOCU_ANEXOS_HT"].ToString();
            HttpFileCollectionBase files = Request.Files;
            //string[] path = new string[files.Count];
            for (var i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                docu_anexo.descripcion = file.FileName;
                docu_anexo.extension = Path.GetExtension(file.FileName);
                docu_anexo.id_documento = 141441;
                docu_anexo.id_documento_anexo = _HojaTramiteService.Documento_anexo_Insertar(docu_anexo);
                string rootPath = Path.Combine(ruta_archivo, docu_anexo.id_documento_anexo.ToString() + "." + docu_anexo.extension);
                docu_anexo.ruta = rootPath.Replace('\\', '/');
                bool success = _HojaTramiteService.Documento_anexo_Update(docu_anexo);
                file.SaveAs(rootPath);
            }
            return Json(files.Count + " Files Uploaded!");

        }


        [AllowAnonymous]
        public ActionResult Consultar_HT_General(int page = 1, string val_txtfechainicio = "", string val_txtfechafin = "", string HT = "",
            string Asunto = "", string Empresa = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "", string cmbtupa = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[1].Trim() == "1")
                {

                    int ival_txtfechainicio = 0;
                    int ival_txtfechafin = 0;

                    if (val_txtfechainicio != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechainicio = val_txtfechainicio.Substring(6, 4) + val_txtfechainicio.Substring(3, 2) + val_txtfechainicio.Substring(0, 2);
                        ival_txtfechainicio = Convert.ToInt32(val_txtfechainicio);
                    }
                    if (val_txtfechafin != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechafin = val_txtfechafin.Substring(6, 4) + val_txtfechafin.Substring(3, 2) + val_txtfechafin.Substring(0, 2);
                        ival_txtfechafin = Convert.ToInt32(val_txtfechafin);
                    }

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


                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.Crear = true;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("NOMBRE_TIPO_DOCUMENTO");
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("NOMBRE_TIPO_TRAMITE");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("NOMBRE_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("REFERENCIA");
                    tbl.Columns.Add("FECHA_EMISION");
                    tbl.Columns.Add("NOMBRE_OFICINA");
                    tbl.Columns.Add("NUMERO_HOJA_TRAMITE");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("CLAVE");
                    tbl.Columns.Add("NUMERO_ID_HT_TEXTO");
                    tbl.Columns.Add("NOM_ESTADO");

                    var documento = _HojaTramiteService.GetAllHT(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, cmbtupa);

                    foreach (var result in documento)
                    {
                        tbl.Rows.Add(
                            result.hoja_tramite.nombre_tipo_documento,
                            result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nombre_tipo_tramite,
                            result.hoja_tramite.nom_tupa,
                            result.nom_doc,
                            result.hoja_tramite.asunto,
                            result.hoja_tramite.referencia,
                            result.hoja_tramite.fecha_emision,
                            result.hoja_tramite.nombre_oficina,
                            result.hoja_tramite.numero,
                            result.hoja_tramite.ver_pdf,
                            result.hoja_tramite.clave,
                            result.hoja_tramite.numero + "|" + result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nom_estado
                            );
                    };

                    ViewData["HT_General_Tabla"] = tbl;

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
        public ActionResult Consultar_mis_HT(int page = 1, string val_txtfechainicio = "", string val_txtfechafin = "", string HT = "", string Asunto = "",
            string Empresa = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "", string cmbtupa = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[2].Trim() == "1")
                {
                    int ival_txtfechainicio = 0;
                    int ival_txtfechafin = 0;

                    if (val_txtfechainicio != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechainicio = val_txtfechainicio.Substring(6, 4) + val_txtfechainicio.Substring(3, 2) + val_txtfechainicio.Substring(0, 2);
                        ival_txtfechainicio = Convert.ToInt32(val_txtfechainicio);
                    }
                    if (val_txtfechafin != "")
                    {/*
                        val_txtfechainicio = DateTime.Now.AddDays(-5).Year.ToString() + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00");
                        val_txtfechafin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    }
                    else
                    {*/
                        val_txtfechafin = val_txtfechafin.Substring(6, 4) + val_txtfechafin.Substring(3, 2) + val_txtfechafin.Substring(0, 2);
                        ival_txtfechafin = Convert.ToInt32(val_txtfechafin);
                    }
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
                    ViewBag.Crear = true;

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("NOMBRE_TIPO_TRAMITE");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("NOMBRE_DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("FECHA_EMISION");
                    tbl.Columns.Add("NOMBRE_OFICINA");
                    tbl.Columns.Add("NUMERO_HOJA_TRAMITE");
                    tbl.Columns.Add("VER_EDITAR");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("CLAVE");
                    tbl.Columns.Add("NUMERO_ID_HT_TEXTO");
                    tbl.Columns.Add("NOM_ESTADO");

                    var documento = _HojaTramiteService.GetmisHT(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, cmbtupa);

                    foreach (var result in documento)
                    {
                        tbl.Rows.Add(
                            result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nombre_tipo_tramite,
                            result.hoja_tramite.nom_tupa,
                            result.nom_doc,
                            result.hoja_tramite.asunto,
                            result.hoja_tramite.fecha_emision,
                            result.hoja_tramite.nombre_oficina,
                            result.hoja_tramite.numero,
                            result.hoja_tramite.ver_editar,
                            result.hoja_tramite.ver_pdf,
                            result.hoja_tramite.clave,
                            result.hoja_tramite.numero + "|" + result.hoja_tramite.hoja_tramite,
                            result.hoja_tramite.nom_estado
                            );
                    };

                    ViewData["mis_HT_Tabla"] = tbl;

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
        public ActionResult Consultar_mis_Finalizados(int page = 1, string HT = "", string Asunto = "", string Empresa = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" &&
                    (HttpContext.User.Identity.Name.Split('|')[5].Trim().Split(',')[0].Trim() == "8" || HttpContext.User.Identity.Name.Split('|')[5].Trim().Split(',')[0].Trim() == "9")
                    )
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
                    IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();
                    ViewBag.lst_tipo_documento = lista_documentos;
                    ViewBag.Crear = true;
                    ViewBag.TotalRows = _HojaTramiteService.CountmisHt_finalizados(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
                    model = _HojaTramiteService.GetmisHT_finalizados(page, 10, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
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

        [AllowAnonymous]
        public ActionResult Consultar_mis_Archivados(int page = 1, string HT = "", string Asunto = "", string Empresa = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[7].Trim() == "1")
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
                    IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();
                    ViewBag.lst_tipo_documento = lista_documentos;
                    ViewBag.Crear = true;
                    ViewBag.TotalRows = _HojaTramiteService.CountmisHt_archivados(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
                    model = _HojaTramiteService.GetmisHT_archivados(page, 10, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
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

        [AllowAnonymous]
        public ActionResult Consultar_mis_Atendidos(int page = 1, string HT = "", string Asunto = "", string Empresa = "", string cmbtipo_documento = "", string num_documento = "", string nom_documento = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[8].Trim() == "1")
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
                    IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();
                    ViewBag.lst_tipo_documento = lista_documentos;
                    ViewBag.Crear = true;
                    ViewBag.TotalRows = _HojaTramiteService.CountmisHt_atendidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
                    model = _HojaTramiteService.GetmisHT_atendidos(page, 10, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
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
        public ActionResult HT_atender(int id = 1, string observacion = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    bool success = _HojaTramiteService.Atender_ht(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), observacion);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");

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
        public ActionResult HT_editar_observacion(int id = 1, string observacion = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    bool success = _HojaTramiteService.Editar_Observacion_Detalle(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), observacion);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");

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
        public ActionResult HT_Desarchivar(int id = 1, string observacion = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && (HttpContext.User.Identity.Name.Split('|')[5].Trim().Split(',')[0].Trim() == "8" || HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[7].Trim() == "1"))
                {
                    bool success = _HojaTramiteService.Quitar_Archivo_Atendido_ht(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), observacion);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");
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
        public ActionResult HT_Desatender(int id = 1, string observacion = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[8].Trim() == "1")
                {
                    bool success = _HojaTramiteService.Quitar_Archivo_Atendido_ht(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), observacion);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");

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
        public ActionResult HT_cancelar(int id = 1)
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    bool success = _HojaTramiteService.Cancelar_Ht(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");
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
        public ActionResult HT_archivar(int id = 1, string observacion = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    bool success = _HojaTramiteService.Archivar_ht(id, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), observacion);

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");
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
        public ActionResult HT_cancelar_recepcion(int id = 1)
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    bool success = _HojaTramiteService.cancelar_recepcion_ht(id);

                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");
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
        public ActionResult HT_Por_Recibir(string id = "")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    int id_recibir = 0;
                    for (int i = 0; i < id.Split('|').Count(); i++)
                    {
                        id_recibir = Convert.ToInt32(id.Split('|')[i].Trim());
                        int var_id_documento = _HojaTramiteService.GetAllDocumentoDetalle(id_recibir).First().id_documento;
                        int var_num_ht = _HojaTramiteService.GetAllDocumento(var_id_documento).First().numero;
                        HojaTramiteRequest ht_request = new HojaTramiteRequest();
                        ht_request = _HojaTramiteService.GetAllHT_x_Numero_request(var_num_ht);
                        ht_request.editar = "0";
                        _HojaTramiteService.Update(ht_request);
                        bool success = _HojaTramiteService.Recibir_ht(id_recibir, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    }
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessHT");


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
        public ActionResult Recibidos_HT(int page = 1, string HT = "", string Asunto = "", string Empresa = "", int cmbestado = 2, string cmbtupa = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();

                    int entra = 0;
                    foreach (var result in _GeneralService.Recupera_oficina_todo())
                    {
                        // Como la oficina que ingresa es la id_oficina=1 entonces no tiene que aparecer esa oficina como destino
                        if (result.ruc == "20565429656")
                        {
                            if (entra == 0)
                            {
                                ViewBag.lstpersonal_oficina = _GeneralService.Recupera_personal_oficina(result.id_oficina).Select(c => new SelectListItem() { Text = c.nom_persona, Value = c.persona_num_documento.ToString() }).ToList();
                                entra = 1;
                            }
                            Lista_Oficina_destino.Add(new SelectListItem()
                            {
                                Text = result.nombre,
                                Value = result.id_oficina.ToString()
                            });
                        }
                    };

                    List<SelectListItem> Lista_estado = new List<SelectListItem>();

                    Lista_estado.Add(new SelectListItem()
                    {
                        Text = "Por atender",
                        Value = "2"
                    });

                    Lista_estado.Add(new SelectListItem()
                    {
                        Text = "Atendido",
                        Value = "5"
                    });

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.vst_check_documento = "0";
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstestado = Lista_estado;

                    //IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();

                    ViewBag.Crear = true;
                    //ViewBag.TotalRows = _HojaTramiteService.CountRecibidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa);

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("TIPO_HOJA_TRAMITE");
                    tbl.Columns.Add("FECHA_RECIBIDA");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("OFICINA_DERIVA");
                    tbl.Columns.Add("FOLIOS");
                    tbl.Columns.Add("ENCARGADO");
                    tbl.Columns.Add("ID_DET_DOCUMENTO");
                    tbl.Columns.Add("NRO_HOJA_TRAMITE");
                    tbl.Columns.Add("OBSERVACION_INDICADORES");
                    tbl.Columns.Add("ID_DET_DOCUMENTO_HOJA_TRAMITE_NRO_HT");
                    tbl.Columns.Add("HOJA_TRAMITE_ID_DET_DOCUMENTO");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("NUMERO_ID_HT_TEXTO");
                    tbl.Columns.Add("ESTADO");
                    tbl.Columns.Add("ID_TIPO_DOCUMENTO");
                    tbl.Columns.Add("VER_DOCU_PDF");
                    tbl.Columns.Add("ID_DOCUMENTO");
                    tbl.Columns.Add("TUPA");


                    IEnumerable<DocumentoDetalleResponse> var_ht_recibidos = new List<DocumentoDetalleResponse>();
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20" || HttpContext.User.Identity.Name.Split('|')[5].Trim() == "18")
                    {
                        var_ht_recibidos = _HojaTramiteService.GetAllRecibidos_x_persona(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbestado, HttpContext.User.Identity.Name.Split('|')[1].Trim(), cmbtupa);
                    }
                    else
                    {
                        var_ht_recibidos = _HojaTramiteService.GetAllRecibidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbestado, cmbtupa);
                    }


                    foreach (var result in var_ht_recibidos)
                    {
                        string ver_docu_pdf = "0";
                        if (result.documento.ruta_pdf != null)
                        {
                            ver_docu_pdf = "1";
                        }
                        tbl.Rows.Add(
                            result.documento.hoja_tramite.hoja_tramite,
                            result.documento.hoja_tramite.nombre_tipo_tramite,
                            result.fecha_recepcion,
                            result.documento.nom_doc,
                            result.documento.hoja_tramite.asunto,
                            result.documento.hoja_tramite.nombre_oficina,
                            result.documento.siglas_oficina,
                            result.documento.folios.ToString(),
                            result.nombre_encargado,
                            result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.numero.ToString(),
                            result.observacion.ToString() + "|" + result.indicadores.ToString(),
                            result.id_det_documento.ToString() + "|" + result.documento.hoja_tramite.hoja_tramite + "|" + result.documento.hoja_tramite.numero.ToString(),
                            result.documento.hoja_tramite.hoja_tramite + "|" + result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.ver_pdf,
                            result.documento.hoja_tramite.numero + "|" + result.documento.hoja_tramite.hoja_tramite,
                            cmbestado.ToString(),
                            result.documento.id_tipo_documento.ToString(),
                            ver_docu_pdf,
                            result.documento.id_documento.ToString(),
                            result.documento.hoja_tramite.nom_tupa
                            );
                    };

                    ViewData["HT_RECIBIDOS"] = tbl;

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
        public ActionResult Recibidos_Archivados_Atendidos(string HT = "", string Asunto = "", string Empresa = "", int Estado = 3)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {

                    List<SelectListItem> Lista_Estado = new List<SelectListItem>();

                    Lista_Estado.Add(new SelectListItem() { Text = "ATENDIDO", Value = "3" });
                    Lista_Estado.Add(new SelectListItem() { Text = "ARCHIVADO", Value = "4" });

                    ViewBag.lstestado = Lista_Estado;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("TIPO_HOJA_TRAMITE");
                    tbl.Columns.Add("FECHA_FIN");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("OFICINA_DERIVA");
                    tbl.Columns.Add("FOLIOS");
                    tbl.Columns.Add("ENCARGADO");
                    tbl.Columns.Add("ID_DET_DOCUMENTO");
                    tbl.Columns.Add("NRO_HOJA_TRAMITE");
                    tbl.Columns.Add("OBSERVACION_INDICADORES");
                    tbl.Columns.Add("ESTADO");
                    tbl.Columns.Add("OBSERVACION_ATENDIDO_ARCHIVO");
                    tbl.Columns.Add("OBSERVACION_HT_ID");
                    tbl.Columns.Add("ID_DET_DOCUMENTO_HOJA_TRAMITE_NRO_HT");
                    tbl.Columns.Add("HOJA_TRAMITE_ID_DET_DOCUMENTO");
                    tbl.Columns.Add("VER_PDF");

                    IEnumerable<DocumentoDetalleResponse> var_ht_archivo_atendido = new List<DocumentoDetalleResponse>();
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20")
                    {
                        var_ht_archivo_atendido = _HojaTramiteService.GetAllRecibidos_x_persona(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, Estado, HttpContext.User.Identity.Name.Split('|')[1].Trim(), "");
                    }
                    else
                    {
                        var_ht_archivo_atendido = _HojaTramiteService.GetAllRecibidos(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, Estado, "");
                    }


                    foreach (var result in var_ht_archivo_atendido)
                    {
                        if (Estado == 3)
                        {
                            tbl.Rows.Add(
                            result.documento.hoja_tramite.hoja_tramite,
                            result.documento.hoja_tramite.nombre_tipo_tramite,
                            result.fecha_atendido,
                            result.documento.nom_doc,
                            result.documento.hoja_tramite.asunto,
                            result.documento.hoja_tramite.nombre_oficina,
                            result.documento.siglas_oficina,
                            result.documento.folios.ToString(),
                            result.nombre_encargado,
                            result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.numero.ToString(),
                            result.observacion.ToString() + "|" + result.indicadores.ToString(),
                            result.estado_tramite.nombre,
                            result.observacion_atendido,
                            result.id_det_documento.ToString() + "|" + result.documento.hoja_tramite.hoja_tramite + "|" + result.observacion_atendido,
                            result.id_det_documento.ToString() + "|" + result.documento.hoja_tramite.hoja_tramite + "|" + result.documento.hoja_tramite.numero.ToString(),
                            result.documento.hoja_tramite.hoja_tramite + "|" + result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.ver_pdf
                            );
                        }
                        else
                        {
                            tbl.Rows.Add(
                            result.documento.hoja_tramite.hoja_tramite,
                            result.documento.hoja_tramite.nombre_tipo_tramite,
                            result.fecha_archivo,
                            result.documento.nom_doc,
                            result.documento.hoja_tramite.asunto,
                            result.documento.hoja_tramite.nombre_oficina,
                            result.documento.siglas_oficina,
                            result.documento.folios.ToString(),
                            result.nombre_encargado,
                            result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.numero.ToString(),
                            result.observacion.ToString() + "|" + result.indicadores.ToString(),
                            result.estado_tramite.nombre,
                            result.observacion_archivo,
                            result.id_det_documento.ToString() + "|" + result.documento.hoja_tramite.hoja_tramite + "|" + result.observacion_archivo,
                            result.id_det_documento.ToString() + "|" + result.documento.hoja_tramite.hoja_tramite + "|" + result.documento.hoja_tramite.numero.ToString(),
                            result.documento.hoja_tramite.hoja_tramite + "|" + result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.ver_pdf
                            );
                        }
                    };

                    ViewData["HT_ATENDIDO_ARCHIVO"] = tbl;

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
        public ActionResult Nuevo_Documento(int id_det_documento, string HT, string id_HT)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {

                    HojaTramiteViewModel model = new HojaTramiteViewModel();

                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_personal = new List<SelectListItem>();
                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
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
                    ViewBag.lst_persona_crea = Lista_per_crea;
                    ViewBag.lbl_sin_original = "0";
                    ViewBag.lbl_sin_documento = "1";
                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstpersonal_oficina = lista_personal;
                    ViewBag.Str_HT = HT;
                    ViewBag.Str_id_HT = id_HT;
                    ViewBag.Str_id_det_documento = id_det_documento.ToString();
                    ViewBag.Str_id_oficina = HttpContext.User.Identity.Name.Split('|')[4].Trim().ToString();
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("", "0").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    ViewBag.cond_grabar = "0";


                    if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                    {
                        ViewBag.ver_numero_doc = "0";
                    }
                    else
                    {
                        ViewBag.ver_numero_doc = "1";
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Nuevo_Documento(HojaTramiteViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    try
                    {
                        int var_id_cab_det_documento = model.documento_detalle.First().id_cab_det_documento;
                        int derivado = _HojaTramiteService.GetAllDocumentoDetalle(var_id_cab_det_documento).First().id_est_tramite;
                        if (derivado == 2 || derivado == 5)
                        {
                            model.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                            model.fecha_envio = DateTime.Now;

                            model.nom_oficina_crea = _GeneralService.recupera_oficina(model.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(model.oficina_crea).nombre;

                            DocumentoRequest request2 = ModelToRequest.documento(model);
                            request2.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            request2.numero = model.numero;

                            string mensaje_final = "";

                            if (model.nom_doc != "NN")
                            {
                                request2.id_indicador_documento = 2;

                                if (model.id_tipo_documento != 149 && model.id_tipo_documento != 150)
                                {
                                    if (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[29].Trim() == "1")
                                    {
                                        if (model.id_tipo_documento != 169)
                                        {
                                            var ultimo = _HojaTramiteService.Recupera_Documento(model.oficina_crea, model.id_tipo_documento, DateTime.Now.Year);

                                            if (ultimo.Count() > 0) { request2.numero_documento = ultimo.First().numero_documento + 1; }
                                            else { request2.numero_documento = 1; }

                                            request2.nom_doc = "-" + DateTime.Now.Year.ToString() + "-" + _GeneralService.recupera_oficina(model.oficina_crea).siglas;
                                        }
                                        else
                                        {
                                            request2.nom_doc = "";
                                        }

                                    }
                                }
                                model.id_documento = _HojaTramiteService.Documento_Create(request2);
                                mensaje_final = "Se Derivo correctamente con el documento:" + _GeneralService.Consulta_Tipo_Documento(model.id_tipo_documento).First().nombre + " N." + request2.numero_documento.ToString() + request2.nom_doc;
                            }
                            else
                            {
                                model.folios = 0;
                                model.id_documento = _HojaTramiteService.GetAllDocumentoDetalle(var_id_cab_det_documento).First().id_documento;
                                mensaje_final = "Se Derivo correctamente";
                            }


                            if (model.documento_detalle != null)
                            {
                                request2.documento_detalle = new List<DocumentoDetalleRequest>();

                                foreach (DocumentoDetalleViewModel obj in model.documento_detalle)
                                {
                                    DocumentoDetalleRequest request3 = ModelToRequest.DocumentoDetalle(obj);
                                    string indic = "";
                                    if (request3.ind_01 == true) { indic = "1"; }
                                    if (request3.ind_02 == true) { if (indic == "") { indic = "2"; } else { indic = indic + ",2"; } }
                                    if (request3.ind_03 == true) { if (indic == "") { indic = "3"; } else { indic = indic + ",3"; } }
                                    if (request3.ind_04 == true) { if (indic == "") { indic = "4"; } else { indic = indic + ",4"; } }
                                    if (request3.ind_05 == true) { if (indic == "") { indic = "5"; } else { indic = indic + ",5"; } }
                                    if (request3.ind_06 == true) { if (indic == "") { indic = "6"; } else { indic = indic + ",6"; } }
                                    if (request3.ind_07 == true) { if (indic == "") { indic = "7"; } else { indic = indic + ",7"; } }
                                    if (request3.ind_08 == true) { if (indic == "") { indic = "8"; } else { indic = indic + ",8"; } }
                                    if (request3.ind_09 == true) { if (indic == "") { indic = "9"; } else { indic = indic + ",9"; } }
                                    if (request3.ind_10 == true) { if (indic == "") { indic = "10"; } else { indic = indic + ",10"; } }
                                    if (request3.ind_11 == true) { if (indic == "") { indic = "11"; } else { indic = indic + ",11"; } }
                                    if (request3.observacion == null)
                                    {
                                        request3.observacion = "";
                                    }
                                    request3.indicadores = indic;
                                    request3.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                    request3.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                                    request3.fecha_crea = DateTime.Now;
                                    request3.id_documento = model.id_documento;
                                    request3.id_est_tramite = 1;
                                    request3.nom_oficina_crea = _GeneralService.recupera_oficina(request3.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_crea).nombre;
                                    request3.nom_oficina_destino = _GeneralService.recupera_oficina(request3.oficina_destino).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_destino).nombre;
                                    _HojaTramiteService.Documento_detalle_Create(request3);
                                }
                            }
                            if (model.ac_sin_original == "0")
                            {
                                bool success = _HojaTramiteService.Derivar_HT(var_id_cab_det_documento, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                            }

                            @ViewBag.Mensaje = mensaje_final;
                        }
                        else
                        {
                            @ViewBag.Mensaje = "";
                        }
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_SuccessHT");

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
        public ActionResult Nuevo_Documento_Externo()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {

                    HojaTramiteViewModel model = new HojaTramiteViewModel();

                    List<SelectListItem> lista_destino = new List<SelectListItem>();

                    lista_destino.Add(new SelectListItem()
                    {
                        Text = "SELECCIONAR DESTINO",
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

                    /*
                    List<SelectListItem> Lista_per_crea = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_per_crea.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        });
                    };

                    ViewBag.lst_persona_crea = Lista_per_crea;
                     */

                    ViewBag.lbl_sin_original = "0";
                    ViewBag.lbl_sin_documento = "1";
                    ViewBag.lst_destino = lista_destino;

                    /*
                    ViewBag.Str_HT = HT;
                    ViewBag.Str_id_HT = id_HT;
                    ViewBag.Str_id_det_documento = id_det_documento.ToString();
                    */

                    ViewBag.lst_tipo_documento = _GeneralService.Recupera_tipo_documento_todo("T", "0").Select(c => new SelectListItem() { Text = c.nombre, Value = c.id_tipo_documento.ToString() }).ToList();
                    ViewBag.cond_grabar = "0";
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
        public ActionResult Nuevo_Documento_Externo(HojaTramiteViewModel model)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    try
                    {

                        model.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                        model.fecha_envio = DateTime.Now;
                        model.persona_crea = HttpContext.User.Identity.Name.Split('|')[1].Trim();

                        model.numero = _HojaTramiteService.GetAllHT_x_HojaTramite(model.Hoja_Tramite).First().numero;

                        model.nom_oficina_crea = _GeneralService.recupera_oficina(model.oficina_crea).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(model.oficina_crea).nombre;

                        DocumentoRequest request2 = ModelToRequest.documento(model);
                        request2.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        request2.numero = model.numero;
                        request2.id_indicador_documento = 2;
                        request2.num_ext = _HojaTramiteService.Get_num_ext_Documento(model.numero) + 1;

                        model.id_documento = _HojaTramiteService.Documento_Create(request2);

                        if (model.documento_detalle != null)
                        {
                            request2.documento_detalle = new List<DocumentoDetalleRequest>();

                            foreach (DocumentoDetalleViewModel obj in model.documento_detalle)
                            {
                                DocumentoDetalleRequest request3 = ModelToRequest.DocumentoDetalle(obj);
                                string indic = "";
                                if (request3.ind_01 == true) { indic = "1"; }
                                if (request3.ind_02 == true) { if (indic == "") { indic = "2"; } else { indic = indic + ",2"; } }
                                if (request3.ind_03 == true) { if (indic == "") { indic = "3"; } else { indic = indic + ",3"; } }
                                if (request3.ind_04 == true) { if (indic == "") { indic = "4"; } else { indic = indic + ",4"; } }
                                if (request3.ind_05 == true) { if (indic == "") { indic = "5"; } else { indic = indic + ",5"; } }
                                if (request3.ind_06 == true) { if (indic == "") { indic = "6"; } else { indic = indic + ",6"; } }
                                if (request3.ind_07 == true) { if (indic == "") { indic = "7"; } else { indic = indic + ",7"; } }
                                if (request3.ind_08 == true) { if (indic == "") { indic = "8"; } else { indic = indic + ",8"; } }
                                if (request3.ind_09 == true) { if (indic == "") { indic = "9"; } else { indic = indic + ",9"; } }
                                if (request3.ind_10 == true) { if (indic == "") { indic = "10"; } else { indic = indic + ",10"; } }
                                if (request3.ind_11 == true) { if (indic == "") { indic = "11"; } else { indic = indic + ",11"; } }
                                if (request3.observacion == null)
                                {
                                    request3.observacion = "";
                                }
                                request3.indicadores = indic;
                                request3.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                                request3.oficina_crea = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());
                                request3.fecha_crea = DateTime.Now;
                                request3.fecha_recepcion = DateTime.Now;
                                request3.fecha_archivo = DateTime.Now;
                                request3.id_documento = model.id_documento;
                                request3.id_est_tramite = 4;
                                request3.observacion_archivo = "Se adjunto al expediente principal";
                                request3.nom_oficina_destino = _GeneralService.recupera_oficina(request3.oficina_destino).nombre + " - " + _GeneralService.Recupera_sede_x_id_ofi_dir(request3.oficina_destino).nombre;
                                _HojaTramiteService.Documento_detalle_Create(request3);
                            }
                        }

                        @ViewBag.Mensaje = "Se Adjunto al expediente principal";
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                    }
                    return PartialView("_SuccessHT");

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
        public ActionResult Ver_Historial_HT(int id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    ViewBag.var_numero = id.ToString();
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
        public ActionResult Imprimir_HT()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    HojaTramiteViewModel model = new HojaTramiteViewModel();
                    ViewBag.str_imprimir = "0";
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

        public ActionResult Imprimir_Pendientes_x_persona()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[6].Trim() == "1")
                {

                    List<SelectListItem> Lista_personal = new List<SelectListItem>();

                    var recupera_persona = _GeneralService.Recupera_personal_oficina(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim().ToString()));
                    foreach (var result in recupera_persona)
                    {
                        Lista_personal.Add(new SelectListItem()
                        {
                            Text = result.nom_persona,
                            Value = result.persona_num_documento.ToString()
                        }
                        );
                    };

                    HojaTramiteViewModel model = new HojaTramiteViewModel();
                    ViewBag.listar_personal = Lista_personal;
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

        public ActionResult Imprimir_Nuevo(string id)
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

        public ActionResult Imprimir_Vacia(string id)
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

        public ActionResult Imprimir_masivo(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    ViewBag.HT = id;
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

        public ActionResult Imprimir_History(string id)
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

        public ActionResult Imprimir_Administrado(string id)
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

        public ActionResult Imprimir_Pendientes(string id)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[6].Trim() == "1")
                {
                    ViewBag.persona_num_documento = id;
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

        public ActionResult HT_Llenar(string HT)
        {
            HojaTramiteViewModel model = new HojaTramiteViewModel();
            if (HT.Trim() != "")
            {
                try
                {
                    DocumentoResponse doc_Response = new DocumentoResponse();
                    doc_Response = _HojaTramiteService.Consultar_HT(HT.Trim());
                    model = ResponseToModel.HojaTramite(doc_Response);
                }
                catch (Exception)
                { }

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Llenar_personal(int id_oficina_destino)
        {
            List<SelectListItem> Lista_personal = new List<SelectListItem>();

            Lista_personal.Add(new SelectListItem()
            {
                Text = "SELECCIONAR PERSONAL",
                Value = ""
            });
            var recupera_persona = _GeneralService.Recupera_personal_oficina(id_oficina_destino);
            foreach (var result in recupera_persona)
            {
                Lista_personal.Add(new SelectListItem()
                {
                    Text = result.nom_persona,
                    Value = result.persona_num_documento.ToString()
                }
                );
            };
            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(Lista_personal, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Salida_HT(int page = 1, string HT = "", string Asunto = "", string Empresa = "", string cmbtupa = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    //IEnumerable<DocumentoDetalleResponse> model = new List<DocumentoDetalleResponse>();

                    List<SelectListItem> Lista_Oficina_destino = new List<SelectListItem>();
                    List<SelectListItem> lista_sedes = new List<SelectListItem>();
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

                    List<SelectListItem> Lista_tupa = new List<SelectListItem>();

                    Lista_tupa.Add(new SelectListItem()
                    {
                        Text = "TRAMITE NO TUPA",
                        Value = ""
                    });

                    foreach (var result in _GeneralService.recupera_tupa().Where(x => x.id_oficina != 18).OrderBy(x => x.id_tipo_tupa).OrderBy(x => x.numero))
                    {
                        Lista_tupa.Add(new SelectListItem()
                        {
                            Text = _GeneralService.recupera_tipo_tupa().Where(y => y.id_tipo_tupa == result.id_tipo_tupa).First().nombre + " : " + result.numero.ToString() + " / " + result.asunto,
                            Value = result.id_tupa.ToString()
                        });
                    };

                    ViewBag.lst_tupa = Lista_tupa;

                    ViewBag.lstsede_destino = lista_sedes;
                    ViewBag.lstOficina_destino = Lista_Oficina_destino;
                    ViewBag.lstpersonal_oficina = lista_personal;
                    ViewBag.Crear = true;
                    //ViewBag.TotalRows = _HojaTramiteService.CountDerivadas(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa);

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("HOJA_TRAMITE");
                    tbl.Columns.Add("TIPO_HOJA_TRAMITE");
                    tbl.Columns.Add("FECHA_DERIVO");
                    tbl.Columns.Add("DIRIGIDO_A");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("ASUNTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("FOLIOS");
                    tbl.Columns.Add("ID_DET_DOCUMENTO");
                    tbl.Columns.Add("HOJA_TRAMITE_ID_CAB_DET_DOCUMENTO_ID_DOCUMENTO");
                    tbl.Columns.Add("HOJA_TRAMITE_ID_DET_DOCUMENTO");
                    tbl.Columns.Add("NRO_HT");
                    tbl.Columns.Add("VER_PDF");
                    tbl.Columns.Add("ID_EST_TRAMITE");
                    tbl.Columns.Add("NUMERO_ID_HT_TEXTO");
                    tbl.Columns.Add("ID_TIPO_DOCUMENTO");
                    tbl.Columns.Add("VER_DOCU_PDF");
                    tbl.Columns.Add("ID_DOCUMENTO");
                    tbl.Columns.Add("TUPA");

                    IEnumerable<DocumentoDetalleResponse> var_ht_recibidos = new List<DocumentoDetalleResponse>();
                    if (HttpContext.User.Identity.Name.Split('|')[5].Trim() == "20" || HttpContext.User.Identity.Name.Split('|')[5].Trim() == "18")
                    {
                        var_ht_recibidos = _HojaTramiteService.GetAllDerivadas_x_persona(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, HttpContext.User.Identity.Name.Split('|')[1].Trim(), cmbtupa);
                    }
                    else
                    {
                        var_ht_recibidos = _HojaTramiteService.GetAllDerivadas(Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HT, Asunto, Empresa, cmbtupa);
                    }

                    foreach (var result in var_ht_recibidos)
                    {

                        string ver_docu_pdf = "0";
                        if (result.documento.ruta_pdf != null)
                        {
                            ver_docu_pdf = "1";
                        }
                        tbl.Rows.Add(
                            result.documento.hoja_tramite.hoja_tramite,
                            result.documento.hoja_tramite.nombre_tipo_tramite,
                            result.fecha_crea,
                            result.nombre_encargado,
                            result.documento.nom_doc,
                            result.documento.hoja_tramite.asunto,
                            result.documento.hoja_tramite.nombre_oficina,
                            result.documento.folios.ToString(),
                            result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.hoja_tramite + "|" + result.id_cab_det_documento.ToString() + "|" + result.id_documento.ToString(),
                            result.documento.hoja_tramite.hoja_tramite + "|" + result.id_det_documento.ToString(),
                            result.documento.hoja_tramite.numero.ToString(),
                            result.documento.hoja_tramite.ver_pdf,
                            result.id_est_tramite,
                            result.documento.hoja_tramite.numero + "|" + result.documento.hoja_tramite.hoja_tramite,
                            result.documento.id_tipo_documento.ToString(),
                            ver_docu_pdf,
                            result.documento.id_documento.ToString(),
                            result.documento.hoja_tramite.nom_tupa
                            );
                    };

                    ViewData["HT_ENVIADOS"] = tbl;

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
        public ActionResult Consultar_DNI_vista(string DNI = "")
        {

            List<SelectListItem> lista_dni = new List<SelectListItem>();

            foreach (var x in _HojaTramiteService.Consultar_DNI(DNI))
            {
                lista_dni.Add(new SelectListItem()
                {
                    Text = x.paterno + " " + x.materno + " " + x.nombres,
                    Value = x.persona_num_documento
                });
            }

            if (lista_dni.Count() <= 0)
            {
                lista_dni.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_dni, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Consultar_DNI_total()
        {

            List<SelectListItem> lista_dni = new List<SelectListItem>();
            lista_dni.Add(new SelectListItem()
            {
                Text = "SELECCIONAR PERSONAL",
                Value = ""
            });

            foreach (var x in _HojaTramiteService.Consultar_DNI_total())
            {
                lista_dni.Add(new SelectListItem()
                {
                    Text = x.paterno + " " + x.materno + " " + x.nombres + " - " + x.persona_num_documento,
                    Value = x.persona_num_documento
                });
            }
            return Json(lista_dni, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult consultar_tipo_expediente(int id = 0)
        {

            List<SelectListItem> lista_tipo = new List<SelectListItem>();

            foreach (var x in _GeneralService.Consulta_Tipo_Documento(id))
            {
                lista_tipo.Add(new SelectListItem()
                {
                    Text = x.tp_e_i,
                    Value = x.tp_e_i
                });
            }

            return Json(lista_tipo, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Consultar_DNI_DIRECCION_vista(string DNI = "")
        {
            List<SelectListItem> lista_dni = new List<SelectListItem>();
            foreach (var x in _HojaTramiteService.Consultar_DNI(DNI))
            {
                lista_dni.Add(new SelectListItem()
                {
                    Text = x.direccion,
                    Value = x.persona_num_documento
                });
            }
            if (lista_dni.Count() <= 0)
            {
                lista_dni.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_dni, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_RUC_vista(string RUC = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _HojaTramiteService.Consultar_RUC(RUC))
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
        public ActionResult recupera_RUC_DIRECCION_vista(int ID_OFICINA = 1)
        {

            List<SelectListItem> lista_direcciones = new List<SelectListItem>();

            foreach (var x in _HojaTramiteService.Consultar_DIRECCION(ID_OFICINA))
            {
                lista_direcciones.Add(new SelectListItem()
                {
                    Text = x.direccion,
                    Value = x.id_oficina_direccion.ToString()
                });
            }

            if (lista_direcciones.Count() <= 0)
            {
                lista_direcciones.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_direcciones, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_PLANTA_vista(int ID_DIRECCION = 1)
        {

            List<SelectListItem> lista_planta = new List<SelectListItem>();

            lista_planta.Add(new SelectListItem()
            {
                Text = "SELECCIONAR PLANTA",
                Value = ""
            });

            foreach (var x in _GeneralService.recupera_planta_x_direccion(ID_DIRECCION, "1"))
            {
                lista_planta.Add(new SelectListItem()
                {
                    Text = x.siglas_tipo_planta + ("000" + x.numero_planta).Substring(("000" + x.numero_planta).Length - 3, 3) + "-" + x.nombre_planta,
                    Value = x.id_planta.ToString()
                });
            }

            return Json(lista_planta, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_ALMACEN_vista(int ID_DIRECCION = 1)
        {

            List<SelectListItem> lista_almacen = new List<SelectListItem>();

            lista_almacen.Add(new SelectListItem()
            {
                Text = "SELECCIONAR ALMACEN",
                Value = ""
            });

            foreach (var x in _GeneralService.lista_almacen("", ID_DIRECCION))
            {
                lista_almacen.Add(new SelectListItem()
                {
                    Text = x.nom_cod_habilitante,
                    Value = x.id_almacen.ToString()
                });
            }

            return Json(lista_almacen, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_CONCESION_vista(string DOCUMENTO = "")
        {

            List<SelectListItem> lista_concesion = new List<SelectListItem>();

            lista_concesion.Add(new SelectListItem()
            {
                Text = "SELECCIONAR CONCESION",
                Value = ""
            });

            foreach (var x in _GeneralService.lista_concesion("", DOCUMENTO))
            {
                lista_concesion.Add(new SelectListItem()
                {
                    Text = x.codigo_habilitacion,
                    Value = x.id_concesion.ToString()
                });
            }

            return Json(lista_concesion, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_RUC_NOM_vista(string NOM = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _HojaTramiteService.Consultar_RUC_X_NOM(NOM))
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
        public ActionResult recupera_DNI_NOM_vista(string NOM = "", string TIPO = "")
        {

            List<SelectListItem> lista_personas = new List<SelectListItem>();

            foreach (var x in _HojaTramiteService.Consultar_DNI_x_NOM(NOM, TIPO))
            {
                lista_personas.Add(new SelectListItem()
                {
                    Text = x.persona_num_documento + " - " + x.paterno + " " + x.materno + " " + x.nombres,
                    Value = x.persona_num_documento.ToString()
                });
            }

            if (lista_personas.Count() <= 0)
            {
                lista_personas.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_personas, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Existe_persona(string persona_num_documento = "")
        {

            string encuentra = "";

            if (_GeneralService.buscar_persona(persona_num_documento) > 0)
            {
                encuentra = "SI";
            }

            return Json(encuentra, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listar_Observacion_x_documento_ht(int id_det_documento = 0)
        {
            IEnumerable<DocDetObservacionesResponse> observaciones = new List<DocDetObservacionesResponse>();

            observaciones = _HojaTramiteService.Listar_Observacion_x_det_documento(id_det_documento);

            return Json(observaciones, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Guardar_Observacion_x_det_documento(int id_det_documento, string observacion)
        {
            DocDetObservacionesRequest observacion_det_documento = new DocDetObservacionesRequest();
            observacion_det_documento.activo = "1";
            observacion_det_documento.fecha_crea = DateTime.Now;
            observacion_det_documento.id_det_documento = id_det_documento;
            observacion_det_documento.observacion = observacion;
            observacion_det_documento.usuario_crea = HttpContext.User.Identity.Name.Split('|')[1].Trim();
            _HojaTramiteService.Grabar_DocDetObservaciones(observacion_det_documento);
            observacion_det_documento.usuario_crea = HttpContext.User.Identity.Name.Split('|')[3].Trim();
            return Json(observacion_det_documento, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Llenar_destino_adjunto_HT(string expediente)
        {
            List<SelectListItem> Lista_destino = new List<SelectListItem>();

            Lista_destino.Add(new SelectListItem()
            {
                Text = "SELECCIONAR OFICINA",
                Value = "0"
            });

            foreach (var result in _HojaTramiteService.GetAllpendienteshtadjuntar(expediente))
            {
                Lista_destino.Add(new SelectListItem()
                {
                    Text = result.oficina + " - " + result.nombre,
                    Value = result.oficina_destino.ToString() + " | " + result.persona_num_documento
                });
            };

            return Json(Lista_destino, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export_Excel_documentos_ht_archivadas_atendidas()
        {

            DataTable tbl_ht_pendientes = new DataTable();
            tbl_ht_pendientes.Columns.Add("Estado");
            tbl_ht_pendientes.Columns.Add("Fecha de Derivación");
            tbl_ht_pendientes.Columns.Add("Fecha de recepción");
            tbl_ht_pendientes.Columns.Add("Fecha de Archivo/Atendido");
            tbl_ht_pendientes.Columns.Add("Hoja de Trámite");
            tbl_ht_pendientes.Columns.Add("Oficina o Externo que crea HT");
            tbl_ht_pendientes.Columns.Add("Documento enviado");
            tbl_ht_pendientes.Columns.Add("Asunto");
            tbl_ht_pendientes.Columns.Add("Persona asignada");
            tbl_ht_pendientes.Columns.Add("Observación");

            int var_id_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

            var list = _HojaTramiteService.Export_Excel_documentos_ht_archivadas_atendidas(var_id_oficina);

            DataRow tbl_row_pendientes;
            foreach (var pendiente in list)
            {
                tbl_row_pendientes = tbl_ht_pendientes.NewRow();
                tbl_row_pendientes["Estado"] = pendiente.estado;
                tbl_row_pendientes["Fecha de Derivación"] = pendiente.fecha_crea;
                tbl_row_pendientes["Fecha de recepción"] = pendiente.fecha_recepcion;
                tbl_row_pendientes["Fecha de Archivo/Atendido"] = pendiente.fecha_fin;
                tbl_row_pendientes["Hoja de Trámite"] = pendiente.hoja_tramite;
                tbl_row_pendientes["Oficina o Externo que crea HT"] = pendiente.externo;
                tbl_row_pendientes["Documento enviado"] = pendiente.documento;
                tbl_row_pendientes["Asunto"] = pendiente.asunto;
                tbl_row_pendientes["Persona asignada"] = pendiente.persona_asignada;
                tbl_row_pendientes["Observación"] = pendiente.observacion;

                tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_ht_pendientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_Reporte_Documentos_En_Oficina_Archivados_Atendidos.xls");
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


        public ActionResult Export_Excel_documentos_ht_pendientes_por_atender()
        {

            DataTable tbl_ht_pendientes = new DataTable();
            tbl_ht_pendientes.Columns.Add("Estado");
            tbl_ht_pendientes.Columns.Add("Fecha de Derivación");
            tbl_ht_pendientes.Columns.Add("Fecha de recepción");
            tbl_ht_pendientes.Columns.Add("Hoja de Trámite");
            tbl_ht_pendientes.Columns.Add("Oficina o Externo que crea HT");
            tbl_ht_pendientes.Columns.Add("Documento enviado");
            tbl_ht_pendientes.Columns.Add("Asunto");
            tbl_ht_pendientes.Columns.Add("Persona asignada");
            tbl_ht_pendientes.Columns.Add("Observación");

            int var_id_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

            var list = _HojaTramiteService.Export_Excel_documentos_ht_pendientes_por_atender(var_id_oficina);

            DataRow tbl_row_pendientes;
            foreach (var pendiente in list)
            {
                tbl_row_pendientes = tbl_ht_pendientes.NewRow();
                tbl_row_pendientes["Estado"] = pendiente.estado;
                tbl_row_pendientes["Fecha de Derivación"] = pendiente.fecha_crea;
                tbl_row_pendientes["Fecha de recepción"] = pendiente.fecha_recepcion;
                tbl_row_pendientes["Hoja de Trámite"] = pendiente.hoja_tramite;
                tbl_row_pendientes["Oficina o Externo que crea HT"] = pendiente.externo;
                tbl_row_pendientes["Documento enviado"] = pendiente.documento;
                tbl_row_pendientes["Asunto"] = pendiente.asunto;
                tbl_row_pendientes["Persona asignada"] = pendiente.persona_asignada;
                tbl_row_pendientes["Observación"] = pendiente.observacion;

                tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_ht_pendientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_Reporte_Documentos_En_Oficina_Archivados_Atendidos.xls");
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

        public ActionResult Export_Excel_documentos_ht_pendientes_por_recibir()
        {

            DataTable tbl_ht_pendientes = new DataTable();
            tbl_ht_pendientes.Columns.Add("Estado");
            tbl_ht_pendientes.Columns.Add("Fecha de Derivación");
            tbl_ht_pendientes.Columns.Add("Hoja de Trámite");
            tbl_ht_pendientes.Columns.Add("Oficina o Externo que crea HT");
            tbl_ht_pendientes.Columns.Add("Documento enviado");
            tbl_ht_pendientes.Columns.Add("Asunto");
            tbl_ht_pendientes.Columns.Add("Persona asignada");

            int var_id_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

            var list = _HojaTramiteService.Export_Excel_documentos_ht_pendientes_por_recibir(var_id_oficina);

            DataRow tbl_row_pendientes;
            foreach (var pendiente in list)
            {
                tbl_row_pendientes = tbl_ht_pendientes.NewRow();
                tbl_row_pendientes["Estado"] = pendiente.estado;
                tbl_row_pendientes["Fecha de Derivación"] = pendiente.fecha_crea;
                tbl_row_pendientes["Hoja de Trámite"] = pendiente.hoja_tramite;
                tbl_row_pendientes["Oficina o Externo que crea HT"] = pendiente.externo;
                tbl_row_pendientes["Documento enviado"] = pendiente.documento;
                tbl_row_pendientes["Asunto"] = pendiente.asunto;
                tbl_row_pendientes["Persona asignada"] = pendiente.persona_asignada;

                tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_ht_pendientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_Reportes_Documentos_Pendientes_por_recibir.xls");
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

        public ActionResult Export_Excel_documentos_ht_enviados()
        {

            DataTable tbl_ht_enviados = new DataTable();
            tbl_ht_enviados.Columns.Add("Estado");
            tbl_ht_enviados.Columns.Add("Fecha de envío");
            tbl_ht_enviados.Columns.Add("Fecha de recibido");
            tbl_ht_enviados.Columns.Add("Hoja de Trámite");
            tbl_ht_enviados.Columns.Add("Oficina o Externo que crea HT");
            tbl_ht_enviados.Columns.Add("Documento enviado");
            tbl_ht_enviados.Columns.Add("Asunto");
            tbl_ht_enviados.Columns.Add("Persona destino");
            tbl_ht_enviados.Columns.Add("Observación");
            tbl_ht_enviados.Columns.Add("Indicadores");

            int var_id_oficina = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim());

            var list = _HojaTramiteService.Export_Excel_documentos_ht_enviadas(var_id_oficina);

            DataRow tbl_row_pendientes;
            foreach (var pendiente in list)
            {
                tbl_row_pendientes = tbl_ht_enviados.NewRow();
                tbl_row_pendientes["Estado"] = pendiente.estado;
                tbl_row_pendientes["Fecha de envío"] = pendiente.fecha_crea;
                tbl_row_pendientes["Fecha de recibido"] = pendiente.fecha_recibido;
                tbl_row_pendientes["Hoja de Trámite"] = pendiente.hoja_tramite;
                tbl_row_pendientes["Oficina o Externo que crea HT"] = pendiente.externo;
                tbl_row_pendientes["Documento enviado"] = pendiente.documento;
                tbl_row_pendientes["Asunto"] = pendiente.asunto;
                tbl_row_pendientes["Persona destino"] = pendiente.persona_destino;
                tbl_row_pendientes["Observación"] = pendiente.observacion;
                tbl_row_pendientes["Indicadores"] = pendiente.indicadores;

                tbl_ht_enviados.Rows.Add(tbl_row_pendientes);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_ht_enviados;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_Reporte_Documentos_enviados.xls");
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


        [AllowAnonymous]
        public ActionResult variable_Subir_archivo_doc_ht(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_document_id_doc_ht"] = id;
                return RedirectToAction("Adjuntar_archivo_document_ht", "HojaTramite");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Imprimir_reporte_usuario(string val_txtfechainicio = "", string val_txtfechafin = "")
        {
            string texto = "";
            if (val_txtfechainicio == "" && val_txtfechafin == "")
            {
                texto = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            }

            if (val_txtfechainicio == "" && val_txtfechafin != "")
            {
                texto = val_txtfechafin;
            }

            if (val_txtfechainicio != "" && val_txtfechafin == "")
            {
                if (val_txtfechainicio != DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())
                {
                    texto = val_txtfechainicio + " al " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    texto = val_txtfechainicio;
                }
            }

            if (val_txtfechainicio != "" && val_txtfechafin != "")
            {
                if (val_txtfechainicio != DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())
                {
                    texto = val_txtfechainicio + " al " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    texto = val_txtfechainicio;
                }
            }

            int ival_txtfechainicio = 0;
            int ival_txtfechafin = 0;

            if (val_txtfechainicio != "")
            {
                val_txtfechainicio = val_txtfechainicio.Substring(6, 4) + val_txtfechainicio.Substring(3, 2) + val_txtfechainicio.Substring(0, 2);
                ival_txtfechainicio = Convert.ToInt32(val_txtfechainicio);
            }
            if (val_txtfechafin != "")
            {
                val_txtfechafin = val_txtfechafin.Substring(6, 4) + val_txtfechafin.Substring(3, 2) + val_txtfechafin.Substring(0, 2);
                ival_txtfechafin = Convert.ToInt32(val_txtfechafin);
            }

            ViewBag.lst_desdehasta = texto;
            ViewBag.text_registro = _HojaTramiteService.Consultar_registro_de_usuario(HttpContext.User.Identity.Name.Split('|')[1].Trim(), ival_txtfechainicio, ival_txtfechafin).First().texto;
            return View();
        }

        [AllowAnonymous]
        public ActionResult imprimir_reporte_usuario_ss_new(string fecini, string fecfin)
        {

            if (fecini == "0") { fecini = ""; } else { fecini = fecini.Replace("-", "/"); }
            if (fecfin == "0") { fecfin = ""; } else { fecfin = fecfin.Replace("-", "/"); }
            string val_txtfechainicio = fecini;
            string val_txtfechafin = fecfin;

            string texto = "";
            if (val_txtfechainicio == "" && val_txtfechafin == "")
            {
                texto = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            }

            if (val_txtfechainicio == "" && val_txtfechafin != "")
            {
                texto = val_txtfechafin;
            }

            if (val_txtfechainicio != "" && val_txtfechafin == "")
            {
                if (val_txtfechainicio != DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())
                {
                    texto = val_txtfechainicio + " al " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    texto = val_txtfechainicio;
                }
            }

            if (val_txtfechainicio != "" && val_txtfechafin != "")
            {
                if (val_txtfechainicio != DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())
                {
                    texto = val_txtfechainicio + " al " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    texto = val_txtfechainicio;
                }
            }

            int ival_txtfechainicio = 0;
            int ival_txtfechafin = 0;

            if (val_txtfechainicio != "")
            {
                val_txtfechainicio = val_txtfechainicio.Substring(6, 4) + val_txtfechainicio.Substring(3, 2) + val_txtfechainicio.Substring(0, 2);
                ival_txtfechainicio = Convert.ToInt32(val_txtfechainicio);
            }
            if (val_txtfechafin != "")
            {
                val_txtfechafin = val_txtfechafin.Substring(6, 4) + val_txtfechafin.Substring(3, 2) + val_txtfechafin.Substring(0, 2);
                ival_txtfechafin = Convert.ToInt32(val_txtfechafin);
            }

            Session["ival_txtfechainicio_sess"] = ival_txtfechainicio;
            Session["ival_txtfechafin_sess"] = ival_txtfechafin;
            Session["texto_sess"] = texto;

            return RedirectToAction("imprimir_reporte_usuario_new", "HojaTramite");
        }

        [AllowAnonymous]
        public ActionResult imprimir_reporte_usuario_new()
        {

            int ival_txtfechainicio = Convert.ToInt32(Session["ival_txtfechainicio_sess"].ToString());
            int ival_txtfechafin = Convert.ToInt32(Session["ival_txtfechafin_sess"].ToString());
            string texto = Session["texto_sess"].ToString();

            Session.Remove("ival_txtfechainicio_sess");
            Session.Remove("ival_txtfechafin_sess");
            Session.Remove("texto_sess");

            ViewBag.lst_desdehasta = texto;
            ViewBag.text_registro = _HojaTramiteService.Consultar_registro_de_usuario(HttpContext.User.Identity.Name.Split('|')[1].Trim(), ival_txtfechainicio, ival_txtfechafin).First().texto;
            return View();

        }

        [AllowAnonymous]
        public ActionResult Imprimir_cuadro_pendientes(int aniodesde = 0, int aniohasta = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[28].Trim() == "1")
                {
                    List<SelectListItem> lista_anio = new List<SelectListItem>();

                    int anio_inicio = 2013;

                    lista_anio.Add(new SelectListItem()
                    {
                        Text = "Seleccionar",
                        Value = "0"
                    });

                    while (anio_inicio <= DateTime.Now.Year)
                    {
                        lista_anio.Add(new SelectListItem()
                        {
                            Text = anio_inicio.ToString(),
                            Value = anio_inicio.ToString()
                        });
                        anio_inicio += 1;
                    };

                    ViewBag.lst_aniodesde = lista_anio;
                    ViewBag.lst_aniohasta = lista_anio;

                    string cad_anios = "";
                    if (aniodesde == 0) { aniodesde = 2013; }
                    if (aniohasta == 0) { aniohasta = DateTime.Now.Year; }
                    IEnumerable<VerPedientesGesdocResponse> res_pendx = new List<VerPedientesGesdocResponse>();
                    res_pendx = _HojaTramiteService.lista_pendientes_sigesdoc(aniodesde, aniohasta).OrderBy(x => x.anio);

                    #region Año crea

                    IList<int> intListanio = new List<int>();
                    foreach (var xyz in res_pendx)
                    {
                        int val = xyz.anio ?? 0;
                        int encontr = 0;
                        foreach (var el in intListanio)
                        {
                            if (el == xyz.anio)
                            {
                                encontr = 1;
                            }
                        }
                        if (encontr == 0) { intListanio.Add(val); }
                    }

                    foreach (var x_llenar in intListanio)
                    {
                        if (cad_anios != "")
                        {
                            cad_anios = cad_anios + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">" + x_llenar + "</td>";
                        }
                        else
                        {
                            cad_anios = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">" + x_llenar + "</td>";
                        }

                    }
                    cad_anios = cad_anios + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">Total</td>";

                    ViewBag.html_llenar_anio = cad_anios;

                    #endregion

                    #region Año tramite_no_tupa

                    IEnumerable<VerPedientesGesdocResponse> res_pend = new List<VerPedientesGesdocResponse>();
                    res_pend = res_pendx.Where(x => x.tupa == null);

                    //////PRESIDENCIA EJECUTIVA/////
                    string cad_xrec_rec = "";
                    string cad_xrec_rec_det = "";
                    IEnumerable<VerPedientesGesdocResponse> res_pend2 = new List<VerPedientesGesdocResponse>();

                    IList<int> intList = new List<int>();
                    var states = new int[] { 1 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    int tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">PRESIDENCIA EJECUTIVA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///CONSEJO DIRECTIVO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 4826 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">CONSEJO DIRECTIVO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///GERENCIA GENERAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            2 //GERENCIA GENERAL
,42 //ATENCION AL CIUDADANO
,438 //COMUNICACIONES
,58 //MESA DE PARTES
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///OFICINA DE ADMINISTRACION///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            8 //OFICINA DE ADMINISTRACION
,1926	//UNIDAD DE PROYECTO DE INVERSION PUBLICA
,28	//UNIDAD DE CONTABILIDAD, FINANZAS Y TESORERIA
,29	//UNIDAD DE ABASTECIMIENTO
,30	//UNIDAD DE TECNOLOGIA DE LA INFORMACION
,31	//UNIDAD DE RECURSOS HUMANOS
,32	//UNIDAD DE EJECUCION COACTIVA
,140	//UNIDAD DE TRANSPORTE
,5326	//Comite de Seleccion de Licitacion Publica Mp-01-2018-SANIPES-1
,5327	//Comité de Selección Concurso Publico  No.002-2018-SANIPES-1
,778	//SECRETARIA TÉCNICA  DE PROCEDIMIENTOS ADMINISTRATIVOS DISCIPLINARIOS
,766	//COMITE DE SEGURIDAD Y SALUD EN EL TRABAJO
,3901	//COMITE DE SELECCION PARA EL PROCEDIMIENTO DE SELECCION DE LICITACION PUBLICA NRO. 1-2017-SANIPES
,5385   //COMITÉ DE SELECCIÓN ADJUDICACIÓN SIMPLIFICADA No.02-2018
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ADMINISTRACION</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE PLANEAMIENTO Y PRESUPUESTO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            9 //OFICINA DE PLANEAMIENTO Y PRESUPUESTO
,33	//UNIDAD DE PLANEAMIENTO Y RACIONALIZACION
,34	//UNIDAD DE PRESUPUESTO
,35	//UNIDAD DE COOPERACION TECNICA
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE PLANEAMIENTO Y PRESUPUESTO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE ASESORIA JURIDICA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 10 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ASESORIA JURIDICA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 //DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA
,127	//JEFATURA DE SUPERVISION DE INSPECTORES
,143	//LABORATORIO
,244	//AREA DE GESTION Y CONTROL
,11	//SUB DIRECCION DE INOCUIDAD PESQUERA
,12	//SUB DIRECCION DE SANIDAD ACUICOLA
,13	//SUB DIRECCION DE NORMATIVIDAD SANITARIA PESQUERA Y ACUICOLA
,4764 //DSNPA - ATENCION AL CLIENTE
,3787	//LABORATORIO DE MICROBIOLOGIA
,3788	//LABORATORIO DE BIOTOXINAS
,3789	//LABORATORIO DE CROMATOGRAFÍA
,3790	//LABORATORIO DE BIOMOLECULAR
,3791	//LABORATORIO DE FITOPLANCTON
,3792	//LABORATORIO DE METALES 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                6 //DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA
,14	//SUB DIRECCION DE SUPERVISION PESQUERA
,15	//SUB DIRECCION DE SUPERVISION ACUICOLA
,16	//SUB DIRECCION DE FISCALIZACION PESQUERA Y ACUICOLA
,706	//DSFPA-ATENCION AL CLIENTE

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE HABILITACIONES Y CERTIFICACIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                7 //DIRECCION DE HABILITACIONES Y CERTIFICACIONES PESQUERAS Y ACUICOLAS
,17	//SUB DIRECCION DE CERTIFICACIONES PESQUERAS Y ACUICOLAS
,18	//SUB DIRECCION DE HABILITACIONES PESQUERAS Y ACUICOLAS

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE HABILITACIONES Y CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION DE SANCIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                130 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SANCIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINAS DESCONCENTRADAS///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
               4
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINAS DESCONCENTRADAS</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_det = cad_xrec_rec;

                    #endregion

                    #region Año tramite_tupa

                    IEnumerable<VerPedientesGesdocResponse> res_pendtup = new List<VerPedientesGesdocResponse>();
                    res_pendtup = res_pendx.Where(x => x.tupa != null);

                    //////SUB DIRECCION DE HABILITACIONES/////
                    cad_xrec_rec = "";
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 18 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">SUB DIRECCION DE HABILITACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";


                    //////SUB DIRECCION DE CERTIFICACIONES/////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 17 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">SUB DIRECCION DE CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    //////DIR. SUPERV. Y FISC./////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 6 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIR. SUPERV. Y FISC.</td>" + cad_xrec_rec_det +
                        "</tr>";

                    //////DIR. SANIT. Y DE NORM./////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIR. SANIT. Y DE NORM.</td>" + cad_xrec_rec_det +
                        "</tr>";

                    //////GERENCIA GENERAL/////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 2 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pendtup.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_det_tup = cad_xrec_rec;

                    #endregion

                    #region Año tramite_total

                    //////PRESIDENCIA EJECUTIVA/////
                    cad_xrec_rec = "";
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 1 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">PRESIDENCIA EJECUTIVA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///CONSEJO DIRECTIVO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 4826 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">CONSEJO DIRECTIVO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///GERENCIA GENERAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                    2 //GERENCIA GENERAL
                    ,42 //ATENCION AL CIUDADANO
                    ,438 //COMUNICACIONES
                    ,58 //MESA DE PARTES
                    };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///OFICINA DE ADMINISTRACION///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            8 //OFICINA DE ADMINISTRACION
,1926	//UNIDAD DE PROYECTO DE INVERSION PUBLICA
,28	//UNIDAD DE CONTABILIDAD, FINANZAS Y TESORERIA
,29	//UNIDAD DE ABASTECIMIENTO
,30	//UNIDAD DE TECNOLOGIA DE LA INFORMACION
,31	//UNIDAD DE RECURSOS HUMANOS
,32	//UNIDAD DE EJECUCION COACTIVA
,140	//UNIDAD DE TRANSPORTE
,5326	//Comite de Seleccion de Licitacion Publica Mp-01-2018-SANIPES-1
,5327	//Comité de Selección Concurso Publico  No.002-2018-SANIPES-1
,778	//SECRETARIA TÉCNICA  DE PROCEDIMIENTOS ADMINISTRATIVOS DISCIPLINARIOS
,766	//COMITE DE SEGURIDAD Y SALUD EN EL TRABAJO
,3901	//COMITE DE SELECCION PARA EL PROCEDIMIENTO DE SELECCION DE LICITACION PUBLICA NRO. 1-2017-SANIPES
,5385   //COMITÉ DE SELECCIÓN ADJUDICACIÓN SIMPLIFICADA No.02-2018
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ADMINISTRACION</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE PLANEAMIENTO Y PRESUPUESTO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            9 //OFICINA DE PLANEAMIENTO Y PRESUPUESTO
,33	//UNIDAD DE PLANEAMIENTO Y RACIONALIZACION
,34	//UNIDAD DE PRESUPUESTO
,35	//UNIDAD DE COOPERACION TECNICA
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE PLANEAMIENTO Y PRESUPUESTO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE ASESORIA JURIDICA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 10 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ASESORIA JURIDICA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 //DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA
,127	//JEFATURA DE SUPERVISION DE INSPECTORES
,143	//LABORATORIO
,244	//AREA DE GESTION Y CONTROL
,11	//SUB DIRECCION DE INOCUIDAD PESQUERA
,12	//SUB DIRECCION DE SANIDAD ACUICOLA
,13	//SUB DIRECCION DE NORMATIVIDAD SANITARIA PESQUERA Y ACUICOLA
,4764 //DSNPA - ATENCION AL CLIENTE
,3787	//LABORATORIO DE MICROBIOLOGIA
,3788	//LABORATORIO DE BIOTOXINAS
,3789	//LABORATORIO DE CROMATOGRAFÍA
,3790	//LABORATORIO DE BIOMOLECULAR
,3791	//LABORATORIO DE FITOPLANCTON
,3792	//LABORATORIO DE METALES 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                6 //DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA
,14	//SUB DIRECCION DE SUPERVISION PESQUERA
,15	//SUB DIRECCION DE SUPERVISION ACUICOLA
,16	//SUB DIRECCION DE FISCALIZACION PESQUERA Y ACUICOLA
,706	//DSFPA-ATENCION AL CLIENTE

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE HABILITACIONES Y CERTIFICACIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                7 //DIRECCION DE HABILITACIONES Y CERTIFICACIONES PESQUERAS Y ACUICOLAS
,17	//SUB DIRECCION DE CERTIFICACIONES PESQUERAS Y ACUICOLAS
,18	//SUB DIRECCION DE HABILITACIONES PESQUERAS Y ACUICOLAS

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE HABILITACIONES Y CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION DE SANCIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                130 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SANCIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINAS DESCONCENTRADAS///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
               4
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINAS DESCONCENTRADAS</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pendx.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_tot = cad_xrec_rec;

                    #endregion

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
        public ActionResult Imprimir_Orden_Ensayo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Imprimir_pendientes_new(int aniodesde = 0, int aniohasta = 0)
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[28].Trim() == "1")
                {
                    string cad_anios = "";
                    if (aniodesde == 0) { aniodesde = 2013; }
                    if (aniohasta == 0) { aniohasta = DateTime.Now.Year; }
                    IEnumerable<VerPedientesGesdocResponse> res_pendx = new List<VerPedientesGesdocResponse>();
                    res_pendx = _HojaTramiteService.lista_pendientes_sigesdoc(aniodesde, aniohasta).OrderBy(x => x.anio);

                    #region Año crea

                    IList<int> intListanio = new List<int>();
                    foreach (var xyz in res_pendx)
                    {
                        int val = xyz.anio ?? 0;
                        int encontr = 0;
                        foreach (var el in intListanio)
                        {
                            if (el == xyz.anio)
                            {
                                encontr = 1;
                            }
                        }
                        if (encontr == 0) { intListanio.Add(val); }
                    }

                    foreach (var x_llenar in intListanio)
                    {
                        if (cad_anios != "")
                        {
                            cad_anios = cad_anios + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">" + x_llenar + "</td>";
                        }
                        else
                        {
                            cad_anios = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">" + x_llenar + "</td>";
                        }

                    }
                    cad_anios = cad_anios + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; " + (char)34 + " width=" + (char)34 + "8%" + (char)34 + ">Total</td>";

                    ViewBag.html_llenar_anio = cad_anios;

                    #endregion

                    #region Año tramite_no_tupa

                    IEnumerable<VerPedientesGesdocResponse> res_pend = new List<VerPedientesGesdocResponse>();
                    res_pend = res_pendx.Where(x => x.tupa == null);

                    //////PRESIDENCIA EJECUTIVA/////
                    string cad_xrec_rec = "";
                    string cad_xrec_rec_det = "";
                    IEnumerable<VerPedientesGesdocResponse> res_pend2 = new List<VerPedientesGesdocResponse>();

                    IList<int> intList = new List<int>();
                    var states = new int[] { 1 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    int tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">PRESIDENCIA EJECUTIVA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///CONSEJO DIRECTIVO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 4826 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">CONSEJO DIRECTIVO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///GERENCIA GENERAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            2 //GERENCIA GENERAL
,42 //ATENCION AL CIUDADANO
,438 //COMUNICACIONES
,58 //MESA DE PARTES
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///OFICINA DE ADMINISTRACION///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            8 //OFICINA DE ADMINISTRACION
,1926	//UNIDAD DE PROYECTO DE INVERSION PUBLICA
,28	//UNIDAD DE CONTABILIDAD, FINANZAS Y TESORERIA
,29	//UNIDAD DE ABASTECIMIENTO
,30	//UNIDAD DE TECNOLOGIA DE LA INFORMACION
,31	//UNIDAD DE RECURSOS HUMANOS
,32	//UNIDAD DE EJECUCION COACTIVA
,140	//UNIDAD DE TRANSPORTE
,5326	//Comite de Seleccion de Licitacion Publica Mp-01-2018-SANIPES-1
,5327	//Comité de Selección Concurso Publico  No.002-2018-SANIPES-1
,778	//SECRETARIA TÉCNICA  DE PROCEDIMIENTOS ADMINISTRATIVOS DISCIPLINARIOS
,766	//COMITE DE SEGURIDAD Y SALUD EN EL TRABAJO
,3901	//COMITE DE SELECCION PARA EL PROCEDIMIENTO DE SELECCION DE LICITACION PUBLICA NRO. 1-2017-SANIPES
,5385   //COMITÉ DE SELECCIÓN ADJUDICACIÓN SIMPLIFICADA No.02-2018
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ADMINISTRACION</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE PLANEAMIENTO Y PRESUPUESTO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            9 //OFICINA DE PLANEAMIENTO Y PRESUPUESTO
,33	//UNIDAD DE PLANEAMIENTO Y RACIONALIZACION
,34	//UNIDAD DE PRESUPUESTO
,35	//UNIDAD DE COOPERACION TECNICA
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE PLANEAMIENTO Y PRESUPUESTO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE ASESORIA JURIDICA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 10 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ASESORIA JURIDICA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 //DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA
,127	//JEFATURA DE SUPERVISION DE INSPECTORES
,143	//LABORATORIO
,244	//AREA DE GESTION Y CONTROL
,11	//SUB DIRECCION DE INOCUIDAD PESQUERA
,12	//SUB DIRECCION DE SANIDAD ACUICOLA
,13	//SUB DIRECCION DE NORMATIVIDAD SANITARIA PESQUERA Y ACUICOLA
,4764 //DSNPA - ATENCION AL CLIENTE
,3787	//LABORATORIO DE MICROBIOLOGIA
,3788	//LABORATORIO DE BIOTOXINAS
,3789	//LABORATORIO DE CROMATOGRAFÍA
,3790	//LABORATORIO DE BIOMOLECULAR
,3791	//LABORATORIO DE FITOPLANCTON
,3792	//LABORATORIO DE METALES 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                6 //DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA
,14	//SUB DIRECCION DE SUPERVISION PESQUERA
,15	//SUB DIRECCION DE SUPERVISION ACUICOLA
,16	//SUB DIRECCION DE FISCALIZACION PESQUERA Y ACUICOLA
,706	//DSFPA-ATENCION AL CLIENTE

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE HABILITACIONES Y CERTIFICACIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                7 //DIRECCION DE HABILITACIONES Y CERTIFICACIONES PESQUERAS Y ACUICOLAS
,17	//SUB DIRECCION DE CERTIFICACIONES PESQUERAS Y ACUICOLAS
,18	//SUB DIRECCION DE HABILITACIONES PESQUERAS Y ACUICOLAS

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE HABILITACIONES Y CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION DE SANCIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                130 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SANCIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINAS DESCONCENTRADAS///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
               4
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pend.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINAS DESCONCENTRADAS</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_det = cad_xrec_rec;

                    #endregion

                    #region Año tramite_tupa

                    IEnumerable<VerPedientesGesdocResponse> res_pendtup = new List<VerPedientesGesdocResponse>();
                    res_pendtup = res_pendx.Where(x => x.tupa != null);

                    //////SUB DIRECCION DE HABILITACIONES/////
                    cad_xrec_rec = "";
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 18 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">SUB DIRECCION DE HABILITACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";


                    //////SUB DIRECCION DE CERTIFICACIONES/////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 17 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">SUB DIRECCION DE CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    //////DIR. SUPERV. Y FISC./////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 6 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIR. SUPERV. Y FISC.</td>" + cad_xrec_rec_det +
                        "</tr>";

                    //////DIR. SANIT. Y DE NORM./////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIR. SANIT. Y DE NORM.</td>" + cad_xrec_rec_det +
                        "</tr>";


                    //////GERENCIA GENERAL/////
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 2 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendtup.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pendtup.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_det_tup = cad_xrec_rec;

                    #endregion

                    #region Año tramite_total

                    //////PRESIDENCIA EJECUTIVA/////
                    cad_xrec_rec = "";
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 1 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;
                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">PRESIDENCIA EJECUTIVA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///CONSEJO DIRECTIVO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 4826 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">CONSEJO DIRECTIVO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///GERENCIA GENERAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            2 //GERENCIA GENERAL
,42 //ATENCION AL CIUDADANO
,438 //COMUNICACIONES
,58 //MESA DE PARTES
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">GERENCIA GENERAL</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///OFICINA DE ADMINISTRACION///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            8 //OFICINA DE ADMINISTRACION
,1926	//UNIDAD DE PROYECTO DE INVERSION PUBLICA
,28	//UNIDAD DE CONTABILIDAD, FINANZAS Y TESORERIA
,29	//UNIDAD DE ABASTECIMIENTO
,30	//UNIDAD DE TECNOLOGIA DE LA INFORMACION
,31	//UNIDAD DE RECURSOS HUMANOS
,32	//UNIDAD DE EJECUCION COACTIVA
,140	//UNIDAD DE TRANSPORTE
,5326	//Comite de Seleccion de Licitacion Publica Mp-01-2018-SANIPES-1
,5327	//Comité de Selección Concurso Publico  No.002-2018-SANIPES-1
,778	//SECRETARIA TÉCNICA  DE PROCEDIMIENTOS ADMINISTRATIVOS DISCIPLINARIOS
,766	//COMITE DE SEGURIDAD Y SALUD EN EL TRABAJO
,3901	//COMITE DE SELECCION PARA EL PROCEDIMIENTO DE SELECCION DE LICITACION PUBLICA NRO. 1-2017-SANIPES
,5385   //COMITÉ DE SELECCIÓN ADJUDICACIÓN SIMPLIFICADA No.02-2018
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ADMINISTRACION</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE PLANEAMIENTO Y PRESUPUESTO///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
            9 //OFICINA DE PLANEAMIENTO Y PRESUPUESTO
,33	//UNIDAD DE PLANEAMIENTO Y RACIONALIZACION
,34	//UNIDAD DE PRESUPUESTO
,35	//UNIDAD DE COOPERACION TECNICA
            };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE PLANEAMIENTO Y PRESUPUESTO</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINA DE ASESORIA JURIDICA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 10 };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINA DE ASESORIA JURIDICA</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 5 //DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA
,127	//JEFATURA DE SUPERVISION DE INSPECTORES
,143	//LABORATORIO
,244	//AREA DE GESTION Y CONTROL
,11	//SUB DIRECCION DE INOCUIDAD PESQUERA
,12	//SUB DIRECCION DE SANIDAD ACUICOLA
,13	//SUB DIRECCION DE NORMATIVIDAD SANITARIA PESQUERA Y ACUICOLA
,4764 //DSNPA - ATENCION AL CLIENTE
,3787	//LABORATORIO DE MICROBIOLOGIA
,3788	//LABORATORIO DE BIOTOXINAS
,3789	//LABORATORIO DE CROMATOGRAFÍA
,3790	//LABORATORIO DE BIOMOLECULAR
,3791	//LABORATORIO DE FITOPLANCTON
,3792	//LABORATORIO DE METALES 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                6 //DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA
,14	//SUB DIRECCION DE SUPERVISION PESQUERA
,15	//SUB DIRECCION DE SUPERVISION ACUICOLA
,16	//SUB DIRECCION DE FISCALIZACION PESQUERA Y ACUICOLA
,706	//DSFPA-ATENCION AL CLIENTE

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///DIRECCION DE HABILITACIONES Y CERTIFICACIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                7 //DIRECCION DE HABILITACIONES Y CERTIFICACIONES PESQUERAS Y ACUICOLAS
,17	//SUB DIRECCION DE CERTIFICACIONES PESQUERAS Y ACUICOLAS
,18	//SUB DIRECCION DE HABILITACIONES PESQUERAS Y ACUICOLAS

        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE HABILITACIONES Y CERTIFICACIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///DIRECCION DE SANCIONES///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
                130 
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">DIRECCION DE SANCIONES</td>" + cad_xrec_rec_det +
                        "</tr>";

                    ///OFICINAS DESCONCENTRADAS///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    states = new int[] { 
               4
        };
                    res_pend2 = new List<VerPedientesGesdocResponse>();
                    res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " >" + sum_cant.ToString() + "</td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + sum_cant.ToString() + "</td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }
                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + ">" + tot_sum_cant.ToString() + "</td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + ">OFICINAS DESCONCENTRADAS</td>" + cad_xrec_rec_det +
                        "</tr>";


                    ///TOTAL///
                    ///
                    cad_xrec_rec_det = "";
                    res_pend2 = new List<VerPedientesGesdocResponse>();

                    intList = new List<int>();
                    tot_sum_cant = 0;

                    foreach (var el in intListanio)
                    {
                        int sum_cant = 0;
                        foreach (var xyz in res_pendx.AsEnumerable().Where(x => x.anio == el))
                        {
                            sum_cant = sum_cant + (xyz.cant ?? 0);
                        }
                        if (cad_xrec_rec_det == "") { cad_xrec_rec_det = "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + " ><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        else { cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + sum_cant.ToString() + "</strong></td>"; }
                        tot_sum_cant = tot_sum_cant + sum_cant;
                    }

                    cad_xrec_rec_det = cad_xrec_rec_det + "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC; text-align: center" + (char)34 + "><strong>" + tot_sum_cant.ToString() + "</strong></td>";

                    cad_xrec_rec = cad_xrec_rec + "<tr style=" + (char)34 + "border-bottom:1px solid #CCC;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "border-bottom:1px solid #CCC; border-right:1px solid #CCC;" + (char)34 + "><strong>TOTAL</strong></td>" + cad_xrec_rec_det +
                        "</tr>";

                    ViewBag.html_llenar_tot = cad_xrec_rec;

                    #endregion

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
        public ActionResult Export_Excel_documentos_pendientes_sanipes(int aniodesde = 0, int aniohasta = 0)
        {

            DataTable tbl_ht_pendientes = new DataTable();
            tbl_ht_pendientes.Columns.Add("Resumen");

            if (aniodesde == 0) { aniodesde = 2013; }
            if (aniohasta == 0) { aniohasta = DateTime.Now.Year; }
            IEnumerable<VerPedientesGesdocResponse> res_pendx = new List<VerPedientesGesdocResponse>();
            res_pendx = _HojaTramiteService.lista_pendientes_sigesdoc(aniodesde, aniohasta).OrderBy(x => x.anio);

            IList<int> intListanio = new List<int>();
            foreach (var xyz in res_pendx)
            {
                int val = xyz.anio ?? 0;
                int encontr = 0;
                foreach (var el in intListanio)
                {
                    if (el == xyz.anio)
                    {
                        encontr = 1;
                    }
                }
                if (encontr == 0) { intListanio.Add(val); }
            }

            foreach (var x_llenar in intListanio)
            {
                tbl_ht_pendientes.Columns.Add(x_llenar.ToString());
            }
            tbl_ht_pendientes.Columns.Add("Total");
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////

            //////PRESIDENCIA EJECUTIVA/////
            IEnumerable<VerPedientesGesdocResponse> res_pend2 = new List<VerPedientesGesdocResponse>();

            IList<int> intList = new List<int>();
            var states = new int[] { 1 };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            int tot_sum_cant = 0;
            DataRow tbl_row_pendientes;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "PRESIDENCIA EJECUTIVA";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);

            ///CONSEJO DIRECTIVO///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 4826 };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "CONSEJO DIRECTIVO";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);

            ///GERENCIA GENERAL///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
            2 //GERENCIA GENERAL
,42 //ATENCION AL CIUDADANO
,438 //COMUNICACIONES
,58 //MESA DE PARTES
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "GERENCIA GENERAL";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);


            ///OFICINA DE ADMINISTRACION///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
            8 //OFICINA DE ADMINISTRACION
,1926	//UNIDAD DE PROYECTO DE INVERSION PUBLICA
,28	//UNIDAD DE CONTABILIDAD, FINANZAS Y TESORERIA
,29	//UNIDAD DE ABASTECIMIENTO
,30	//UNIDAD DE TECNOLOGIA DE LA INFORMACION
,31	//UNIDAD DE RECURSOS HUMANOS
,32	//UNIDAD DE EJECUCION COACTIVA
,140	//UNIDAD DE TRANSPORTE
,5326	//Comite de Seleccion de Licitacion Publica Mp-01-2018-SANIPES-1
,5327	//Comité de Selección Concurso Publico  No.002-2018-SANIPES-1
,778	//SECRETARIA TÉCNICA  DE PROCEDIMIENTOS ADMINISTRATIVOS DISCIPLINARIOS
,766	//COMITE DE SEGURIDAD Y SALUD EN EL TRABAJO
,3901	//COMITE DE SELECCION PARA EL PROCEDIMIENTO DE SELECCION DE LICITACION PUBLICA NRO. 1-2017-SANIPES
,5385   //-COMITÉ DE SELECCIÓN ADJUDICACIÓN SIMPLIFICADA No.02-2018
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "OFICINA DE ADMINISTRACION";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);


            ///OFICINA DE PLANEAMIENTO Y PRESUPUESTO///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
            9 //OFICINA DE PLANEAMIENTO Y PRESUPUESTO
,33	//UNIDAD DE PLANEAMIENTO Y RACIONALIZACION
,34	//UNIDAD DE PRESUPUESTO
,35	//UNIDAD DE COOPERACION TECNICA
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "OFICINA DE PLANEAMIENTO Y PRESUPUESTO";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);

            ///OFICINA DE ASESORIA JURIDICA///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
           10 
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "OFICINA DE ASESORIA JURIDICA";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);

            ///DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
           5 //DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA
,127	//JEFATURA DE SUPERVISION DE INSPECTORES
,143	//LABORATORIO
,244	//AREA DE GESTION Y CONTROL
,11	//SUB DIRECCION DE INOCUIDAD PESQUERA
,12	//SUB DIRECCION DE SANIDAD ACUICOLA
,13	//SUB DIRECCION DE NORMATIVIDAD SANITARIA PESQUERA Y ACUICOLA
,4764 //DSNPA - ATENCION AL CLIENTE
,3787	//LABORATORIO DE MICROBIOLOGIA
,3788	//LABORATORIO DE BIOTOXINAS
,3789	//LABORATORIO DE CROMATOGRAFÍA
,3790	//LABORATORIO DE BIOMOLECULAR
,3791	//LABORATORIO DE FITOPLANCTON
,3792	//LABORATORIO DE METALES
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "DIRECCION SANITARIA Y DE NORMATIVIDAD PESQUERA Y ACUICOLA";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);


            ///DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
           6 //DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA
,14	//SUB DIRECCION DE SUPERVISION PESQUERA
,15	//SUB DIRECCION DE SUPERVISION ACUICOLA
,16	//SUB DIRECCION DE FISCALIZACION PESQUERA Y ACUICOLA
,706	//DSFPA-ATENCION AL CLIENTE
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "DIRECCION DE SUPERVISION Y FISCALIZACION PESQUERA Y ACUICOLA";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);



            ///DIRECCION DE HABILITACIONES Y CERTIFICACIONES///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
           7 //DIRECCION DE HABILITACIONES Y CERTIFICACIONES PESQUERAS Y ACUICOLAS
,17	//SUB DIRECCION DE CERTIFICACIONES PESQUERAS Y ACUICOLAS
,18	//SUB DIRECCION DE HABILITACIONES PESQUERAS Y ACUICOLAS
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "DIRECCION DE HABILITACIONES Y CERTIFICACIONES";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);



            ///DIRECCION DE SANCIONES///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
          130
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "DIRECCION DE SANCIONES";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);



            ///OFICINAS DESCONCENTRADAS///
            ///
            res_pend2 = new List<VerPedientesGesdocResponse>();

            intList = new List<int>();
            states = new int[] { 
          4
            };
            res_pend2 = new List<VerPedientesGesdocResponse>();
            res_pend2 = res_pendx.Join(states, p => p.id_oficina, s => s, (p, s) => p);

            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "OFICINAS DESCONCENTRADAS";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pend2.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);


            ///TOTAL///
            ///

            intList = new List<int>();
            tot_sum_cant = 0;

            tbl_row_pendientes = tbl_ht_pendientes.NewRow();
            tbl_row_pendientes["Resumen"] = "TOTAL";
            foreach (var el in intListanio)
            {
                int sum_cant = 0;
                foreach (var xyz in res_pendx.AsEnumerable().Where(x => x.anio == el))
                {
                    sum_cant = sum_cant + (xyz.cant ?? 0);
                }
                tbl_row_pendientes[el.ToString()] = sum_cant.ToString();
                tot_sum_cant = tot_sum_cant + sum_cant;
            }
            tbl_row_pendientes["Total"] = tot_sum_cant.ToString();
            tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);




            GridView gv = new GridView();
            gv.DataSource = tbl_ht_pendientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_documentos_pendientes_sanipes.xls");
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

        [AllowAnonymous]
        public ActionResult Export_Excel_documentos_pendientes_sanipes_detalle(int aniodesdex = 0, int aniohastax = 0)
        {
            if (aniodesdex == 0) { aniodesdex = 2013; }
            if (aniohastax == 0) { aniohastax = DateTime.Now.Year; }

            DataTable tbl_ht_pendientes = new DataTable();
            tbl_ht_pendientes.Columns.Add("Hoja Trámite");
            tbl_ht_pendientes.Columns.Add("Fecha inicio");
            tbl_ht_pendientes.Columns.Add("TUPA");
            tbl_ht_pendientes.Columns.Add("Asunto TUPA");
            tbl_ht_pendientes.Columns.Add("Estado");
            tbl_ht_pendientes.Columns.Add("Fecha envío");
            tbl_ht_pendientes.Columns.Add("Fecha recepción");
            tbl_ht_pendientes.Columns.Add("Oficina");
            tbl_ht_pendientes.Columns.Add("Asunto");
            tbl_ht_pendientes.Columns.Add("Destino");
            tbl_ht_pendientes.Columns.Add("Persona Envía");

            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////

            var list = _HojaTramiteService.lista_pendientes_sigesdoc_det(aniodesdex, aniohastax);

            DataRow tbl_row_pendientes;
            foreach (var pendiente in list)
            {
                tbl_row_pendientes = tbl_ht_pendientes.NewRow();
                tbl_row_pendientes["Hoja Trámite"] = pendiente.hoja_tramite;
                tbl_row_pendientes["Fecha inicio"] = pendiente.fecha_emision;
                tbl_row_pendientes["TUPA"] = pendiente.tupa;
                tbl_row_pendientes["Asunto TUPA"] = pendiente.asunto_tupa;
                tbl_row_pendientes["Estado"] = pendiente.estado;
                tbl_row_pendientes["Fecha envío"] = pendiente.fecha_envio;
                tbl_row_pendientes["Fecha recepción"] = pendiente.fecha_recepcion;
                tbl_row_pendientes["Oficina"] = pendiente.oficina;
                tbl_row_pendientes["Asunto"] = pendiente.asunto;
                tbl_row_pendientes["Destino"] = pendiente.destino;
                tbl_row_pendientes["Persona Envía"] = pendiente.persona_envia;
                tbl_ht_pendientes.Rows.Add(tbl_row_pendientes);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl_ht_pendientes;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Excel_documentos_pendientes_sanipes.xls");
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documentos_hoja_tramite(int numero_ht = 0)
        {
            IEnumerable<DocumentoResponse> Documento = new List<DocumentoResponse>();

            Documento = _HojaTramiteService.GetAllDocumento_lista_resp_x_ht(numero_ht);

            return Json(Documento, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_documento_anexo(int id_documento = 0)
        {
            IEnumerable<DocumentoAnexoResponse> llenar_documento_a = new List<DocumentoAnexoResponse>();

            llenar_documento_a = _HojaTramiteService.Lista_Documentos_anexos(id_documento);

            return Json(llenar_documento_a, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public FileResult descargar_anexo(int id)
        {
            DocumentoAnexoResponse uno_documento_a = new DocumentoAnexoResponse();
            uno_documento_a = _HojaTramiteService.Documento_Anexo_HT(id);

            byte[] fileBytes = System.IO.File.ReadAllBytes(@"" + uno_documento_a.ruta);
            //string fileName = "myfile.ext";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, uno_documento_a.descripcion);
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_Documento_anexo(string id)
        {
            if (id != null && id != "")
            {
                Session["pdf_document_id_documento_anexo"] = id;
                return RedirectToAction("Adjuntar_Documento_anexo_ht", "HojaTramite");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }

        [AllowAnonymous]
        public ActionResult Adjuntar_Documento_anexo_ht()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {

                    int var_id_documento = Convert.ToInt32(Session["pdf_document_id_documento_anexo"].ToString());
                    Session.Remove("pdf_document_id_documento_anexo");

                    DocumentoResponse res_doc = _HojaTramiteService.GetAllDocumento_resp(var_id_documento);

                    string documento = "";

                    if (res_doc.numero_documento != null && res_doc.numero_documento != 0)
                    {
                        documento = _GeneralService.Consulta_Tipo_Documento(res_doc.id_tipo_documento).First().nombre + " N." + res_doc.numero_documento.ToString() + res_doc.nom_doc;
                    }
                    else
                    {
                        documento = _GeneralService.Consulta_Tipo_Documento(res_doc.id_tipo_documento).First().nombre + " " + res_doc.nom_doc;
                    }

                    ViewBag.var_nombre_documento = documento;
                    ViewBag.var_id_documento = var_id_documento.ToString();

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
        public ActionResult Adjuntar_Documento_anexo_ht(HttpPostedFileBase[] fileupload, int id_documento_padre)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    DocumentoAnexoRequest docu_anexo = new DocumentoAnexoRequest();
                    foreach (HttpPostedFileBase file in fileupload)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {

                            docu_anexo.descripcion = file.FileName;
                            docu_anexo.activo = "1";
                            docu_anexo.usuario_crea = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                            docu_anexo.fecha_crea = DateTime.Now;
                            docu_anexo.extension = Path.GetExtension(file.FileName);
                            docu_anexo.id_documento = id_documento_padre;
                            docu_anexo.id_documento_anexo = _HojaTramiteService.Documento_anexo_Insertar(docu_anexo);

                            string ruta_archivo = ConfigurationManager.AppSettings["RUTA_PDF_DOCU_ANEXOS_HT"].ToString();

                            var path = Path.Combine(ruta_archivo, docu_anexo.id_documento_anexo.ToString() + docu_anexo.extension);

                            docu_anexo.ruta = path.Replace('\\', '/');
                            bool success = _HojaTramiteService.Documento_anexo_Update(docu_anexo);

                            file.SaveAs(path);
                        }

                    }
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

        [HttpPost]
        public ActionResult ConsultaDNI(string person_num_doc)
        {
            IDictionary<string, object> coResultado;
            List<Dni_Output> listDniOut = new List<Dni_Output>();
            Dni_Output dni = new Dni_Output();
            //Serializando Numero de DNI
            Dni_Input dni_Input = new Dni_Input();
            dni_Input.dni = person_num_doc;
            string outjson = JsonConvert.SerializeObject(dni_Input, Formatting.Indented);
           
            //Llamando URL del Servicio RENIEC
            var url = ConfigurationManager.AppSettings["SrvReniec"].ToString();

            //Variable de carga de respuesta
            string _json = string.Empty;

            //Configuraciones de llamado de Servicio
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                
                Stream resStream = response.GetResponseStream();
                var sr = new StreamReader(response.GetResponseStream());
                string responseText = sr.ReadToEnd();
                if (String.IsNullOrEmpty(person_num_doc))
                {

                    dni.msg = "Por favor ingrese un Numero de DNI";
                    listDniOut.Add(dni);

                }
                else
                {
                    var obj = ToObject(responseText) as IDictionary<string, object>;

                    foreach (var item in obj)
                    {
                        
                        switch (item.Key)
                        {
                            
                            case "datosPersona":
                                var datosPersona = obj[item.Key] as IDictionary<string, object>;

                                foreach (var items in datosPersona)
                                {

                                    switch (items.Key)
                                    {
 
                                        case "datosPersona":
                                            var Value = datosPersona[items.Key] as IDictionary<string, object>;

                                            coResultado = obj[item.Key] as IDictionary<string, object>;
                                            dni.coResultado = coResultado["coResultado"].ToString();

                                            if (Value != null || dni.coResultado == "0000")
                                            {
                                                dni.apPrimer = Value["apPrimer"].ToString() == string.Empty ? string.Empty : Value["apPrimer"].ToString();
                                                dni.apSegundo = Value["apSegundo"].ToString() == string.Empty ? string.Empty : Value["apSegundo"].ToString();
                                                dni.prenombres = Value["prenombres"].ToString() == string.Empty ? string.Empty : Value["prenombres"].ToString();
                                                dni.direccion = Value["direccion"].ToString() == string.Empty ? string.Empty : Value["direccion"].ToString();
                                                dni.ubigeo = Value["ubigeo"].ToString() == string.Empty ? string.Empty : Value["ubigeo"].ToString();

                                                listDniOut.Add(dni);
                                            }
                                            else if(dni.coResultado.Trim() == "0001")
                                            {
                                                dni.msg = "El número de DNI corresponde a un menor de edad";
                                                listDniOut.Add(dni);
                                            }
                                            else if (dni.coResultado.Trim() == "0999")
                                            {
                                                dni.msg = "No se ha encontrado información para el número de DNI";
                                                listDniOut.Add(dni);
                                            }
                                            else if (dni.coResultado.Trim() == "1999")
                                            {
                                                dni.msg = "Error desconocido / inesperado";
                                                listDniOut.Add(dni);
                                            }
                                            else if (dni.coResultado.Trim() == "1001")
                                            {
                                                dni.msg = "Uno o más datos de la petición no son válidos";
                                                listDniOut.Add(dni);
                                            }
                                            else
                                            {
                                                dni.msg = "El DNI ingresado no fue encontrado en los registros de la RENIEC";
                                                listDniOut.Add(dni);
                                            }

                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                dni.msg = "Servicio de Reniec sin conexión";
                listDniOut.Add(dni);
            }

           return Json(listDniOut, JsonRequestBehavior.AllowGet);
        }

        public static object ToObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return ToObject(JToken.Parse(json));
        }

        public static object ToObject(JToken token)
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

        public ActionResult Nuevo_Documento_Informacion()
        {
            return RedirectToAction("Index", "Inicio");
        }

        public void PreViewWord()
        {
            object missing = System.Reflection.Missing.Value;
            Word.Application application = new Word.Application();

            string file = @"sigesdoc\SIGESDOC.INFORMEUTI\bin\Debug\INFORME_UTI.docx";
            string root = Path.GetDirectoryName(file);
            FileInfo f = new FileInfo(file);
            string path = f.FullName;

            Word.Document document = application.Documents.Open(@"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc\SIGESDOC.INFORMEUTI\bin\Debug\INFORME_UTI.docx", ref missing);
            //Word.Document document = application.Documents.Open(path, ref missing);

            application.Visible = true;

        }



        //Add by HM - 28/11/2019
        [AllowAnonymous]
        public ActionResult Nuevo_Documento_Externos(int IdRegistro)
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

                    foreach (var result in _GeneralService.Recupera_tipo_documento_some())
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

                    //Enviamos el ID Registro del Documento
                    ViewBag.ID_REGISTRO = Convert.ToInt32(IdRegistro);

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
                                                                              //req_documento_dhcpa.id_det_documento = 

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
    }
}

