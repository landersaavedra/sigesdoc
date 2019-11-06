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
    public partial class EspeciesHabilitacionesRepositorio : IEspeciesHabilitacionesRepositorio
    {

        public IEnumerable<EspeciesHabilitacionesResponse> lista_especies_habilitaciones(string nombre_comun, string nombre_cientifico)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from DESP_HAB in _dataContext.DAT_ESPECIES_HABILITACIONES

                          from VCESP in _dataContext.VW_CONSULTAR_ESPECIES
                               .Where(VCESP => DESP_HAB.CODIGO_ESPECIE == VCESP.CODIGO)

                          from VCESPCAT in _dataContext.VW_CONSULTAR_ESPECIES_CATEGORIAS
                          .Where(VCESPCAT => VCESPCAT.FLAG == VCESP.FLAG)

                          where VCESP.nombre_comun.Contains(nombre_comun) && VCESP.Nombre_Cientifico.Contains(nombre_cientifico)

                          select new EspeciesHabilitacionesResponse
                          {
                              id_det_espec_hab = DESP_HAB.ID_DET_ESPEC_HAB,
                              nombre_comun = VCESP.nombre_comun,
                              nombre_cientifico = VCESP.Nombre_Cientifico,
                              especie_categoria = VCESPCAT.NOMBRE
                          }).OrderBy(r => r.id_det_espec_hab).AsEnumerable();
            return result;
        }


    }
}
