using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarOficinaRepositorio
    {
        IEnumerable<Response.SP_EDITA_DB_GENERAL_MAE_SEDE_Result> Edita_db_general_mae_sede(int id_sede, string direccion, string ubigeo, string sede, string referencia);
        IEnumerable<ConsultarPersonalResponse> Asigna_oficina_persona(string persona_num_documento, int id_oficina_dir, string usuario);
        IEnumerable<ConsultarPersonalResponse> quita_oficina_persona(int id_per_empresa, string usuario);
        IEnumerable<ConsultarOficinaDireccionLegalResponse> insert_update_direccion_legal(int id_direccion_legal, string ruc, int id_sede, string usuario);
        IEnumerable<ConsultarEmpresaPersonaLegalResponse> insert_update_persona_legal(int id_persona_legal, string documento, string telefono, string correo, string RUC, string USUARIO);
        IEnumerable<ConsultarDniPersonalLegalResponse> insertar_actualizar_persona_legal_DNI(int id_dni_persona_legal, string documento, string telefono, string correo, string DNI, string USUARIO);
        string Recupera_RUC_x_ID_OFI_DIR(int id_ofi_dir);
        IEnumerable<Response.SP_ACTUALIZA_NOM_EMPRESA_Result> Edita_db_general_nom_empresa(string nombres, string ruc, string usuario);
        
    }
}
