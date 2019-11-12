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
    public partial class ConsultarDbGeneralMaeAlmacenSedeRepositorio : IConsultarDbGeneralMaeAlmacenSedeRepositorio
    {

        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> GetAllAlmacenes_sin_paginado(string CODIGO_ALMACEN, int ID_ACTIVIDAD_ALMACEN, int ID_FILIAL, string EXTERNO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_CONSULTAR_MAE_ALMACEN_SEDE(CODIGO_ALMACEN, ID_ACTIVIDAD_ALMACEN, ID_FILIAL,"1",EXTERNO)
                         select new ConsultarDbGeneralMaeAlmacenSedeResponse()
                         {

                             id_almacen = r.ID_ALMACEN,
                             externo = r.EXTERNO,
                             direccion = r.DIRECCION,
                             nom_cod_habilitante = r.COD_HABILITANTE,
                             nom_actividad = r.ACTIVIDAD,
                             nom_filial = r.FILIAL
                         };
            return result;
        }

        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> Guarda_Almacen(int ID_ALMACEN, int ID_SEDE, int ID_CODIGO_ALMACEN, int NUM_ALMACEN, string NOM_ALMACEN, int ID_FILIAL, int ID_ACTIVIDAD_ALMACEN, string USUARIO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_INSERT_UPDATE_MAE_ALMACEN_SEDE(ID_ALMACEN, ID_SEDE, ID_CODIGO_ALMACEN, NUM_ALMACEN, NOM_ALMACEN, ID_FILIAL, ID_ACTIVIDAD_ALMACEN, USUARIO)
                         select new ConsultarDbGeneralMaeAlmacenSedeResponse()
                         {
                             id_almacen = r.ID_ALMACEN
                         };
            return result;
        }

        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> lista_almacen(string COD_ALMACEN, int var_id_oficina_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MALMA in _dataContext.vw_CONSULTAR_DB_GENERAL_MAE_ALMACEN_SEDE

                          from MCOD in _dataContext.vw_CONSULTAR_COD_HAB_ALMACEN
                               .Where(MCOD => MALMA.ID_CODIGO_ALMACEN == MCOD.ID_CODIGO_ALMACEN)

                          from MACTV in _dataContext.vw_CONSULTAR_ACTV_ALMACEN
                          .Where(MACTV => MALMA.ID_ACTIVIDAD_ALMACEN == MACTV.ID_ACTIVIDAD_ALMACEN)

                          from VW_DIR in _dataContext.vw_CONSULTAR_DIRECCION
                          .Where(VW_DIR => MALMA.ID_SEDE == VW_DIR.ID_SEDE)
                          
                          where MALMA.CODIGO_HABILITANTE.Contains(COD_ALMACEN) && VW_DIR.ID_OFICINA_DIRECCION == var_id_oficina_dir

                          select new ConsultarDbGeneralMaeAlmacenSedeResponse
                          {
                              id_almacen = MALMA.ID_ALMACEN,
                              nom_cod_habilitante = MALMA.CODIGO_HABILITANTE,
                              nom_actividad = MACTV.NOMBRE_ACTIVIDAD                              
                          }).OrderBy(r => r.id_almacen).AsEnumerable();
            return result;
        }



        public ConsultarDbGeneralMaeAlmacenSedeResponse recupera_almacen_x_id(int id_almacen)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MALMA in _dataContext.vw_CONSULTAR_DB_GENERAL_MAE_ALMACEN_SEDE

                          from MCOD in _dataContext.vw_CONSULTAR_COD_HAB_ALMACEN
                               .Where(MCOD => MALMA.ID_CODIGO_ALMACEN == MCOD.ID_CODIGO_ALMACEN)

                          from MACTV in _dataContext.vw_CONSULTAR_ACTV_ALMACEN
                          .Where(MACTV => MALMA.ID_ACTIVIDAD_ALMACEN == MACTV.ID_ACTIVIDAD_ALMACEN)

                          from VW_DIR in _dataContext.vw_CONSULTAR_DIRECCION
                          .Where(VW_DIR => MALMA.ID_SEDE == VW_DIR.ID_SEDE)

                          where MALMA.ID_ALMACEN==id_almacen

                          select new ConsultarDbGeneralMaeAlmacenSedeResponse
                          {
                              id_almacen = MALMA.ID_ALMACEN,
                              nom_cod_habilitante = MALMA.CODIGO_HABILITANTE,
                              nom_actividad = MACTV.NOMBRE_ACTIVIDAD,
                              id_actividad_almacen = MALMA.ID_ACTIVIDAD_ALMACEN,
                              ruta_pdf = MACTV.RUTA_PDF
                          }).OrderBy(r => r.id_almacen).First();
            return result;
        }


        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> genera_protocolo_almacen()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_ALMACEN()
                         select new ConsultarDbGeneralMaeAlmacenSedeResponse()
                         {
                             genera_data_externo = r.EXTERNO,
                             genera_data_direccion = r.DIRECCION,
                             genera_data_actividad = r.ACTIVIDAD,
                             genera_data_codigo_habilitacion = r.CODIGO_HABILITACION,
                             genera_data_archivo = r.ARCHIVOS
                         };
            return result;
        }

    }
}
