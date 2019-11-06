using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IDocumentoSeguimientoRepositorio
    {
        IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente);
        IEnumerable<ExpedientesResponse> GetAllExpediente_x_Documento(int id_documento_seg);
        IEnumerable<ConsultaFacturasResponse> GetAllfacturas_x_Documento(int id_documento_seg);
        IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcacion_x_documento(int id_documento_seg);
        IEnumerable<ConsultarPlantasResponse> GetAllPlanta_x_seguimiento(int id_documento_seg);
        IEnumerable<DocumentoSeguimientoResponse> lista_documentos_recibidos_x_seguimiento(int id_seguimiento);
        IEnumerable<DocumentoDhcpaResponse> lista_documentos_emitidos_dhcpa_x_seguimiento(int id_seguimiento);
        IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos_x_rec(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente);

    }
}
