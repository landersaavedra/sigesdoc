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
    public partial class DbGeneralMaeTransporteRepositorio : IDbGeneralMaeTransporteRepositorio
    {


        public IEnumerable<DbGeneralMaeTransporteResponse> genera_protocolo_transporte()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_GENERA_DATA_PROTOCOLO_TRANSPORTE()
                         select new DbGeneralMaeTransporteResponse()
                         {
                             genera_data_externo = r.externo,
                             genera_data_Placa = r.Placa,
                             genera_data_cod_Habilitacion = r.cod_Habilitacion,
                             genera_data_protocolo = r.protocolo,
                             genera_data_fecha = r.fecha,
                             genera_data_fecha_id = r.fecha_id,
                             genera_data_ruta = r.ruta,
                             genera_data_pdf = r.pdf
                         };
            return result;
        }

    }
}
