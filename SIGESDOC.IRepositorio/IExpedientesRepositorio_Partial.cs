using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IExpedientesRepositorio
    {
        IEnumerable<ExpedientesResponse> GetAllExpediente_sin_paginado(string numero_exp, int id_oficina_dir, string usuario);
        IEnumerable<ExpedientesResponse> Lista_expediente_sin_seguimiento();
        ExpedientesResponse GetExpediente_x_id(int id_exp);
        IEnumerable<SubTupaResponse> recuperatupa(decimal monto);
    }
}
