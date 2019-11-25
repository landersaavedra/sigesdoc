using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SIGESDOC.Web.Models
{

    public class HojaTramiteViewModel
    {
        public string clave { get; set; }
        public string editar { get; set; }
        public string modifica_persona_externa { get; set; }
        
        public HttpPostedFileBase archivo_adjunto { get; set; }
        public byte id_indicador_documento { get; set; }
        public string referencia { get; set; }
        public string persona_crea { get; set; }
        public string ac_sin_original { get; set; }
        public string nombre_expediente { get; set; }
        public int tipo_expediente { get; set; }
        public Nullable<int> pedido_siga { get; set; }
        //HOJA DE TRAMITE
        [Display(Name = "Expediente:")]
        public int id_expediente { get; set; }
        public Nullable<int> id_tipo_pedido_siga { get; set; }
        public Nullable<int> anno_siga { get; set; }        

        [Display(Name = "Hoja de Trámite:")]
        public int numero { get; set; }

        [Display(Name = "Relacionado con:")]
        public string HT_PADRE { get; set; }
        public string Hoja_Tramite { get; set; }
        public int numero_HT { get; set; }
        // si es uno (01) es Externa si es cero (0) es interna
        public byte id_tipo_tramite { get; set; }
        public string nombre_tipo_tramite { get; set; }

        [Display(Name = "Oficina Externa:")]
        public Nullable<int> id_oficina { get; set; }
        public string nombre_oficina_tramite { get; set; }

        [Display(Name = "")]
        public string nom_doc { get; set; }

        [Display(Name = "Fecha de emisión:")]
        public System.DateTime fecha_emision { get; set; }
        public Nullable<int> num_documento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo Asunto es obligatorio")]
        [Display(Name = "Asunto:")]
        public string asunto { get; set; }
        public string nom_externo { get; set; }
        

        [Display(Name = "Usuario de emisión:")]
        public string usuario_emision { get; set; }

        //DOCUMENTO
        [Display(Name = "Id del Documento:")]
        public int id_documento { get; set; }

        [Display(Name = "Tipo de Documento:")]
        public byte id_tipo_documento { get; set; }

        public string nombre_tipo_documento_tramite { get; set; }
        public Nullable<int> id_tupa { get; set; }

        [Display(Name = "Número de Documento:")]
        public Nullable<int> numero_documento { get; set; }
        [Display(Name = "Anexos:")]
        public string anexos { get; set; }
        public string nom_oficina_crea { get; set; }
        

        
        /*
        [Display(Name = "Fecha del Documento:")]
        public System.DateTime fecha_documento { get; set; }

        [Display(Name = "Clasificación Trámite:")]
        public byte id_clasificacion_tramite { get; set; }

        */

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingresar cantidad de folios")]
        [Display(Name = "Folios:")]
        public int folios { get; set; }
        [Display(Name = "Oficina que crea el documento:")]
        public int oficina_crea { get; set; }
        [Display(Name = "Fecha de creación del documento:")]
        public System.DateTime fecha_envio { get; set; }
        
        public string usuario_crea { get; set; }
        
        public System.DateTime fecha_crea { get; set; }

        public ICollection<DocumentoDetalleViewModel> documento_detalle { get; set; }

        public string persona_num_documento { get; set; }
        public byte tipo_per { get; set; }
        

        //grid consulta

        public int id_oficina_direccion { get; set; }
        public string ruc { get; set; }
        public string nom_oficina { get; set; }
        public string siglas { get; set; }
        public string direccion { get; set; }



    

    }
    
    public class SeguimientoViewModel
    {
        public byte id_tipo_documento { get; set; }
        public int id_seguimiento { get; set; }
        public Nullable<int> tupa { get; set; }
        public Nullable<int> id_tipo_procedimiento { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public string protocolo { get; set; }
        public Nullable<int> id_protocolo { get; set; }
        public Nullable<int> id_ofi_dir { get; set; }
        public string persona_num_documento { get; set; }
        public int id_tipo_seguimiento { get; set; }
        public int id_habilitante { get; set; }
        public string cod_habilitante { get; set; }
        public string estado { get; set; }
        public DateTime fecha_crea_Seguimiento { get; set; }
        public string usuario_crea_Seguimiento { get; set; }

        public int id_expediente_seg { get; set; }
        public int id_servicio_dhcpa { get; set; }
        public string fecha_recibido_od { get; set; }


        public int id_doc_seg { get; set; }
        public Nullable<int> num_documento { get; set; }
        public string nom_documento { get; set; }
        public string asunto { get; set; }
        public string fecha_documento { get; set; }
        public string fecha_recep_otd { get; set; }
        public string evaluador { get; set; }
        public System.DateTime fecha_recibido_evaluador { get; set; }
        public string indicador { get; set; }
        public int folios { get; set; }
        public string nombre_externo { get; set; }
        public string nom_oficina_crea { get; set; }

        public ICollection<DetDocFactViewModel> det_fac_doc { get; set; } 
        public ICollection<detsegexpViewModel> det_seg_exp { get; set; }
        public ICollection<detsegpadreViewModel> det_seg_padre { get; set; }

    }

    public class DetDocFactViewModel
    {
        public int id_det_doc_fact { get; set; }
        public Nullable<int> id_documento_seg { get; set; }
        public Nullable<int> id_factura { get; set; }
        public int num1_fact { get; set; }
        public int num2_fact { get; set; }
        public string fecha_fact { get; set; }
        public string importe_total { get; set; }
        public DateTime fecha_crea_Factura { get; set; }
        public string usuario_crea_Factura { get; set; }

    }

    public class detsegexpViewModel
    {
        public int num_expediente { get; set; }
        public int id_tipo_expediente { get; set; }
        public string expediente { get; set; }
        public int id_expediente { get; set; }
        public DateTime fecha_crea_Expediente { get; set; }
        public string usuario_crea_Expediente { get; set; }
    }

    public class detsegpadreViewModel    
    {
        public int id_seguimiento { get; set; }
        public string seguimiento { get; set; }
    }


    public class DocumentoDetalleViewModel
    {

        [Display(Name = "Id del detalle del documento:")]
        public int id_det_documento { get; set; }
        [Display(Name = "Id del documento:")]
        public int id_documento { get; set; }
        [Display(Name = "Id de la cabecera del detalle del documento:")]
        public int id_cab_det_documento { get; set; }
        [Display(Name = "Id de la Oficina del destino:")]
        public int oficina_destino { get; set; }
        [Display(Name = "Observación:")]
        public string observacion { get; set; }

        public System.DateTime fecha_crea { get; set; }
        public string usuario_crea { get; set; }

        public int oficina_crea { get; set; }


        public Nullable<System.DateTime> fecha_recepcion { get; set; }
        public string usuario_recepcion { get; set; }
        public Nullable<System.DateTime> fecha_derivado { get; set; }
        public string usuario_derivado { get; set; }
        public Nullable<System.DateTime> fecha_atendido { get; set; }
        public string usuario_atendido { get; set; }
        public Nullable<System.DateTime> fecha_archivo { get; set; }
        public string usuario_archivo { get; set; }
        [Display(Name = "Estado del Trámite:")]
        public byte id_est_tramite { get; set; }
        [Display(Name = "Persona encargada:")]
        public string persona_num_documento { get; set; }

        [Display(Name = "ACCIÓN NECESARIA")]
        public Nullable<bool> ind_01 { get; set; }

        [Display(Name = "CONOCIMIENTO")]
        public Nullable<bool> ind_02 { get; set; }

        [Display(Name = "ATENDER")]
        public Nullable<bool> ind_03 { get; set; }

        [Display(Name = "CONVERSAR")]
        public Nullable<bool> ind_04 { get; set; }

        [Display(Name = "INFORMAR")]
        public Nullable<bool> ind_05 { get; set; }

        [Display(Name = "COORDINAR CON")]
        public Nullable<bool> ind_06 { get; set; }

        [Display(Name = "RESPONDER")]
        public Nullable<bool> ind_07 { get; set; }

        [Display(Name = "ARCHIVAR")]
        public Nullable<bool> ind_08 { get; set; }

        [Display(Name = "REVISAR")]
        public Nullable<bool> ind_09 { get; set; }

        [Display(Name = "DEVOLVER")]
        public Nullable<bool> ind_10 { get; set; }

        [Display(Name = "TRAMITE")]
        public Nullable<bool> ind_11 { get; set; }
        public string nom_oficina_crea { get; set; }
        public string nom_oficina_destino { get; set; }
        public string indicadores { get; set; }
        public string usuario_cancelar { get; set; }
        public Nullable<System.DateTime> fecha_cancelar { get; set; }

    }
    
    public class ConsultarUsuarioViewModel
    {
        public string ruc { get; set; }
        public string persona_num_documento { get; set; }
        
        [DataType(DataType.Password)]
        public string clave { get; set; }
        public int id_perfil { get; set; }
        public string empresa { get; set; }
        public string persona { get; set; }
        public string perfil { get; set; }
    }

    public class ConsultarDniViewModel
    {
        public string persona_num_documento { get; set; } // VARCHAR 15
        [Display(Name = "Tipo de Documento")]
        public int tipo_doc_iden { get; set; }
        [Display(Name = "Apellido Paterno")]
        public string paterno { get; set; } // VARCHAR(50)
        [Display(Name = "Apellido Materno")]
        public string materno { get; set; } // VARCHAR(50)
        [Display(Name = "Nombres")]
        public string nombres { get; set; } // VARCHAR(50)
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime fecha_nacimiento { get; set; } 
        public string ubigeo { get; set; } // CHAR (6)
        [Display(Name = "Sexo")]
        public string sexo { get; set; } // F - M
        [Display(Name = "Dirección")]
        public string direccion { get; set; } // VARCHAR(200)
        [Display(Name = "RUC")]
        public string ruc { get; set; } // VARCHAR(11)
        [Display(Name = "NOMBRE TIPO DOCUMENTO")]
        public string nom_tipo_doc { get; set; } // VARCHAR(100)                
    }

    public class ConsultarOficinaViewModel
    {
        [Display(Name = "RUC")]
        public string RUC { get; set; }
        [Display(Name = "Siglas")]
        public string SIGLAS { get; set; }
        [Display(Name = "Sede principal")]
        public string NOMBRE_SEDE { get; set; }
        [Display(Name = "Dirección")]
        public string DIRECCION { get; set; }
        [Display(Name = "Referencia")]
        public string REFERENCIA { get; set; }
        public string UBIGEO { get; set; }

    }
    
    public class DocumentodhcpaViewModel
    {

        public int id_doc_dhcpa { get; set; }
        public int id_tipo_documento { get; set; }
        public string nom_tipo_documento { get; set; }

        [Display(Name = "Número de Documento:")]
        public int num_doc { get; set; }
        public string nom_doc { get; set; }
        public System.DateTime fecha_doc { get; set; }
        [Display(Name = "Asunto:")]
        public string asunto { get; set; }
        [Display(Name = "Anexos:")]
        public string anexos { get; set; }
        public int id_archivador { get; set; }
        public int id_filial { get; set; }
        public int numero_ht { get; set; }

        public Nullable<int> id_oficina_direccion { get; set; }

        public string ruc { get; set; } 
        public string evaluador_cdl_notif { get; set; }
        public string direccion_cdl_notif { get; set; }
        public string empresa_cdl_notif { get; set; }
        public string folia_cdl_notif { get; set; }
        public string doc_notificar_cdl_notif { get; set; }
        public string exp_o_ht_cdl_notif { get; set; }
        public string exp_o_ht_n_cdl_notif { get; set; }
        public int ind_agregar_celula { get; set; }
        public ICollection<detDocdhcpaViewModel> documento_dhcpa_detalle { get; set; }
        public ICollection<detdocdhcpasegViewModel> documento_dhcpa_seguimiento { get; set; }

    }

    public class detDocdhcpaViewModel
    {
        public int id_doc_dhcpa_det { get; set; }
        public int id_doc_dhcpa { get; set; }
        public int id_oficina_direccion { get; set; }
        public string persona_destino { get; set; }
        
    }

    public class detdocdhcpasegViewModel
    {
        public int id_det_dsdhcpa { get; set; }
        public int id_doc_dhcpa { get; set; }
        public int id_seguimiento { get; set; }
    }

    public class ProtocoloViewModel
    {
        public int id_protocolo { get; set; }
        public int id_seguimiento { get; set; }
        public string nombre { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public System.DateTime fecha_fin { get; set; }
        public string evaluador { get; set; }
        public string ind_concha_abanico { get; set; }
        public string ind_otros { get; set; }
        public string ind_peces { get; set; }
        public string ind_crustaceos { get; set; }
        public Nullable<int> id_tipo_ch { get; set; }
        
    }

    public class SolicitudInspeccionViewModel
    {

        public int id_sol_ins { get; set; }
        public Nullable<int> id_seguimiento { get; set; }
        public Nullable<int> numero_documento { get; set; }
    
    }

}
    