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
    
    public partial class SubTupaRequest
    {
        public int id_sub_tupa { get; set; }
        public Nullable<int> id_tupa { get; set; }
        public string indice { get; set; }
        public string nombre { get; set; }
        public Nullable<decimal> precio { get; set; }
        public string activo { get; set; }
        public Nullable<int> indicador { get; set; }
    
        public virtual TupaRequest tupa { get; set; }
    }
}
