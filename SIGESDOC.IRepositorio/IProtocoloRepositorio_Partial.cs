using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IProtocoloRepositorio
    {
        IEnumerable<ProtocoloResponse> GetAllProtocolo_x_planta(int id_planta);
        int Generar_numero_protocolo_transporte(int anno);


        
        
    }
}
