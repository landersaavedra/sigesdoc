//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Response
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProtocoloEspecieResponse
    {
        public int id_pro_espe { get; set; }
        public Nullable<int> id_protocolo { get; set; }
        public Nullable<int> id_det_espec_hab { get; set; }
        public string activo { get; set; }
    
        public virtual EspeciesHabilitacionesResponse especies_habilitaciones { get; set; }
        public virtual ProtocoloResponse protocolo { get; set; }
    }
}