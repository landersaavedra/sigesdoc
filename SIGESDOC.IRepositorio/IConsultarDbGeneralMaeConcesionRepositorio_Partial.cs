using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarDbGeneralMaeConcesionRepositorio
    {
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> GetAllconsecion_sin_paginado(int id_zona_produccion, int id_area_produccion, int id_tipo_concesion, string externo);
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> Guardar_Concesion(int ID_CONCESION, string RUC, string CODIGO_HABILITACION, string PARTIDA_REGISTRAL, string UBICACION, string UBIGEO, int ID_AREA_PRODUCCION, int ID_TIPO_CONCESION, int ID_TIPO_ACTIVIDAD_CONCESION, string USUARIO);
        
        ConsultarDbGeneralMaeConcesionResponse recupera_mae_concesion_x_id(int id_concesion);
        IEnumerable<ConsultarDbGeneralMaeConcesionResponse> genera_protocolo_concesion();
        
    }
}
