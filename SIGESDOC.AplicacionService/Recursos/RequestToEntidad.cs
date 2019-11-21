using System;
using SIGESDOC.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Request;

namespace SIGESDOC.AplicacionService
{
    public class RequestToEntidad
    {
        public static MAE_TIPO_DOCUMENTO tipodocumento(TipoDocumentoRequest request)
        {
            MAE_TIPO_DOCUMENTO item = new MAE_TIPO_DOCUMENTO
            {
                ID_TIPO_DOCUMENTO = request.id_tipo_documento,
                NOMBRE = request.nombre
            };

            return item;
        }
        
        public static MAE_EXPEDIENTES expedientes(ExpedientesRequest request)
        {
            MAE_EXPEDIENTES item = new MAE_EXPEDIENTES
            {
                ID_EXPEDIENTE = request.id_expediente,
                NUMERO_EXPEDIENTE = request.numero_expediente,
                ID_TIPO_EXPEDIENTE = request.id_tipo_expediente,
                USUARIO_REGISTRO = request.usuario_registro,
                FECHA_REGISTRO = request.fecha_registro,
                USUARIO_MODIFICO = request.usuario_modifico,
                FECHA_MODIFICO = request.fecha_modifico,
                INDICADOR_SEGUIMIENTO = request.indicador_seguimiento,
                NOM_EXPEDIENTE = request.nom_expediente,
                AÑO_CREA = request.año_crea
            };

            return item;
        }
         
        public static MAE_HOJA_TRAMITE HojaTramite(HojaTramiteRequest request)
        {
            MAE_HOJA_TRAMITE item = new MAE_HOJA_TRAMITE
            {
                NUMERO = request.numero,
                ID_TIPO_TRAMITE = request.id_tipo_tramite,
                ID_OFICINA = request.id_oficina,
                FECHA_EMISION = request.fecha_emision,
                USUARIO_EMISION = request.usuario_emision,
                ASUNTO = request.asunto,
                persona_num_documento = request.persona_num_documento,
                TIPO_PER = request.tipo_per,
                HOJA_TRAMITE = request.hoja_tramite,
                ID_EXPEDIENTE = request.id_expediente,
                NUMERO_PADRE = request.numero_padre,
                RUTA_PDF = request.ruta_pdf,
                REFERENCIA = request.referencia,
                EDITAR = request.editar,
                PEDIDO_SIGA = request.pedido_siga,
                ID_TIPO_PEDIDO_SIGA = request.id_tipo_pedido_siga,
                ANNO_SIGA = request.anno_siga,
                CLAVE = request.clave,
                ID_TUPA = request.id_tupa,
                NOMBRE_EXTERNO = request.nombre_externo
            };

            return item;
        }
        
        public static MAE_DOCUMENTO Documento(DocumentoRequest request)
        {
            MAE_DOCUMENTO item = new MAE_DOCUMENTO
            {
                ID_DOCUMENTO = request.id_documento,
                NUMERO = request.numero,
                ID_TIPO_DOCUMENTO = request.id_tipo_documento,
                NUMERO_DOCUMENTO = request.numero_documento,
                ANEXOS = request.anexos,
                //FECHA_DOCUMENTO = request.fecha_documento,
                FOLIOS = request.folios,
                OFICINA_CREA = request.oficina_crea,
                FECHA_ENVIO = request.fecha_envio,
                USUARIO_CREA = request.usuario_crea,
                NOM_DOC = request.nom_doc,
                PERSONA_CREA = request.persona_crea,
                ID_INDICADOR_DOCUMENTO =request.id_indicador_documento,
                RUTA_PDF = request.ruta_pdf,
                NUM_EXT = request.num_ext,
                NOM_OFICINA_CREA = request.nom_oficina_crea
            };

            return item;
        }

        public static MAE_DOCUMENTO_ANEXO Documentoanexo(DocumentoAnexoRequest request)
        {
            MAE_DOCUMENTO_ANEXO item = new MAE_DOCUMENTO_ANEXO
            {
                ID_DOCUMENTO_ANEXO = request.id_documento_anexo,
                ID_DOCUMENTO = request.id_documento,
                RUTA = request.ruta,
                DESCRIPCION = request.descripcion,
                EXTENSION = request.extension,
                USUARIO_CREA = request.usuario_crea,
                FECHA_CREA = request.fecha_crea,
                ACTIVO = request.activo
            };

            return item;
        }
        public static DAT_DOCUMENTO_DETALLE DocumentoDetalle(DocumentoDetalleRequest request)
        {
            DAT_DOCUMENTO_DETALLE item = new DAT_DOCUMENTO_DETALLE
            {
                ID_DET_DOCUMENTO = request.id_det_documento,
                ID_DOCUMENTO = request.id_documento,
                ID_CAB_DET_DOCUMENTO = request.id_cab_det_documento,
                OFICINA_DESTINO = request.oficina_destino,
                OFICINA_CREA = request.oficina_crea,
                OBSERVACION = request.observacion,
                FECHA_RECEPCION = request.fecha_recepcion,
                USUARIO_RECEPCION = request.usuario_recepcion,
                FECHA_DERIVADO = request.fecha_derivado,
                USUARIO_DERIVADO = request.usuario_derivado,
                FECHA_ATENDIDO = request.fecha_atendido,
                USUARIO_CREA = request.usuario_crea,
                FECHA_CREA = request.fecha_crea,
                USUARIO_ATENDIDO = request.usuario_atendido,
                FECHA_ARCHIVO = request.fecha_archivo,
                USUARIO_ARCHIVO = request.usuario_archivo,
                ID_EST_TRAMITE = request.id_est_tramite,
                persona_num_documento = request.persona_num_documento,
                IND_01 = request.ind_01,
                IND_02 = request.ind_02,
                IND_03 = request.ind_03,
                IND_04 = request.ind_04,
                IND_05 = request.ind_05,
                IND_06 = request.ind_06,
                IND_07 = request.ind_07,
                IND_08 = request.ind_08,
                IND_09 = request.ind_09,
                IND_10 = request.ind_10,
                IND_11 = request.ind_11,
                INDICADORES = request.indicadores,
                USUARIO_CANCELAR = request.usuario_cancelar,
                FECHA_CANCELAR = request.fecha_cancelar,
                NOM_OFICINA_CREA = request.nom_oficina_crea,
                NOM_OFICINA_DESTINO = request.nom_oficina_destino
            };
            return item;
        }
        
        public static MAE_SEGUIMIENTO_DHCPA Seguimiento_dhcpa(SeguimientoDhcpaRequest request)
        {
            MAE_SEGUIMIENTO_DHCPA item = new MAE_SEGUIMIENTO_DHCPA
            {
               ID_SEGUIMIENTO = request.id_seguimiento,
               ID_EXPEDIENTE = request.id_expediente,
               TUPA = request.tupa,
               ID_TIPO_PROCEDIMIENTO = request.id_tipo_procedimiento,
               FECHA_INICIO = request.fecha_inicio,
               FECHA_FIN = request.fecha_fin,
               ID_OFI_DIR = request.id_ofi_dir,
               PERSONA_NUM_DOCUMENTO = request.persona_num_documento,
               ID_TIPO_SEGUIMIENTO = request.id_tipo_seguimiento,
               ESTADO = request.estado,
               EVALUADOR = request.evaluador,
               OFICINA_CREA = request.oficina_crea,
               PERSONA_CREA =request.persona_crea,
               ID_HABILITANTE = request.id_habilitante,
               DURACION_SDHPA = request.duracion_sdhpa,
               DURACION_TRAMITE = request.duracion_tramite,
               OBSERVACIONES = request.observaciones,
               INSPECTO_DESIGNADO = request.inspecto_designado,
               FECHA_AUDITORIA = request.fecha_auditoria,
               FECHA_ENVIO_ACTA = request.fecha_envio_acta,
               FECHA_ENVIO_OFICIO_SDHPA = request.fecha_envio_oficio_sdhpa,
               CON_PROCESO = request.con_proceso,
               COD_HABILITANTE = request.cod_habilitante,
               NOM_OFICINA_CREA = request.nom_oficina_crea,
               NOMBRE_EXTERNO = request.nombre_externo
            };

            return item;
        }


        public static MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES Seguimiento_dhcpa_observaciones(SeguimientoDhcpaObservacionesRequest request)
        {
            MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES item = new MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES
            {
                ID_SEGUIMIENTO = request.id_seguimiento,
                ACTIVO = request.activo,
                FECHA_CREA = request.fecha_crea,
                ID_SEG_DHCPA_OBSERVACION = request.id_seg_dhcpa_observacion,
                OBSERVACION = request.observacion,
                USUARIO_CREA = request.usuario_crea
            };

            return item;
        }


        public static MAE_CONSTANCIA_HACCP constancia_haccp(ConstanciaHaccpRequest request)
        {
            MAE_CONSTANCIA_HACCP item = new MAE_CONSTANCIA_HACCP
            {
               ID_CONSTANCIA_HACCP = request.id_constancia_haccp,
               ID_SEGUIMIENTO = request.id_seguimiento,
               NOMBRE = request.nombre,
               ACTIVO = request.activo,
               USUARIO_REGISTRO = request.usuario_registro,
               FECHA_REGISTRO = request.fecha_registro
            };

            return item;
        }
        public static MAE_DOCUMENTO_SEGUIMIENTO documento_seguimiento(DocumentoSeguimientoRequest request)
        {
            MAE_DOCUMENTO_SEGUIMIENTO item = new MAE_DOCUMENTO_SEGUIMIENTO
            {
                ID_DOCUMENTO_SEG = request.id_documento_seg,
                ID_TIPO_DOCUMENTO = request.id_tipo_documento,
                NUM_DOCUMENTO = request.num_documento,
                NOM_DOCUMENTO = request.nom_documento,
                ASUNTO = request.asunto,
                FECHA_CREA = request.fecha_crea,
                EVALUADOR = request.evaluador,
                FECHA_RECIBIDO_EVALUADOR = request.fecha_recibido_evaluador,
                ESTADO = request.estado,
                INDICADOR = request.indicador,
                FECHA_DOCUMENTO = request.fecha_documento,
                FECHA_RECEPCION_SDHPA = request.fecha_recepcion_sdhpa,
                OFICINA_CREA = request.oficina_crea,
                USUARIO_CREA = request.usuario_crea,
                USUARIO_RECEPCION_SDHPA = request.usuario_recepcion_sdhpa,
                FECHA_ASIGNACION_EVALUADOR = request.fecha_asignacion_evaluador,
                ID_SERVICIO_DHCPA = request.id_servicio_dhcpa,
                FECHA_OD = request.fecha_od,
                USUARIO_OD = request.usuario_od,
                EXPEDIENTES_RELACION = request.expedientes_relacion,
                RUTA_PDF = request.ruta_pdf,
                FOLIOS = request.folios,
                NOM_OFI_CREA = request.nom_ofi_crea
            };

            return item;
        }
        
        public static DAT_DET_DOC_FACT det_doc_fac(DetDocFactRequest request)
        {
            DAT_DET_DOC_FACT item = new DAT_DET_DOC_FACT
            {
                ID_DET_DOC_FACT = request.id_det_doc_fact,
                ID_DOCUMENTO_SEG = request.id_documento_seg,
                ID_FACTURA = request.id_factura,
                ACTIVO = request.activo
            };

            return item;
        }
        public static MAE_ACTA_INSPECCION_DSFPA acta_inspeccion_dsfpa(ActaInspeccionDsfpaRequest request)
        {
            MAE_ACTA_INSPECCION_DSFPA item = new MAE_ACTA_INSPECCION_DSFPA
            {
                ID_ACTA_INSP = request.id_acta_insp,
                ID_SOL_INS = request.id_sol_ins,
                NOMBRE_ACTA = request.nombre_acta,
                USUARIO_CARGA = request.usuario_carga,
                USUARIO_OFICINA = request.usuario_oficina,
                INSPECTOR = request.inspector,
                FECHA_CARGA = request.fecha_carga,
                ACTIVO = request.activo,
                RUTA_PDF = request.ruta_pdf
            };

            return item;
        }

        public static MAE_INFORME_INSPECCION_DSFPA informe_inspeccion_dsfpa(InformeInspeccionDsfpaRequest request)
        {
            MAE_INFORME_INSPECCION_DSFPA item = new MAE_INFORME_INSPECCION_DSFPA
            {
                ID_INFORME_INSP = request.id_informe_insp,
                ID_SOL_INS = request.id_sol_ins,
                NOMBRE_INFORME = request.nombre_informe,
                USUARIO_CARGA = request.usuario_carga,
                USUARIO_OFICINA = request.usuario_oficina,
                INSPECTOR = request.inspector,
                FECHA_CARGA = request.fecha_carga,
                ACTIVO = request.activo,
                RUTA_PDF = request.ruta_pdf
            };

            return item;
        }
        public static MAE_CHECK_LIST_INSPECCION_DSFPA check_list_inspeccion_dsfpa(CheckListInspeccionDsfpaRequest request)
        {
            MAE_CHECK_LIST_INSPECCION_DSFPA item = new MAE_CHECK_LIST_INSPECCION_DSFPA
            {
                ID_CHK_LIST_INSP = request.id_chk_list_insp,
                ID_SOL_INS = request.id_sol_ins,
                NOMBRE_CHECK_LIST = request.nombre_check_list,
                USUARIO_CARGA = request.usuario_carga,
                USUARIO_OFICINA = request.usuario_oficina,
                INSPECTOR = request.inspector,
                FECHA_CARGA = request.fecha_carga,
                ACTIVO = request.activo,
                RUTA_PDF = request.ruta_pdf
            };

            return item;
        }

        public static MAE_PRUEBA_INSPECCION_DSFPA pruebas_inspeccion_dsfpa(PruebaInspeccionDsfpaRequest request)
        {
            MAE_PRUEBA_INSPECCION_DSFPA item = new MAE_PRUEBA_INSPECCION_DSFPA
            {
                ID_PRUEBA_INSP = request.id_prueba_insp,
                ID_SOL_INS = request.id_sol_ins,
                USUARIO_CARGA = request.usuario_carga,
                USUARIO_OFICINA = request.usuario_oficina,
                INSPECTOR = request.inspector,
                FECHA_CARGA = request.fecha_carga,
                ACTIVO = request.activo,
                RUTA_PDF = request.ruta_pdf
            };

            return item;
        }
        public static DAT_DET_SEG_DOC det_doc_seg(DetSegDocRequest request)
        {
            DAT_DET_SEG_DOC item = new DAT_DET_SEG_DOC
            {
                ID_DET_DOC = request.id_det_doc,
                ID_DOCUMENTO_SEG = request.id_documento_seg,
                ID_SEGUIMIENTO = request.id_seguimiento,
                ACTIVO = request.activo
            };

            return item;
        }

        public static MAE_DOCUMENTO_SEGUIMIENTO_ADJUNTO DocumentoSeguimientoAdjunto(DocumentoSeguimientoAdjuntoRequest request)
        {
            MAE_DOCUMENTO_SEGUIMIENTO_ADJUNTO item = new MAE_DOCUMENTO_SEGUIMIENTO_ADJUNTO
            {
                ID_DOC_SEG_ADJUNTO = request.id_doc_seg_adjunto,
                ID_DOCUMENTO_SEG = request.id_documento_seg,
                ID_TIPO_DOC_SEG_ADJUNTO = request.id_tipo_doc_seg_adjunto,
                USUARIO_CREA = request.usuario_crea,
                FECHA_CREA = request.fecha_crea,
                ACTIVO = request.activo
            };

            return item;
        }
        public static DAT_DET_SEG_EVALUADOR det_seg_evaluador(DetSegEvaluadorRequest request)
        {
            DAT_DET_SEG_EVALUADOR item = new DAT_DET_SEG_EVALUADOR
            {
                ID_DET_EXP_EVA = request.id_det_exp_eva,
                ID_SEGUIMIENTO = request.id_seguimiento,
                EVALUADOR = request.evaluador,
                INDICADOR = request.indicador,
                FECHA_RECIBIDO = request.fecha_recibido,
                FECHA_DERIVADO = request.fecha_derivado
            };

            return item;
        }
        
        public static MAE_DOCUMENTO_DHCPA documento_dhcpa(DocumentoDhcpaRequest request)
        {
            MAE_DOCUMENTO_DHCPA item = new MAE_DOCUMENTO_DHCPA
            {
                ID_DOC_DHCPA = request.id_doc_dhcpa,
                ID_TIPO_DOCUMENTO = request.id_tipo_documento,
                NUM_DOC = request.num_doc,
                NOM_DOC = request.nom_doc,
                FECHA_DOC = request.fecha_doc,
                ASUNTO = request.asunto,
                ANEXOS = request.anexos,
                FECHA_REGISTRO = request.fecha_registro,
                USUARIO_REGISTRO = request.usuario_registro,
                ID_ARCHIVADOR = request.id_archivador,
                ID_FILIAL = request.id_filial,
                NUMERO_HT = request.numero_ht,
                PDF = request.pdf,
                ID_OFICINA_DIRECCION = request.id_oficina_direccion,

                //Add by HM - 13/11/2019
                RUC = request.ruc,
                EVALUADOR_CDL_NOTIF = request.evaluador_cdl_notif,
                DIRECCION_CDL_NOTIF = request.direccion_cdl_notif,
                EMPRESA_CDL_NOTIF = request.empresa_cdl_notif,
                FOLIA_CDL_NOTIF = request.folia_cdl_notif,
                DOC_NOTIFICAR_CDL_NOTIF = request.doc_notificar_cdl_notif,
                EXP_O_HT_CDL_NOTIF = request.exp_o_ht_cdl_notif,
                EXP_O_HT_N_CDL_NOTIF = request.exp_o_ht_n_cdl_notif
            };

            return item;
        }

        public static DAT_DOCUMENTO_DHCPA_DETALLE documento_dhcpa_detalle(DocumentoDhcpaDetalleRequest request)
        {
            DAT_DOCUMENTO_DHCPA_DETALLE item = new DAT_DOCUMENTO_DHCPA_DETALLE
            {
                ID_DOC_DHCPA_DET = request.id_doc_dhcpa_det,
                ID_DOC_DHCPA = request.id_doc_dhcpa,
                ID_OFICINA_DIRECCION_DESTINO = request.id_oficina_direccion_destino,
                PERSONA_DESTINO = request.persona_destino,
                ACTIVO = request.activo,
                USUARIO_REGISTRO = request.usuario_registro,
                FECHA_REGISTRO = request.fecha_registro,
                USUARIO_MODIFICA = request.usuario_modifica,
                FECHA_MODIFICA = request.fecha_modifica,
                NOMBRE_PERSONA_DESTINO = request.nombre_persona_destino
            };

            return item;
        }

        public static DAT_DET_SEG_DOC_DHCPA documento_dhcpa_seguimiento(DetSegDocDhcpaRequest request)
        {
            DAT_DET_SEG_DOC_DHCPA item = new DAT_DET_SEG_DOC_DHCPA
            {
                ID_DET_DSDHCPA = request.id_det_dsdhcpa,
                ID_SEGUIMIENTO = request.id_seguimiento,
                ID_DOC_DHCPA = request.id_doc_dhcpa,
                ACTIVO = request.activo
            };

            return item;
        }

        public static MAE_PROTOCOLO protocolo(ProtocoloRequest request)
        {
            MAE_PROTOCOLO item = new MAE_PROTOCOLO
            {
                ID_PROTOCOLO = request.id_protocolo, 
                ID_SEGUIMIENTO = request.id_seguimiento,
                NOMBRE = request.nombre,
                FECHA_INICIO = request.fecha_inicio,
                FECHA_FIN = request.fecha_fin,
                FECHA_REGISTRO = request.fecha_registro,
                EVALUADOR = request.evaluador,
                ACTIVO = request.activo,
                IND_CONCHA_ABANICO = request.ind_concha_abanico,
                IND_CRUSTACEOS = request.ind_crustaceos,
                IND_OTROS = request.ind_otros,
                IND_PECES = request.ind_peces,
                ID_TIPO_CH = request.id_tipo_ch,
                ID_IND_PRO_ESP = request.id_ind_pro_esp,
                ID_EST_PRO =request.id_est_pro,
                ID_PROTOCOLO_REEMPLAZA = request.id_protocolo_reemplaza
            };

            return item;
        }

        public static DAT_PROTOCOLO_PLANTA dat_protocolo_planta(ProtocoloPlantaRequest request)
        {
            DAT_PROTOCOLO_PLANTA item = new DAT_PROTOCOLO_PLANTA
            {
                ID_DAT_PRO_PLA = request.id_dat_pro_pla,
                ID_PROTOCOLO = request.id_protocolo,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                LICENCIA_OPERACION = request.licencia_operacion,
                ACTIVO = request.activo,
                IND_CONCHA_ABANICO = request.ind_concha_abanico,
                IND_CRUSTACEOS = request.ind_crustaceos,
                IND_OTROS = request.ind_otros,
                IND_PECES = request.ind_peces,
                ID_TIPO_CH = request.id_tipo_ch
            };

            return item;
        }

        public static DAT_PROTOCOLO_DESEMBARCADERO dat_protocolo_desembarcadero(ProtocoloDesembarcaderoRequest request)
        {
            DAT_PROTOCOLO_DESEMBARCADERO item = new DAT_PROTOCOLO_DESEMBARCADERO
            {
                ID_DET_PRO_DESEMB = request.id_det_pro_desemb,
                ID_PROTOCOLO = request.id_protocolo,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                DERECHO_USO_AREA_ACUATICA = request.derecho_uso_area_acuatica,
                ID_DESEMBARCADERO = request.id_desembarcadero
            };

            return item;
        }

        public static DAT_PROTOCOLO_ALMACEN dat_protocolo_almacen(ProtocoloAlmacenRequest request)
        {
            DAT_PROTOCOLO_ALMACEN item = new DAT_PROTOCOLO_ALMACEN
            {
                ID_DAT_PRO_ALMACEN = request.id_dat_pro_almacen,
                ID_PROTOCOLO = request.id_protocolo,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                LICENCIA = request.licencia,
                ID_TIPO_CH = request.id_tipo_ch
            };

            return item;
        }

        public static DAT_PROTOCOLO_TRANSPORTE ProtocoloTransporte(ProtocoloTransporteRequest request)
        {
            DAT_PROTOCOLO_TRANSPORTE item = new DAT_PROTOCOLO_TRANSPORTE
            {
                ID_DAT_PRO_TRANSPORTE = request.id_dat_pro_transporte,
                ID_PROTOCOLO = request.id_protocolo,
                NUMERO = request.numero,
                ANNO = request.anno,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                ID_TIPO_CAMARA_TRANS = request.id_tipo_camara_trans,
                ID_TRANSPORTE = request.id_transporte,
                PLACA = request.placa,
                COD_HABILITACION = request.cod_habilitacion,
                ID_TIPO_CARROCERIA = request.id_tipo_carroceria,
                ID_UM = request.id_um,
                CARGA_UTIL = request.carga_util,
                ACTA_INSPECCION = request.acta_inspeccion,
                INFORME_AUDITORIA = request.informe_auditoria,
                INFORME_TECNICO_EVALUACION = request.informe_tecnico_evaluacion,
                PERSONA_2 = request.persona_2,
                DIRECCION_LEGAL_DNI = request.direccion_legal_dni,
                REPRESENTANTE_LEGAL_DNI = request.representante_legal_dni,
                ID_TIPO_FURGON = request.id_tipo_furgon,
                ID_TIPO_ATENCION = request.id_tipo_atencion,
                ID_TIPO_CARROCERIA_TARPRO = request.id_tipo_carroceria_tarpro,
                INFORME_SDHPA = request.informe_sdhpa
            };

            return item;
        }

        public static DAT_PROTOCOLO_CONCESION dat_protocolo_concesion(ProtocoloConcesionRequest request)
        {
            DAT_PROTOCOLO_CONCESION item = new DAT_PROTOCOLO_CONCESION
            {
                ID_DET_PRO_CONCE = request.id_det_pro_conce,
                ID_PROTOCOLO = request.id_protocolo,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                ID_CONCESION = request.id_concesion,
                RESOLUCION = request.resolucion,
                FECHA_RESOLUCION = request.fecha_resolucion,
                ID_TIP_ACT_CONCE = request.id_tip_act_conce,
                AREA_HA = request.area_ha,
                TOTAL_HA = request.total_ha,
                LOTE = request.lote,
                ESPEJO_AGUA = request.espejo_agua,
                AREA = request.area,
                CAPACIDAD_PRODUCCION = request.capacidad_produccion,
                DIRECCION_LEGAL_DNI = request.direccion_legal_dni,
                REPRESENTANTE_LEGAL_DNI = request.representante_legal_dni
            };

            return item;
        }

        public static DAT_PROTOCOLO_AUTORIZACION_INSTALACION dat_protocolo_autorizacion_instalacion(ProtocoloAutorizacionInstalacionRequest request)
        {
            DAT_PROTOCOLO_AUTORIZACION_INSTALACION item = new DAT_PROTOCOLO_AUTORIZACION_INSTALACION
            {
                ID_PRO_AUTORIZACION_INSTALACION = request.id_pro_autorizacion_instalacion,
                ID_PROTOCOLO = request.id_protocolo,
                RUC = request.ruc,
                ID_SEDE = request.id_sede,
                ID_REPRESENTANTE_LEGAL = request.id_representante_legal,
                ID_TIPO_AUTORIZACION_INSTALACION = request.id_tipo_autorizacion_instalacion,
                ACTIVIDAD = request.actividad
            };

            return item;
        }


        public static DAT_PROTOCOLO_LICENCIA_OPERACION dat_protocolo_licencia_operacion(ProtocoloLicenciaOperacionRequest request)
        {
            DAT_PROTOCOLO_LICENCIA_OPERACION item = new DAT_PROTOCOLO_LICENCIA_OPERACION
            {
                ID_PRO_LICENCIA_OPERACION = request.id_pro_licencia_operacion,
                ID_PROTOCOLO = request.id_protocolo,
                RESOLUCION_AUTORIZACION_INSTALACION = request.resolucion_autorizacion_instalacion,
                FECHA_RESOLUCION = request.fecha_resolucion,
                RUC = request.ruc,
                ID_SEDE = request.id_sede,
                ID_REPRESENTANTE_LEGAL = request.id_representante_legal,
                ID_TIPO_LICENCIA_OPERACION = request.id_tipo_licencia_operacion,
                ACTIVIDAD = request.actividad
            };

            return item;
        }

        public static DAT_PROTOCOLO_ESPECIE dat_protocolo_especie(ProtocoloEspecieRequest request)
        {
            DAT_PROTOCOLO_ESPECIE item = new DAT_PROTOCOLO_ESPECIE
            {
                ACTIVO = request.activo,
                ID_DET_ESPEC_HAB = request.id_det_espec_hab,
                ID_PRO_ESPE = request.id_pro_espe,
                ID_PROTOCOLO = request.id_protocolo
            };

            return item;
        }

        public static MAE_SOLICITUD_INSPECCION solicitud_inspeccion(SolicitudInspeccionRequest request)
        {
            MAE_SOLICITUD_INSPECCION item = new MAE_SOLICITUD_INSPECCION
            {
                ID_SOL_INS = request.id_sol_ins,
                ID_SEGUIMIENTO = request.id_seguimiento,
                NUMERO_DOCUMENTO = request.numero_documento,
                FECHA_CREA = request.fecha_crea,
                OFICINA_CREA = request.oficina_crea,
                USUARIO_CREA = request.usuario_crea,
                AÑO_CREA = request.año_crea,
                ID_VERSION_SOLICITUD = request.id_version_solicitud,
                RESOLUCION = request.resolucion,
                PERSONA_CONTACTO = request.persona_contacto,
                TELEFONO_OFICINA = request.telefono_oficina,
                TELEFONO_PLANTA = request.telefono_planta,
                CORREO = request.correo,
                OBSERVACIONES = request.observaciones,
                ID_TIPO_SER_HAB = request.id_tipo_ser_hab,
                ID_FILIAL = request.id_filial,
                ID_DEST_SOL_INS = request.id_dest_sol_ins,
                COND_MANUALES = request.cond_manuales,
                NORMA_APLICA = request.norma_aplica,
                NOM_OFI_CREA = request.nom_ofi_crea,
                ID_ESTADO = request.id_estado,
                ID_OFICINA_DESTINO = request.id_oficina_destino,
                NOM_OFICINA_DESTINO = request.nom_oficina_destino
            };

            return item;
        }
        public static MAE_INFORME_TECNICO_EVAL informe_tecnico(InformeTecnicoEvalRequest request)
        {
            MAE_INFORME_TECNICO_EVAL item = new MAE_INFORME_TECNICO_EVAL
            {
                ID_INF_TEC_EVAL = request.id_inf_tec_eval,
                ID_SEGUIMIENTO = request.id_seguimiento,
                NUMERO_DOCUMENTO = request.numero_documento,
                OBSERVACIONES = request.observaciones,
                FECHA_CREA = request.fecha_crea,
                OFICINA_CREA = request.oficina_crea,
                USUARIO_CREA = request.usuario_crea,
                AÑO_CREA = request.año_crea
            };

            return item;
        }


        public static DAT_PROTOCOLO_EMBARCACION dat_protocolo_embarcacion(ProtocoloEmbarcacionRequest request)
        {
            DAT_PROTOCOLO_EMBARCACION item = new DAT_PROTOCOLO_EMBARCACION
            {
                ID_DET_PRO_HAB = request.id_det_pro_hab,
                ID_PROTOCOLO = request.id_protocolo,
                DIRECCION_LEGAL = request.direccion_legal,
                REPRESENTANTE_LEGAL = request.representante_legal,
                RESOLUCION = request.resolucion,
                ID_TIP_PRO_EMB = request.id_tip_pro_emb,
                NOM_EMBARCACION = request.nom_embarcacion,
                DIRECCION_PERSONA_NATURAL = request.direccion_persona_natural,
                ID_PERSONA_TELEFONO = request.id_persona_telefono
            };

            return item;
        }

        public static DAT_DOC_DET_OBSERVACIONES doc_det_observaciones(DocDetObservacionesRequest request)
        {
            DAT_DOC_DET_OBSERVACIONES item = new DAT_DOC_DET_OBSERVACIONES
            {
                ID_DET_DOC_OBSERVACION = request.id_det_doc_observacion,
                ID_DET_DOCUMENTO = request.id_det_documento,
                OBSERVACION = request.observacion,
                USUARIO_CREA = request.usuario_crea,
                FECHA_CREA = request.fecha_crea,
                ACTIVO = request.activo,
            };

            return item;
        }
    }
}
