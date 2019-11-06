using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGESDOC.Response;

namespace SIGESDOC.Web.Models
{
    public class ResponseToModel
    {

        public static HojaTramiteViewModel HojaTramite(DocumentoResponse response)
        {
            HojaTramiteViewModel item = new HojaTramiteViewModel
            {
               id_documento = response.id_documento,
               numero = response.numero,
               nombre_tipo_documento_tramite = response.tipo_documento.nombre,
               numero_documento = response.numero_documento,
               nom_doc = response.nom_doc,
               persona_crea = response.persona_crea,
               asunto = response.hoja_tramite.asunto,
               nombre_tipo_tramite = response.hoja_tramite.nombre_tipo_tramite,
               nombre_oficina_tramite = response.hoja_tramite.nombre_oficina,
               numero_HT = response.numero,
               referencia = response.hoja_tramite.referencia,
               Hoja_Tramite = response.hoja_tramite.hoja_tramite,
               id_expediente = response.hoja_tramite.id_expediente,
               editar = response.hoja_tramite.editar
            };

            return item;
        }
    }
}