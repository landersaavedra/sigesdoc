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
    
    public partial class MAE_TIPO_PROCEDIMIENTO
    {
        public MAE_TIPO_PROCEDIMIENTO()
        {
            this.MAE_TUPA = new HashSet<MAE_TUPA>();
        }
    
        public int ID_TIPO_PROCEDIMIENTO { get; set; }
        public string NOMBRE { get; set; }
    
        public virtual ICollection<MAE_TUPA> MAE_TUPA { get; set; }
    }
}