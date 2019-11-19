﻿using System;
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
using SIGESDOC.Response;
using SIGESDOC.Request;
using SIGESDOC.IAplicacionService;


namespace SIGESDOC.VSTO
{
    public partial class ThisDocument
    {
        private Microsoft.Office.Tools.Word.RichTextContentControl richTextControl2;

        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
            PlanillaDocumentoDHCPA(00000918181);
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
            this.Startup += new System.EventHandler(ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(ThisDocument_Shutdown);
        }

        private void PlanillaDocumentoDHCPA(int numerodocumento)
        {
            Word.Document document = this.Application.ActiveDocument;
            Document extendedDocument = Globals.Factory.GetVstoObject(document);

            this.Paragraphs[1].Range.InsertParagraphBefore();
            this.Paragraphs[1].Range.Select();

            AddRichTextControlAtSelection();
            richTextControl2.PlaceholderText = numerodocumento.ToString();

        }

        private void AddRichTextControlAtSelection()
        {

            if (this.Application.ActiveDocument == null)
                return;

            this.Paragraphs[1].Range.InsertParagraphBefore();
            this.Paragraphs[1].Range.Select();

            richTextControl2 = this.Controls.AddRichTextContentControl("richTextControl2");
        }

        #endregion
    }
}