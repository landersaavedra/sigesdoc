using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace SIGESDOC.VSTO_SANIPES
{
    public partial class ThisDocument
    {
        //private Microsoft.Office.Tools.Word.RichTextContentControl richTextControl2;

        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
            //PlanillaDocumentoDHCPA(90918181);
        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region Código generado por el Diseñador de VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.A_DOC_NOTIFICAR_CDL_NOTIF1.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(this.A_DOC_NOTIFICAR_CDL_NOTIF1_SelectionChange);
            this.Startup += new System.EventHandler(this.ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(this.ThisDocument_Shutdown);

        }

        private void A_DOC_NOTIFICAR_CDL_NOTIF1_SelectionChange(object sender, SelectionEventArgs e)
        {

        }

        //private void PlanillaDocumentoDHCPA(int numerodocumento)
        //{
        //    Word.Document document = this.Application.ActiveDocument;
        //    Document extendedDocument = Globals.Factory.GetVstoObject(document);

        //    this.Paragraphs[1].Range.InsertParagraphBefore();
        //    this.Paragraphs[1].Range.Select();



        //}

        //private void AddRichTextControlAtSelection()
        //{

        //    if (this.Application.ActiveDocument == null)
        //        return;

        //    this.Paragraphs[1].Range.InsertParagraphBefore();
        //    this.Paragraphs[1].Range.Select();

        //    richTextControl2 = this.Controls.AddRichTextContentControl("richTextControl2");
        //}

        #endregion

        //private void richTextContentControl1_Entering(object sender, ContentControlEnteringEventArgs e)
        //{
        //    this.richTextContentControl1.PlaceholderText = "Black eyes Peas";
        //}

        //private void richTextContentControl2_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl3_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl4_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl5_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl6_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl7_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl8_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl9_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl10_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl11_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl12_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl13_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl14_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //private void richTextContentControl15_Entering(object sender, ContentControlEnteringEventArgs e)
        //{

        //}

        //public void RellenarRichText(string datos)
        //{

        //}
    }
}