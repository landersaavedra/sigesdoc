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
    
    public partial class ProtocoloDesembarcaderoRequest
    {
        public int id_det_pro_desemb { get; set; }
        public Nullable<int> id_protocolo { get; set; }
        public Nullable<int> id_desembarcadero { get; set; }
        public string derecho_uso_area_acuatica { get; set; }
        public Nullable<int> direccion_legal { get; set; }
        public Nullable<int> representante_legal { get; set; }
    
        public virtual ProtocoloRequest protocolo { get; set; }
    }
}