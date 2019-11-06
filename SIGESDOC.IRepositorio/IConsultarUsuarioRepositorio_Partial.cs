using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarUsuarioRepositorio
    {
        IEnumerable<ConsultarUsuarioResponse> ModificarContraseña(string ruc, string persona_num_documento, string clave_ini, string clave_fin);
        IEnumerable<ConsultarUsuarioResponse> Validar_Contraseña(string ruc, string clave, string clave_fin, string persona_num_documento, int proceso);
    }
}
