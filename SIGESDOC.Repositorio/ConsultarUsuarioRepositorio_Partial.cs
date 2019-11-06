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
    public partial class ConsultarUsuarioRepositorio : IConsultarUsuarioRepositorio
    {
        public IEnumerable<Response.ConsultarUsuarioResponse> ModificarContraseña(string ruc, string persona_num_documento, string clave_ini, string clave_fin)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_CAMBIO_CONTRASEÑA(ruc, persona_num_documento, clave_ini, clave_fin)
                         select new ConsultarUsuarioResponse()
                         {
                             ruc = r.RUC,
                             persona_num_documento = r.persona_num_documento
                         };

            return result;
        }

        public IEnumerable<Response.ConsultarUsuarioResponse> Validar_Contraseña(string ruc, string clave, string clave_fin, string persona_num_documento, int proceso)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            // PROCESO SI ES 0 CONSULTA CLAVE --SI ES 1 MODIFICA CLAVE
            var result = from r in _dataContext.P_SEGURIDAD_CONSULTAR_USUARIO(ruc, clave, clave_fin, persona_num_documento, proceso)
                         select new Response.ConsultarUsuarioResponse()
                         {
                             persona_num_documento = r.VALOR
                         };

            return result;
        }

    }
}
