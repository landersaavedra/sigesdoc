using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultaEmbarcacionesRepositorio
    {
        IEnumerable<Response.ConsultaEmbarcacionesResponse> Guarda_Embarcacion(string matricula, string nombre, int id_tipo_embarcacion, string usuario, int codigo_hab, int num_cod_hab, string nom_cod_hab, int id_tipo_act_emb, string fecha_const);
        ConsultaEmbarcacionesResponse Recupera_Embarcacion(int id_seguimiento, int id_embarcacion);
        IEnumerable<Response.ConsultaEmbarcacionesResponse> GetAllEmbarcaciones_sin_paginado(string matricula, string nombre, int cmb_actividad);
        ConsultaEmbarcacionesResponse buscar_embarcacion_x_seguimiento(int id_seguimiento);
        IEnumerable<ConsultaEmbarcacionesResponse> genera_protocolo_embarcacion();
    }

}
