using System;
using SIGESDOC.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.AplicacionService
{
    public class EntidadToResponse
    {
        public static DocumentoDhcpaResponse documentodhcpa(MAE_DOCUMENTO_DHCPA entidad)
        {
            DocumentoDhcpaResponse item = new DocumentoDhcpaResponse
            {
                id_doc_dhcpa = entidad.ID_DOC_DHCPA,
                id_tipo_documento = entidad.ID_TIPO_DOCUMENTO,
                num_doc = entidad.NUM_DOC,
                nom_doc = entidad.NOM_DOC,
                fecha_doc = entidad.FECHA_DOC,
                asunto = entidad.ASUNTO,
                anexos = entidad.ANEXOS,
                fecha_registro = entidad.FECHA_REGISTRO,
                usuario_registro = entidad.USUARIO_REGISTRO,
                id_archivador = entidad.ID_ARCHIVADOR,
                id_filial = entidad.ID_FILIAL,
                numero_ht = entidad.NUMERO_HT,
                pdf = entidad.PDF,
                id_oficina_direccion = entidad.ID_OFICINA_DIRECCION,
                ruc = entidad.RUC

            };
            return item;
        }
        
        public static ConsultaDbGeneralMaeOperacionResponse dbgeneraloperacion(VW_CONSULTA_DB_GENERAL_MAE_OPERACION entidad)
        {
            ConsultaDbGeneralMaeOperacionResponse item = new ConsultaDbGeneralMaeOperacionResponse
            {
                id_operacion = entidad.ID_OPERACION,
                fecha_deposito = entidad.FECHA_DEPOSITO,
                abono = entidad.ABONO,
                cargo = entidad.CARGO,
                oficina = entidad.OFICINA,
                factura = entidad.FACTURA,
                numero = entidad.NUMERO,
                usuario_crea = entidad.USUARIO_CREA,
                fecha_crea = entidad.FECHA_CREA,
                usuario_modifica = entidad.USUARIO_MODIFICA,
                fecha_modifica = entidad.FECHA_MODIFICA,
                ruta_pdf = entidad.RUTA_PDF
            };
            return item;
        }
    }
}
