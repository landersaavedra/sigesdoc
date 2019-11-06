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
    
    public partial class MAE_PROTOCOLO
    {
        public MAE_PROTOCOLO()
        {
            this.DAT_PROTOCOLO_ALMACEN = new HashSet<DAT_PROTOCOLO_ALMACEN>();
            this.DAT_PROTOCOLO_AUTORIZACION_INSTALACION = new HashSet<DAT_PROTOCOLO_AUTORIZACION_INSTALACION>();
            this.DAT_PROTOCOLO_CONCESION = new HashSet<DAT_PROTOCOLO_CONCESION>();
            this.DAT_PROTOCOLO_DESEMBARCADERO = new HashSet<DAT_PROTOCOLO_DESEMBARCADERO>();
            this.DAT_PROTOCOLO_EMBARCACION = new HashSet<DAT_PROTOCOLO_EMBARCACION>();
            this.DAT_PROTOCOLO_ESPECIE = new HashSet<DAT_PROTOCOLO_ESPECIE>();
            this.DAT_PROTOCOLO_LICENCIA_OPERACION = new HashSet<DAT_PROTOCOLO_LICENCIA_OPERACION>();
            this.DAT_PROTOCOLO_PLANTA = new HashSet<DAT_PROTOCOLO_PLANTA>();
            this.DAT_PROTOCOLO_TRANSPORTE = new HashSet<DAT_PROTOCOLO_TRANSPORTE>();
            this.MAE_ACTIVIDAD_PROTOCOLO = new HashSet<MAE_ACTIVIDAD_PROTOCOLO>();
        }
    
        public int ID_PROTOCOLO { get; set; }
        public Nullable<int> ID_SEGUIMIENTO { get; set; }
        public string NOMBRE { get; set; }
        public Nullable<System.DateTime> FECHA_INICIO { get; set; }
        public Nullable<System.DateTime> FECHA_FIN { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public string EVALUADOR { get; set; }
        public string IND_CONCHA_ABANICO { get; set; }
        public string IND_OTROS { get; set; }
        public string IND_PECES { get; set; }
        public string IND_CRUSTACEOS { get; set; }
        public Nullable<int> ID_TIPO_CH { get; set; }
        public string ACTIVO { get; set; }
        public Nullable<int> ID_IND_PRO_ESP { get; set; }
        public Nullable<int> ID_EST_PRO { get; set; }
        public Nullable<int> ID_PROTOCOLO_REEMPLAZA { get; set; }
    
        public virtual ICollection<DAT_PROTOCOLO_ALMACEN> DAT_PROTOCOLO_ALMACEN { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_AUTORIZACION_INSTALACION> DAT_PROTOCOLO_AUTORIZACION_INSTALACION { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_CONCESION> DAT_PROTOCOLO_CONCESION { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_DESEMBARCADERO> DAT_PROTOCOLO_DESEMBARCADERO { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_EMBARCACION> DAT_PROTOCOLO_EMBARCACION { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_ESPECIE> DAT_PROTOCOLO_ESPECIE { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_LICENCIA_OPERACION> DAT_PROTOCOLO_LICENCIA_OPERACION { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_PLANTA> DAT_PROTOCOLO_PLANTA { get; set; }
        public virtual MAE_TIPO_CONSUMO_HUMANO MAE_TIPO_CONSUMO_HUMANO { get; set; }
        public virtual ICollection<DAT_PROTOCOLO_TRANSPORTE> DAT_PROTOCOLO_TRANSPORTE { get; set; }
        public virtual ICollection<MAE_ACTIVIDAD_PROTOCOLO> MAE_ACTIVIDAD_PROTOCOLO { get; set; }
        public virtual MAE_SEGUIMIENTO_DHCPA MAE_SEGUIMIENTO_DHCPA { get; set; }
    }
}
