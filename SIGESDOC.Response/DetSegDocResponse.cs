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
    
    public partial class DetSegDocResponse
    {
        public int id_det_doc { get; set; }
        public int id_documento_seg { get; set; }
        public int id_seguimiento { get; set; }
        public string activo { get; set; }
    
        public virtual DocumentoSeguimientoResponse documento_seguimiento { get; set; }
        public virtual SeguimientoDhcpaResponse seguimiento_dhcpa { get; set; }
    }
}
