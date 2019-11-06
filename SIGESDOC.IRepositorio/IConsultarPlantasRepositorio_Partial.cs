using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarPlantasRepositorio
    {
        IEnumerable<Response.ConsultarPlantasResponse> Guarda_Plantas(int id_sede, int id_tipo_planta, int numero_planta, string nombre_planta, int id_tipo_actividad, int id_filial, string usuario);

        IEnumerable<ConsultarPlantasResponse> GetAllPlantas_sin_paginado(string id_tipo_planta, string var_numero, string var_nombre, int var_id_filial, int var_id_actividad, string var_entidad);
        IEnumerable<ConsultarPlantasResponse> Consulta_planta(int id_direccion, string activo);
        ConsultarPlantasResponse Recupera_Planta(int id_seguimiento, int id_planta);
        bool Actualiza_habilitacion_planta(DateTime fecha_habilitacion_final, int id_planta);
        IEnumerable<ConsultarPlantasResponse> genera_protocolo_planta();
        
    }
}
