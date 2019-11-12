using System;
using SIGESDOC.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Request;

namespace SIGESDOC.AplicacionService
{
    public class EntidadToRequest
    {
        public static DocumentoDhcpaRequest documentodhcpa(MAE_DOCUMENTO_DHCPA entidad)
        {
            DocumentoDhcpaRequest item = new DocumentoDhcpaRequest
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
                id_oficina_direccion = entidad.ID_OFICINA_DIRECCION
            };

            return item;
        }
    }
}
