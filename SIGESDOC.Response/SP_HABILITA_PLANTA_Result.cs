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
    
    public partial class SP_HABILITA_PLANTA_Result
    {
        public int id_planta { get; set; }
        public Nullable<int> id_sede_oficina { get; set; }
        public Nullable<int> id_tipo_planta { get; set; }
        public Nullable<int> numero_planta { get; set; }
        public string nombre_planta { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public string usuario_registro { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
        public string usuario_modificacion { get; set; }
        public string activo { get; set; }
        public Nullable<int> id_tipo_actividad { get; set; }
        public Nullable<int> id_filial { get; set; }
        public Nullable<System.DateTime> fecha_habilitacion_final { get; set; }
        public Nullable<int> ind_habilitacion { get; set; }
    }
}
