//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class MAE_HOJA_TRAMITE
    {
        public MAE_HOJA_TRAMITE()
        {
            this.MAE_DOCUMENTO = new HashSet<MAE_DOCUMENTO>();
        }
    
        public int NUMERO { get; set; }
        public byte ID_TIPO_TRAMITE { get; set; }
        public Nullable<int> ID_OFICINA { get; set; }
        public System.DateTime FECHA_EMISION { get; set; }
        public string USUARIO_EMISION { get; set; }
        public string ASUNTO { get; set; }
        public string persona_num_documento { get; set; }
        public Nullable<byte> TIPO_PER { get; set; }
        public string HOJA_TRAMITE { get; set; }
        public int ID_EXPEDIENTE { get; set; }
        public Nullable<int> NUMERO_PADRE { get; set; }
        public string RUTA_PDF { get; set; }
        public string REFERENCIA { get; set; }
        public string EDITAR { get; set; }
        public Nullable<int> PEDIDO_SIGA { get; set; }
        public Nullable<int> ID_TIPO_PEDIDO_SIGA { get; set; }
        public Nullable<int> ANNO_SIGA { get; set; }
        public string CLAVE { get; set; }
        public Nullable<int> ID_TUPA { get; set; }
        public string NOMBRE_EXTERNO { get; set; }
    
        public virtual MAE_TIPO_TRAMITE MAE_TIPO_TRAMITE { get; set; }
        public virtual ICollection<MAE_DOCUMENTO> MAE_DOCUMENTO { get; set; }
    }
}
