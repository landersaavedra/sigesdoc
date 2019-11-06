﻿//------------------------------------------------------------------------------
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

    public partial class SeguimientoDhcpaResponse
    {
        public string ruc { get; set; }
        public string Expediente { get; set; }
        public string id_tipo_expediente { get; set; }
        public string nom_tipo_expediente { get; set; }
        public string nom_oficina_ext { get; set; }
        public string nom_persona_ext { get; set; }
        public string nom_direccion_ext { get; set; }
        public int id_sede_ext { get; set; }
        public string nom_tipo_procedimiento { get; set; }
        public string nom_embarcacion { get; set; }
        public string nom_estado { get; set; }
        public string nom_planta { get; set; }
        public string codigo_planta { get; set; }
        public string nom_actividad { get; set; }
        public string nom_filial { get; set; }
        public string nom_evaluador { get; set; }
        public int? num_tupa { get; set; }
        public bool cond_expediente { get; set; }
        public bool cond_finalizar { get; set; }
        public string matricula { get; set; }
        public Nullable<int> id_tipo_ser_hab { get; set; }
        public string num_tupa_cadena { get; set; }
        public string nom_tipo_tupa { get; set; }
        public string codigo_habilitacion { get; set; }
        public string excel_oficina_crea { get; set; }
        public string excel_usuario_crea { get; set; }
        public string excel_sede_crea { get; set; }
        

        public bool cond_habilitante { get; set; }
        
        public string num_solicitud_dhcpa { get; set; }
        public DateTime? fecha_solicitud_dhcpa { get; set; }
        public bool cond_no_tiene_expediente { get; set; }
        public DateTime? fecha_recepcion_evaluador { get; set; }
        public string asunto { get; set; }

        // DATOS ENTIDAD
        public string Nom_direccion_legal { get; set; }
        public int? id_direccion_legal { get; set; }
        public string Nom_persona_legal { get; set; }
        public int? id_persona_legal { get; set; }
        public string telefono_legal { get; set; }
        public string correo_legal { get; set; }

        // DATOS PERSONA NATURAL
        public string str_direccion_persona_natural { get; set; }
        public string Nom_persona_legal_DNI { get; set; }
        public int? id_dni_persona_legal { get; set; }
        public string telefono_legal_DNI { get; set; }
        public string correo_legal_DNI { get; set; }


        // TIPOS SEGUIMIENTO

        public bool cond_planta { get; set; }
        public bool cond_embarcacion { get; set; }
        public string nom_tipo_seguimiento { get; set; }

        //DATOS PARA EXCEL SOLICITUD

        public string excel_documento_resolutivo { get; set; }
        public string excel_ini_vigencia { get; set; }
        public string excel_fin_vigencia { get; set; }
        public string excel_fecha_emision { get; set; }

    }
}