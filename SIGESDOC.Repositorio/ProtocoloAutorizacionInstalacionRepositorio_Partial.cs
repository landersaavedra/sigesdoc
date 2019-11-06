using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Contexto;
using SIGESDOC.IRepositorio;
using SIGESDOC.Response;

namespace SIGESDOC.Repositorio
{
    public partial class ProtocoloAutorizacionInstalacionRepositorio : IProtocoloAutorizacionInstalacionRepositorio
    {

        public IEnumerable<ProtocoloAutorizacionInstalacionResponse> genera_protocolo_autorizacion_instalacion()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_AUTORIZACION_INSTALACION()
                         select new ProtocoloAutorizacionInstalacionResponse()
                         {
                             genera_data_protocolo = r.protocolo,
                             genera_data_fecha = r.fecha,
                             genera_data_fecha_id = r.fecha_id,
                             genera_data_proposito = r.proposito,
                             genera_data_establecimiento = r.establecimiento,
                             genera_data_actividad = r.actividad,
                             genera_data_ubicacion = r.ubicacion,
                             genera_data_departamento = r.departamento,
                             genera_data_provincia = r.provincia,
                             genera_data_distrito = r.distrito,
                             genera_data_ruta = r.ruta,
                             genera_data_pdf = r.pdf,
                             genera_data_annio = r.annio
                         };
            return result;
        }


    }
}
