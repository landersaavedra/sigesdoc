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
    
    public partial class DAT_DET_SEG_EVALUADOR
    {
        public int ID_DET_EXP_EVA { get; set; }
        public Nullable<int> ID_SEGUIMIENTO { get; set; }
        public string EVALUADOR { get; set; }
        public string INDICADOR { get; set; }
        public Nullable<System.DateTime> FECHA_RECIBIDO { get; set; }
        public Nullable<System.DateTime> FECHA_DERIVADO { get; set; }
    
        public virtual MAE_SEGUIMIENTO_DHCPA MAE_SEGUIMIENTO_DHCPA { get; set; }
    }
}