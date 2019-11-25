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
    public partial class ConsultarDbGeneralMaeConcesionRepositorio : IConsultarDbGeneralMaeConcesionRepositorio
    {
        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> GetAllconsecion_sin_paginado(int id_zona_produccion, int id_area_produccion, int id_tipo_concesion, string externo)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.VW_CONSULTAR_DB_GENERAL_MAE_CONCESION
                         where 
                            (id_zona_produccion == 0 || (id_zona_produccion!=0 && r.ID_ZONA_PRODUCCION == id_zona_produccion)) &&
                            (id_area_produccion == 0 || (id_area_produccion != 0 && r.ID_AREA_PRODUCCION == id_area_produccion)) &&
                            (id_tipo_concesion == 0 || (id_tipo_concesion != 0 && r.ID_TIPO_CONCESION == id_tipo_concesion)) &&
                            r.RAZON_SOCIAL.Contains(externo)
                         select new ConsultarDbGeneralMaeConcesionResponse()
                         {
                             id_concesion = r.ID_CONCESION,
                             razon_social = r.RAZON_SOCIAL,
                             tipo_concesion = r.TIPO_CONCESION,
                             codigo_habilitacion = r.CODIGO_HABILITACION,
                             partida_registral = r.PARTIDA_REGISTRAL,
                             ubicacion = r.UBICACION,
                             ubigeo = r.UBIGEO,
                             departamento = r.DEPARTAMENTO,
                             provincia = r.PROVINCIA,
                             distrito = r.DISTRITO,
                             cod_area_produccion = r.COD_AREA_PRODUCCION,
                             nombre_area_produccion = r.NOMBRE_AREA_PRODUCCION,
                             cod_zona_produccion = r.COD_ZONA_PRODUCCION,
                             nombre_zona_produccion = r.NOMBRE_ZONA_PRODUCCION
                         };
            return result;
        }

        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> Guardar_Concesion(int ID_CONCESION, string RUC, string CODIGO_HABILITACION, string PARTIDA_REGISTRAL, string UBICACION, string UBIGEO, int ID_AREA_PRODUCCION, int ID_TIPO_CONCESION, int ID_TIPO_ACTIVIDAD_CONCESION, string USUARIO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            
                var result = from r in _dataContext.P_INSERT_UPDATE_DB_GENERAL_MAE_CONCESION(ID_CONCESION, RUC, CODIGO_HABILITACION, PARTIDA_REGISTRAL, UBICACION, UBIGEO, ID_AREA_PRODUCCION, ID_TIPO_CONCESION, ID_TIPO_ACTIVIDAD_CONCESION,USUARIO)
                             select new ConsultarDbGeneralMaeConcesionResponse()
                             {
                                 id_concesion = r.ID_CONCESION,
                             };
                return result;
        }


        public ConsultarDbGeneralMaeConcesionResponse recupera_mae_concesion_x_id(int id_concesion)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MCONCES in _dataContext.VW_CONSULTAR_DB_GENERAL_MAE_CONCESION

                          from VWCTC in _dataContext.VW_CONSULTAR_DB_GENERAL_MAE_TIPO_CONCESION
                          .Where(VWCTC => MCONCES.ID_TIPO_CONCESION == VWCTC.ID_TIPO_CONCESION)

                          where MCONCES.ID_CONCESION==id_concesion

                          select new ConsultarDbGeneralMaeConcesionResponse
                          {
                              id_concesion = MCONCES.ID_CONCESION,
                              codigo_habilitacion = MCONCES.CODIGO_HABILITACION,
                              ruta_pdf = VWCTC.RUTA_PDF
                          }).Distinct().OrderBy(r => r.codigo_habilitacion).AsEnumerable();
            return result.First();
        }
        
        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> genera_protocolo_concesion()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_CONCESION()
                         select new ConsultarDbGeneralMaeConcesionResponse()
                         {
                             genera_data_externo = r.externo,
                             genera_data_actividad = r.actividad,
                             genera_data_codigo_concesion = r.codigo_concesion,
                             genera_data_departamento = r.departamento,
                             genera_data_provincia = r.provincia,
                             genera_data_distrito = r.distrito,
                             genera_data_ubicacion = r.ubicacion,
                             genera_data_archivos = r.archivos
                         };
            return result;
        }

    }
}
