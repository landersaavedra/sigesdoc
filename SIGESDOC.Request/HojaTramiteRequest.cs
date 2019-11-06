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
    
    public partial class HojaTramiteRequest
    {
        public int numero { get; set; }
        public byte id_tipo_tramite { get; set; }
        public Nullable<int> id_oficina { get; set; }
        public System.DateTime fecha_emision { get; set; }
        public string usuario_emision { get; set; }
        public string asunto { get; set; }
        public string persona_num_documento { get; set; }
        public Nullable<byte> tipo_per { get; set; }
        public string hoja_tramite { get; set; }
        public int id_expediente { get; set; }
        public Nullable<int> numero_padre { get; set; }
        public string ruta_pdf { get; set; }
        public string referencia { get; set; }
        public string editar { get; set; }
        public Nullable<int> pedido_siga { get; set; }
        public Nullable<int> id_tipo_pedido_siga { get; set; }
        public Nullable<int> anno_siga { get; set; }
        public string clave { get; set; }
        public Nullable<int> id_tupa { get; set; }
        public string nombre_externo { get; set; }
    
        public virtual TipoTramiteRequest tipo_tramite { get; set; }
        public virtual List<DocumentoRequest> documento { get; set; }
    }
}
