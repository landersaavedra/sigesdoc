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
    
    public partial class VW_CONSULTAR_DNI_PERSONAL_LEGAL
    {
        public int ID_DNI_PERSONA_LEGAL { get; set; }
        public string DNI { get; set; }
        public string DOCUMENTO { get; set; }
        public string NOMBRES_Y_APELLIDOS { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string FECHA_REGISTRO { get; set; }
        public string FECHA_DESACTIVADO { get; set; }
        public Nullable<int> ACTIVO { get; set; }
    }
}