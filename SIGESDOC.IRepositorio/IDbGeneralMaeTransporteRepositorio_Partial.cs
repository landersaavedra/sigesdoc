using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IDbGeneralMaeTransporteRepositorio
    {
        IEnumerable<DbGeneralMaeTransporteResponse> genera_protocolo_transporte();
    }
}
