//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Response
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentoDhcpaDetalleResponse
    {
        public int id_doc_dhcpa_det { get; set; }
        public Nullable<int> id_doc_dhcpa { get; set; }
        public Nullable<int> id_oficina_direccion_destino { get; set; }
        public string persona_destino { get; set; }
        public string activo { get; set; }
        public string usuario_registro { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string nombre_persona_destino { get; set; }
    
        public virtual DocumentoDhcpaResponse documento_dhcpa { get; set; }
    }
}
