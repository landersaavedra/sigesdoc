//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES
    {
        public int ID_SEG_DHCPA_OBSERVACION { get; set; }
        public Nullable<int> ID_SEGUIMIENTO { get; set; }
        public string OBSERVACION { get; set; }
        public string USUARIO_CREA { get; set; }
        public Nullable<System.DateTime> FECHA_CREA { get; set; }
        public string ACTIVO { get; set; }
    
        public virtual MAE_SEGUIMIENTO_DHCPA MAE_SEGUIMIENTO_DHCPA { get; set; }
    }
}
