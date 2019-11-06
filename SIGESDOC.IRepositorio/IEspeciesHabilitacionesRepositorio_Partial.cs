using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IEspeciesHabilitacionesRepositorio
    {
        IEnumerable<EspeciesHabilitacionesResponse> lista_especies_habilitaciones(string nombre_comun, string nombre_cientifico);
    }
}
