using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Entidades;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IHojaTramiteRepositorio
    {
        IEnumerable<Response.SP_CONSULTAR_REGISTRO_DE_USUARIO_Result> Consultar_registro_de_usuario(string usuario, int fechaini, int fechafin);
        IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_ATENDER_Result> Export_Excel_documentos_ht_pendientes_por_atender(int id_oficina);
        IEnumerable<Response.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS_Result> Export_Excel_documentos_ht_archivadas_atendidas(int id_oficina);
        IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR_Result> Export_Excel_documentos_ht_pendientes_por_recibir(int id_oficina);
        IEnumerable<Response.SP_EXCEL_HT_ENVIADAS_Result> Export_Excel_documentos_ht_enviadas(int id_oficina);
        IEnumerable<ExpedientesResponse> GetallRecupera_expediente();
        
        IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos(int id_oficina_logeo, string HT, string Asunto, string empresa, int id_ofi_crea, string cmbtupa);
        IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int id_ofi_crea, string persona_num_documento, string cmbtupa); 
        IEnumerable<DocumentoDetalleResponse> GetAllHoja_Tramite_x_PEDIDO_SIGA(int id_tipo_pedido_siga, int pedido_siga, int anno_siga, int id_oficina_dir);
        IEnumerable<DocumentoResponse> GetAllHT(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa); 
        IEnumerable<DocumentoResponse> GetmisHT(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa); 
        IEnumerable<DocumentoResponse> GetmisDoc(int id_oficina_logeo, string HT, string asunto, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa, string anexos, string Empresa);

        IEnumerable<DocumentoResponse> GetAllDocumento_lista_resp_x_ht(int numero_ht);

        IEnumerable<DocumentoDetalleResponse> GetmisHT_finalizados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        int CountmisHt_finalizados(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);

        IEnumerable<DocumentoDetalleResponse> GetmisHT_archivados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        int CountmisHt_archivados(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);

        IEnumerable<DocumentoDetalleResponse> GetmisHT_atendidos(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);
        int CountmisHt_atendidos(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento);


        IEnumerable<ConsultarDniResponse> GetAllPersona_Natural(int pageIndex, int pageSize, string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE);
        int CountPersona_Natural(string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE);
        IEnumerable<DocumentoDetalleResponse> GetAllRecibidos(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string cmbtupa);
        IEnumerable<DocumentoDetalleResponse> GetAllRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string persona_num_documento, string cmbtupa);
        IEnumerable<DocumentoDetalleResponse> GetAllDerivadas(int id_oficina_logeo, string HT, string Asunto, string Empresa,string cmbtupa);
        IEnumerable<DocumentoDetalleResponse> GetAllDerivadas_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, string persona_num_documento,string cmbtupa);
        DocumentoResponse Consultar_HT(string HT);
        bool Crear_Empresa(string ruc, string nombre, string siglas, string usuario);

        IEnumerable<ConsultarOficinaResponse> GetallOficina_x_sede(int sede);
        
        IEnumerable<ConsultarDireccionResponse> GetAllEmpresa_RUC(string CONSUL_RUC);


        IEnumerable<ConsultarDireccionResponse> GetAll_Oficinas_Direcciones(string RUC);

        IEnumerable<ConsultarOficinaResponse> GetAll_Oficinas_Direcciones_X_NOM(string NOM);

        IEnumerable<ConsultarDireccionResponse> Getall_Direccion_x_Oficina(int ID_OFICINA);

        IEnumerable<ConsultarDireccionResponse> GetAll_Empresas_con_Oficinas();




        IEnumerable<Response.ConsultarOficinaResponse> OF_GetallOficina_x_RUC_NOMBRE(int pageIndex, int pageSize, string RUC, string NOMBRE);
        int OF_CountOficina_x_RUC_NOMBRE(string RUC, string NOMBRE);
        IEnumerable<Response.ConsultarDireccionResponse> OF_GetallOficina_DIR_x_RUC(int pageIndex, int pageSize, string CONS_RUC);
        int OF_CountOficina_DIR_x_RUC(string RUC);
        Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_Result Consultar_documentos_pendientes(string documento, int id_ofi_dir);
        IEnumerable<Response.SP_CONSULTA_HISTORIAL_HT_Result> recupera_historial_ht(int numero);
        IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result> Consultar_documentos_pendientes_detalle(string documento, int id_ofi_dir);
        IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result> Consultar_documentos_pendientes_detalle_desagregado(string documento, int id_ofi_dir, string fecha);
        string genera_clave_documento_externo();
        IEnumerable<DocDetObservacionesResponse> Listar_Observacion_x_det_documento(int id_det_documento);
       // IEnumerable<Consulta_Detalle_Mae_Documento> Listar_Detalle_Documento_Interno(string num_doc);

    }
}
