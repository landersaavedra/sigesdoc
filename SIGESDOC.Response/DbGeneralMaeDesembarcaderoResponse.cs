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
    
    public partial class DbGeneralMaeDesembarcaderoResponse
    {
        public int id_desembarcadero { get; set; }
        public Nullable<int> id_sede { get; set; }
        public Nullable<int> id_tipo_desembarcadero { get; set; }
        public string entidad { get; set; }
        public string nombre_tipo_desembarcadero { get; set; }
        public string denominacion { get; set; }
        public Nullable<double> latitud { get; set; }
        public Nullable<double> longitud { get; set; }
        public string codigo_desembarcadero { get; set; }
        public string estado_desemb { get; set; }
    }
}