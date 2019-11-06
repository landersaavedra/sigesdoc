using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultaSolicitudInspeccionOdRepositorio
    {
        IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp(string usuario, int id_ofi_dir);
        IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp_x_inspector(string usuario, int id_ofi_dir, string inspector);
    }
}
