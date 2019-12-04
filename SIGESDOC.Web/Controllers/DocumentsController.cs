using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGESDOC.Request;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace SIGESDOC.Web.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Documents
        public ActionResult Index()
        {
            return View();
        }

        #region Cedula de Notificacion
        public void CedulaNotificacionWord(CargaWordCedulaNotificacion tableData)
        {
            DateTime fecha_PATH = DateTime.Now;
            //DESARROLLO
            // string path = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            //alterar en web.config para pre-produccion o/u produccion
            string path = ConfigurationManager.AppSettings["cedula"];

            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/CÉDULANOTIFICACIÓN.docx");

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
                string nuevopath = Path.Combine(path, "CEDULA_NOTIFICACION_" + fecha_PATH.ToString("ddMMyy") + ".docx");
                stream.Close();
                System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());

                //Process process = new Process();
                // process.StartInfo.FileName = Server.MapPath(nuevopath);
                // process.Start();
                Process.Start(nuevopath);

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

        #region Informe uti
        public void informeUTIWord(CargaWordInformeUTI tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            //tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["informe"];
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
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["oficio"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_OFICIO.docx");

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

                string nuevopath = Path.Combine(path, "OFICIO_"+fecha_PATH.ToString("ddMMyy")+".docx");
                stream.Close();
                System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());

                //Process process = new Process();
                // process.StartInfo.FileName = Server.MapPath(nuevopath);
                // process.Start();
                Process.Start(nuevopath);
            }


        }

        #endregion

        #region INVITACION

        [HttpGet]
        public void InvitacionWord(CargaInvitacionWord oficioWord)
        {

            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            //tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["invitacion"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_INVITACION.docx");

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

        #region RESOLUCION

        [HttpGet]
        public void ResolucionWord(CargaResolucionWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["resolucion"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_RESOLUCION.docx");

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

        #region INFORME

        [HttpGet]
        public void InformeWord(CargaInformeWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["informe"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_INFORME.docx");

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

                string nuevopath = Path.Combine(path,"INFORME_"+fecha_PATH.ToString("ddMMyy")+".docx");
                stream.Close();
                System.IO.File.WriteAllBytes(nuevopath, stream.ToArray());

                //Process process = new Process();
                // process.StartInfo.FileName = Server.MapPath(nuevopath);
                // process.Start();
                Process.Start(nuevopath);
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
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["comunicado"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_COMUNICADO.docx");

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

        #region CARTA MULTIPLE

        [HttpGet]
        public void CartaMultipleWord(CargaCartaMultipleWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["cartamultiple"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_CARTA_MULTIPLE.docx");

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
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["oficiomultiple"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_OFICIO_MULTIPLE.docx");

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

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["memorando"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_MEMORANDO.docx");

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

                string nuevopath = Path.Combine(path, "MEMORANDO_" + fecha_PATH.ToString("ddMMyy") + ".docx");
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

        [HttpGet]
        public void MemorandoMultipleWord(CargaMemorandoMultipleWord tableData)
        {

            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["memorandomultiple"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_MEMORANDO_MULTIPLE.docx");

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

        #region CARTA

        [HttpGet]
        public void CartaWord(CargaCartaWord tableData)
        {
            DateTime fecha_PATH = DateTime.Now;

            DateTime fecha = DateTime.Now;
            tableData.FECHA_ACTUAL = fecha.ToString("dd MMMM yyyy");
            //desarrollo
            //string path  = @"C:\Users\PSSPERU069\Documents\Proyecto\sigesdoc_sanipes\sigesdoc\documentos externos";

            string path = ConfigurationManager.AppSettings["carta"];
            byte[] byteArray = System.IO.File.ReadAllBytes(path + @"/MODELO_DE_CARTA.docx");

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

    }
}
