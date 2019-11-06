using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultarDbGeneralMaeAlmacenSedeRepositorio
    {
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> GetAllAlmacenes_sin_paginado(string CODIGO_ALMACEN, int ID_ACTIVIDAD_ALMACEN, int ID_FILIAL, string EXTERNO);
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> Guarda_Almacen(int ID_ALMACEN, int ID_SEDE, int ID_CODIGO_ALMACEN, int NUM_ALMACEN, string NOM_ALMACEN, int ID_FILIAL, int ID_ACTIVIDAD_ALMACEN, string USUARIO);
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> lista_almacen(string COD_ALMACEN, int var_id_oficina_dir);
        ConsultarDbGeneralMaeAlmacenSedeResponse recupera_almacen_x_id(int  id_almacen);
        IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> genera_protocolo_almacen();
    }
}
