using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGESDOC.Request;

namespace SIGESDOC.Web.Models
{
    public class ModelToRequest
    {


        public static HojaTramiteRequest HojaTramite(HojaTramiteViewModel model)
        {
            HojaTramiteRequest item = new HojaTramiteRequest
            {
                numero = model.numero,
                id_tipo_tramite = model.id_tipo_tramite,
                id_oficina = model.id_oficina,
                //id_clasificacion_tramite = model.id_clasificacion_tramite,
                fecha_emision = model.fecha_emision,
                usuario_emision = model.usuario_emision,
                persona_num_documento = model.persona_num_documento,
                tipo_per = model.tipo_per,
                asunto = model.asunto,
                referencia = model.referencia,
                id_expediente = model.id_expediente,
                editar = model.editar,
                pedido_siga = model.pedido_siga,
                id_tipo_pedido_siga = model.id_tipo_pedido_siga,
                anno_siga = model.anno_siga,
                clave = model.clave,
                id_tupa= model.id_tupa,
                nombre_externo = model.nom_externo
            };
            
            return item;
        }

        public static DocumentoRequest documento(HojaTramiteViewModel model)
        {
            DocumentoRequest item = new DocumentoRequest
            {
                id_documento = model.id_documento,
                numero = model.numero,
                id_tipo_documento = model.id_tipo_documento,
                numero_documento = model.numero_documento,
                anexos = model.anexos,
                //fecha_documento = model.fecha_documento,
                folios = model.folios,
                oficina_crea = model.oficina_crea,
                fecha_envio = model.fecha_envio,
                usuario_crea = model.usuario_crea,
                nom_doc = model.nom_doc,
                persona_crea = model.persona_crea,
                id_indicador_documento = model.id_indicador_documento,
                nom_oficina_crea = model.nom_oficina_crea
            };

            return item;

        }
        
        public static DocumentoDetalleRequest DocumentoDetalle(DocumentoDetalleViewModel model)
        {
            DocumentoDetalleRequest item = new DocumentoDetalleRequest
            {
                id_det_documento = model.id_det_documento,
                id_documento = model.id_documento,
                id_cab_det_documento = model.id_cab_det_documento,
                oficina_crea = model.oficina_crea,
                oficina_destino = model.oficina_destino,
                observacion = model.observacion,
                fecha_recepcion = model.fecha_recepcion,
                usuario_recepcion = model.usuario_recepcion,
                fecha_derivado = model.fecha_derivado,
                usuario_derivado = model.usuario_derivado,
                fecha_atendido = model.fecha_atendido,
                usuario_atendido = model.usuario_atendido,
                fecha_archivo = model.fecha_archivo,
                usuario_archivo = model.usuario_archivo,
                fecha_crea = model.fecha_crea,
                usuario_crea = model.usuario_crea,
                id_est_tramite = model.id_est_tramite,
                persona_num_documento = model.persona_num_documento,
                ind_01 = model.ind_01,
                ind_02 = model.ind_02,
                ind_03 = model.ind_03,
                ind_04 = model.ind_04,
                ind_05 = model.ind_05,
                ind_06 = model.ind_06,
                ind_07 = model.ind_07,
                ind_08 = model.ind_08,
                ind_09 = model.ind_09,
                ind_10 = model.ind_10,
                ind_11 = model.ind_11,
                indicadores = model.indicadores,
                usuario_cancelar = model.usuario_cancelar,
                fecha_cancelar = model.fecha_cancelar,
                nom_oficina_crea = model.nom_oficina_crea
            };
            return item;
        }
        
        public static SeguimientoDhcpaRequest Seguimiento_dhcpa(SeguimientoViewModel model)
        {
            SeguimientoDhcpaRequest item = new SeguimientoDhcpaRequest
            {
                id_seguimiento = model.id_seguimiento,
                id_expediente = model.id_expediente_seg,
                tupa = model.tupa,
                id_tipo_procedimiento = model.id_tipo_procedimiento,
                fecha_inicio = model.fecha_inicio,
                fecha_fin = model.fecha_fin,
                id_ofi_dir = model.id_ofi_dir,
                persona_num_documento = model.persona_num_documento,
                id_tipo_seguimiento = model.id_tipo_seguimiento,
                estado = model.estado,
                evaluador = model.evaluador,
                id_habilitante = model.id_habilitante,
                cod_habilitante = model.cod_habilitante,
                nombre_externo = model.nombre_externo,
                nom_oficina_crea = model.nom_oficina_crea
            };

            return item;
        }
        
        public static DocumentoSeguimientoRequest Documento_Seguimiento(SeguimientoViewModel model)
        {
            DocumentoSeguimientoRequest item = new DocumentoSeguimientoRequest
            {
                id_documento_seg = model.id_doc_seg,
                id_tipo_documento = model.id_tipo_documento,
                num_documento = model.num_documento,
                nom_documento = model.nom_documento,
                asunto = model.asunto,
                fecha_crea = model.fecha_crea_Seguimiento,
                usuario_crea = model.usuario_crea_Seguimiento,
                evaluador = model.evaluador,
                fecha_recibido_evaluador = model.fecha_recibido_evaluador,
                estado = model.estado,
                indicador = model.indicador,
                id_servicio_dhcpa = model.id_servicio_dhcpa,
                folios = model.folios,
                nom_ofi_crea = model.nom_oficina_crea
            };

            return item;
        }
        
        public static DetDocFactRequest Documento_Factura(DetDocFactViewModel model)
        {
            DetDocFactRequest item = new DetDocFactRequest
            {
                id_det_doc_fact = model.id_det_doc_fact,
                id_documento_seg = model.id_documento_seg,
                id_factura = model.id_factura
            };
            return item;
        }

        public static DocumentoDhcpaRequest Documento_dhcpa(DocumentodhcpaViewModel model)
        {
            DocumentoDhcpaRequest item = new DocumentoDhcpaRequest
            {
                id_doc_dhcpa = model.id_doc_dhcpa,
                id_tipo_documento = model.id_tipo_documento,
                num_doc = model.num_doc,
                nom_doc = model.nom_doc,
                fecha_doc = model.fecha_doc,
                asunto = model.asunto,
                anexos = model.anexos,
                id_archivador = model.id_archivador,
                id_filial = model.id_filial,
                id_oficina_direccion = model.id_oficina_direccion,

                //Add by HM - 13/11/2019
                ruc = model.ruc,
                evaluador_cdl_notif = model.evaluador_cdl_notif,
                direccion_cdl_notif = model.direccion_cdl_notif,
                empresa_cdl_notif = model.empresa_cdl_notif,
                folia_cdl_notif = Convert.ToInt32(model.folia_cdl_notif),
                doc_notificar_cdl_notif = model.doc_notificar_cdl_notif,
                exp_o_ht_cdl_notif = model.exp_o_ht_cdl_notif,
                exp_o_ht_n_cdl_notif = model.exp_o_ht_n_cdl_notif
            };

            return item;
        }
        
        public static DocumentoDhcpaDetalleRequest Documento_dhcpa_detalle(detDocdhcpaViewModel model)
        {
            DocumentoDhcpaDetalleRequest item = new DocumentoDhcpaDetalleRequest
            {
                id_doc_dhcpa= model.id_doc_dhcpa,
                id_oficina_direccion_destino = model.id_oficina_direccion,
                nombre_persona_destino = model.persona_destino
            };

            return item;
        }

        public static DetSegDocDhcpaRequest Documento_dhcpa_seguimiento(detdocdhcpasegViewModel model)
        {
            DetSegDocDhcpaRequest item = new DetSegDocDhcpaRequest
            {
                id_doc_dhcpa = model.id_doc_dhcpa,
                id_seguimiento = model.id_seguimiento
            };

            return item;
        }

        public static ProtocoloRequest Protocolo(ProtocoloViewModel model)
        {
            ProtocoloRequest item = new ProtocoloRequest
            {
                id_protocolo = model.id_protocolo,
                id_seguimiento = model.id_seguimiento,
                nombre = model.nombre,
                fecha_inicio = model.fecha_inicio,
                fecha_fin = model.fecha_fin,
                ind_concha_abanico = model.ind_concha_abanico,
                ind_otros = model.ind_otros,
                ind_peces = model.ind_peces,
                ind_crustaceos = model.ind_crustaceos,
                id_tipo_ch = model.id_tipo_ch
            };

            return item;
        }

    }
}