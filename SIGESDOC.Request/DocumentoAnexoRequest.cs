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
    
    public partial class DocumentoAnexoRequest
    {
        public int id_documento_anexo { get; set; }
        public Nullable<int> id_documento { get; set; }
        public string ruta { get; set; }
        public string descripcion { get; set; }
        public string extension { get; set; }
        public string usuario_crea { get; set; }
        public Nullable<System.DateTime> fecha_crea { get; set; }
        public string activo { get; set; }
    
        public virtual DocumentoRequest documento { get; set; }
    }
}