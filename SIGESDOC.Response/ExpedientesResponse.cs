//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Response
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExpedientesResponse
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
    
        public virtual TipoExpedienteResponse tipo_expediente { get; set; }
    }
}
