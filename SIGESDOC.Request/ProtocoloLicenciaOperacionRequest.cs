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
    
    public partial class ProtocoloLicenciaOperacionRequest
    {
        public int id_pro_licencia_operacion { get; set; }
        public Nullable<int> id_protocolo { get; set; }
        public string resolucion_autorizacion_instalacion { get; set; }
        public Nullable<System.DateTime> fecha_resolucion { get; set; }
        public string ruc { get; set; }
        public Nullable<int> id_sede { get; set; }
        public Nullable<int> id_representante_legal { get; set; }
        public Nullable<int> id_tipo_licencia_operacion { get; set; }
        public string actividad { get; set; }
    
        public virtual ProtocoloRequest protocolo { get; set; }
        public virtual TipoLicenciaOperacionRequest tipo_licencia_operacion { get; set; }
    }
}
