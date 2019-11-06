using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Request;
using SIGESDOC.Response;

namespace SIGESDOC.IAplicacionService
{
    [ServiceContract]
    public interface IAccountService
    {
        /*01*/
        [OperationContract]
        string valida_usuario(string ruc, string persona_num_documento, string clave);
        /*02*/
        [OperationContract]
        ConsultarUsuarioResponse RecuperaDatos(string ruc, string persona_num_documento, int id_oficina_dir);
        /*03*/
        [OperationContract]
        bool Modificar_clave(string ruc, string persona_num_documento, string clave_ini, string clave_fin);
    }
}
