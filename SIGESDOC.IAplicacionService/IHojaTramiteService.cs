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
    public interface IHojaTramiteService
    {
        [OperationContract]
        IEnumerable<Response.SP_EDITA_DB_SEGURIDAD_PERSONA_Result> editar_persona(string persona_num_documento, string paterno, string materno, string nombres, string direccion, string ubigeo);
        /*01*/
        [OperationContract]
        bool Crear_Empresa(string ruc, string nombre, string siglas, string usuario);
        /*02*/
        [OperationContract]
        int Create(HojaTramiteRequest request);
        /*03*/
        [OperationContract]
        bool Update(HojaTramiteRequest request);
        /*04*/
        [OperationContract]
        int Documento_Create(DocumentoRequest request);
        /*05*/
        [OperationContract]
        bool Documento_Update(DocumentoRequest request);
        [OperationContract]
        int Documento_anexo_Insertar(DocumentoAnexoRequest request);
        [OperationContract]
        bool Documento_anexo_Update(DocumentoAnexoRequest request);
        /*06*/
        [OperationContract]
        int Create_Expediente(ExpedientesRequest request);
        /*07*/
        [OperationContract]
        string Create_numero(int tipo_ht);
        /*08*/
        [OperationContract]
        int Documento_detalle_Create(DocumentoDetalleRequest request);
        /*09*/
        [OperationContract]
        bool Documento_detalle_Update(DocumentoDetalleRequest request);
        /*10*/
        [OperationContract]
        IEnumerable<DocumentoResponse> GetAllDocumento(int id_documento);

        [OperationContract]
        DocumentoResponse GetAllDocumento_resp(int id_documento);

        [OperationContract]
        IEnumerable<DocumentoResponse> GetAllDocumento_lista_resp_x_ht(int numero_ht);
        
        [OperationContract]
        IEnumerable<DocumentoAnexoResponse> Lista_Documentos_anexos(int id_documento);

        [OperationContract]
        DocumentoAnexoResponse Documento_Anexo_HT(int id_documento_anexo);

        [OperationContract]
        DocumentoRequest GetAllDocumento_req(int id_documento);

        [OperationContract]
        DocumentoRequest GetAllDocumento_req_x_ht(int numero_ht);

        /*11*/
        [OperationContract]
        IEnumerable<DocumentoRequest> GetAllDocumento_x_Numero_HT_request(int numero);
        /*12*/
        [OperationContract]
        IEnumerable<DocumentoResponse> GetAllDocumento_x_Numero_HT(int numero);
        /*13*/
        [OperationContract]
        string Consult_tipo_docuemnto(int id_tipo_documento);
        /*14*/
        [OperationContract]
        HojaTramiteRequest GetAllHT_x_Numero_request(int numero);
        /*15*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllDocumentoDetalle(int id_det_documento);
        /*16*/
        [OperationContract]
        IEnumerable<HojaTramiteResponse> GetAllHT_x_HojaTramite(string HT);
        /*17*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> Consultar_Doc_detalle(int id_det_documento);
        /*18*/
        [OperationContract]
        IEnumerable<HojaTramiteResponse> GetAllHojaTramite_Padre();
        /*18*/
        [OperationContract]
        IEnumerable<Response.SP_CONSULTA_HISTORIAL_HT_Result> recupera_historial_ht(int numero);
        /*19*/
        [OperationContract]
        IEnumerable<ConsultarDniResponse> Consultar_DNI(string DNI);
        /*20*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consultar_RUC(string RUC);
        /*21*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consultar_RUC_X_NOM(string NOM);
        /*22*/
        [OperationContract]
        IEnumerable<ConsultarDireccionResponse> Consultar_DIRECCION(int ID_OFICINA);
        /*23*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos(int id_oficina_logeo, string HT, string Asunto, string Empresa, int id_ofi_crea, string cmbtupa);
        /*23*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int id_ofi_crea, string persona_num_documento, string cmbtupa);
        /*24*/
        [OperationContract]
        IEnumerable<DocumentoResponse> GetAllHT(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa);
        /*26*/
        [OperationContract]
        IEnumerable<DocumentoResponse> GetmisHT(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa);

        /*26*/
        [OperationContract]
        IEnumerable<DocumentoResponse> GetmisDoc(int id_oficina_logeo, string HT, string asunto, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa, string anexos, string Empresa);
        [OperationContract]
        IEnumerable<DocumentoResponse> Recupera_Documento(int id_oficina_logeo, int id_tipo_documento, int anio_doc);

        /*23*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllHoja_Tramite_x_PEDIDO_SIGA(int id_tipo_pedido_siga, int pedido_siga, int anno_siga, int id_oficina_dir);
        /*28*/
        [OperationContract]
        IEnumerable<ConsultarDniResponse> GetAllPersona_Natural(int pageIndex, int pageSize, string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE);
        /*29*/
        [OperationContract]
        int CountPersona_Natural(string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE);
        /*30*/
        [OperationContract]
        int CountHT();
        /*32*/
        [OperationContract]
        DocumentoResponse Consultar_HT(string HT);
        /*33*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllRecibidos(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string cmbtupa);

        /*33*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string persona_num_documento, string cmbtupa);

        /*35*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllDerivadas(int id_oficina_logeo, string HT, string Asunto, string Empresa,string cmbtupa);
        /*35*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetAllDerivadas_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, string persona_num_documento,string cmbtupa);

        /*37*/
        [OperationContract]
        bool Recibir_ht(int id, string usuario_logeo);
        /*38*/
        [OperationContract]
        bool Archivar_ht(int id, string usuario_logeo, string observacion);

        [OperationContract]
        bool cancelar_recepcion_ht(int id);
        /*39*/
        [OperationContract]
        bool Cancelar_Ht(int id, string usuario_logeo);
        /*40*/
        [OperationContract]
        bool Atender_ht(int id, string usuario_logeo, string observacion);
        /*40*/
        [OperationContract]
        bool Editar_Observacion_Detalle(int id, string usuario_logeo, string observacion);

        /*41*/
        [OperationContract]
        bool Derivar_HT(int id, string usuario_logeo);
        /*42*/
        [OperationContract]
        bool Asignar_HT(int id, string persona_num_documento);
        /*43*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetmisHT_archivados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        /*44*/
        [OperationContract]
        int CountmisHt_archivados(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        /*43*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetmisHT_finalizados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        /*44*/
        [OperationContract]
        int CountmisHt_finalizados(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);

        /*45*/
        [OperationContract]
        bool Quitar_Archivo_Atendido_ht(int id, string usuario_logeo, string observacion);
        /*46*/
        [OperationContract]
        IEnumerable<DocumentoDetalleResponse> GetmisHT_atendidos(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        /*47*/
        [OperationContract]
        int CountmisHt_atendidos(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        /*48*/
        [OperationContract]
        IEnumerable<ConsultarDniResponse> Consultar_DNI_x_NOM(string NOM, string TIPO);
        [OperationContract]
        IEnumerable<EstadoTramiteResponse> lista_estado_tramite();
        /*49*/
        [OperationContract]
        ConsultarDniResponse Recupera_persona_x_documento(string persona_num_doc);
        /*50*/
        [OperationContract]
        IEnumerable<ConsultarPersonalResponse> Recupera_oficina_x_persona(int pageIndex, int pageSize, string persona_num_doc);
        /*51*/
        [OperationContract]
        int Count_oficina_x_persona(string persona_num_doc);
        /*52*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consulta_Empresas();
        /*53*/
        [OperationContract]
        IEnumerable<ConsultarDniResponse> Consultar_DNI_total();

        [OperationContract]
        IEnumerable<VerPedientesGesdocResponse> lista_pendientes_sigesdoc(int aniodesde, int aniohasta);

        [OperationContract]
        IEnumerable<ConsultaPendientesSanipesDetalleResponse> lista_pendientes_sigesdoc_det(int aniodesde, int aniohasta);

        /*54*/
        [OperationContract]
        IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_ATENDER_Result> Export_Excel_documentos_ht_pendientes_por_atender(int id_oficina);

        [OperationContract]
        IEnumerable<Response.SP_CONSULTAR_REGISTRO_DE_USUARIO_Result> Consultar_registro_de_usuario(string usuario, int fechaini, int fechafin);

        [OperationContract]
        IEnumerable<Response.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS_Result> Export_Excel_documentos_ht_archivadas_atendidas(int id_oficina);

        /*54*/
        [OperationContract]
        IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR_Result> Export_Excel_documentos_ht_pendientes_por_recibir(int id_oficina);
        /*55*/
        [OperationContract]
        IEnumerable<Response.SP_EXCEL_HT_ENVIADAS_Result> Export_Excel_documentos_ht_enviadas(int id_oficina);
        [OperationContract]
        string genera_clave_documento_externo();
        [OperationContract]
        int Grabar_DocDetObservaciones(DocDetObservacionesRequest request);
        [OperationContract]
        IEnumerable<DocDetObservacionesResponse> Listar_Observacion_x_det_documento(int id_det_documento);

        [OperationContract]
        int Get_num_ext_Documento(int id_ht);

        [OperationContract]
        IEnumerable<ConsultarPendientesHtParaAdjuntar20190104Response> GetAllpendienteshtadjuntar(string expediente);

    }
}
