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
    
    public partial class DAT_DET_DOC_FACT
    {
        public int ID_DET_DOC_FACT { get; set; }
        public Nullable<int> ID_DOCUMENTO_SEG { get; set; }
        public Nullable<int> ID_FACTURA { get; set; }
        public string ACTIVO { get; set; }
    
        public virtual MAE_DOCUMENTO_SEGUIMIENTO MAE_DOCUMENTO_SEGUIMIENTO { get; set; }
    }
}
