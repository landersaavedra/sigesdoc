using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Contexto;
using SIGESDOC.IRepositorio;
using SIGESDOC.Response;

namespace SIGESDOC.Repositorio
{
    public partial class ConsultarDniRepositorio : IConsultarDniRepositorio
    {

        public IEnumerable<Response.SP_EDITA_DB_SEGURIDAD_PERSONA_Result> editar_persona(string persona_num_documento, string paterno, string materno, string nombres, string direccion, string ubigeo)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_EDITA_DB_SEGURIDAD_PERSONA(persona_num_documento, paterno,materno,nombres,direccion,ubigeo)
                         select new Response.SP_EDITA_DB_SEGURIDAD_PERSONA_Result()
                         {
                             persona_num_documento = r.persona_num_documento,
                             paterno = r.paterno,
                             materno = r.materno,
                             nombres = r.nombres,
                             fecha_nacimiento = r.fecha_nacimiento,
                             sexo = r.sexo,
                             ubigeo = r.ubigeo,
                             direccion = r.direccion
                         };
            return result;
        }

        public IEnumerable<Response.ConsultarDniResponse> CreaPersona(string persona_num_documento, byte tipo_doc_iden, string paterno, string materno, string nombres, DateTime fecha_nacimiento, string ubigeo, string sexo, string direccion, string ruc, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_CREA_PERSONAL(persona_num_documento, tipo_doc_iden, paterno, materno, nombres, fecha_nacimiento, ubigeo, sexo, direccion, ruc, usuario)
                         select new ConsultarDniResponse()
                         {
                             persona_num_documento = r.persona_num_documento,
                             paterno = r.paterno,
                             materno = r.materno,
                             nombres = r.nombres,
                             fecha_nacimiento = r.fecha_nacimiento,
                             sexo = r.sexo,
                             ubigeo = r.ubigeo,
                             direccion = r.direccion
                         };
            return result;
        }


        public IEnumerable<Response.ConsultarOficinaResponse> CreaEmpresa(string ruc,string nombre_empresa,string siglas,string nombre_sede,string direccion,string referencia,string ubigeo, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_CREA_OFICINA_PRINCIPAL(ruc,nombre_empresa,siglas,nombre_sede,direccion,referencia,ubigeo, usuario)
                         select new ConsultarOficinaResponse()
                         {
                             id_oficina = r.ID_OFICINA
                         };
            return result;
        }

        public IEnumerable<Response.ConsultarSedeOficinaResponse> crea_sede_secundaria(string nombre_sede,string direccion,string referencia,string ubigeo,int id_oficina, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_CREA_SEDE_SECUNDARIA(nombre_sede,direccion,referencia,ubigeo,id_oficina, usuario)
                         select new ConsultarSedeOficinaResponse()
                         {
                             id_sede = r.ID_SEDE,
                             nombre = r.NOMBRE,
                             direccion = r.DIRECCION,
                             referencia = r.REFERENCIA
                         };
            return result;
        }

        public IEnumerable<Response.ConsultarOficinaResponse> crea_oficina_secundaria(string nombre_oficina,int id_ofi_padre,string siglas,string ruc,int id_sede, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_CREA_OFICINA_DIRECCION(nombre_oficina,id_ofi_padre,siglas,ruc,id_sede,usuario)
                         select new ConsultarOficinaResponse()
                         {
                             id_oficina = r.ID_OFICINA,
                             id_ofi_padre = r.ID_OFI_PADRE,
                             nombre = r.NOMBRE,
                             siglas = r.SIGLAS,
                             ruc = r.RUC
                         };
            return result;
        }

        public ConsultarDniResponse actualizar_persona(string persona_num_documento, string direccion, string ubigeo, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.p_ACTUALIZA_PERSONA(persona_num_documento,1,"","","",DateTime.Now,ubigeo,"",direccion,"",usuario)
                         select new ConsultarDniResponse()
                         {
                             direccion = r.direccion,
                             persona_num_documento = r.persona_num_documento
                         }).First();
            return result;
        }
    }
}
