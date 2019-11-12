//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class MAE_DOCUMENTO_DHCPA
    {
        public MAE_DOCUMENTO_DHCPA()
        {
            this.DAT_DET_SEG_DOC_DHCPA = new HashSet<DAT_DET_SEG_DOC_DHCPA>();
            this.DAT_DOCUMENTO_DHCPA_DETALLE = new HashSet<DAT_DOCUMENTO_DHCPA_DETALLE>();
        }
    
        public int ID_DOC_DHCPA { get; set; }
        public Nullable<int> ID_TIPO_DOCUMENTO { get; set; }
        public int NUM_DOC { get; set; }
        public string NOM_DOC { get; set; }
        public Nullable<System.DateTime> FECHA_DOC { get; set; }
        public string ASUNTO { get; set; }
        public string ANEXOS { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public string USUARIO_REGISTRO { get; set; }
        public Nullable<int> ID_ARCHIVADOR { get; set; }
        public Nullable<int> ID_FILIAL { get; set; }
        public Nullable<int> NUMERO_HT { get; set; }
        public string PDF { get; set; }
        public Nullable<int> ID_OFICINA_DIRECCION { get; set; }
        public string EVALUADOR_CDL_NOTIF { get; set; }
        public string DIRECCION_CDL_NOTIF { get; set; }
        public string EMPRESA_CDL_NOTIF { get; set; }
        public Nullable<int> FOLIA_CDL_NOTIF { get; set; }
        public string DOC_NOTIFICAR_CDL_NOTIF { get; set; }
        public string EXP_O_HT_CDL_NOTIF { get; set; }
        public string EXP_O_HT_N_CDL_NOTIF { get; set; }
    
        public virtual ICollection<DAT_DET_SEG_DOC_DHCPA> DAT_DET_SEG_DOC_DHCPA { get; set; }
        public virtual ICollection<DAT_DOCUMENTO_DHCPA_DETALLE> DAT_DOCUMENTO_DHCPA_DETALLE { get; set; }
    }
}
