using SIGESDOC.IAplicacionService;
using SIGESDOC.Response;
using SIGESDOC.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using RazorPDF;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using DotCMIS.Client;
using DotCMIS.CMISWebServicesReference;

namespace SIGESDOC.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IHojaTramiteService _HojaTramiteService;

        public DocumentsController(IHojaTramiteService HojaTramiteService)
        {
            _HojaTramiteService = HojaTramiteService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        #region Cedula de Notificacion
        public void CedulaNotificacionWord(CargaWordCedulaNotificacion tableData)
        {
            DateTime fecha_PATH = DateTime.Now;
            //DESARROLLO
            string path = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            //alterar en web.config para pre-produccion o/u produccion
            //string path = ConfigurationManager.AppSettings["cedula"];

            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\CÉDULANOTIFICACIÓN.docx");

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
                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "CEDULA_NOTIFICACION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "CEDULA_NOTIFICACION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }

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
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

           // string path = ConfigurationManager.AppSettings["resoluciondirectoral"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\RESOLUCION_DIRECTORAL.docx");

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

                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "RESOLUCION_DIRECTORAL_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "RESOLUCION_DIRECTORAL_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
        }
        #endregion

        #region Informe uti
        public void informeUTIWord(CargaWordInformeUTI tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            //tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

           // string path = ConfigurationManager.AppSettings["informe"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\RESOLUCION_DIRECTORAL.docx");

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
                }
            }

        }

        #endregion

        #region OFICIO

        [HttpGet]
        public void OficioWord(CargaOficioWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            //string path = ConfigurationManager.AppSettings["oficio"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_OFICIO.docx");

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

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    Run EXPEDIENTE = bookmarkMaps["EXPEDIENTE"].NextSibling<Run>();
                    EXPEDIENTE.GetFirstChild<Text>().Text = tableData.EXPEDIENTE;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                    Run CARGO = bookmarkMaps["CARGO"].NextSibling<Run>();
                    CARGO.GetFirstChild<Text>().Text = tableData.CARGO;

                    Run DIRECCION = bookmarkMaps["DIRECCION"].NextSibling<Run>();
                    DIRECCION.GetFirstChild<Text>().Text = tableData.DIRECCION;

                    Run NOMBRES = bookmarkMaps["NOMBRES"].NextSibling<Run>();
                    NOMBRES.GetFirstChild<Text>().Text = tableData.NOMBRES;

                    Run REFERENCIA = bookmarkMaps["REFERENCIA"].NextSibling<Run>();
                    REFERENCIA.GetFirstChild<Text>().Text = tableData.REFERENCIA;


                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                }

                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "OFICIO_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "OFICIO_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
        }

        #endregion

        #region INVITACION

        [HttpGet]
        public void InvitacionWord(CargaInvitacionWord tableData)
        {

            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            //tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

           // string path = ConfigurationManager.AppSettings["invitacion"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_INVITACION.docx");

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

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;


                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();

                }
                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "INVITACION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "INVITACION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
        }

        #endregion

        #region RESOLUCION

        [HttpGet]
        public void ResolucionWord(CargaResolucionWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

           // string path = ConfigurationManager.AppSettings["resolucion"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_RESOLUCION.docx");

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


                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;


                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;

                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();

                }

                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "RESOLUCION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "RESOLUCION_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());
                    Process.Start(nuevoWord);
                }
            }
        }

        #endregion

        #region INFORME

        [HttpGet]
        public void InformeWord(CargaInformeWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

          //  string path = ConfigurationManager.AppSettings["informe"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_INFORME.docx");

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

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                    Run REFERENCIA = bookmarkMaps["REFERENCIA"].NextSibling<Run>();
                    REFERENCIA.GetFirstChild<Text>().Text = tableData.REFERENCIA;

                    Run NOMBRES = bookmarkMaps["NOMBRES"].NextSibling<Run>();
                    NOMBRES.GetFirstChild<Text>().Text = tableData.NOMBRES;

                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;

                    wordDocument.MainDocumentPart.Document.Save();
                   
                    wordDocument.Close();

                    
                }

                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "INFORME_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_word, "INFORME_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
            
          }

        #endregion

        #region COMUNICADO

        [HttpGet]
        public void ComunicadoWord(CargaComunicadoWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

           // string path = ConfigurationManager.AppSettings["comunicado"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_COMUNICADO.docx");

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

                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;


                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();

                }
                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);

                }
                else
                {
                    string nuevoWord = Path.Combine(path_word, "COMUNICADO_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_pdf, "COMUNICADO_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
        }

        #endregion

        #region CARTA MULTIPLE

        [HttpGet]
        public void CartaMultipleWord(CargaCartaMultipleWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            //string path = ConfigurationManager.AppSettings["cartamultiple"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_CARTA_MULTIPLE.docx");

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
                }
            }
        }

        #endregion

        #region OFICIO MULTIPLE

        [HttpGet]
        public void OficioMultipleWord(CargaOficioMultipleWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            //string path = ConfigurationManager.AppSettings["oficiomultiple"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_OFICIO_MULTIPLE.docx");

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
                }
            }
        }

        #endregion

        #region MEMORANDO

        [HttpGet]
        public void MemorandoWord(CargaMemorandoWord tableData)
        {
            
            DateTime fecha_PATH = DateTime.Now;
            DocExtGetProperties docExt = new DocExtGetProperties();

            string uuidMemorando = ConfigurationManager.AppSettings["templateMemorando"].ToString();

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");

            //conexion a alfresco
            string login = "login";
            string ticket = DevuelveTicket(login);

            //string path = ConfigurationManager.AppSettings["memorando"];
            string pathAlfresco = ConfigurationManager.AppSettings["alfresco"];
            string metodoAlfresco = @"getProperties";
            string json = POSTFormDataAlfresco(uuidMemorando, pathAlfresco, metodoAlfresco, ticket);

            //string url = "https://172.16.1.230:8080/alfresco/service/api/node/content/workspace/SpacesStore/45036dfe-8cfe-4099-a531-49218df2212f/MODELO_DE_MEMORANDO.docx";
            
           // var ruta = WebRequest.CreateHttp(url);
   

            
            
            byte[] byteArray = System.IO.File.ReadAllBytes(json);
                
                //string uuid = @"45036dfe-8cfe-4099-a531-49218df2212f";
                //var servicio = pathAlfresco+ "getProperties?alf_ticket="+ticket;

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(servicio);
                //request.KeepAlive = true;
                //request.Method = "POST";
                //byte[] postbytes = Encoding.UTF8.GetBytes(uuid);
                ////request.Accept = "application/json";
                //request.ContentType = "multipart/form-data; uuid="+ uuid;
                //request.MediaType = "application/json";
                //request.ContentLength = postbytes.Length;
                //Stream requestStream = request.GetRequestStream();
                // requestStream.Read(postbytes, 0, postbytes.Length);
                //requestStream.Close();
                //requestStream.Write(postbytes, 0, postbytes.Length);
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //if(response.StatusCode == HttpStatusCode.OK)
                //{
                //    Stream resStream = response.GetResponseStream();
                //    //var sr = new StreamReader(response.GetResponseStream());
                //    StreamReader reader = new StreamReader(resStream);
                //    string responseText = reader.ReadToEnd();
                //}


                // byte[] byteArray = System.IO.File.ReadAllBytes(pathAlfresco + @"MODELO_DE_MEMORANDO.docx");

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

                        Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                        NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                        Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                        ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                        Run REFERENCIA = bookmarkMaps["REFERENCIA"].NextSibling<Run>();
                        REFERENCIA.GetFirstChild<Text>().Text = tableData.REFERENCIA;

                        Run NOMBRES = bookmarkMaps["NOMBRES"].NextSibling<Run>();
                        NOMBRES.GetFirstChild<Text>().Text = tableData.NOMBRES;

                        Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                        FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;

                        wordDocument.MainDocumentPart.Document.Save();
                        wordDocument.Close();
                    }

                    string nuevopath = Path.Combine(json, "MEMORANDO_" + fecha_PATH.ToString("ddMMyy") + ".docx");
                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());

                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevopath);
                
            }
        }

        #endregion

        #region MEMORANDO MULTIPLE

        [AllowAnonymous]
        [HttpGet]
        public void MemorandoMultipleWord(CargaMemorandoMultipleWord tableData)
        {

            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");

            DocExtGetProperties docExt = new DocExtGetProperties();
            string uuidMemorandoMultiple = ConfigurationManager.AppSettings["templateMemorandoMultiple"].ToString();

            //conexion a alfresco
            string login = "login";
            string ticket = DevuelveTicket(login);

            //string path = ConfigurationManager.AppSettings["memorando"];
            string pathAlfresco = ConfigurationManager.AppSettings["alfresco"];
            string metodoAlfresco = @"/getProperties";
            string json = POSTFormDataAlfresco(uuidMemorandoMultiple, pathAlfresco, metodoAlfresco, ticket);

            docExt = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DocExtGetProperties>(json);


            int id_documento = Convert.ToInt32(tableData.ID_DOCUMENTO);

            IEnumerable<DetalleMaeDocumentoResponse> documentoRequest = new List<DetalleMaeDocumentoResponse>();

            //desarrollo
            // string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            string path = "http://172.16.1.230:8080";

            string filename = System.IO.Path.Combine(path + docExt.urlDownload);
           // string path2 = System.IO.Path.GetFullPath(path);
           // string descFilePathAndName = System.IO.Path.Combine(path2, filename);

            WebClient web = new WebClient();
            web.Credentials = CredentialCache.DefaultCredentials;
            web.Credentials = CredentialCache.DefaultNetworkCredentials;
            web.UseDefaultCredentials = true;
            string ticket2 = DevuelveTicket(login);
            string down = filename + "?alf_ticket=" + ticket2;
            //byte[] byteArray = System.IO.File.ReadAllBytes(request.Address.OriginalString);
            byte[] byteArray = web.DownloadData(down);

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

                    documentoRequest = _HojaTramiteService.Listar_Detalle_Documento_Interno(id_documento);

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = documentoRequest.First().nom_doc;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = documentoRequest.First().asunto;

                    Run FECHA_ACTUAL = bookmarkMaps["FECHA_ACTUAL"].NextSibling<Run>();
                    FECHA_ACTUAL.GetFirstChild<Text>().Text = tableData.FECHA_ACTUAL;


                    foreach (var memorandomultiple in documentoRequest)
                    {
                        Body body = wordDocument.MainDocumentPart.Document.GetFirstChild<Body>();
                        Run UNO = bookmarkMaps["NOMBRES_1"].NextSibling<Run>();
                        UNO.GetFirstChild<Text>().Text = memorandomultiple.nombres;

                        Paragraph para = body.AppendChild(new Paragraph());
                        Run run = para.AppendChild(new Run());
                        run.AppendChild(new Text(memorandomultiple.nombres));
                    }
                  

                    //foreach(var memorandomultiple in documentoRequest)
                    //{
                    //    Body body = wordDocument.MainDocumentPart.Document.GetFirstChild<Body>();
                    //    var paras = body.Elements<Paragraph>();

                    //    foreach(var para in paras)
                    //    {
                    //        var bookmarkStarts = para.Elements<BookmarkStart>();
                    //        var bookMarkEnds = para.Elements<BookmarkEnd>();

                    //        foreach (BookmarkStart bookMarkStart in bookmarkStarts)
                    //        {
                                
                    //                //Get the id of the bookmark start to find the bookmark end 
                    //                var id = bookMarkStart.Id.Value;
                    //                var bookmarkEnd = bookMarkEnds.Where(i => i.Id.Value == id).First();

                    //                var runElement = new Run(new Text(memorandomultiple.nombres));

                    //                para.InsertAfter(runElement, bookmarkEnd);
                    //        }
                    //    }
                    //}

                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                }

                    string path_word = @"C:\SIGESDOC\WORD\";
                    string path_pdf = @"C:\SIGESDOC\PDF\";

                    if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                    {
                        Directory.CreateDirectory(path_word);
                        Directory.CreateDirectory(path_pdf);

                        string nuevoWord = Path.Combine(path_word, "MEMORANDO_MULTIPLE_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                        string nuevoPDF = Path.Combine(path_pdf, "MEMORANDO_MULTIPLE_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                        stream.Close();
                        System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                        System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());

                        Process.Start(nuevoWord);

                    }
                    else
                    {
                        string nuevoWord = Path.Combine(path_word, "MEMORANDO_MULTIPLE_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                        string nuevoPDF = Path.Combine(path_pdf, "MEMORANDO_MULTIPLE_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                        stream.Close();
                        System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                        System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());

                        Process.Start(nuevoWord);
                    }
             }
        }

        #endregion

        #region CARTA

        [HttpGet]
        public void CartaWord(CargaCartaWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            string path  = @"C:\Users\PSSERU-TI\Source\Repos\landersaavedra\sigesdoc\documentos externos";

            //string path = ConfigurationManager.AppSettings["carta"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"\MODELO_DE_CARTA.docx");

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

                    Run NOM_DOC = bookmarkMaps["NOM_DOC"].NextSibling<Run>();
                    NOM_DOC.GetFirstChild<Text>().Text = tableData.NOM_DOC;

                    Run ASUNTO = bookmarkMaps["ASUNTO"].NextSibling<Run>();
                    ASUNTO.GetFirstChild<Text>().Text = tableData.ASUNTO;

                    Run REFERENCIA = bookmarkMaps["REFERENCIA"].NextSibling<Run>();
                    REFERENCIA.GetFirstChild<Text>().Text = tableData.REFERENCIA;

                    Run NOMBRES = bookmarkMaps["NOMBRES"].NextSibling<Run>();
                    NOMBRES.GetFirstChild<Text>().Text = tableData.NOMBRES;

                    Run DIRECCION = bookmarkMaps["DIRECCION"].NextSibling<Run>();
                    DIRECCION.GetFirstChild<Text>().Text = tableData.DIRECCION;

                    Run EMPRESA = bookmarkMaps["EMPRESA"].NextSibling<Run>();
                    EMPRESA.GetFirstChild<Text>().Text = tableData.EMPRESA;

                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();

                }
                string path_word = @"C:\SIGESDOC\WORD\";
                string path_pdf = @"C:\SIGESDOC\PDF\";

                if (!Directory.Exists(path_word) && !Directory.Exists(path_pdf))
                {
                    Directory.CreateDirectory(path_word);
                    Directory.CreateDirectory(path_pdf);
                 
                }else {
                    string nuevoWord = Path.Combine(path_word, "CARTA_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".docx");
                    string nuevoPDF = Path.Combine(path_word, "CARTA_" + fecha_PATH.ToString("ddMMyy_HHMMSS") + ".pdf");

                    stream.Close();
                    System.IO.File.WriteAllBytes(nuevoWord, stream.ToArray());
                    System.IO.File.WriteAllBytes(nuevoPDF, stream.ToArray());


                    //Process process = new Process();
                    // process.StartInfo.FileName = Server.MapPath(nuevopath);
                    // process.Start();
                    Process.Start(nuevoWord);
                }
            }
        }

        #endregion

        public string DevuelveTicket(string connection)
        {
            //variable de salida del token
            string salida_token = string.Empty;

            //variable de desearealizacion de Username y Password de ALfresco
            string connect = ConfigurationManager.AppSettings[connection].ToString();
            login acceso = new login();
            acceso = JsonConvert.DeserializeObject<login>(System.IO.File.ReadAllText(connect));
            string outjson = JsonConvert.SerializeObject(acceso, Formatting.Indented);

            //path de alfresco para el servicio
            string connectAlfresco = ConfigurationManager.AppSettings["Alfresco"].ToString();

            //path de llamado Alfresco para token
            string pathAlfresco = connectAlfresco + "/api/login";

            //configuracion de llamado de servicio 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pathAlfresco);
            request.KeepAlive = true;
            request.Method = "POST";
            byte[] postBytes = Encoding.UTF8.GetBytes(outjson);
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.ContentLength = postBytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if(response.StatusCode == HttpStatusCode.OK)
            {
                Stream reStream = response.GetResponseStream();
                var sr = new StreamReader(response.GetResponseStream());
                string salida = sr.ReadToEnd();
                var data = ToObject(salida) as IDictionary<string, object>;

                foreach(var token in data)
                {
                    var tikets = data[token.Key] as IDictionary<string, object>;

                    foreach(var tiket in tikets)
                    { 
                        acceso.token = tiket.Value.ToString();
                    }

                    salida_token = acceso.token;
                }

            }

            return salida_token;
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

       private string POSTFormDataAlfresco(string uuid, string url, string method, string ticket)
        {

            string JsonSalida = string.Empty;
            string remoteURL = url + method + "?alf_ticket=" + ticket;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            string posString = String.Format("{0}", uuid);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteURL);

            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                string data = "Content-Disposition: form-data; name=\"" + "uuid" + "\"\r\n\r\n" + uuid;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                requestStream.Write(bytes, 0, bytes.Length);
                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();
            }

            using (WebResponse response = request.GetResponse())
            {
                System.Text.StringBuilder sb = new StringBuilder();
                using (Stream responseStream = response.GetResponseStream())

                using (StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        
    }
}


