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
    public partial class DbGeneralMaeDesembarcaderoRepositorio : IDbGeneralMaeDesembarcaderoRepositorio
    {

        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> GetAlldesembarcadero_sin_paginado(int id_tipo_desembarcadero, string codigo_desembarcadero, string externo)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.VW_DB_GENERAL_MAE_DESEMBARCADERO
                         where
                            (id_tipo_desembarcadero == 0 || (id_tipo_desembarcadero != 0 && r.ID_TIPO_DESEMBARCADERO == id_tipo_desembarcadero)) &&
                            r.ENTIDAD.Contains(externo) && r.CODIGO_DESEMBARCADERO.Contains(codigo_desembarcadero)
                         select new DbGeneralMaeDesembarcaderoResponse()
                         {
                             id_desembarcadero = r.ID_DESEMBARCADERO,
                             entidad = r.ENTIDAD,
                             nombre_tipo_desembarcadero = r.NOMBRE_TIPO_DESEMBARCADERO,
                             denominacion = r.DENOMINACION,
                             latitud = r.LATITUD,
                             longitud = r.LONGITUD,
                             codigo_desembarcadero = r.CODIGO_DESEMBARCADERO,
                             estado_desemb = r.ESTADO_DESEMB
                         };
            return result;
        }

        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> Guardar_Desembarcadero(int ID_DESEMBARCADERO, int ID_SEDE, int ID_TIPO_DESEMBARCADERO, int ID_COD_DESEMB, int NUM_DESEMB, string NOMBRE_DESEMB, string DENOMINACION, string TEMPORAL, double LATITUD, double LONGITUD, string USUARIO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_INSERT_UPDATE_DB_GENERAL_MAE_DESEMBARCADERO(ID_DESEMBARCADERO, ID_SEDE, ID_TIPO_DESEMBARCADERO, ID_COD_DESEMB, NUM_DESEMB, NOMBRE_DESEMB, DENOMINACION, TEMPORAL, LATITUD, LONGITUD, USUARIO)
                         select new DbGeneralMaeDesembarcaderoResponse()
                         {
                             id_desembarcadero = r.ID_DESEMBARCADERO,
                         };
            return result;
        }

        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> lista_desembarcadero_x_sede(int var_id_oficina_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from VW_DESEMB in _dataContext.VW_DB_GENERAL_MAE_DESEMBARCADERO

                         from VW_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                         .Where(VW_SEDE => VW_SEDE.ID_SEDE == VW_DESEMB.ID_SEDE)

                         from VW_DIR in _dataContext.vw_CONSULTAR_DIRECCION
                         .Where(VW_DIR => VW_DIR.ID_OFICINA_DIRECCION == var_id_oficina_dir && VW_DIR.ID_SEDE == VW_SEDE.ID_SEDE)

                         select new DbGeneralMaeDesembarcaderoResponse()
                         {
                             id_desembarcadero = VW_DESEMB.ID_DESEMBARCADERO,
                             codigo_desembarcadero = VW_DESEMB.CODIGO_DESEMBARCADERO,
                         };
            return result.Distinct();
        }
        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> genera_protocolo_desembarcadero()
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_DESEMBARCADERO()
                         select new DbGeneralMaeDesembarcaderoResponse()
                         {
                             genera_data_tipo_desembarcadero = r.tipo_desembarcadero,
                             genera_data_codigo_desembarcadero = r.codigo_desembarcadero,
                             genera_data_denominacion = r.denominacion,
                             genera_data_externo = r.externo,
                             genera_data_direccion = r.direccion,
                             genera_data_pesca_acuicultura = r.pesca_acuicultura,
                             genera_data_archivos = r.ARCHIVOS
                             
                         };
            return result;
        }
    }
}
