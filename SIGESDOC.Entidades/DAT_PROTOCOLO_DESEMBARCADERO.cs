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
    
    public partial class DAT_PROTOCOLO_DESEMBARCADERO
    {
        public int ID_DET_PRO_DESEMB { get; set; }
        public Nullable<int> ID_PROTOCOLO { get; set; }
        public Nullable<int> ID_DESEMBARCADERO { get; set; }
        public string DERECHO_USO_AREA_ACUATICA { get; set; }
        public Nullable<int> DIRECCION_LEGAL { get; set; }
        public Nullable<int> REPRESENTANTE_LEGAL { get; set; }
    
        public virtual MAE_PROTOCOLO MAE_PROTOCOLO { get; set; }
    }
}
