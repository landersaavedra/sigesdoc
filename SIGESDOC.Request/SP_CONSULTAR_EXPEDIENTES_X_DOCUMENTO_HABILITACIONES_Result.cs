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
    
    public partial class SP_CONSULTAR_EXPEDIENTES_X_DOCUMENTO_HABILITACIONES_Result
    {
        public int id_documento_seg { get; set; }
        public byte id_tipo_documento { get; set; }
        public Nullable<System.DateTime> fecha_crea { get; set; }
        public Nullable<System.DateTime> fecha_documento { get; set; }
        public string nombre_tipo_documento { get; set; }
        public string nombre_externo { get; set; }
        public string asunto { get; set; }
        public Nullable<int> num_documento { get; set; }
        public string nombre_documento { get; set; }
        public string evaluador { get; set; }
        public string expedientes { get; set; }
        public Nullable<System.DateTime> fecha_od { get; set; }
        public string codigo_habilitante { get; set; }
        public string ruta_pdf { get; set; }
        public string estado { get; set; }
        public string nom_oficina_crea { get; set; }
        public string usu_crea { get; set; }
    }
}
