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
    
    public partial class DAT_PROTOCOLO_EMBARCACION
    {
        public int ID_DET_PRO_HAB { get; set; }
        public Nullable<int> ID_PROTOCOLO { get; set; }
        public string NOM_EMBARCACION { get; set; }
        public Nullable<int> REPRESENTANTE_LEGAL { get; set; }
        public Nullable<int> DIRECCION_LEGAL { get; set; }
        public Nullable<int> ID_TIP_PRO_EMB { get; set; }
        public string RESOLUCION { get; set; }
        public string DIRECCION_PERSONA_NATURAL { get; set; }
        public Nullable<int> ID_PERSONA_TELEFONO { get; set; }
    
        public virtual MAE_PROTOCOLO MAE_PROTOCOLO { get; set; }
    }
}
