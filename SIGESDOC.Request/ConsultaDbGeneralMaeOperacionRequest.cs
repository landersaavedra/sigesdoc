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
    
    public partial class ConsultaDbGeneralMaeOperacionRequest
    {
        public int id_operacion { get; set; }
        public Nullable<int> numero { get; set; }
        public Nullable<System.DateTime> fecha_deposito { get; set; }
        public Nullable<int> oficina { get; set; }
        public Nullable<decimal> abono { get; set; }
        public Nullable<decimal> cargo { get; set; }
        public string factura { get; set; }
        public string usuario_crea { get; set; }
        public Nullable<System.DateTime> fecha_crea { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string ruta_pdf { get; set; }
    }
}