using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IDbGeneralMaeDesembarcaderoRepositorio
    {
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> GetAlldesembarcadero_sin_paginado(int id_tipo_desembarcadero, string codigo_desembarcadero, string externo);
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> Guardar_Desembarcadero(int ID_DESEMBARCADERO, int ID_SEDE, int ID_TIPO_DESEMBARCADERO, int ID_COD_DESEMB, int NUM_DESEMB, string NOMBRE_DESEMB, string DENOMINACION, string TEMPORAL, double LATITUD, double LONGITUD, string USUARIO);
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> lista_desembarcadero_x_sede(int var_id_oficina_dir);
        IEnumerable<DbGeneralMaeDesembarcaderoResponse> genera_protocolo_desembarcadero();
    }
}
