//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGESDOC.IRepositorio
{
    using System;
    
    public partial class P_ASIGNA_OFICINA_PERSONA_Result
    {
        public int id_per_empresa { get; set; }
        public string persona_num_documento { get; set; }
        public bool activo { get; set; }
        public int id_oficina_direccion { get; set; }
        public string usuario_crea { get; set; }
        public Nullable<System.DateTime> fecha_crea { get; set; }
        public string usuario_modifica { get; set; }
        public Nullable<System.DateTime> fecha_modifica { get; set; }
    }
}
