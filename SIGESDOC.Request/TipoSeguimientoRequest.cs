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
    
    public partial class TipoSeguimientoRequest
    {
        public int id_tipo_seguimiento { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
    
        public virtual List<TipoDocumentoSeguimientoAdjuntoRequest> tipo_documento_seguimiento_adjunto { get; set; }
    }
}