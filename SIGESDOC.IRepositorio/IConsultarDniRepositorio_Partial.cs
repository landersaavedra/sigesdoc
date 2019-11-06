using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarDniRepositorio
    {
        IEnumerable<Response.SP_EDITA_DB_SEGURIDAD_PERSONA_Result> editar_persona(string persona_num_documento, string paterno, string materno, string nombres, string direccion, string ubigeo);
        IEnumerable<Response.ConsultarDniResponse> CreaPersona(string persona_num_documento, byte tipo_doc_iden, string paterno, string materno, string nombres, DateTime fecha_nacimiento, string ubigeo, string sexo, string direccion, string ruc, string usuario);
        IEnumerable<Response.ConsultarOficinaResponse> CreaEmpresa(string ruc, string nombre_empresa, string siglas, string nombre_sede, string direccion, string referencia, string ubigeo, string usuario);
        IEnumerable<Response.ConsultarSedeOficinaResponse> crea_sede_secundaria(string nombre_sede, string direccion, string referencia, string ubigeo, int id_oficina, string usuario);
        IEnumerable<Response.ConsultarOficinaResponse> crea_oficina_secundaria(string nombre_oficina, int id_ofi_padre, string siglas, string ruc, int id_sede, string usuario);
        ConsultarDniResponse actualizar_persona(string persona_num_documento, string direccion, string ubigeo, string usuario);
    }
}
