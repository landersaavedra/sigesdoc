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
    
    public partial class ConsultarUsuarioRequest
    {
        public string ruc { get; set; }
        public string persona_num_documento { get; set; }
        public int id_perfil { get; set; }
        public string empresa { get; set; }
        public string razon_social { get; set; }
        public string persona { get; set; }
        public string perfil { get; set; }
        public int id_oficina_direccion { get; set; }
        public int id_oficina { get; set; }
        public string nom_ofi { get; set; }
        public int id_sede { get; set; }
        public string nom_sede { get; set; }
        public Nullable<int> id_perfil_jefe_od { get; set; }
        public Nullable<int> id_perfil_inspector_od { get; set; }
    }
}
