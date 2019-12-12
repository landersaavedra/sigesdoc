using SIGESDOC.Request;
using SIGESDOC.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.IAplicacionService
{
    [ServiceContract]
    public interface IGeneralService
    {
        
        [OperationContract]
        ConsultarDniResponse actualizar_persona(string persona_num_documento, string direccion, string ubigeo, string usuario);
        /*01*/
        [OperationContract]
        IEnumerable<TipoTramiteResponse> Recupera_tipo_tramite_todo();
        /*02*/
        [OperationContract]
        IEnumerable<TipoExpedienteResponse> Recupera_tipo_expediente();
        /*03*/
        [OperationContract]
        IEnumerable<ExpedientesResponse> Recupera_expedientes();
        /*04*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Recupera_oficina_todo();
        /*05*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Recupera_oficina_todo_x_bus(string nombre);
        /*06*/
        [OperationContract]
        IEnumerable<ConsultarUsuarioResponse> Consulta_Usuario(string ruc, string persona_num_documento);
        /*07*/
        [OperationContract]
        IEnumerable<ConsultarUsuarioResponse> Recupera_oficina_dni_y_sede(string persona_num_documento, int id_sede);
        /*08*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Recupera_oficina_all_x_sede(int sede);
        /*09*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Recupera_oficina_all_x_ruc(string ruc);
        /*10*/
        [OperationContract]
        ConsultarSedeOficinaResponse Recupera_sede_x_id_ofi_dir(int id_ofi_dir);
        /*10*/
        [OperationContract]
        IEnumerable<ConsultarSedeOficinaResponse> Recupera_sede_all(int id_oficina);
        /*11*/
        [OperationContract]
        IEnumerable<TipoDocumentoResponse> Consulta_Tipo_Documento(int id);
        /*12*/
        [OperationContract]
        IEnumerable<TipoDocumentoResponse> Recupera_tipo_documento_todo(string tipo_e_i, string tipo_e_i_2);
        /*13*/
        [OperationContract]
        IEnumerable<EstadoTramiteResponse> Recupera_estado_tramite_todo();
        /*14*/
        [OperationContract]
        IEnumerable<ConsultarPersonalResponse> Recupera_personal_todo();

        [OperationContract]
        IEnumerable<SubTupaResponse> recuperatupa(decimal monto);

        /*15*/
        [OperationContract]
        IEnumerable<ConsultarPersonalResponse> Recupera_personal_oficina(int id_oficina);
        /*16*/
        [OperationContract]
        IEnumerable<ConsultarDepartamentoResponse> llenar_departamento();
        /*17*/
        [OperationContract]
        IEnumerable<ConsultarProvinciaResponse> llenar_provincia_x_departamento(string id_departamento);
        /*18*/
        [OperationContract]
        IEnumerable<ConsultarUbigeoResponse> llenar_distrito_x_provincia(string id_provincia);
        /*19*/
        [OperationContract]
        IEnumerable<ConsultarTipoDocumentoIdentidadResponse> llenar_tipo_documento_identidad();
        /*19*/
        [OperationContract]
        IEnumerable<TipoPedidoSigaResponse> llenar_tipo_pedido_siga();

        /*20*/
        [OperationContract]
        IEnumerable<ExpedientesResponse> llenar_expediente(string indicador);
        /*21*/
        [OperationContract]
        IEnumerable<TipoExpedienteResponse> llenar_tipo_expediente(int id_tipo, int id_oficina_dir);
        [OperationContract]
        IEnumerable<ConsultaDbGeneralMaeTipoFacturaResponse> lista_tipo_comprobante();
        [OperationContract]
        /*20*/
        IEnumerable<ConsultaDbGeneralMaeOperacionResponse> lista_operacion(int num_ope);
        [OperationContract]
        /*20*/
        ConsultaDbGeneralMaeOperacionResponse lista_operacion_x_id(int id_operacion);
        

        [OperationContract]
        IEnumerable<ConsultaDbGeneralMaeOperacionResponse> busca_operacion_x_num_x_fecha_oficina(int num_ope, DateTime fecha, int oficina);
        [OperationContract]
        Response.P_INSERT_UPDATE_MAE_OPERACION_Result Guardar_Operacion(int numero, DateTime fecha, int oficina, decimal abono, string usuario); 

        [OperationContract]
        void update_db_general_mae_operacion(ConsultaDbGeneralMaeOperacionResponse ope_rq);

        [OperationContract]
        IEnumerable<ConsultaPersonaReciboSerie1Response> lista_personareciboserie1_sin_direc(string documento);
        [OperationContract]
        /*20*/
        IEnumerable<ConsultaPersonaReciboSerie1Response> lista_direc_personareciboserie1(string documento);
        /*22*/
        [OperationContract]
        IEnumerable<TipoProcedimientoResponse> llenar_tipo_procedimiento(int id_tipo_procedimiento);
        /*22*/
        [OperationContract]
        IEnumerable<ConsultarCodHabEmbarcacionResponse> llenar_codigo_embarcacion();
        /*22*/
        [OperationContract]
        IEnumerable<ConsultarActvEmbarcacionResponse> llenar_actividad_embarcacion();
        /*23*/
        [OperationContract]
        IEnumerable<ConsultaEmbarcacionesResponse> listar_embarcaciones(string matricula);
        /*24*/
        [OperationContract]
        IEnumerable<ConsultaFacturasResponse> listar_factura();

        [OperationContract]
        ConsultaReciboSerie1Response lista_recibo(int id); 

        /*25*/
        [OperationContract]
        bool Guardar_Embarcacion(string matricula, string nombre, int id_tipo_embarcacion, string usuario,int codigo_hab, int num_cod_hab, string nom_cod_hab, int id_tipo_act_emb, string fecha_const);

        /*26*/
        [OperationContract]
        Response.ConsultaFacturasResponse Guardar_Factura(string num1, string num2, DateTime fecha, decimal importe_total, string usuario, int id_tipo_factura, string ruc_dni, string nombre, string direccion, int id_sub_tupa, int cantidad, int id_ofi_crea);
         
        [OperationContract]
        Response.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA_Result Guardar_det_fac_opera(int id_factura, int id_operacion);


        [OperationContract]
        int ultimo_numero_comprobante(int id_tipo_comprobante);

        /*27*/
        [OperationContract]
        int Guardar_Expediente(ExpedientesRequest request);

        /*28-A*/
        [OperationContract]
        IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcaciones_sin_paginado(string matricula, string nombre, int cmb_actividad);
        [OperationContract]
        IEnumerable<Response.SP_EDITA_DB_GENERAL_MAE_SEDE_Result> Edita_db_general_mae_sede(int id_sede, string direccion, string ubigeo, string sede, string referencia);
        /*30*/
        [OperationContract]
        int buscar_embarcacion(string matricula);

        /*18*/
        [OperationContract]
        IEnumerable<ConsultarOficinaDireccionLegalResponse> GetAllDireccionLegal_x_ruc(string RUC);

        /*18*/
        [OperationContract]
        IEnumerable<ConsultarEmpresaPersonaLegalResponse> GetAllPersonaLegal_x_ruc(string RUC);
        
            /*18*/
        [OperationContract]
        IEnumerable<ConsultarDniPersonalLegalResponse> GetAllPersonaLegal_x_dni(string DNI);
        [OperationContract]
        IEnumerable<ConsultaDbGeneralMaeOperacionResponse> Lista_todo_operacion(string operacion, string factura);
        /*31*/
        

        [OperationContract]
        IEnumerable<ConsultaDbGeneralMaeFacturaResponse> GetAllFacturas(string comprobante, string tipo_comprobante, string documento, string externo, string operac);

        [OperationContract]
        IEnumerable<ReporteComprobanteXMesConsultaResponse> GetAllComprobantes_x_mes(int mes, int anio);

        [OperationContract]
        IEnumerable<ReporteComprobanteXMesConsultaResponse> GetAllComprobantes_x_fecha(int fecha); 
        /*33*/
        [OperationContract]
        IEnumerable<ConsultaReporteDiarioSerie1Response> recupera_reporte_diario_serie1(int fecha);

        [OperationContract]
        int buscar_factura(int num1, int num2);
        
        /*34*/
        [OperationContract]
        int buscar_persona(string persona_num_documento);

        [OperationContract]
        ConsultarDniRequest buscar_persona_resp(string persona_num_documento);
        
        /*35*/
        [OperationContract]
        IEnumerable<ExpedientesResponse> GetAllExpediente_sin_paginado(string numero_exp, int id_oficina_dir, string usuario);
        
        /*37*/
        [OperationContract]
        int buscar_expediente(int numero_exp, int id_tipo_expediente, int año_crea);
        /*38*/
        [OperationContract]
        IEnumerable<ConsultarTipoPlantaResponse> recupera_tipo_planta();
        /*39*/
        [OperationContract]
        bool Guardar_Planta(int id_sede, int id_tipo_planta, int numero, string nombre_planta,int id_tipo_actividad, int id_filial, string usuario);
        /*40*/
        [OperationContract]
        IEnumerable<ConsultarPlantasResponse> GetAllPlantas_sin_paginado(string id_tipo_planta, string var_numero, string var_nombre, int var_id_filial, int var_id_actividad, string var_entidad);
        /*42*/
        [OperationContract]
        IEnumerable<ConsultaTipoEmbarcacionesResponse> recupera_tipo_embarcacion(int id_tipo_embarcacion);
        /*43*/
        [OperationContract]
        IEnumerable<TipoConsumoHumanoResponse> recupera_tipo_consumo();
        /*44*/
        [OperationContract]
        IEnumerable<TupaResponse> recupera_tupa();
        [OperationContract]
        IEnumerable<TipoTupaResponse> recupera_tipo_tupa();
        /*44*/
        [OperationContract]
        IEnumerable<DestinoSolicitudInspeccionResponse> recupera_destino_si();
        /*44_A*/
        [OperationContract]
        IEnumerable<TipoSeguimientoResponse> recupera_tipo_seguimiento();

        /*45*/
        [OperationContract]
        IEnumerable<ConsultarPlantasResponse> recupera_planta_x_direccion(int id_direccion, string activo);
        /*46*/
        [OperationContract]
        IEnumerable<FilialDhcpaResponse> recupera_filial(string tipo_e_i);
        /*47*/
        [OperationContract]
        IEnumerable<ConsultarTipoActividadPlantaResponse> recupera_tipo_actividad_planta(int id_tipo_planta);
        /*48*/
        [OperationContract]
        IEnumerable<ConsultarTipoActividadPlantaResponse> recupera_toda_tipo_actividad_planta();

        [OperationContract]
        ConsultarOficinaResponse recupera_oficina(int id_oficina_direccion);

        /*48*/
        [OperationContract]
        ConsultarTipoActividadPlantaResponse recupera_toda_tipo_actividad_planta_x_id(int id_tipo_actividad_planta);
        /*49*/
        [OperationContract]
        IEnumerable<ProtocoloResponse> GetAllProtocolo_x_planta(int id_planta);
        
        /*51*/
        [OperationContract]
        ConsultarPlantasResponse recupera_planta_x_id(int id_planta);
        /*52*/
        [OperationContract]
        IEnumerable<ServicioDhcpaResponse> llenar_servicio_dhcpa();
        /*53*/
        [OperationContract]
        IEnumerable<TipoProtocoloEmbarcacionResponse> Lista_tipo_protocolo_emb();
        /*54*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> GetAllAlmacenes_sin_paginado(string CODIGO_ALMACEN, int ID_ACTIVIDAD_ALMACEN, int ID_FILIAL, string EXTERNO);
        
        
        /*55*/
        [OperationContract]
        IEnumerable<ConsultarActvAlmacenResponse> recupera_actividad_almacen();
        /*56*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> Guarda_Almacen(int ID_ALMACEN, int ID_SEDE, int ID_CODIGO_ALMACEN, int NUM_ALMACEN, string NOM_ALMACEN, int ID_FILIAL, int ID_ACTIVIDAD_ALMACEN, string USUARIO);
        /*55*/
        [OperationContract]
        IEnumerable<ConsultarCodHabAlmacenResponse> recupera_codigo_almacen();
        /*23*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> lista_almacen(string COD_ALMACEN, int var_id_oficina_dir);
        
        /*30*/
        [OperationContract]
        ConsultaEmbarcacionesResponse buscar_embarcacion_x_seguimiento(int id_seguimiento);

        /*51*/
        [OperationContract]
        ConsultarDbGeneralMaeAlmacenSedeResponse recupera_almacen_x_id(int id_almacen);
        
        /*55*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeAreaProduccionResponse> recupera_area_produccion(int id_zona_produccion);
        /*54*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> GetAllconsecion_sin_paginado(int id_zona_produccion, int id_area_produccion, int id_tipo_concesion, string externo);

        /*43*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeTipoConcesionResponse> recupera_tipo_concesion();
        
        /*43*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> recupera_entidad();
        /*55*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeZonaProduccionResponse> recupera_zona_produccion();
        /*55*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeZonaProduccionResponse> recupera_zona_produccion_x_ubigeo(string ubigeo);
        /*25*/
        [OperationContract]
        bool Guardar_Concesion(int ID_CONCESION, string RUC, string CODIGO_HABILITACION, string PARTIDA_REGISTRAL, string UBICACION, string UBIGEO, int ID_AREA_PRODUCCION, int ID_TIPO_CONCESION, int ID_TIPO_ACTIVIDAD_CONCESION, string USUARIO);
        /*23*/
        [OperationContract]
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> lista_concesion(string COD_CONCESION, string documento);
        

        /*43*/
        [OperationContract]
        IEnumerable<TipoActividadConcesionResponse> recupera_actividad_concesion();
        /*54*/
        [OperationContract]
        ConsultarDbGeneralMaeConcesionResponse recupera_mae_concesion_x_id(int id_concesion);

        /*43*/
        [OperationContract]
        IEnumerable<DbGeneralMaeTipoDesembarcaderoResponse> recupera_tipo_desembarcadero();
        /*43*/
        [OperationContract]
        IEnumerable<DbGeneralMaeCodigoDesembarcaderoResponse> recupera_codigo_desembarcadero(int id_tipo_desembarcadero);

        
            /*43*/
        [OperationContract]
        DbGeneralMaeTipoDesembarcaderoResponse recupera_tipo_desembarcadero_x_id_desembarcadero(int id_desembarcadero);
        /*25*/
        [OperationContract]
        bool Guardar_Desembarcadero(int ID_DESEMBARCADERO, int ID_SEDE, int ID_TIPO_DESEMBARCADERO, int ID_COD_DESEMB, int NUM_DESEMB, string NOMBRE_DESEMB, string DENOMINACION, string TEMPORAL, double LATITUD, double LONGITUD, string USUARIO);
        /*26*/
        [OperationContract]
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> GetAlldesembarcadero_sin_paginado(int id_tipo_desembarcadero, string codigo_desembarcadero, string externo);
        /*23*/
        [OperationContract]
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> lista_desembarcadero_x_sede(int var_id_oficina_dir);
        [OperationContract]
        IEnumerable<DbGeneralMaeTransporteResponse> listar_transporte_x_placa(string placa);
        [OperationContract]
        DbGeneralMaeTransporteResponse recuperar_transporte_x_id_transporte(int id_transporte);
        [OperationContract]
        IEnumerable<UnionentidadpersonaResponse> buscar_entidad_persona(string nombre);
        [OperationContract]
        string Recupera_RUC_x_ID_OFI_DIR(int id_ofi_dir);
        [OperationContract]
        Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_Result Consultar_documentos_pendientes(string documento, int id_ofi_dir);
        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result> Consultar_documentos_pendientes_detalle(string documento, int id_ofi_dir);
        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result> Consultar_documentos_pendientes_detalle_desagregado(string documento, int id_ofi_dir, string fecha);
        [OperationContract]
        IEnumerable<Response.SP_ACTUALIZA_NOM_EMPRESA_Result> Edita_db_general_nom_empresa(string nombres, string ruc, string usuario);
        [OperationContract]
        IEnumerable<TipoDocumentoResponse> Recupera_tipo_documento_some();
    }
}

