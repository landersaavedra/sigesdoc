//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Request
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentoIndicadorRequest
    {
        public byte id_indicador_documento { get; set; }
        public string nombre { get; set; }
    
        public virtual List<DocumentoRequest> documento { get; set; }
    }
}
