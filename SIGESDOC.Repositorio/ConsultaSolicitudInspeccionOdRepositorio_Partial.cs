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
    public partial class ConsultaSolicitudInspeccionOdRepositorio : IConsultaSolicitudInspeccionOdRepositorio
    {

        public IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp(string usuario, int id_ofi_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_RECIBIR_MAE_SOLICITUD_INSPECCION(usuario, id_ofi_dir)
                         select new SolicitudInspeccionResponse()
                         {
                             numero_documento = r.NUMERO_DOCUMENTO,
                             id_seguimiento = r.ID_SEGUIMIENTO,
                             año_crea = r.AÑO_CREA
                         };

            return result;
        }

        public IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp_x_inspector(string usuario, int id_ofi_dir, string inspector)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_RECIBIR_MAE_SOLICITUD_INSPECCION_X_INSPECTOR(usuario, id_ofi_dir, inspector)
                         select new SolicitudInspeccionResponse()
                         {
                             numero_documento = r.NUMERO_DOCUMENTO,
                             id_seguimiento = r.ID_SEGUIMIENTO,
                             año_crea = r.AÑO_CREA
                         };

            return result;
        }
    }
}
