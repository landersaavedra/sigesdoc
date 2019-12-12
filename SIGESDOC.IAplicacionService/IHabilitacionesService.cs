using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Request;
using SIGESDOC.Response;

namespace SIGESDOC.IAplicacionService
{
    [ServiceContract]
    public interface IHabilitacionesService
    {
        [OperationContract]
        FirmasSdhpaResponse lista_firmas_sdhpa_activas(string persona_num_documento);

        
        [OperationContract]
        PtocoloTransporteXIdTransporte2018V1Response lista_PtocoloTransporteXIdTransporte2018V1Response_x_id(int id);

        [OperationContract]
        ConsultaExpedienteXExpedienteResponse Consulta_expediente_x_expediente(string expediente);

        [OperationContract]
        IEnumerable<DbGeneralMaeTransporteResponse> Lista_db_general_mae_transporte(string placa = "", string cod_habilitacion = "", int id_tipo_carroceria = 0, int id_tipo_furgon = 0);
        /*01*/
        [OperationContract]
        int Create_documento_sdhcp(DocumentoSeguimientoRequest request);
        /*02*/
        [OperationContract]
        int Create_det_doc_fac(DetDocFactRequest request);
        
        [OperationContract]
        int Create_ActaInspeccionDsfpa(ActaInspeccionDsfpaRequest request);
        [OperationContract]
        int Create_InformeInspeccionDsfpa(InformeInspeccionDsfpaRequest request);
        [OperationContract]
        int Create_ChecklistInspeccionDsfpa(CheckListInspeccionDsfpaRequest request);
        [OperationContract]
        int Create_pruebaInspeccionDsfpa(PruebaInspeccionDsfpaRequest request);
        /*03*/
        [OperationContract]
        int Create_Seguimiento(SeguimientoDhcpaRequest request);
        /*04*/
        [OperationContract]
        int Create_det_doc_seg(DetSegDocRequest request);
        /*04*/
        [OperationContract]
        int Create_documento_seguimiento_adjunto(DocumentoSeguimientoAdjuntoRequest request);
        /*05*/
        [OperationContract]
        bool Update_mae_expediente(ExpedientesRequest request);
        /*06*/
        [OperationContract]
        ExpedientesRequest GetExpediente(int id_expediente);
        /*06*/
        [OperationContract]
        ExpedientesResponse GetExpediente_x_id(int id_expediente);
        /*07*/
        [OperationContract]
        IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente);
           
        /*09*/
        [OperationContract]
        IEnumerable<ExpedientesResponse> GetAllExpediente_x_Documento(int id_documento_seg);
        /*10*/
        [OperationContract]
        IEnumerable<ConsultaFacturasResponse> GetAllfacturas_x_Documento(int id_documento_seg);
        /*11*/
        [OperationContract]
        IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcacion_x_documento(int id_documento_seg);
        /*12*/
        [OperationContract]
        int Create_det_seg_evaluador(DetSegEvaluadorRequest request);
        /*13*/
        [OperationContract]
        DocumentoSeguimientoRequest GetAllDocumento_req(int id_documento_seg);
        [OperationContract]
        ActaInspeccionDsfpaRequest GetAllacta_inspeccion_req(int id_acta_insp);
        [OperationContract]
        InformeInspeccionDsfpaRequest GetAllinforme_inspeccion_req(int id_informe_insp);
        [OperationContract]
        CheckListInspeccionDsfpaRequest GetAllchk_list_inspeccion_req(int id_chck_list);
        [OperationContract]
        PruebaInspeccionDsfpaRequest GetAllpruebas_inspeccion_req(int id_prueba_insp);
        [OperationContract]
        string enviar_correo_notificacion_solicitud_sdhpa(int id_solicitud, string destinos);
        [OperationContract]
        IEnumerable<PruebaInspeccionDsfpaRequest> GetAllpruebas_inspeccion_req_x_id_sol_insp_sdhpa(int id_sol_ins);
        /*14*/
        /*14*/
        [OperationContract]
        bool Update_mae_documento_seg(DocumentoSeguimientoRequest request);
        [OperationContract]
        bool Update_acta_insp_dsfpa(ActaInspeccionDsfpaRequest request);
        [OperationContract]
        bool Update_informe_insp_dsfpa(InformeInspeccionDsfpaRequest request);
        [OperationContract]
        bool Update_chk_list_insp_dsfpa(CheckListInspeccionDsfpaRequest request);
        [OperationContract]
        bool Update_prueba_insp_dsfpa(PruebaInspeccionDsfpaRequest request);
        /*15*/
        [OperationContract]
        IEnumerable<DetSegDocResponse> GetAllDet_seg_doc(int id_documento_seg);
        /*16*/
        [OperationContract]
        IEnumerable<DetSegEvaluadorRequest> GetAlldet_seg_evaluador(int id_seguimiento);
        /*17*/
        [OperationContract]
        bool Update_det_seg_evalua(DetSegEvaluadorRequest request);
        /*18*/
        [OperationContract]
        IEnumerable<ArchivadorDhcpaResponse> GetAll_Archivador();
        /*19*/
        [OperationContract]
        IEnumerable<FilialDhcpaResponse> GetAll_Filial();
        /*20*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento(string persona_num_documento);
        /*20*/
        [OperationContract]
        SeguimientoDhcpaResponse GetAllSeguimiento_x_id(int id_seguimiento);
        /*20*/
        [OperationContract]
        DocumentoSeguimientoAdjuntoResponse documento_seguimiento_x_tipo_documento_adjunto(int id_documento_seg,int tipo_documento_adjunto);
        /*20*/
        [OperationContract]
        IEnumerable<DocumentoSeguimientoAdjuntoResponse> lita_documento_seguimiento_x_documento_seg(int id_documento_seg);
        /*20*/
        [OperationContract]
        IEnumerable<TipoDocumentoSeguimientoAdjuntoResponse> Lista_tipo_documento_seguimiento_adjunto_x_tipo_seguimiento(int id_tipo_seguimiento);
        /*21*/
        [OperationContract]
        int CountDocumentos_x_tipo(int id_tipo_documento);
        /*22*/
        [OperationContract]
        int Create_documento_dhcpa(DocumentoDhcpaRequest request);
        /*22*/
        [OperationContract]
        bool Update_documento_dhcpa(DocumentoDhcpaRequest request);
        /*23*/
        [OperationContract]
        int Create_documento_dhcpa_detalle(DocumentoDhcpaDetalleRequest request);
        /*24*/
        [OperationContract]
        int Create_documento_dhcpa_seguimiento(DetSegDocDhcpaRequest request);
        /*25*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_sin_paginado(string expediente, string evaluador, string externo, string habilitante, string cmbestado,int oficina_filtro, int id_tupa);
        /*25-A*/
        [OperationContract]
        int Create_Protocolo(ProtocoloRequest request);
        [OperationContract]
        int Insertar_actividad_estado_protocolo(int estado, int id_protocolo);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Planta(ProtocoloPlantaRequest request);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Desembarcadero(ProtocoloDesembarcaderoRequest request);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Almacen(ProtocoloAlmacenRequest request);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Concesion(ProtocoloConcesionRequest request);
        
            /*27*/
        [OperationContract]
        int Create_Protocolo_Autorizacion_Instalacion(ProtocoloAutorizacionInstalacionRequest request);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Licencia_Operacion(ProtocoloLicenciaOperacionRequest request);
        /*27*/
        [OperationContract]
        int Create_Protocolo_Especie(ProtocoloEspecieRequest request);
        /*28*/
        [OperationContract]
        SeguimientoDhcpaRequest recupera_todo_seguimiento_dhcpa(int id_seguimiento);
        /*29*/
        [OperationContract]
        bool Update_seguimiento_dhcpa(SeguimientoDhcpaRequest request);
        /*29*/
        [OperationContract]
        bool insert_constancia(ConstanciaHaccpRequest request);      
        /*29*/
        [OperationContract]
        bool Guardar_Observacion_seguimiento(SeguimientoDhcpaObservacionesRequest request);    
        
        /*30*/
        [OperationContract]
        IEnumerable<ConsultarPlantasResponse> GetAllPlanta_x_seguimiento(int id_documento_seg);
        /*31*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaRequest> Recupera_seguimiento_x_id(int id_seguimiento);
        /*32*/
        [OperationContract]
        int recupera_cantidad_solicitud_inspeccion(int var_oficina_crea, int var_año);
        /*33*/
        [OperationContract]
        int Create_solicitud_inspeccion(SolicitudInspeccionRequest request);
        /*34*/
        [OperationContract]
        int recupera_cantidad_informe_tecnico(int var_oficina_crea, int var_año);
        /*35*/
        [OperationContract]
        bool Create_informe_tecnico(InformeTecnicoEvalRequest request);
        
        /*36*/
        [OperationContract]
        ConsultarPlantasResponse Recupera_Planta(int id_seguimiento, int id_planta);
        /*37*/
        [OperationContract]
        bool Actualiza_habilitacion_planta(DateTime fecha_habilitacion_final, int id_planta);
        /*38*/
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa(string evaluador, int tipo_doc_dhcpa, string asunto, int anno);
        [OperationContract]
        DocumentoDhcpaRequest Lista_Documento_dhcpa_x_id_rq(int id_doc_dhcpa);
        [OperationContract]
        DocumentoDhcpaResponse Lista_Documento_dhcpa_x_id_rs(int id_doc_dhcpa);
        
        /*40*/
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_destino_documentos_dhcpa(int id_documento_dhcpa);
        /*41*/
        [OperationContract]
        IEnumerable<SolicitudInspeccionResponse> Lista_solicitud_seguimiento(int id_seguimiento);
        /*41*/
        [OperationContract]
        SolicitudInspeccionResponse Lista_solicitud_seguimiento_x_id_solicitud(int id_solicitud);
        /*42*/
        [OperationContract]
        IEnumerable<InformeTecnicoEvalResponse> Lista_informe_tecnico_seguimiento(int id_seguimiento);
        /*42*/
        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result> consulta_correo_x_solicitud(int id_solicitud);
        /*43*/
        [OperationContract]
        IEnumerable<DocumentoSeguimientoResponse> lista_documentos_recibidos_x_seguimiento(int id_seguimiento);

        /*44*/
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> lista_documentos_emitidos_dhcpa_x_seguimiento(int id_seguimiento);
        /*45*/
        [OperationContract]
        IEnumerable<ProtocoloResponse> lista_protocolo_x_seguimiento(int id_seguimiento);

        [OperationContract]
        IEnumerable<ConsultaProtocolosAiResponse> lista_protocolo_ai_x_seguimiento(int id_seguimiento);
        [OperationContract]
        IEnumerable<ConsultaProtocolosLoResponse> lista_protocolo_lo_x_seguimiento(int id_seguimiento);
        /*45*/
        [OperationContract]
        IEnumerable<ProtocoloResponse> lista_protocolo_x_id_transporte(int id_transporte);
        /*45*/
        [OperationContract]
        IEnumerable<ConstanciaHaccpResponse> lista_haccp_x_seguimiento(int id_seguimiento);
        /*45*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaObservacionesResponse> Listar_Observacion_x_seguimiento(int id_seguimiento);
        /*45*/
        [OperationContract]
        IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> CONSULTA_HISTORIAL_EVALUADOR(int id_seguimiento);
        /*46*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_dhcpa();
        /*48*/
        [OperationContract]
        IEnumerable<DocumentoSeguimientoResponse> Lista_Documento_OD_pendientes_x_recibir(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, string expediente);
          
        /*50*/
        [OperationContract]
        IEnumerable<ExpedientesResponse> Lista_expediente_sin_seguimiento();
        /*51*/
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento(int id_tipo_documento, int anno);
        /*52*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_excel();
        /*52*/
        [OperationContract]
        IEnumerable<ConsultarPlantasResponse> Lista_plantas_excel();
        /*53*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consultar_RUC_X_NOM_Seguimiento(string NOM);
        /*54*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consultar_RUC_seguimiento(string RUC);
        /*55*/
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Destino_Documentos_x_tipo_documento(int id_doc_dhcpa);
        /*56*/
        [OperationContract]
        IEnumerable<EstadoSeguimientoDhcpaResponse> Lista_estado_seguimiento_dhcpa();
        /*56*/
        [OperationContract]
        IEnumerable<TipoServicioHabilitacionResponse> Lista_tipo_servicio_habilitaciones();
        /*57*/
        [OperationContract]
        IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_excel(int id_oficina);
        /*58*/
        [OperationContract]
        SeguimientoDhcpaResponse Lista_protocolo_solicitud(int id_seguimiento);
        /*58*/
        [OperationContract]
        SeguimientoDhcpaResponse Lista_protocolo_seguimiento_planta(int id_planta);
        /*59*/
        [OperationContract]
        SeguimientoDhcpaResponse Lista_datos_evaluador(int id_seguimiento);
        /*60*/
        [OperationContract]
        int Create_Protocolo_Embarcacion(ProtocoloEmbarcacionRequest request);
        /*60*/
        [OperationContract]
        int Create_Persona_telefono(string persona_num_documento, string telefono, string usuario);
        
        /*36*/
        [OperationContract]
        ConsultaEmbarcacionesResponse Recupera_Embarcacion(int id_seguimiento, int id_embarcacion);
        /*36*/
        [OperationContract]
        IEnumerable<IndicadorProtocoloEspecieResponse> Lista_indicadorprotocoloespecie();
        /*36*/
        [OperationContract]
        IEnumerable<TipoAutorizacionInstalacionResponse> Lista_tipo_autorizacion(int id_tipo_ai);
        /*36*/
        [OperationContract]
        IEnumerable<TipoLicenciaOperacionResponse> Lista_tipo_licencia_operacion(int id_tipo_lo);
        /*44*/
        [OperationContract]
        IEnumerable<EspeciesHabilitacionesResponse> lista_especies_habilitaciones(string nombre_comun, string nombre_cientifico);
        /*44*/
        [OperationContract]
        IEnumerable<ConsultarPlantasResponse> genera_protocolo_planta();
        /*44*/
        [OperationContract]
        IEnumerable<ProtocoloLicenciaOperacionResponse> genera_protocolo_licencia_operacion();
        /*44*/
        [OperationContract]
        IEnumerable<ProtocoloAutorizacionInstalacionResponse> genera_protocolo_autorizacion_instalacion();
        /*44*/
        [OperationContract]
        IEnumerable<ConsultaEmbarcacionesResponse> genera_protocolo_embarcacion();
        /*44*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> genera_protocolo_almacen();
        [OperationContract]
        IEnumerable<DbGeneralMaeTransporteResponse> genera_protocolo_transporte();
        /*44*/
        [OperationContract]
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> genera_protocolo_desembarcadero();
        /*45*/
        [OperationContract]
        ProtocoloRequest lista_protocolo_x_id(int id_protocolo);
        /*45*/
        [OperationContract]
        ProtocoloTransporteRequest lista_protocolo_transporte_x_id_protocolo(int id_protocolo);
        /*45*/
        [OperationContract]
        bool actualizar_protocolo(ProtocoloRequest request);
        /*44*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> genera_protocolo_concesion();

        /*20*/
        [OperationContract]
        IEnumerable<ConsultarPersonaTelefonoResponse> consulta_persona_natural_telefono(string persona_num_documento);
        /*20*/
        [OperationContract]
        DbGeneralMaeTransporteResponse consulta_db_general_transporte_x_id(int id_transporte);
        /*20*/
        [OperationContract]
        IEnumerable<TipoCamaraTransporteResponse> consulta_todo_activo_tipoCamaraTransporte();
        /*20*/
        [OperationContract]
        IEnumerable<DbGeneralMaeTipoCarroceriaResponse> consulta_todo_activo_tipocarroceria();

        [OperationContract]
        DbGeneralMaeTipoCarroceriaResponse consulta_todo_activo_tipocarroceria_x_id(int id_tc);

        [OperationContract]
        TipoAtencionInspeccionResponse consulta_tipo_atencion_x_id(int id_ta);
        /*20*/
        [OperationContract]
        IEnumerable<ConsultarTipoFurgonTransporteResponse> consulta_todo_activo_tipofurgon(int id_tipo_carroceria);
        /*20*/
        [OperationContract]
        IEnumerable<DbGeneralMaeUnidadMedidaResponse> consulta_todo_activo_unidad_medida();

        /*20*/
        [OperationContract]
        IEnumerable<TipoAtencionInspeccionResponse> consulta_todo_tipo_atencion();
        
        [OperationContract]
        DbGeneralMaeTransporteResponse registrar_nuevo_transporte(string nueva_placa, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario);
        [OperationContract]
        DbGeneralMaeTransporteResponse actualizar_nuevo_transporte(int id_transporte, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario);

        [OperationContract]
        int Generar_numero_protocolo_transporte(int anno);
        [OperationContract]
        int Create_Protocolo_Transporte(ProtocoloTransporteRequest request);
        [OperationContract]
        bool Update_Protocolo_Transporte(ProtocoloTransporteRequest request);
        /*52*/
        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO_Result> lista_transportes_con_protocolo_habilitado();
        [OperationContract]
        IEnumerable<ProtocoloResponse> Lista_mae_protocolo(string nombre_protocolo);

        
        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result> Lista_acta_info_pru_por_si(int id_sol_ins);
        /*54*/
        [OperationContract]
        SolicitudInspeccionResponse Consultar_solicitud_inspeccion_sdhpa_x_id(int id_sol_ins);

        [OperationContract]
        IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos_x_rec(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente);
        //Add by HM - 28/11/2019
        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa_externos(string evaluador, int tipo_doc_dhcpa, string asunto, int anno, int oficina_direccion);

        //Add by HM - 28/11/2019
        [OperationContract]
        int CountDocumentos_x_tipo_oficina_direccion(int id_tipo_documento, int oficina_direccion);

        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento_oficina_direccion(int id_tipo_documento, int anno, int oficina_direccion);

        [OperationContract]
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_externos(string evaluador, int tipo_doc_dhcpa, string asunto, int anno, int oficina_direccion);
    }
}

