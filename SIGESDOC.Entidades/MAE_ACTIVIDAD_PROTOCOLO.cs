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
    
    public partial class MAE_ACTIVIDAD_PROTOCOLO
    {
        public int ID_ACTIVIDAD_PROTO { get; set; }
        public Nullable<int> ID_PROTOCOLO { get; set; }
        public Nullable<int> ID_EST_PRO { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
    
        public virtual MAE_ESTADOS_DEL_PROTOCOLO MAE_ESTADOS_DEL_PROTOCOLO { get; set; }
        public virtual MAE_PROTOCOLO MAE_PROTOCOLO { get; set; }
    }
}