//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class VW_CONSULTA_DB_GENERAL_MAE_OPERACION
    {
        public int ID_OPERACION { get; set; }
        public Nullable<int> NUMERO { get; set; }
        public Nullable<System.DateTime> FECHA_DEPOSITO { get; set; }
        public Nullable<int> OFICINA { get; set; }
        public Nullable<decimal> ABONO { get; set; }
        public Nullable<decimal> CARGO { get; set; }
        public string FACTURA { get; set; }
        public string USUARIO_CREA { get; set; }
        public Nullable<System.DateTime> FECHA_CREA { get; set; }
        public string USUARIO_MODIFICA { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICA { get; set; }
        public string RUTA_PDF { get; set; }
    }
}
