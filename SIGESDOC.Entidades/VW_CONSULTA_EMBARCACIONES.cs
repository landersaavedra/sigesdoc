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
    
    public partial class VW_CONSULTA_EMBARCACIONES
    {
        public int ID_EMBARCACION { get; set; }
        public string MATRICULA { get; set; }
        public string NOMBRE { get; set; }
        public Nullable<int> ID_TIPO_EMBARCACION { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public string USUARIO_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public string ACTIVO { get; set; }
        public Nullable<int> CODIGO_HABILITACION { get; set; }
        public Nullable<int> NUM_COD_HABILITACION { get; set; }
        public string NOM_COD_HABILITACION { get; set; }
        public Nullable<int> ID_TIPO_ACT_EMB { get; set; }
        public Nullable<System.DateTime> FECHA_CONSTRUCCION { get; set; }
    }
}
