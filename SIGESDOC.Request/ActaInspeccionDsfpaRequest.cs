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
    
    public partial class ActaInspeccionDsfpaRequest
    {
        public int id_acta_insp { get; set; }
        public Nullable<int> id_sol_ins { get; set; }
        public string nombre_acta { get; set; }
        public string usuario_carga { get; set; }
        public Nullable<int> usuario_oficina { get; set; }
        public Nullable<System.DateTime> fecha_carga { get; set; }
        public string activo { get; set; }
        public string ruta_pdf { get; set; }
        public string inspector { get; set; }
    
        public virtual SolicitudInspeccionRequest solicitud_inspeccion { get; set; }
    }
}
