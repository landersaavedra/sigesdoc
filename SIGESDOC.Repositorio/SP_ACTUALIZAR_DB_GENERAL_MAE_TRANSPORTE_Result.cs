//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Repositorio
{
    using System;
    
    public partial class SP_ACTUALIZAR_DB_GENERAL_MAE_TRANSPORTE_Result
    {
        public int id_transporte { get; set; }
        public string placa { get; set; }
        public string cod_habilitacion { get; set; }
        public Nullable<int> id_tipo_carroceria { get; set; }
        public string nombre_carroceria { get; set; }
        public Nullable<int> id_tipo_furgon { get; set; }
        public string nombre_furgon { get; set; }
        public Nullable<int> id_um { get; set; }
        public string nombre_um { get; set; }
        public string siglas_um { get; set; }
        public Nullable<decimal> carga_util { get; set; }
        public string estado { get; set; }
        public string nombre_estado { get; set; }
    }
}
