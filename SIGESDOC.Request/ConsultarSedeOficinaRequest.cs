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
    
    public partial class ConsultarSedeOficinaRequest
    {
        public int id_sede { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public string ubigeo { get; set; }
        public bool activo { get; set; }
        public int id_oficina { get; set; }
    }
}
