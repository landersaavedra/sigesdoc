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
    
    public partial class TipoLicenciaOperacionResponse
    {
        public int id_tipo_licencia_operacion { get; set; }
        public string nombre { get; set; }
        public string ruta_pdf { get; set; }
        public string activo { get; set; }
    
        public virtual List<ProtocoloLicenciaOperacionResponse> protocolo_licencia_operacion { get; set; }
    }
}