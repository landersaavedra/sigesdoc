//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Request
{
    using System;
    
    public partial class P_UPDATE_MAE_OPERACION_Result
    {
        public int id_operacion { get; set; }
        public Nullable<int> numero { get; set; }
        public Nullable<System.DateTime> fecha_deposito { get; set; }
        public Nullable<int> oficina { get; set; }
        public Nullable<decimal> abono { get; set; }
        public Nullable<decimal> cargo { get; set; }
        public string usuario_crea { get; set; }
        public Nullable<System.DateTime> fecha_crea { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
        public string ruta_pdf { get; set; }
    }
}
