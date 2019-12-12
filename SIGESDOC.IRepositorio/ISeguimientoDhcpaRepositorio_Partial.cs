using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface ISeguimientoDhcpaRepositorio
    {
        DbGeneralMaeTransporteResponse registrar_nuevo_transporte(string nueva_placa, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario);

        DbGeneralMaeTransporteResponse actualizar_nuevo_transporte(int id_transporte, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario);
        
        IEnumerable<SeguimientoDhcpaResponse> Consulta_Seguimiento(string persona_num_documento);
        IEnumerable<DocumentoSeguimientoAdjuntoResponse> lita_documento_seguimiento_x_documento_seg(int id_documento_seg);
        SeguimientoDhcpaResponse Consulta_Seguimiento_x_id_seguimiento(int id_seguimiento);
        /*IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta(int pageIndex, int pageSize, string expediente, string evaluador, string externo, string matricula, string cmbestado);*/
        IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_sin_paginado(string expediente, string evaluador, string externo, string habilitante, string cmbestado, int id_oficina_filtro, int id_tupa);
        IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_excel(int id_oficina); 
        /*int CountSeguimiento_Consulta(string expediente, string evaluador, string externo, string matricula, string cmbestado);*/
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa(string evaluador, int tipo_doc_dhcpa, string asunto, int anno);
        IEnumerable<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result> consulta_correo_x_solicitud(int id_solicitud);
        string enviar_correo_notificacion_solicitud_sdhpa(int id_solicitud, string destinos);
        IEnumerable<DocumentoDhcpaResponse> Lista_destino_documentos_dhcpa(int id_documento_dhcpa);
        IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_dhcpa();
        IEnumerable<SeguimientoDhcpaObservacionesResponse> Listar_Observacion_x_seguimiento(int id_seguimiento);
        IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> CONSULTA_HISTORIAL_EVALUADOR(int id_seguimiento);
        IEnumerable<ProtocoloResponse> lista_protocolo_x_id_transporte(int id_transporte);
        int Create_Persona_telefono(string persona_num_documento, string telefono, string usuario);
        IEnumerable<DocumentoSeguimientoResponse> Lista_Documento_OD_pendientes_x_recibir(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, string expediente); 
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento(int id_tipo_documento, int anno);
        IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_excel();
        IEnumerable<ConsultarPlantasResponse> Lista_plantas_excel();
        IEnumerable<DocumentoDhcpaResponse> Lista_Destino_Documentos_x_tipo_documento(int id_doc_dhcpa);
        SeguimientoDhcpaResponse Lista_protocolo_solicitud(int id_seguimiento);
        SeguimientoDhcpaResponse Lista_protocolo_seguimiento_planta(int id_planta);
        SeguimientoDhcpaResponse Lista_datos_evaluador(int id_seguimiento);
        IEnumerable<Response.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO_Result> lista_transportes_con_protocolo_habilitado();
        IEnumerable<Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result> Lista_acta_info_pru_por_si(int id_sol_ins);

        //Add by HM - 28/11/2019
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa_externos(string evaluador, int tipo_doc_dhcpa, string asunto, int anno, int oficina_direccion);
        //Add by HM - 28/11/2019
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento_oficina_direccion(int id_tipo_documento, int anno, int oficina_direccion);
        IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_externos(string evaluador, int tipo_doc_dhcpa, string asunto, int anno, int oficina_direccion);
    }
}

