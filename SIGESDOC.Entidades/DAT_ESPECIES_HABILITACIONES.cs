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
    
    public partial class DAT_ESPECIES_HABILITACIONES
    {
        public DAT_ESPECIES_HABILITACIONES()
        {
            this.DAT_PROTOCOLO_ESPECIE = new HashSet<DAT_PROTOCOLO_ESPECIE>();
        }
    
        public int ID_DET_ESPEC_HAB { get; set; }
        public string CODIGO_ESPECIE { get; set; }
        public string ACTIVO { get; set; }
    
        public virtual ICollection<DAT_PROTOCOLO_ESPECIE> DAT_PROTOCOLO_ESPECIE { get; set; }
    }
}
