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
    
    public partial class DetalleMaeDocumentoResponse
    {
        public int id_documento { get; set; }
        public int oficina_destino { get; set; }
        public string observacion { get; set; }
        public Nullable<int> id_cab_det_documento { get; set; }
        public int oficina_crea { get; set; }
        public Nullable<bool> flag_destino_principal { get; set; }
        public Nullable<int> numero_documento { get; set; }
        public string nom_doc { get; set; }
        public string nombres { get; set; }
        public string asunto { get; set; }
    }
}