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
    
    public partial class MAE_PRUEBA_INSPECCION_DSFPA
    {
        public int ID_PRUEBA_INSP { get; set; }
        public Nullable<int> ID_SOL_INS { get; set; }
        public string USUARIO_CARGA { get; set; }
        public Nullable<int> USUARIO_OFICINA { get; set; }
        public Nullable<System.DateTime> FECHA_CARGA { get; set; }
        public string ACTIVO { get; set; }
        public string RUTA_PDF { get; set; }
        public string INSPECTOR { get; set; }
    
        public virtual MAE_SOLICITUD_INSPECCION MAE_SOLICITUD_INSPECCION { get; set; }
    }
}
