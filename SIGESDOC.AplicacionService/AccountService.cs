using SIGESDOC.Entidades;
using SIGESDOC.IAplicacionService;
using SIGESDOC.IRepositorio;
using SIGESDOC.Request;
using SIGESDOC.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGESDOC.AplicacionService
{
    public class AccountService : IAccountService
    {
        /*01*/
        private readonly IConsultarUsuarioRepositorio _consultarusuarioRepositorio;
        /*02*/
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(
            /*01*/  IConsultarUsuarioRepositorio consultarusuarioRepositorio,
            /*02*/  IUnitOfWork unitOfWork
            )
        {
            /*01*/
            _consultarusuarioRepositorio = consultarusuarioRepositorio;
            /*02*/
            _unitOfWork = unitOfWork;
        }

        /*01*/
        public string valida_usuario(string ruc, string persona_num_documento, string clave)
        {
            string Valor = _consultarusuarioRepositorio.Validar_Contraseña(ruc, clave, "", persona_num_documento, 0).First().persona_num_documento;

            return Valor;
        }
        /*02*/
        public ConsultarUsuarioResponse RecuperaDatos(string ruc, string persona_num_documento, int id_oficina_dir)
        {
            var result = (from zp in _consultarusuarioRepositorio.Listar(x => x.RUC == ruc && x.persona_num_documento == persona_num_documento && x.ID_OFICINA_DIRECCION == id_oficina_dir)
                          select new ConsultarUsuarioResponse
                          {
                              ruc = zp.RUC,
                              persona_num_documento = zp.persona_num_documento,
                              id_perfil = zp.ID_PERFIL,
                              empresa = zp.empresa,
                              persona = zp.persona,
                              perfil = zp.perfil,
                              id_perfil_jefe_od = zp.ID_PERFIL_JEFE_OD,
                              id_perfil_inspector_od = zp.ID_PERFIL_INSPECTOR_OD
                          }).First();
            return result;
        }
        /*03*/
        public bool Modificar_clave(string ruc, string persona_num_documento, string clave_ini, string clave_fin)
        {
            try
            {
                string Valor = _consultarusuarioRepositorio.Validar_Contraseña(ruc, clave_ini, clave_fin, persona_num_documento, 1).First().persona_num_documento;

                if (Valor == "NO")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }


        }
    }
}
