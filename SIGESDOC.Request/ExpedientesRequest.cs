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
    
    public partial class ExpedientesRequest
    {
        public int id_expediente { get; set; }
        public Nullable<int> numero_expediente { get; set; }
        public int id_tipo_expediente { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public string usuario_registro { get; set; }
        public Nullable<System.DateTime> fecha_modifico { get; set; }
        public string usuario_modifico { get; set; }
        public string indicador_seguimiento { get; set; }
        public string nom_expediente { get; set; }
        public Nullable<int> año_crea { get; set; }
    
        public virtual TipoExpedienteRequest tipo_expediente { get; set; }
    }
}
