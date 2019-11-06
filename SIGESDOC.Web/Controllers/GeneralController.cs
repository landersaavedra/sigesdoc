using SIGESDOC.IAplicacionService;
using SIGESDOC.Response;
using SIGESDOC.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGESDOC.Web.Models;
using Excel;
using System.Web.Configuration;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

namespace SIGESDOC.Web.Controllers
{
    public class GeneralController : Controller
    {

        private readonly IGeneralService _GeneralService;
        private readonly IHojaTramiteService _HojaTramiteService;
        private readonly IOficinaService _OficinaService;
        

        public GeneralController(
            IGeneralService GeneralService,
            IHojaTramiteService HojaTramiteService,
            IOficinaService OficinaService
            )
        {
            _GeneralService = GeneralService;
            _HojaTramiteService = HojaTramiteService;
            _OficinaService = OficinaService;
        }

        [AllowAnonymous]
        public ActionResult Movimiento_operacion_exportacion()
        {

            if (HttpContext.Request.IsAuthenticated)
            {
                ViewBag.Str_correcto = "";
                ViewBag.Error = "";
                ViewBag.file_text = "";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }






















        [AllowAnonymous]
        public ActionResult variable_Subir_archivo_operacion(string id)
        {
            if (id != null && id != "")
            {
                Session["archivo_operacion"] = id;
                return RedirectToAction("Adjuntar_archivo_operacion", "General");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }
        }
        
        
        [AllowAnonymous]
        public ActionResult Adjuntar_archivo_operacion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                     (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Nuevo Comprobante
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 19: Unidad de Contabilidad, Finanzas y Tesoreria
                {

                    int id_operacion = 0;

                    try
                    {
                        id_operacion = Convert.ToInt32(Session["archivo_operacion"].ToString());
                        Session.Remove("archivo_operacion");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }

                    ConsultaDbGeneralMaeOperacionResponse opera = new ConsultaDbGeneralMaeOperacionResponse();
                    //doc = _HabilitacionesService.Lista_Documento_dhcpa_x_id_rs(id_operacion);
                    opera = _GeneralService.lista_operacion_x_id(id_operacion);

                    ViewBag.Str_comprobante = "Comprobante: " + opera.factura.ToString();
                    ViewBag.var_id_operacion = id_operacion.ToString();
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
        public ActionResult Adjuntar_archivo_operacion(HttpPostedFileBase file, int lbl_id_operacion)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                     (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                     (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Nuevo Comprobante
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 19: Unidad de Contabilidad, Finanzas y Tesoreria
                {

                    ConsultaDbGeneralMaeOperacionResponse ope_rq = new ConsultaDbGeneralMaeOperacionResponse();
                    ope_rq = _GeneralService.lista_operacion_x_id(lbl_id_operacion);

                    string ruta_pdf = ConfigurationManager.AppSettings["RUTA_PDF_OPERA"].ToString();
                    ope_rq.ruta_pdf = Path.Combine(ruta_pdf, lbl_id_operacion.ToString() + ".pdf").ToString();
                    ope_rq.usuario_modifica = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();

                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(ruta_pdf, lbl_id_operacion.ToString() + ".pdf"));
                        _GeneralService.update_db_general_mae_operacion(ope_rq);
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
        public ActionResult Movimiento_operacion_exportacion(HttpPostedFileBase excelfile, string txt_file_text)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Por favor seleccionar archivo Excel";
                ViewBag.Str_correcto = "";
                ViewBag.file_text = "";
                return View();
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = excelfile.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;


                    if (excelfile.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (excelfile.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                    reader.IsFirstRowAsColumnNames = true;

                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable tabla_excel = result.Tables[0];

                    string cadena = "";

                    cadena = "<div id=" + (char)34 + "grid_factura_wrapper" + (char)34 + " class=" + (char)34 + "dataTables_wrapper no-footer" + (char)34 + "><table id=" + (char)34 + "tbl" + (char)34 + " class=" + (char)34 + "table table-striped table-hover table-condensed tabla small" + (char)34 + ">" +
                                "<thead> <tr class=" + (char)34 + "cabecera text-center" + (char)34 + ">" +
                                            "<th style=" + (char)34 + "text-align:center;" + (char)34 + " scope=" + (char)34 + "col" + (char)34 + ">FILA</th>" +
                                            "<th style=" + (char)34 + "text-align:center;" + (char)34 + " scope=" + (char)34 + "col" + (char)34 + ">NUMERO</th>" +
                                            "<th style=" + (char)34 + "text-align:center;" + (char)34 + " scope=" + (char)34 + "col" + (char)34 + ">FECHA</th>" +
                                            "<th style=" + (char)34 + "text-align:center;" + (char)34 + " scope=" + (char)34 + "col" + (char)34 + ">OFICINA</th>" +
                                            "<th style=" + (char)34 + "text-align:center;" + (char)34 + " scope=" + (char)34 + "col" + (char)34 + ">ABONO</th>" +
                                        "</tr> </thead> <tbody>";
                    int fila = 1;

                    List<ConsultaDbGeneralMaeOperacionResponse> lista_fila_error = new List<ConsultaDbGeneralMaeOperacionResponse>();

                    foreach (DataRow row in tabla_excel.Rows)
                    {
                        fila = fila + 1;
                        if (row[0] == null || row[0].ToString().Trim() == "") break;
                        ConsultaDbGeneralMaeOperacionResponse ope_rq = new ConsultaDbGeneralMaeOperacionResponse();

                        string fila_x = "";
                        string numero = "";
                        string fecha = "";
                        string oficina = "";
                        string abono = "";

                        try{ ope_rq = _GeneralService.busca_operacion_x_num_x_fecha_oficina(Convert.ToInt32(row[0]), Convert.ToDateTime(row[1]), Convert.ToInt32(row[2])).First();}
                        catch (Exception) { }
                        
                        if (ope_rq.numero != null)
                        {
                            fila_x = "<td style=" + (char)34 + " text-align:center;" + (char)34 + ">" + fila.ToString() + "</td>";
                            numero = "<td style=" + (char)34 + " text-align:center;" + (char)34 + ">" + row[0].ToString() + "</td>";
                            fecha = "<td style=" + (char)34 + " text-align:center;" + (char)34 + ">" + row[1].ToString() + "</td>";
                            oficina = "<td style=" + (char)34 + "text-align:center;" + (char)34 + ">" + row[2].ToString() + "</td>";
                            abono = "<td style=" + (char)34 + "text-align:center;" + (char)34 + ">" + row[3].ToString() + "</td>";
                            lista_fila_error.Add(ope_rq);
                            cadena = cadena + "<tr>" +fila_x+ numero+fecha+oficina+abono+"</tr>";
                        }

                        //num_der = "<td style=" + (char)34 + "text-align:center; color:white; background-color:red; " + (char)34 + "  >" + row[0].ToString() + ": No es número</td>";
                    }
                    

                    if (lista_fila_error.Count() > 0)
                    {
                        ViewBag.Str_correcto = "";
                        ViewBag.file_text = "";
                        ViewBag.MSExcelTable = "<div id=" + (char)34 + "grid" + (char)34 + "><h2 style=" + (char)34 + "color: #B44D4D" + (char)34 + "> Ocurrio un problema en las siguientes Filas</h2>" + cadena + "</tbody></table></div></div>";
                    }
                    else
                    {
                        ViewBag.Str_correcto = "OK";
                        ViewBag.MSExcelTable = "<div id=" + (char)34 + "grid" + (char)34 + "><h2 style=" + (char)34 + "color: #5bc0de " + (char)34 + " >La información es correcta</h2></div>";
                        string registro = "";

                        foreach (DataRow row in tabla_excel.Rows)
                        {
                            var fec = row[1].ToString().Split('.');
                            string fech = fec[2].ToString() + "/" + fec[1].ToString() + "/" + fec[0].ToString();
                            if (row[0] == null || row[0].ToString().Trim() == "") break;
                            if (registro != "")
                            {
                                registro = registro + "+";
                            }
                            registro = registro + row[0].ToString() + "|" + // numero
                            fech + "|" + //fecha
                            row[2].ToString() + "|" + //oficina
                            row[3].ToString(); //abono
                        }
                        Session["file_text_exportados"] = registro;
                    }

                    return View();
                }
                else
                {
                    ViewBag.Str_correcto = "";
                    ViewBag.file_text = "";
                    ViewBag.Error = "Tipo de Archivo incorrecto <br>";
                    return View();
                }
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_operacion_exportados()
        {
            string ope_lista = Session["file_text_exportados"].ToString();

            var Lista_ope = ope_lista.Split('+');

            for (int i = 0; i < Lista_ope.Count(); i++)
            {
                // registro = registro + row[0].ToString() + "|" + // numero
                //row[1].ToString() + "|" + //fecha
                //row[2].ToString() + "|" + //oficina
                //row[3].ToString(); //abono
                var lista_im = Lista_ope[i].Split('|');
                _GeneralService.Guardar_Operacion(Convert.ToInt32(lista_im[0]), Convert.ToDateTime(lista_im[1]), Convert.ToInt32(lista_im[2]), Convert.ToDecimal(lista_im[3]), HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Descarga_formato_carga_operacion_exportacion(string para1 = "")
        {

            var products = new System.Data.DataTable("teste");
            products.Columns.Add("NUMERO", typeof(int));
            products.Columns.Add("FECHA", typeof(string));
            products.Columns.Add("OFICINA", typeof(string));
            products.Columns.Add("ABONO", typeof(Decimal));

            products.Rows.Add(1050, DateTime.Now.Year.ToString()+".01.01", "50", 10.0);

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DATA_OPERACION.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }










        [AllowAnonymous]
        public ActionResult variable_ver_protocolo_x_planta(int id)
        {
            if (id != null && id != 0)
            {
                Session["General_ver_protocolos_x_planta_id_planta"] = id;
                return RedirectToAction("Ver_Protocolos_x_planta", "General");
            }
            else
            {
                return RedirectToAction("Index", "Inicio");
            }


        }
        [AllowAnonymous]
        public ActionResult Ver_Protocolos_x_planta(int page = 1)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[16].Trim() == "1" // Acceso a Planta
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28"))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    int var_id_planta = 0;
                    try
                    {
                        var_id_planta = Convert.ToInt32(Session["General_ver_protocolos_x_planta_id_planta"].ToString());
                        Session.Remove("General_ver_protocolos_x_planta_id_planta");
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Inicio");
                    }
                    

                    ViewBag.id_planta = var_id_planta.ToString();
                    var p = _GeneralService.recupera_planta_x_id(var_id_planta);
                    ViewBag.nom_planta = p.siglas_tipo_planta + '-' + p.numero_planta.ToString() + p.nombre_planta;
                    
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("FECHA_REGISTRO");
                    tbl.Columns.Add("PROTOCOLO");
                    tbl.Columns.Add("FECHA_INICIO");
                    tbl.Columns.Add("FECHA_FIN");
                    tbl.Columns.Add("CONCHA_ABANICO");
                    tbl.Columns.Add("CRUSTACEOS");
                    tbl.Columns.Add("PECES");
                    tbl.Columns.Add("OTROS");

                    var protocolo_planta = _GeneralService.GetAllProtocolo_x_planta(var_id_planta);

                    foreach (var result in protocolo_planta)
                    {
                        tbl.Rows.Add(

                            result.fecha_registro,
                            result.nombre,
                            result.fecha_inicio.Value.ToShortDateString(),
                            result.fecha_fin.Value.ToShortDateString(),
                            result.ind_concha_abanico,
                            result.ind_crustaceos,
                            result.ind_otros,
                            result.ind_peces
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
        public ActionResult Consulta_planta(int page = 1, string id_tipo_planta = "", string var_numero = "", string var_nombre = "", int var_id_filial = 0, int var_id_actividad = 0, string var_entidad="")
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[16].Trim() == "1" // Acceso a Planta
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    IEnumerable<ConsultarPlantasResponse> model = new List<ConsultarPlantasResponse>();

                    #region llenar_combos

                    List<SelectListItem> lista_tipo_planta = new List<SelectListItem>();

                    lista_tipo_planta.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = ""
                    });
                    
                    foreach (var result in _GeneralService.recupera_tipo_planta())
                    {
                        lista_tipo_planta.Add(new SelectListItem()
                        {
                            Text = result.siglas,
                            Value = result.id_tipo_planta.ToString()
                        });
                    };

                    List<SelectListItem> lista_od_filial = new List<SelectListItem>();

                    lista_od_filial.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.recupera_filial("I"))
                    {
                        lista_od_filial.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_filial.ToString()
                        });
                    };

                    List<SelectListItem> lista_actividad = new List<SelectListItem>();

                    lista_actividad.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.recupera_toda_tipo_actividad_planta())
                    {
                        lista_actividad.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_actividad.ToString()
                        });
                    };

                    ViewBag.lista_actividad = lista_actividad; 
                    ViewBag.lista_filial = lista_od_filial; // lista lifial a cargo de la planta
                    ViewBag.lista_combo = lista_tipo_planta; // lista tipo de planta (CODIGO DE PLANTA)
                    #endregion

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("PLANTA");
                    tbl.Columns.Add("ENTIDAD");
                    tbl.Columns.Add("ACTIVIDAD");
                    tbl.Columns.Add("DIRECCION");
                    tbl.Columns.Add("ESTADO");
                    tbl.Columns.Add("ID_PLANTA");
                    tbl.Columns.Add("COND_PROTOCOLO");

                    var plantas = _GeneralService.GetAllPlantas_sin_paginado(id_tipo_planta.Trim(), var_numero.Trim(), var_nombre.Trim(), var_id_filial, var_id_actividad, var_entidad);

                    foreach (var result in plantas)
                    {
                        tbl.Rows.Add(
                            result.siglas_tipo_planta+result.numero_planta.ToString()+result.nombre_planta,
                            result.nombre_entidad,
                            result.nombre_actividad,
                            result.direccion_entidad,
                            result.nombre_estado,
                            result.id_planta,
                            result.cond_protocolo
                            );
                    };
                    
                    ViewData["Planta_Tabla"] = tbl; 

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

        public ActionResult Nueva_Planta()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[16].Trim() == "1" // Acceso a Planta
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                    // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
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
        public ActionResult Grabar_Nueva_Planta(int id_sede, int id_tipo_planta, int numero, string nombre_planta,int id_tipo_actividad,int id_filial)
        {
                    
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[16].Trim() == "1" // Acceso a Planta
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        _GeneralService.Guardar_Planta(id_sede, id_tipo_planta, numero, nombre_planta,id_tipo_actividad,id_filial, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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







        public ActionResult Consulta_desembarcadero(int id_tipo_desembarcadero=0, string codigo_desembarcadero="",string externo="")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[22].Trim() == "1" // Acceso a concesion
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    #region llenar_combos

                    List<SelectListItem> lista_tipo_desembarcadero = new List<SelectListItem>();

                    lista_tipo_desembarcadero.Add(new SelectListItem() { Text = "TODO", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tipo_desembarcadero())
                    {
                        lista_tipo_desembarcadero.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_desembarcadero.ToString()
                        });
                    };

                    ViewBag.lst_tipo_desembarcadero = lista_tipo_desembarcadero;
                    

                    #endregion

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_DESEMBARCADERO");
                    tbl.Columns.Add("ENTIDAD");
                    tbl.Columns.Add("NOMBRE_TIPO_DESEMBARCADERO");
                    tbl.Columns.Add("CODIGO_DESEMBARCADERO");
                    tbl.Columns.Add("DENOMINACION");
                    tbl.Columns.Add("LATITUD");
                    tbl.Columns.Add("LONGITUD");
                    tbl.Columns.Add("ESTADO_DESEMB");

                    var desembarcadero = _GeneralService.GetAlldesembarcadero_sin_paginado(id_tipo_desembarcadero, codigo_desembarcadero,externo);

                    foreach (var result in desembarcadero)
                    {
                            tbl.Rows.Add(
                             result.id_desembarcadero,
                             result.entidad,
                             result.nombre_tipo_desembarcadero,
                             result.codigo_desembarcadero,
                             result.denominacion,
                             result.latitud,
                             result.longitud,
                             result.estado_desemb
                            );
                    };

                    ViewData["Desembarcadero_Tabla"] = tbl;

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

        public ActionResult Nuevo_Desembarcadero()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[22].Trim() == "1" // Acceso a desembarcadero
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    
                    List<SelectListItem> lista_tipo_desembarcadero = new List<SelectListItem>();
                    List<SelectListItem> lista_codigo_desembarcadero = new List<SelectListItem>();
                    
                    int entra = 0;
                    foreach (var result in _GeneralService.recupera_tipo_desembarcadero())
                    {
                        
                        lista_tipo_desembarcadero.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_desembarcadero.ToString()
                        });
                        if (entra == 0)
                        {
                            entra = 1;
                            foreach (var result2 in _GeneralService.recupera_codigo_desembarcadero(result.id_tipo_desembarcadero))
                            {
                                lista_codigo_desembarcadero.Add(new SelectListItem()
                                {
                                    Text = result2.codigo,
                                    Value = result2.id_cod_desemb.ToString()
                                });
                            }
                        }
                    };

                    ViewBag.lbl_check_temporal = "0";
                    ViewBag.lista_combo = list_combo;
                    ViewBag.lst_tipo_desembarcadero = lista_tipo_desembarcadero;
                    ViewBag.lst_codigo_desembarcadero = lista_codigo_desembarcadero;

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


        public ActionResult Grabar_Nuevo_Desembarcadero(int id_sede, int id_tipo_desembarcadero, int id_codigo_desembarcadero, int numero, string nombre_desembarcadero, string denominacion, string temporal, double latitud, double longitud)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[22].Trim() == "1" // Acceso a Desembarcadero
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        _GeneralService.Guardar_Desembarcadero(0,id_sede, id_tipo_desembarcadero, id_codigo_desembarcadero, numero, nombre_desembarcadero, denominacion, temporal, latitud, longitud, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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







        public ActionResult Consulta_concesion(int id_zona_produccion = 0, int id_area_produccion = 0, int id_tipo_concesion = 0, string externo = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[21].Trim() == "1" // Acceso a concesion
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    #region llenar_combos

                    List<SelectListItem> lista_tipo_concesion = new List<SelectListItem>();

                    lista_tipo_concesion.Add(new SelectListItem() { Text = "TODO", Value = "0" });

                    foreach (var result in _GeneralService.recupera_tipo_concesion())
                    {
                        lista_tipo_concesion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_concesion.ToString()
                        });
                    };

                    List<SelectListItem> lista_zona_produccion = new List<SelectListItem>();

                    lista_zona_produccion.Add(new SelectListItem() { Text = "TODO", Value = "0" });

                    foreach (var result in _GeneralService.recupera_zona_produccion())
                    {
                        lista_zona_produccion.Add(new SelectListItem()
                        {
                            Text = result.cod_zona_produccion + " / " + result.nombre,
                            Value = result.id_zona_produccion.ToString()
                        });
                    };

                    List<SelectListItem> lista_area_produccion = new List<SelectListItem>();

                    lista_area_produccion.Add(new SelectListItem() { Text = "TODO", Value = "0" });

                    ViewBag.lst_zona_produccion = lista_zona_produccion;
                    ViewBag.lst_area_produccion = lista_area_produccion;
                    ViewBag.lst_tipo_concesion = lista_tipo_concesion;


                    #endregion

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_CONSECION");
                    tbl.Columns.Add("TIPO_CONCESION");
                    tbl.Columns.Add("CODIGO_HABILITACION");
                    tbl.Columns.Add("PARTIDA_REGISTRAL");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("UBICACION");
                    tbl.Columns.Add("UBIGEO");
                    tbl.Columns.Add("DEPARTAMENTO");
                    tbl.Columns.Add("PROVINCIA");
                    tbl.Columns.Add("DISTRITO");
                    tbl.Columns.Add("ZONA_PRODUCCION");
                    tbl.Columns.Add("NOMBRE_ZONA_PRODUCCION");
                    tbl.Columns.Add("AREA_PRODUCCION");
                    tbl.Columns.Add("NOMBRE_AREA_PRODUCCION");

                    var concesion = _GeneralService.GetAllconsecion_sin_paginado(id_zona_produccion, id_area_produccion, id_tipo_concesion, externo);

                    foreach (var result in concesion)
                    {
                        if (result.cod_zona_produccion == null)
                        {
                            tbl.Rows.Add(
                             result.id_concesion,
                             result.tipo_concesion,
                             result.codigo_habilitacion,
                             result.partida_registral,
                             result.razon_social,
                             result.ubicacion,
                             result.ubigeo,
                             result.departamento,
                             result.provincia,
                             result.distrito,
                             "",
                             "",
                             "",
                             ""
                            );
                        }
                        else
                        {
                            tbl.Rows.Add(
                             result.id_concesion,
                             result.tipo_concesion,
                             result.codigo_habilitacion,
                             result.partida_registral,
                             result.razon_social,
                             result.ubicacion,
                             result.ubigeo,
                             result.departamento,
                             result.provincia,
                             result.distrito,
                             result.cod_zona_produccion,
                             result.nombre_zona_produccion,
                             result.cod_area_produccion,
                             result.nombre_area_produccion
                            );
                        }
                    };

                    ViewData["Concesion_Tabla"] = tbl;

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



        public ActionResult Nueva_Concesion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[21].Trim() == "1" // Acceso a concesion
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;

                    List<SelectListItem> lista_tipo_concesion = new List<SelectListItem>();

                    foreach (var result in _GeneralService.recupera_tipo_concesion())
                    {
                        lista_tipo_concesion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_concesion.ToString()
                        });
                    };

                    List<SelectListItem> lista_zona_produccion = new List<SelectListItem>();

                    lista_zona_produccion.Add(new SelectListItem() { Text = "SELECCIONAR ZONA DE PRODUCCION", Value = "0" });

                    List<SelectListItem> lista_area_produccion = new List<SelectListItem>();

                    lista_area_produccion.Add(new SelectListItem() { Text = "SELECCIONAR AREA DE PRODUCCION", Value = "0" });


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

                    ViewBag.lst_zona_produccion = lista_zona_produccion;
                    ViewBag.lst_area_produccion = lista_area_produccion;
                    ViewBag.lst_tipo_concesion = lista_tipo_concesion;
                    

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

        public ActionResult Grabar_Nueva_Concesion(string ruc,string codigo_habilitacion, string partida_registral,string ubicacion,string ubigeo,int id_area_produccion,string id_tipo_actividad_concesion,int id_tipo_concesion)
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[21].Trim() == "1" // Acceso a concesion
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        if(id_tipo_actividad_concesion=="")
                        {
                            id_tipo_actividad_concesion = "0";
                        }
                        _GeneralService.Guardar_Concesion(0,ruc, codigo_habilitacion,partida_registral,ubicacion,ubigeo,id_area_produccion,id_tipo_concesion,Convert.ToInt32(id_tipo_actividad_concesion), HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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

        public ActionResult Consulta_almacen(string CODIGO_ALMACEN="",int ID_ACTIVIDAD_ALMACEN=0,int ID_FILIAL=0,string EXTERNO="")
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[19].Trim() == "1" // Acceso a Almacen
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    #region llenar_combos

                    List<SelectListItem> lista_actividad = new List<SelectListItem>();

                    lista_actividad.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.recupera_actividad_almacen())
                    {
                        lista_actividad.Add(new SelectListItem()
                        {
                            Text = result.nombre_actividad,
                            Value = result.id_actividad_almacen.ToString()
                        });
                    };

                    List<SelectListItem> lista_od_filial = new List<SelectListItem>();

                    lista_od_filial.Add(new SelectListItem()
                    {
                        Text = "TODO",
                        Value = "0"
                    });

                    foreach (var result in _GeneralService.recupera_filial("I"))
                    {
                        lista_od_filial.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_filial.ToString()
                        });
                    };

                    ViewBag.lista_actividad = lista_actividad;
                    ViewBag.lista_filial = lista_od_filial;
                    #endregion

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_ALMACEN");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("DIRECCION");
                    tbl.Columns.Add("COD_HABILITANTE");
                    tbl.Columns.Add("ACTIVIDAD");
                    tbl.Columns.Add("FILIAL");

                    var almacen = _GeneralService.GetAllAlmacenes_sin_paginado(CODIGO_ALMACEN.Trim(), ID_ACTIVIDAD_ALMACEN, ID_FILIAL, EXTERNO.Trim());

                    foreach (var result in almacen)
                    {
                        tbl.Rows.Add(
                             result.id_almacen,
                             result.externo,
                             result.direccion,
                             result.nom_cod_habilitante,
                             result.nom_actividad,
                             result.nom_filial
                            );
                    };

                    ViewData["Almacen_Tabla"] = tbl;

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

        public ActionResult Nuevo_Almacen()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[19].Trim() == "1" // Acceso a Almacen
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                    // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    List<SelectListItem> list_combo = new List<SelectListItem>();
                    ViewBag.lista_combo = list_combo;
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
        public ActionResult Grabar_Nuevo_Almacen(int id_sede,int id_codigo_almacen,int numero,string nombre_almacen,int id_actividad,int id_filial)
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                var oficinas_permiso_od = ConfigurationManager.AppSettings["OFICINAS_PERMISOS_OD"].ToString().Split(',');
                int permiso = 0;

                for (int i = 0; i < oficinas_permiso_od.Count(); i++)
                {
                    if (HttpContext.User.Identity.Name.Split('|')[4].Trim() == oficinas_permiso_od[i])
                    {
                        permiso = 1;
                    }
                }

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[19].Trim() == "1" // Acceso a Almacen
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1 ))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        _GeneralService.Guarda_Almacen(0, id_sede, id_codigo_almacen,numero,nombre_almacen, id_filial, id_actividad, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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

        public ActionResult Consulta_embarcacion(int page = 1, string var_matricula = "", string var_nombre = "", int cmb_actividad=0)
        {
            if (HttpContext.Request.IsAuthenticated)
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
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[10].Trim() == "1" // Acceso a Embarcación
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso==1))))
                    // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    List<SelectListItem> lista_actividad_embarcacion = new List<SelectListItem>();

                    lista_actividad_embarcacion.Add(new SelectListItem() { Text = "SELECCION", Value = "0" });

                    foreach (var result in _GeneralService.llenar_actividad_embarcacion())
                    {
                        lista_actividad_embarcacion.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_act_emb.ToString()
                        });
                    };

                    ViewBag.lst_actividad = lista_actividad_embarcacion;

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_EMBARCACION");
                    tbl.Columns.Add("MATRICULA");
                    tbl.Columns.Add("NOMBRE_EMBARCACION");
                    tbl.Columns.Add("TIPO_EMBARCACION");
                    tbl.Columns.Add("ACTIVIDAD");
                    tbl.Columns.Add("CODIGO_HABILITANTE");

                    var embarcacion = _GeneralService.GetAllEmbarcaciones_sin_paginado(var_matricula.Trim(), var_nombre.Trim(), cmb_actividad);

                    foreach (var result in embarcacion)
                    {
                        tbl.Rows.Add(

                            result.id_embarcacion,
                            result.matricula,
                            result.nombre,
                            result.nombre_tipo_embarcacion,
                            result.nombre_actividad,
                            result.cod_habilitante
                            );
                    };

                    ViewData["Embarcacion_Tabla"] = tbl; 
                    
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

        public ActionResult Nueva_Embarcacion()
        {
            if (HttpContext.Request.IsAuthenticated)
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
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[10].Trim() == "1" // Acceso a Embarcación
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso == 1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    List<SelectListItem> lista_tipo_embarcacion = new List<SelectListItem>();
                    List<SelectListItem> lista_codigo_embarcacion = new List<SelectListItem>();
                    List<SelectListItem> lista_actividad_embarcacion = new List<SelectListItem>();

                    lista_tipo_embarcacion.Add(new SelectListItem(){Text = "SELECCION",Value = ""});
                    lista_codigo_embarcacion.Add(new SelectListItem(){Text = "SELECCION",Value = "0"});
                    lista_actividad_embarcacion.Add(new SelectListItem(){Text = "SELECCION",Value = "0"});

                    foreach (var result in _GeneralService.recupera_tipo_embarcacion(0)){
                        lista_tipo_embarcacion.Add(new SelectListItem(){
                            Text = result.nombre,
                            Value = result.id_tipo_embarcacion.ToString()});
                    };
                    foreach (var result in _GeneralService.llenar_codigo_embarcacion()){
                        lista_codigo_embarcacion.Add(new SelectListItem(){
                            Text = result.codigo,
                            Value = result.id_cod_hab_emb.ToString()});
                    };
                    foreach (var result in _GeneralService.llenar_actividad_embarcacion()){
                        lista_actividad_embarcacion.Add(new SelectListItem(){
                            Text = result.nombre,
                            Value = result.id_tipo_act_emb.ToString()
                        });
                    };
                    
                    ViewBag.lista_embarcacion = lista_tipo_embarcacion;
                    ViewBag.lista_codigo_emb = lista_codigo_embarcacion;
                    ViewBag.lista_actv_embarcacion = lista_actividad_embarcacion;

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
        public ActionResult Grabar_Nueva_Embarcacion(string matricula, string nombre, int id_tipo_embarcacion, int codigo, int numero, string nombre_codigo, int actividad, string fecha_construccion)
        {
            if (HttpContext.Request.IsAuthenticated)
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
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[10].Trim() == "1" // Acceso a Embarcación
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso == 1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        if (fecha_construccion == "") { fecha_construccion = null; }
                        _GeneralService.Guardar_Embarcacion(matricula, nombre, id_tipo_embarcacion, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), codigo, numero, nombre_codigo, actividad, fecha_construccion);
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

        public ActionResult Consulta_Operaciones(string operacion = "", string comprobante = "")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 19: Unidad de Contabilidad, Finanzas y Tesoreria
                {
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_OPERACION");
                    tbl.Columns.Add("NUMERO");
                    tbl.Columns.Add("FECHA_DEPOSITO");
                    tbl.Columns.Add("ABONO");
                    tbl.Columns.Add("OFICINA");
                    tbl.Columns.Add("FACTURA");

                    var v_operaciones = _GeneralService.Lista_todo_operacion(operacion, comprobante).Take(500);

                    foreach (var result in v_operaciones)
                    {
                        tbl.Rows.Add(
                            result.id_operacion,
                            result.numero,
                            result.fecha_deposito.Value.ToShortDateString(),
                            result.abono,
                            result.oficina,
                            result.factura
                            );
                    };

                    ViewData["Operaciones_tabla"] = tbl; 

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

        public ActionResult Consulta_comprobantes_x_mes(int mes=0, int anio=0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19")))
                // Oficina 19: Unidad de CFT
                { 

                    int c_mes = DateTime.Now.Month;
                    int c_anio = DateTime.Now.Year;

                    decimal total_cert = 0;
                    decimal total_prot = 0;
                    decimal total_ensayo = 0;
                    decimal total_acceso_inf = 0;
                    decimal total = 0;

                    if (mes != 0) { c_mes = mes; }
                    if (anio != 0) { c_anio = anio; }

                    var v_comprobante = _GeneralService.GetAllComprobantes_x_mes(c_mes, c_anio);

                    string cadena = "";
                    string cadena_total = "";

                    string mes_text = "";
                    if (c_mes == 1) { mes_text = "ENERO"; }
                    if (c_mes == 2) { mes_text = "FEBRERO"; }
                    if (c_mes == 3) { mes_text = "MARZO"; }
                    if (c_mes == 4) { mes_text = "ABRIL"; }
                    if (c_mes == 5) { mes_text = "MAYO"; }
                    if (c_mes == 6) { mes_text = "JUNIO"; }
                    if (c_mes == 7) { mes_text = "JULIO"; }
                    if (c_mes == 8) { mes_text = "AGOSTO"; }
                    if (c_mes == 9) { mes_text = "SETIEMBRE"; }
                    if (c_mes == 10) { mes_text = "OCTUBRE"; }
                    if (c_mes == 11) { mes_text = "NOVIEMBRE"; }
                    if (c_mes == 12) { mes_text = "DICIEMBRE"; }


                    string mes_minis_text = "";
                    if (DateTime.Now.Month == 1) { mes_minis_text = "Enero"; }
                    if (DateTime.Now.Month == 2) { mes_minis_text = "Febrero"; }
                    if (DateTime.Now.Month == 3) { mes_minis_text = "Marzo"; }
                    if (DateTime.Now.Month == 4) { mes_minis_text = "Abril"; }
                    if (DateTime.Now.Month == 5) { mes_minis_text = "Mayo"; }
                    if (DateTime.Now.Month == 6) { mes_minis_text = "Junio"; }
                    if (DateTime.Now.Month == 7) { mes_minis_text = "Julio"; }
                    if (DateTime.Now.Month == 8) { mes_minis_text = "Agosto"; }
                    if (DateTime.Now.Month == 9) { mes_minis_text = "Setiembre"; }
                    if (DateTime.Now.Month == 10) { mes_minis_text = "Octubre"; }
                    if (DateTime.Now.Month == 11) { mes_minis_text = "Noviembre"; }
                    if (DateTime.Now.Month == 12) { mes_minis_text = "Diciembre"; }

                    foreach (var result in v_comprobante)
                    {
                        if (result.venta_cert != null) { total_cert = total_cert + (result.venta_cert ?? 0); }
                        if (result.venta_prot != null) { total_prot = total_prot + (result.venta_prot ?? 0); }
                        if (result.venta_ensayo != null) { total_ensayo = total_ensayo + (result.venta_ensayo ?? 0); }
                        if (result.acceso_info != null) { total_acceso_inf = total_acceso_inf + (result.acceso_info ?? 0); }
                        if (result.total != null) { total = total + (result.total ?? 0); }

                        cadena = cadena + "<tr style=" + (char)34 + "border-color:black;" + (char)34 + ">" +
                        "<td style=" + (char)34 + "text-align: left" + (char)34 + ">" + result.mes + "</td>" +
                        "<td style=" + (char)34 + "text-align: left" + (char)34 + ">" + result.dia_semana + "</td>" +
                        "<td align=" + (char)34 + "center" + (char)34 + ">" + result.dia_num + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "> " + (result.venta_cert ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.venta_prot ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.venta_ensayo ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.acceso_info ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td align=" + (char)34 + "right" + (char)34 + " style=" + (char)34 + "text-align: right; border-right:1px solid;" + (char)34 + ">" + (result.total ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align:left;" + (char)34 + ">" +
                            "<ul class=" + (char)34 + "list-inline" + (char)34 + " style=" + (char)34 + "margin-bottom:0" + (char)34 + ">" +
                                "<li>" +
                                    "<a class=" + (char)34 + "Imprimir_li" + (char)34 + " id=" + result.fecha.ToString() + " href=" + (char)34 + "#" + (char)34 + " title=" + (char)34 + "Imprimir" + (char)34 + ">" +
                            " - Liquidación de Ingresos</a></li></ul>"+
                            "<ul class=" + (char)34 + "list-inline" + (char)34 + " style=" + (char)34 + "margin-bottom:0" + (char)34 + ">" +
                                "<li>" +
                                    "<a class=" + (char)34 + "Imprimir_rd" + (char)34 + " id=" + result.fecha.ToString() + " href=" + (char)34 + "#" + (char)34 + " title=" + (char)34 + "Imprimir" + (char)34 + ">" +
                            " - Reporte Diario</a></li></ul>" +
                        "</td>" +
                        "</tr>";
                    };

                    cadena_total = "<tr style=" + (char)34 + "font-size:15px; border-color:black;" + (char)34 + ">" +
                        "<td>&nbsp;</td>" +
                        "<td colspan=" + (char)34 + "2" + (char)34 + "><strong>TOTAL S/.</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_cert.ToString("###,###,##0.00") + "   </strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_prot.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_ensayo.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_acceso_inf.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td align=" + (char)34 + "right" + (char)34 + " style=" + (char)34 + "text-align: right; border-right:1px solid;" + (char)34 + "><strong>" + total.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td align=" + (char)34 + "right" + (char)34 + " style=" + (char)34 + "text-align: right; border-right:1px solid;" + (char)34 + "></td>" +
                        "</tr>";

                    ViewBag.mes_anio_text = " - "+mes_text + " "+ c_anio.ToString();
                    ViewBag.dia_text = " "+DateTime.Now.Day.ToString() + " de " + mes_minis_text + " " + DateTime.Now.Year.ToString();
                    ViewBag.html_detalle_mes = cadena;
                    ViewBag.html_detalle_mes_total = cadena_total;

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


        public ActionResult Consulta_comprobantes_x_mes_pdf(int mes = 0, int anio = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19")))
                // Oficina 19: Unidad de CFT
                {

                    int c_mes = DateTime.Now.Month;
                    int c_anio = DateTime.Now.Year;

                    decimal total_cert = 0;
                    decimal total_prot = 0;
                    decimal total_ensayo = 0;
                    decimal total_acceso_inf = 0;
                    decimal total = 0;

                    if (mes != 0) { c_mes = mes; }
                    if (anio != 0) { c_anio = anio; }

                    var v_comprobante = _GeneralService.GetAllComprobantes_x_mes(c_mes, c_anio);

                    string cadena = "";
                    string cadena_total = "";

                    string mes_text = "";

                    if (c_mes == 1) { mes_text = "ENERO"; }
                    if (c_mes == 2) { mes_text = "FEBRERO"; }
                    if (c_mes == 3) { mes_text = "MARZO"; }
                    if (c_mes == 4) { mes_text = "ABRIL"; }
                    if (c_mes == 5) { mes_text = "MAYO"; }
                    if (c_mes == 6) { mes_text = "JUNIO"; }
                    if (c_mes == 7) { mes_text = "JULIO"; }
                    if (c_mes == 8) { mes_text = "AGOSTO"; }
                    if (c_mes == 9) { mes_text = "SETIEMBRE"; }
                    if (c_mes == 10) { mes_text = "OCTUBRE"; }
                    if (c_mes == 11) { mes_text = "NOVIEMBRE"; }
                    if (c_mes == 12) { mes_text = "DICIEMBRE"; }

                    string mes_minis_text = "";
                    if (DateTime.Now.Month == 1) { mes_minis_text = "Enero"; }
                    if (DateTime.Now.Month == 2) { mes_minis_text = "Febrero"; }
                    if (DateTime.Now.Month == 3) { mes_minis_text = "Marzo"; }
                    if (DateTime.Now.Month == 4) { mes_minis_text = "Abril"; }
                    if (DateTime.Now.Month == 5) { mes_minis_text = "Mayo"; }
                    if (DateTime.Now.Month == 6) { mes_minis_text = "Junio"; }
                    if (DateTime.Now.Month == 7) { mes_minis_text = "Julio"; }
                    if (DateTime.Now.Month == 8) { mes_minis_text = "Agosto"; }
                    if (DateTime.Now.Month == 9) { mes_minis_text = "Setiembre"; }
                    if (DateTime.Now.Month == 10) { mes_minis_text = "Octubre"; }
                    if (DateTime.Now.Month == 11) { mes_minis_text = "Noviembre"; }
                    if (DateTime.Now.Month == 12) { mes_minis_text = "Diciembre"; }

                    foreach (var result in v_comprobante)
                    {
                        if (result.venta_cert != null) { total_cert = total_cert + (result.venta_cert ?? 0); }
                        if (result.venta_prot != null) { total_prot = total_prot + (result.venta_prot ?? 0); }
                        if (result.venta_ensayo != null) { total_ensayo = total_ensayo + (result.venta_ensayo ?? 0); }
                        if (result.acceso_info != null) { total_acceso_inf = total_acceso_inf + (result.acceso_info ?? 0); }
                        if (result.total != null) { total = total + (result.total ?? 0); }

                        cadena = cadena + "<tr>" +
                        "<td style=" + (char)34 + "text-align: left" + (char)34 + ">" + result.mes + "</td>" +
                        "<td style=" + (char)34 + "text-align: left" + (char)34 + ">" + result.dia_semana + "</td>" +
                        "<td align=" + (char)34 + "center" + (char)34 + ">" + result.dia_num + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "> " + (result.venta_cert ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.venta_prot ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.venta_ensayo ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + ">" + (result.acceso_info ?? 0).ToString("###,###,##0.00") + "</td>" +
                        "<td align=" + (char)34 + "right" + (char)34 + " style=" + (char)34 + "text-align: right; border-right:1px solid #d1d1d1;" + (char)34 + ">" + (result.total ?? 0).ToString("###,###,##0.00") + "</td></tr>";
                    };

                    cadena_total = "<tr style=" + (char)34 + "font-size:15px;" + (char)34 + ">" +
                        "<td>&nbsp;</td>" +
                        "<td colspan=" + (char)34 + "2" + (char)34 + "><strong>TOTAL S/.</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_cert.ToString("###,###,##0.00") + "   </strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_prot.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_ensayo.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td style=" + (char)34 + "text-align: right" + (char)34 + "><strong>" + total_acceso_inf.ToString("###,###,##0.00") + "</strong></td>" +
                        "<td align=" + (char)34 + "right" + (char)34 + " style=" + (char)34 + "text-align: right; border-right:1px solid #d1d1d1;" + (char)34 + "><strong>" + total.ToString("###,###,##0.00") + "</strong></td>" +
                        "</tr>";

                    ViewBag.mes_anio_text = " - " + mes_text + " " + c_anio.ToString();

                    ViewBag.dia_text = " " + DateTime.Now.Day.ToString() + " de " + mes_minis_text + " " + DateTime.Now.Year.ToString();
                    ViewBag.html_detalle_mes = cadena;
                    ViewBag.html_detalle_mes_total = cadena_total;

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
        public ActionResult Imprimir_li(int id = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19")))
                // Oficina 19: Unidad de CFT
                {
                    var v_comprobante = _GeneralService.GetAllComprobantes_x_fecha(id);

                    string txt_compr_cert = "";
                    string txt_val_cert = "";
                    string txt_compr_prot = "";
                    string txt_val_prot = "";
                    string txt_compr_ensa = "";
                    string txt_val_ensa = "";
                    string txt_compr_ainfo = "";
                    string txt_val_ainfo = "";
                    string txt_sub_total = "";


                    foreach (var x in v_comprobante)
                    {
                        txt_compr_cert = x.compr_cert;
                        txt_val_cert = (x.venta_cert ?? 0).ToString("###,###,##0.00");
                        txt_compr_prot = x.compr_prot;
                        txt_val_prot = (x.venta_prot ?? 0).ToString("###,###,##0.00");
                        txt_compr_ensa = x.compr_ensayo;
                        txt_val_ensa = (x.venta_ensayo ?? 0).ToString("###,###,##0.00");;
                        txt_compr_ainfo = x.compr_ainfo;
                        txt_val_ainfo = (x.acceso_info ?? 0).ToString("###,###,##0.00");;
                        txt_sub_total = (x.total ?? 0).ToString("###,###,##0.00");;
                    }

                    ViewBag.v_txt_compr_cert = txt_compr_cert;
                    ViewBag.v_txt_val_cert = txt_val_cert;
                    ViewBag.v_txt_compr_prot = txt_compr_prot;
                    ViewBag.v_txt_val_prot = txt_val_prot;
                    ViewBag.v_txt_compr_ensa = txt_compr_ensa;
                    ViewBag.v_txt_val_ensa = txt_val_ensa;
                    ViewBag.v_txt_compr_ainfo = txt_compr_ainfo;
                    ViewBag.v_txt_val_ainfo = txt_val_ainfo;
                    ViewBag.v_txt_sub_total = txt_sub_total;

                    int v_dia_n = v_comprobante.First().dia_num ?? 0;
                    int v_mes_n = v_comprobante.First().mes_num ?? 0;
                    int v_anio_n = v_comprobante.First().anio_num ?? 0;

                    ViewBag.dia_text = " " + v_dia_n.ToString("00") + "/" + v_mes_n.ToString("00") + "/" + v_anio_n.ToString();
                    ViewBag.anio_text = " " + v_anio_n.ToString();

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

        public ActionResult Imprimir_rd(int id = 0)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19")))
                // Oficina 19: Unidad de CFT
                {
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("OD");
                    tbl.Columns.Add("TIPO COMPROBANTE");
                    tbl.Columns.Add("COMPROBANTE");
                    tbl.Columns.Add("FECHA");
                    tbl.Columns.Add("RUC");
                    tbl.Columns.Add("NOMBRE");
                    tbl.Columns.Add("CONCEPTO");
                    tbl.Columns.Add("EXP.");
                    tbl.Columns.Add("CANTIDAD");
                    tbl.Columns.Add("TUPA");
                    tbl.Columns.Add("IMPORTE");
                    tbl.Columns.Add("NUMERO OPERACION");
                    tbl.Columns.Add("FECHA OPERACION");

                    var v_operaciones = _GeneralService.recupera_reporte_diario_serie1(id);

                    DataRow tbl_row;
                    foreach (var res in v_operaciones)
                    {
                         string str_concepto=res.concepto;
                        if (res.concepto.Length > 50)
                        {
                            str_concepto = res.concepto.Substring(0, 50)+"...";
                        }

                        tbl_row = tbl.NewRow();
                        tbl_row["OD"] = res.n_oficina_crea;
                        if(res.id_tipo_factura == 2){
                            tbl_row["TIPO COMPROBANTE"] = "RECIBO";
                        }
                        else
                        {
                            if (res.id_tipo_factura == 1)
                            {
                                tbl_row["TIPO COMPROBANTE"] = "FACTURA";
                            }
                            else
                            {
                                tbl_row["TIPO COMPROBANTE"] = "";
                            }
                        }

                        tbl_row["COMPROBANTE"] = res.num_fact;
                        tbl_row["FECHA"] = res.fecha;
                        tbl_row["RUC"] = "'" + res.documento;
                        tbl_row["NOMBRE"] = res.datos;
                        tbl_row["CONCEPTO"] = str_concepto;
                        tbl_row["EXP."] =res.expediente;
                        tbl_row["CANTIDAD"] = res.cantidad;
                        tbl_row["TUPA"] = res.tupa;
                        tbl_row["IMPORTE"] = (res.importe_total ?? 0).ToString("###,###,##0.00");
                        tbl_row["NUMERO OPERACION"] = res.operacion;
                        tbl_row["FECHA OPERACION"] = res.fecha_operacion;

                        tbl.Rows.Add(tbl_row);
                    }

                    GridView gv = new GridView();
                    gv.DataSource = tbl;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=Reporte_Diario.xls");
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
       
        public ActionResult Consulta_factura(int page = 1, string comprobante = "", string tipo_comprobante = "", string documento="", string externo="", string operac="")
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("ID_COMPROBANTE");
                    tbl.Columns.Add("FECHA_TEXT");
                    tbl.Columns.Add("TIPO_COMPROBANTE");
                    tbl.Columns.Add("COMPROBANTE");
                    tbl.Columns.Add("TUPA_SERV");
                    tbl.Columns.Add("VALOR_FACT_EXP");
                    tbl.Columns.Add("IMPORTE_TOTAL");
                    tbl.Columns.Add("DOCUMENTO");
                    tbl.Columns.Add("EXTERNO");
                    tbl.Columns.Add("DIRECCION");
                    tbl.Columns.Add("ID_OPERACION");
                    tbl.Columns.Add("OPERACIONES");
                    tbl.Columns.Add("USUARIO_REGISTRO");
                    tbl.Columns.Add("RUTA_PDF");

                    var v_comprobante = _GeneralService.GetAllFacturas(comprobante, tipo_comprobante, documento, externo, operac);
                    
                    foreach (var result in v_comprobante)
                    {
                        tbl.Rows.Add(
                            result.id_factura,
                            result.fecha_text,
                            result.tipo_factura,
                            result.comprobante,
                            result.tupa_serv,
                            result.valor_fact_exp,
                            result.importe,
                            result.documento,
                            result.externo,
                            result.direccion,
                            result.id_operacion,
                            result.operaciones,
                            result.usuario_registro,
                            result.ruta_pdf
                            );
                    };

                    ViewData["Comprobante_tabla"] = tbl; 

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
        public ActionResult Ver_voucher_oper(int id = 0)
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    Session["pdf_voucher_id_comprobante"] = id;
                    return RedirectToAction("Ver_voucher_oper_sv", "General");
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
        public ActionResult Ver_voucher_oper_sv()
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    int var_id_operacion = Convert.ToInt32(Session["pdf_voucher_id_comprobante"].ToString());
                    Session.Remove("pdf_voucher_id_comprobante");

                    string ruta = _GeneralService.lista_operacion_x_id(var_id_operacion).ruta_pdf;
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


        public ActionResult Nueva_Operacion()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 19: Contabilidad y Tesoreria
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
        public ActionResult Grabar_Nueva_Operacion(int numero, int oficina, decimal importe, string fecha)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    //(HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325")))
                // Oficina 19: Unidad de Contab.
                {
                    DateTime fec_env = Convert.ToDateTime(fecha);
                    _GeneralService.Guardar_Operacion(numero, fec_env, oficina, importe, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    return PartialView("_SuccessGN");
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

        public ActionResult Nueva_Factura()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325"))))
                // Oficina 19: Contabilidad y Tesoreria
                {

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

                    ViewBag.lst_departamento_new_oficina = Lista_departamento;
                    ViewBag.lst_provincia_new_oficina = Lista_provincia;
                    ViewBag.lst_distrito_new_oficina = Lista_distrito;


                    List<SelectListItem> lista_tipo_fact = new List<SelectListItem>();

                    foreach (var result in _GeneralService.lista_tipo_comprobante()){
                        lista_tipo_fact.Add(new SelectListItem(){
                            Text = result.nombre,
                            Value = result.id_tipo_factura.ToString()});
                    };

                    List<SelectListItem> lista_x = new List<SelectListItem>();
                    /*
                    foreach (var result in _GeneralService.lista_personareciboserie1_sin_direc()){
                        lista_persona.Add(new SelectListItem(){
                            Text = result.documento+" "+result.nombre,
                            Value = result.documento.ToString()});
                    };
                    */

                    ViewBag.lst_tipo_comprobante = lista_tipo_fact;
                    ViewBag.lst_x = lista_x;
                    

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

        /*
         url: "/General/Grabar_Nueva_Factura",
                data: {
                    "tipo_comprobante": $("#cmb_tipo_comprobante").val(),
                    "documento": $("#documento").val(),
                    "datos": $("#cmb_persona").val(),
                    "direccion": $("#cmb_direccion").val(),
                    "importe_total": $("#TXT_IMPORTE_TOTAL").val(),
                    "id_sub_tupa": $("#txt_id_sub_tupa").val()
         */

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Grabar_Nueva_Factura(int tipo_comprobante, string documento, 
            string datos, string direccion, decimal importe_total, int id_sub_tupa, 
            int opera1, int opera2, int opera3, int opera4, int num2, int cantidad)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    //(HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[11].Trim() == "1" // Acceso a Factura
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "19" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "66" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "67" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "68" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "69" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "70" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "71" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "72" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "73" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "74" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "75" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "377" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "386" ||
            HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1303" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "1325")))
                // Oficina 19: Unidad de Contab.
                {
                    try
                    {
                        string num_sec = "";

                        if (tipo_comprobante == 2)
                        {
                            num_sec = (_GeneralService.ultimo_numero_comprobante(tipo_comprobante) + 1).ToString();
                        }
                        else
                        {
                            num_sec = num2.ToString();
                        }

                        int id_factr = _GeneralService.Guardar_Factura("1", num_sec, DateTime.Now, importe_total, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim(), tipo_comprobante, documento, datos, direccion, id_sub_tupa, cantidad, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())).id_factura;

                        if (opera1 != null && opera1 != 0) { _GeneralService.Guardar_det_fac_opera(id_factr, opera1); }
                        if (opera2 != null && opera2 != 0) { _GeneralService.Guardar_det_fac_opera(id_factr, opera2); }
                        if (opera3 != null && opera3 != 0) { _GeneralService.Guardar_det_fac_opera(id_factr, opera3); }
                        if (opera4 != null && opera4 != 0) { _GeneralService.Guardar_det_fac_opera(id_factr, opera4); }

                        if (tipo_comprobante == 2)
                        {
                            @ViewBag.Mensaje = ""+id_factr.ToString();
                            return PartialView("_SuccessGN");
                        }
                        else
                        {
                            @ViewBag.Mensaje = "Se Genero la factura:" + " E001" + " - " + num_sec.ToString();
                            return PartialView("_Success");
                        }
                        
                    }
                    catch (Exception)
                    {
                        @ViewBag.Mensaje = "";
                        return PartialView("_Success");
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

        public ActionResult Consulta_expediente(int page = 1, string var_expediente= "")
        {
            if (HttpContext.Request.IsAuthenticated)
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

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[12].Trim() == "1" // Acceso a Expediente
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso == 1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("PERSONA_CREA");
                    tbl.Columns.Add("FECHA_CREA");
                    tbl.Columns.Add("NUM_EXPEDIENTE");
                    tbl.Columns.Add("TIPO_EXPEDIENTE");
                    tbl.Columns.Add("ESTADO");

                    var v_expediente = _GeneralService.GetAllExpediente_sin_paginado(var_expediente, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim()), HttpContext.User.Identity.Name.Split('|')[1].Trim());
                    
                    foreach (var result in v_expediente)
                    {
                        tbl.Rows.Add(
                            result.nom_usuario,
                            result.fecha_registro,
                            result.nom_expediente,
                            result.tipo_expediente.nombre,
                            result.estado_seguimiento
                            );
                    };

                    ViewData["Expediente_Tabla"] = tbl; 
                    
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

        public ActionResult Nuevo_Expediente()
        {
            if (HttpContext.Request.IsAuthenticated)
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
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[12].Trim() == "1" // Acceso a Expediente
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso == 1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {

                    List<SelectListItem> lista_tip_exp = new List<SelectListItem>();

                    foreach (var result in _GeneralService.llenar_tipo_expediente(0, Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())))
                    {
                        lista_tip_exp.Add(new SelectListItem()
                        {
                            Text = result.nombre,
                            Value = result.id_tipo_expediente.ToString()
                        }
                            );
                    };

                    ViewBag.lst_tipo_expediente = lista_tip_exp;
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
        public ActionResult Grabar_Nuevo_Expediente(int num_expediente, int id_tipo_expediente)
        {
            if (HttpContext.Request.IsAuthenticated)
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

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && // Sistema N° 2: Sistema de Gestión de Documentos
                    (HttpContext.User.Identity.Name.Split('|')[5].ToString().Split(',')[0].Trim() == "16" || //Administrador
                    (HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[12].Trim() == "1" // Acceso a Expediente
                    && (HttpContext.User.Identity.Name.Split('|')[4].Trim() == "18" || HttpContext.User.Identity.Name.Split('|')[4].Trim() == "28" || permiso == 1))))
                // Oficina 18: Sub dirección de Habilitaciones ó Oficina 28: Atención al Usuario
                {
                    try
                    {
                        ExpedientesRequest req_exp = new ExpedientesRequest();
                        req_exp.numero_expediente = num_expediente;
                        req_exp.id_tipo_expediente = 90;
                        req_exp.usuario_registro = HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim();
                        req_exp.fecha_registro = DateTime.Now;
                        req_exp.nom_expediente = _HojaTramiteService.Create_numero(1);
                        req_exp.indicador_seguimiento = "0";
                        req_exp.año_crea = DateTime.Now.Year;
                        _GeneralService.Guardar_Expediente(req_exp);
                        @ViewBag.Mensaje = "Se Generó exitosamente el Expediente:" + req_exp.nom_expediente;
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
        
        public ActionResult llenar_provincia_x_departamento(string id_departamento)
        {
            List<SelectListItem> Lista_provincia = new List<SelectListItem>();

            Lista_provincia.Add(new SelectListItem()
            {
                Text = "SELECCIONAR PROVINCIA",
                Value = ""
            });

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
            return Json(Lista_provincia, JsonRequestBehavior.AllowGet);
        }

        public ActionResult llenar_distrito_x_provincia(string id_provincia)
        {
            List<SelectListItem> Lista_distrito = new List<SelectListItem>();

            Lista_distrito.Add(new SelectListItem()
            {
                Text = "SELECCIONAR DISTRITO",
                Value = ""
            });

            var recupera_distrito = _GeneralService.llenar_distrito_x_provincia(id_provincia);

            foreach (var result in recupera_distrito)
            {
                Lista_distrito.Add(new SelectListItem()
                {
                    Text = result.distrito,
                    Value = result.ubigeo
                }
                );
            };
            return Json(Lista_distrito, JsonRequestBehavior.AllowGet);
        }

        public ActionResult recupera_transporte(string PLACA)
        {
            List<SelectListItem> Lista_Transporte = new List<SelectListItem>();

            var consulta = _GeneralService.listar_transporte_x_placa(PLACA);
            if (consulta.ToList().Count > 0)
            {
                foreach (var result in consulta)
                {
                    Lista_Transporte.Add(new SelectListItem()
                    {
                        Text = result.placa+" / "+result.nombre_carroceria,
                        Value = result.id_transporte.ToString()
                    });
                }
            }
            else
            {
                Lista_Transporte.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }

            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(Lista_Transporte, JsonRequestBehavior.AllowGet);
        }


        public ActionResult recuperar_transporte_x_id_transporte(int id_transporte)
        {
            DbGeneralMaeTransporteResponse Lista_Transporte = new DbGeneralMaeTransporteResponse();

            Lista_Transporte = _GeneralService.recuperar_transporte_x_id_transporte(id_transporte);

            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(Lista_Transporte, JsonRequestBehavior.AllowGet);
        }


        public ActionResult recupera_embarcacion(string MATRICULA)
        {
            List<SelectListItem> Lista_Embarcaciones = new List<SelectListItem>();
            
            var consulta = _GeneralService.listar_embarcaciones(MATRICULA);
            if (consulta.ToList().Count > 0)
            {
                foreach (var result in consulta)
                {
                    Lista_Embarcaciones.Add(new SelectListItem()
                    {
                        Text = result.matricula + " / " + result.nombre,
                        Value = result.id_embarcacion.ToString()
                    });
                }
            }
            else
            {
                Lista_Embarcaciones.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }

            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(Lista_Embarcaciones, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_almacen(string COD_ALMACEN, int VAR_ID_OFI_DIR)
        {
            List<SelectListItem> lista_almacen = new List<SelectListItem>();

            var consulta = _GeneralService.lista_almacen(COD_ALMACEN, VAR_ID_OFI_DIR);

            if (consulta.ToList().Count > 0)
            {
                foreach (var result in consulta)
                {
                    lista_almacen.Add(new SelectListItem()
                    {
                        Text = result.nom_cod_habilitante,
                        Value = result.id_almacen.ToString()
                    });
                }
            }
            else
            {
                lista_almacen.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }

            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(lista_almacen, JsonRequestBehavior.AllowGet);
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_concesion(string COD_CONCESION, int VAR_ID_OFI, string DOCUMENTO_PERSONA)
        {
            List<SelectListItem> lista_concesion = new List<SelectListItem>();

            if (VAR_ID_OFI != 0)
            {
                DOCUMENTO_PERSONA = _GeneralService.Recupera_RUC_x_ID_OFI_DIR(VAR_ID_OFI);
            }

            var consulta = _GeneralService.lista_concesion(COD_CONCESION, DOCUMENTO_PERSONA);

            if (consulta.ToList().Count > 0)
            {
                foreach (var result in consulta)
                {
                    lista_concesion.Add(new SelectListItem()
                    {
                        Text = result.codigo_habilitacion,
                        Value = result.id_concesion.ToString()
                    });
                }
            }
            else
            {
                lista_concesion.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }

            //Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[4].Trim())
            return Json(lista_concesion, JsonRequestBehavior.AllowGet);
        }
                
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Existe_matricula(string var_matricula = "")
        {

            string encuentra = "";

            if(_GeneralService.buscar_embarcacion(var_matricula)>0)
            {
                encuentra = "SI";
            }

            return Json(encuentra, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Existe_factura(int var_num1 = 0, int var_num2 = 0)
        {

            string encuentra = "";

            if (_GeneralService.buscar_factura(var_num1, var_num2) > 0)
            {
                encuentra = "SI";
            }

            return Json(encuentra, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Existe_persona(string var_persona_num_doc = "")
        {
            string encuentra = "";

            if (_GeneralService.buscar_persona(var_persona_num_doc) > 0)
            {
                encuentra = "SI";
            }

            return Json(encuentra, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Existe_expediente(int num_expediente = 0, int id_tipo_expediente=0)
        {
            string encuentra = "";

            if (_GeneralService.buscar_expediente(num_expediente,id_tipo_expediente,DateTime.Now.Year) > 0)
            {
                encuentra = "SI";
            }

            return Json(encuentra, JsonRequestBehavior.AllowGet);
        } 
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_empresa()
        {
            List<SelectListItem> Lista_Empresa = new List<SelectListItem>();

            Lista_Empresa.Add(new SelectListItem()
            {
                Text = "SELECCIONAR EMPRESA",
                Value = "0"
            });

            var recupera_provincia = _HojaTramiteService.Consulta_Empresas();

            foreach (var result in recupera_provincia)
            {
                Lista_Empresa.Add(new SelectListItem()
                {
                    Text = result.nombre + "-" + result.ruc,
                    Value = result.id_oficina.ToString()
                }
                );
            };

            return Json(Lista_Empresa, JsonRequestBehavior.AllowGet);
        } 
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_Sedes_empresa(int ruc=0)
        {            
            List<SelectListItem> lista_sedes = new List<SelectListItem>();
            
            lista_sedes.Add(new SelectListItem()
            {
                Text = "SELECCIONAR SEDE",
                Value = "0"
            });
            
            foreach (var result in _GeneralService.Recupera_sede_all(ruc))
            {
                lista_sedes.Add(new SelectListItem()
                {
                    Text = result.nombre,
                    Value = result.id_sede.ToString()
                });    
            };

            return Json(lista_sedes, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_Sedes_empresa_planta(int ruc = 0)
        {
            List<SelectListItem> lista_sedes = new List<SelectListItem>();

            lista_sedes.Add(new SelectListItem()
            {
                Text = "SELECCIONAR SEDE",
                Value = ""
            });

            foreach (var result in _GeneralService.Recupera_sede_all(ruc))
            {
                lista_sedes.Add(new SelectListItem()
                {
                    Text = result.direccion +"("+result.nombre+")",
                    Value = result.id_sede.ToString()
                });
            };

            return Json(lista_sedes, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_tipo_planta()
        {
            List<SelectListItem> lista_tip_planta = new List<SelectListItem>();
            
            foreach (var result in _GeneralService.recupera_tipo_planta())
            {
                lista_tip_planta.Add(new SelectListItem()
                {
                    Text = result.siglas,
                    Value = result.id_tipo_planta.ToString()
                });
            };

            return Json(lista_tip_planta, JsonRequestBehavior.AllowGet);
        }
                                
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult llenar_OD()
        {
            List<SelectListItem> lista_od = new List<SelectListItem>();
            
            foreach (var result in _GeneralService.recupera_filial("I"))
            {
                lista_od.Add(new SelectListItem()
                {
                    Text = result.nombre,
                    Value = result.id_filial.ToString()
                });
            };

            return Json(lista_od, JsonRequestBehavior.AllowGet);
        }
                
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_tipo_actividad_planta(int id_codigo)
        {
            List<SelectListItem> lista_act_pla = new List<SelectListItem>();

            foreach (var result in _GeneralService.recupera_tipo_actividad_planta(id_codigo))
            {
                lista_act_pla.Add(new SelectListItem()
                {
                    Text = result.nombre,
                    Value = result.id_tipo_actividad.ToString()
                });
            };

            return Json(lista_act_pla, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_actividad_almacen()
        {
            List<SelectListItem> lista_actividad_almacen = new List<SelectListItem>();

            foreach (var result in _GeneralService.recupera_actividad_almacen())
            {
                lista_actividad_almacen.Add(new SelectListItem()
                {
                    Text = result.nombre_actividad,
                    Value = result.id_actividad_almacen.ToString()
                });
            };

            return Json(lista_actividad_almacen, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llenar_codigo_almacen()
        {
            List<SelectListItem> lista_codigo_almacen = new List<SelectListItem>();

            foreach (var result in _GeneralService.recupera_codigo_almacen())
            {
                lista_codigo_almacen.Add(new SelectListItem()
                {
                    Text = result.siglas,
                    Value = result.id_codigo_almacen.ToString()
                });
            };

            return Json(lista_codigo_almacen, JsonRequestBehavior.AllowGet);
        }

        

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llena_consumo_humano()
        {
            List<SelectListItem> lista_consumo_humano = new List<SelectListItem>();

            lista_consumo_humano.Add(new SelectListItem()
            {
                Text = "SELECCION",
                Value = ""
            });

            foreach (var result in _GeneralService.recupera_tipo_consumo())
            {
                lista_consumo_humano.Add(new SelectListItem()
                {
                    Text = result.siglas,
                    Value = result.id_tipo_ch.ToString()
                });
            };

            return Json(lista_consumo_humano, JsonRequestBehavior.AllowGet);
        }

        

            
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Llena_actividad_concesion()
        {
            List<SelectListItem> lista_actividad_concesion = new List<SelectListItem>();

            lista_actividad_concesion.Add(new SelectListItem()
            {
                Text = "SELECCION",
                Value = ""
            });

            foreach (var result in _GeneralService.recupera_actividad_concesion())
            {
                lista_actividad_concesion.Add(new SelectListItem()
                {
                    Text = result.nombre,
                    Value = result.id_tip_act_conce.ToString()
                });
            };

            return Json(lista_actividad_concesion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult llenar_zona_produccion_x_ubigeo(string ubigeo)
        {
            List<SelectListItem> lista_zona_produccion = new List<SelectListItem>();

            lista_zona_produccion.Add(new SelectListItem()
            {
                Text = "SELECCIONAR ZONA DE PRODUCCION",
                Value = "0"
            });

            var recupera_zona_produccion = _GeneralService.recupera_zona_produccion_x_ubigeo(ubigeo);

            foreach (var result in recupera_zona_produccion)
            {
                lista_zona_produccion.Add(new SelectListItem()
                {
                    Text = result.cod_zona_produccion+" / "+result.nombre,
                    Value = result.id_zona_produccion.ToString()
                }
                );
            };
            return Json(lista_zona_produccion, JsonRequestBehavior.AllowGet);
        }


        public ActionResult llenar_area_produccion_x_zona_produccion(int id_zona_produccion)
        {
            List<SelectListItem> lista_area_produccion = new List<SelectListItem>();

            lista_area_produccion.Add(new SelectListItem()
            {
                Text = "SELECCIONAR AREA DE PRODUCCION",
                Value = "0"
            });

            var recupera_area_produccion = _GeneralService.recupera_area_produccion(id_zona_produccion);

            foreach (var result in recupera_area_produccion)
            {
                lista_area_produccion.Add(new SelectListItem()
                {
                    Text = result.cod_area_produccion + " / " + result.nombre,
                    Value = result.id_area_produccion.ToString()
                }
                );
            };
            return Json(lista_area_produccion, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ajax_llenar_codigo_desembarcadero(int id_tipo_desembarcadero = 0)
        {
            List<SelectListItem> lista_codigo = new List<SelectListItem>();

            foreach (var result2 in _GeneralService.recupera_codigo_desembarcadero(id_tipo_desembarcadero))
            {
                lista_codigo.Add(new SelectListItem()
                {
                    Text = result2.codigo,
                    Value = result2.id_cod_desemb.ToString()
                });
            }

            return Json(lista_codigo, JsonRequestBehavior.AllowGet);
        }

        /*
         "id_sede": $('#txt_id_sede_direccion_edit').val(),
                    "direccion": $('#txt_direccion_edit').val(),
                    "ubigeo": $('#cmblista_distrito_edit').val(),
                },
         */

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult HT_editar_direccion(int id_sede = 0, string direccion = "", string ubigeo="", string sede="", string referencia="")
        {

            if (HttpContext.Request.IsAuthenticated)
            {

                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2" && HttpContext.User.Identity.Name.Split('|')[9].Trim().Split(',')[3].Trim() == "1")
                {
                    var success = _GeneralService.Edita_db_general_mae_sede(id_sede, direccion, ubigeo, sede, referencia);
                    @ViewBag.Mensaje = "La operación se realizo satisfactoriamente";
                    return PartialView("_SuccessGN");

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
        public ActionResult buscar_persona_entidad_por_nombre(string nombre = "")
        {
            IEnumerable<UnionentidadpersonaResponse> doc_per_ent = new List<UnionentidadpersonaResponse>();
            doc_per_ent = _GeneralService.buscar_entidad_persona(nombre);
            return Json(doc_per_ent, JsonRequestBehavior.AllowGet);
        }

        // VISTAS: NUEVA_OFICINA
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LLENAR_DIR_LEGAL_X_ENTIDAD(string RUC) /// ME QUEDE ACA
        {
            IEnumerable<ConsultarOficinaDireccionLegalResponse> DIRECCION_LEGAL = new List<ConsultarOficinaDireccionLegalResponse>();
            DIRECCION_LEGAL = _GeneralService.GetAllDireccionLegal_x_ruc(RUC);
            return Json(DIRECCION_LEGAL, JsonRequestBehavior.AllowGet);
        }
        // VISTAS: NUEVA_OFICINA
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LLENAR_PERSONA_LEGAL_X_ENTIDAD(string RUC) /// ME QUEDE ACA
        {
            IEnumerable<ConsultarEmpresaPersonaLegalResponse> PERSONA_LEGAL = new List<ConsultarEmpresaPersonaLegalResponse>();
            PERSONA_LEGAL = _GeneralService.GetAllPersonaLegal_x_ruc(RUC);
            return Json(PERSONA_LEGAL, JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LLENAR_PERSONA_LEGAL_X_DNI(string DNI) /// ME QUEDE ACA
        {
            IEnumerable<ConsultarDniPersonalLegalResponse> PERSONA_LEGAL = new List<ConsultarDniPersonalLegalResponse>();
            PERSONA_LEGAL = _GeneralService.GetAllPersonaLegal_x_dni(DNI);
            return Json(PERSONA_LEGAL, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Imprimir_recibo_serie1(int id)
        {
            ConsultaReciboSerie1Response recs1 = new ConsultaReciboSerie1Response();
            recs1 = _GeneralService.lista_recibo(id);

            ViewBag.nombre = recs1.nombre;
            ViewBag.direccion = recs1.direccion;
            ViewBag.ruc_dni = recs1.ruc_dni;
            ViewBag.fecha_emision = recs1.fecha_emision;
            ViewBag.dia = recs1.dia;
            ViewBag.año = recs1.año;
            ViewBag.mes_text = recs1.mes_text;
            ViewBag.fecha_nom = recs1.dia + " " + recs1.mes_text + " " + recs1.año;
            ViewBag.letra_importe = recs1.letra_importe;
            ViewBag.importe_total = recs1.importe_total.ToString();
            ViewBag.decimal_text = recs1.decimal_text.ToString();
            ViewBag.nombre_vfe = recs1.nombre_vfe;
            ViewBag.num_fact = recs1.num_fact;
            ViewBag.operacion = recs1.operacion;
            ViewBag.fecha_operacion = recs1.fecha_operacion;
            ViewBag.tupa_serv = recs1.tupa_serv;
            ViewBag.cantidad = recs1.cantidad.ToString();
            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Imprimir_liquidacion_ingreso_serie_1()
        {

            string mes_minis_text = "";
            if (DateTime.Now.Month == 1) { mes_minis_text = "Enero"; }
            if (DateTime.Now.Month == 2) { mes_minis_text = "Febrero"; }
            if (DateTime.Now.Month == 3) { mes_minis_text = "Marzo"; }
            if (DateTime.Now.Month == 4) { mes_minis_text = "Abril"; }
            if (DateTime.Now.Month == 5) { mes_minis_text = "Mayo"; }
            if (DateTime.Now.Month == 6) { mes_minis_text = "Junio"; }
            if (DateTime.Now.Month == 7) { mes_minis_text = "Julio"; }
            if (DateTime.Now.Month == 8) { mes_minis_text = "Agosto"; }
            if (DateTime.Now.Month == 9) { mes_minis_text = "Setiembre"; }
            if (DateTime.Now.Month == 10) { mes_minis_text = "Octubre"; }
            if (DateTime.Now.Month == 11) { mes_minis_text = "Noviembre"; }
            if (DateTime.Now.Month == 12) { mes_minis_text = "Diciembre"; }

            ViewBag.dia_text = " " + DateTime.Now.Day.ToString() + " de " + mes_minis_text + " " + DateTime.Now.Year.ToString();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_operacion(int operacion = 0)
        {

            List<SelectListItem> lista_operaciones = new List<SelectListItem>();

            foreach (var x in _GeneralService.lista_operacion(operacion).Distinct().Where(x => x.factura==null))
            {
                int entra = 0;
                foreach (var y in lista_operaciones)
                {
                    if (y.Value == x.id_operacion.ToString())
                    {
                        entra = 1;
                    }
                }

                if (entra == 0)
                {
                    lista_operaciones.Add(new SelectListItem()
                    {
                        Text = x.numero.ToString()+" ( "+x.fecha_deposito.Value.ToShortDateString() + ", Abono: "+x.abono.ToString()+", Ofc: "+x.oficina.ToString()+" )",
                        Value = x.id_operacion.ToString() + "|" + x.abono.ToString() + "|" + x.numero.ToString() + "|" + x.fecha_deposito.Value.ToShortDateString()
                    });
                }
            }

            if (lista_operaciones.Count() <= 0)
            {
                lista_operaciones.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "NO"
                });
            }
            return Json(lista_operaciones, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult recupera_RUC_DNI_recibo_serie1(string RUC_DNI = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _GeneralService.lista_personareciboserie1_sin_direc(RUC_DNI).Distinct())
            {
                int entra = 0;
                foreach (var y in lista_oficinas)
                {
                    if (y.Value == x.documento.ToString() + "|" + x.nombre.ToString())
                    {
                        entra = 1;
                    }
                }

                if (entra == 0)
                {
                    lista_oficinas.Add(new SelectListItem()
                    {
                        Text = x.documento + "-" + x.nombre,
                        Value = x.documento.ToString() + "|" + x.nombre.ToString()
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
        public ActionResult recupera_RUC_DNI_DIRECCION_serie1(string DOC = "")
        {

            List<SelectListItem> lista_oficinas = new List<SelectListItem>();

            foreach (var x in _GeneralService.lista_direc_personareciboserie1(DOC))
            {
                lista_oficinas.Add(new SelectListItem()
                {
                    Text = x.direccion,
                    Value = x.direccion.ToString()
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
        public ActionResult recupera_sub_tupa_x_monto(decimal monto = 0)
        {

            List<SelectListItem> lista_tupa = new List<SelectListItem>();

            foreach (var x in _GeneralService.recuperatupa(monto))
            {
                lista_tupa.Add(new SelectListItem()
                {
                    Text = x.tupa.tipo_tupa.nombre+" : "+x.tupa.numero.ToString()+" "+x.indice.ToString() +", monto: "+x.precio.ToString(),
                    Value = x.id_sub_tupa.ToString() + "|" + x.precio.ToString() + '|' + x.tupa.asunto + '|' + x.tupa.tipo_tupa.nombre + " : " + x.tupa.numero.ToString() + " " + x.indice.ToString()
                });
            }

            if (lista_tupa.Count() <= 0)
            {
                lista_tupa.Add(new SelectListItem()
                {
                    Text = "NO",
                    Value = "0"
                });
            }
            return Json(lista_tupa, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export_Excel_operaciones(string operacion = "", string comprobante = "")
        {

            DataTable tbl = new DataTable();
            tbl.Columns.Add("Número de Operación");
            tbl.Columns.Add("Fecha");
            tbl.Columns.Add("Importe");
            tbl.Columns.Add("Oficina");
            tbl.Columns.Add("Comprobante");

            var v_operaciones = _GeneralService.Lista_todo_operacion(operacion, comprobante);

            DataRow tbl_row_documento;
            foreach (var document in v_operaciones)
            {
                tbl_row_documento = tbl.NewRow();
                tbl_row_documento["Número de Operación"] = document.numero.ToString();
                tbl_row_documento["Fecha"] = document.fecha_deposito.Value.ToShortDateString();
                tbl_row_documento["Importe"] = document.abono;
                tbl_row_documento["Oficina"] = document.oficina;
                tbl_row_documento["Comprobante"] = document.factura;
                tbl.Rows.Add(tbl_row_documento);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Operaciones.xls");
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
        public ActionResult Export_Excel_comprobantes(string comprobante = "", string tipo_comprobante = "", string documento = "", string externo = "", string operac = "")
        {

            DataTable tbl = new DataTable();
            tbl.Columns.Add("Fecha Comprobante");
            tbl.Columns.Add("Tipo Comprobante");
            tbl.Columns.Add("Comprobante");
            tbl.Columns.Add("TUPA/SE/TUSNE");
            tbl.Columns.Add("Concepto");
            tbl.Columns.Add("Importe");
            tbl.Columns.Add("Documento");
            tbl.Columns.Add("Externo");
            tbl.Columns.Add("Dirección");
            tbl.Columns.Add("Operaciones");

            var v_comprobante = _GeneralService.GetAllFacturas(comprobante, tipo_comprobante, documento, externo, operac);

            DataRow tbl_row_documento;
            foreach (var document in v_comprobante)
            {
                tbl_row_documento = tbl.NewRow();
                tbl_row_documento["Fecha Comprobante"] = document.fecha.ToString();
                tbl_row_documento["Tipo Comprobante"] = document.tipo_factura;
                tbl_row_documento["Comprobante"] = document.comprobante;
                tbl_row_documento["TUPA/SE/TUSNE"] = document.tupa_serv;
                tbl_row_documento["Concepto"] = document.valor_fact_exp;
                tbl_row_documento["Importe"] = document.importe;
                tbl_row_documento["Documento"] = document.documento;
                tbl_row_documento["Externo"] = document.externo;
                tbl_row_documento["Dirección"] = document.direccion;
                tbl_row_documento["Operaciones"] = document.operaciones;
                tbl.Rows.Add(tbl_row_documento);
            }

            GridView gv = new GridView();
            gv.DataSource = tbl;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Comprobantes.xls");
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
        public ActionResult Grabar_Nueva_Direccion(string new_oficina_nom_direccion,
                    string new_oficina_nom_sede, string new_oficina_nom_referencia, string new_oficina_ubigeo, int id_oficina_padre, string new_oficina_ruc)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    try
                    {
                        id_oficina_padre = _OficinaService.GetAllEmpresa_RUC(new_oficina_ruc).First().id_oficina;

                        _OficinaService.crea_sede_secundaria(new_oficina_nom_sede.ToUpper().Trim(), new_oficina_nom_direccion.ToUpper().Trim(), new_oficina_nom_referencia.ToUpper().Trim(), new_oficina_ubigeo, id_oficina_padre, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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
        public ActionResult Actualizar_nombre_pj(string actualiza_nom_pj,string actualiza_ruc)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.User.Identity.Name.Split('|')[7].Trim() == "2")
                {
                    try
                    {
                        _GeneralService.Edita_db_general_nom_empresa(actualiza_nom_pj, actualiza_ruc, HttpContext.User.Identity.Name.Split('|')[0].Trim() + " - " + HttpContext.User.Identity.Name.Split('|')[1].Trim());
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


	}
}