//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.Request
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConsultarOficinaDireccionLegalRequest
    {
        public Nullable<int> id_oficina_direccion_legal { get; set; }
        public int id_sede { get; set; }
        public string ruc { get; set; }
        public string direccion { get; set; }
        public string ubigeo { get; set; }
        public string fecha_registro { get; set; }
        public string fecha_desactivado { get; set; }
        public Nullable<int> activo { get; set; }
        public string id_ubigeo { get; set; }
        public string nom_sede { get; set; }
        public string nom_direccion { get; set; }
        public string nom_referencia { get; set; }
    }
}
