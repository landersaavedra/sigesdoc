using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Response;

namespace SIGESDOC.IRepositorio
{
    public partial interface IConsultaFacturasRepositorio
    {
        Response.ConsultaFacturasResponse Guardar_Factura(string num1, string num2, DateTime fecha, decimal importe_total, string usuario, int id_tipo_factura, string ruc_dni, string nombre, string direccion, int id_sub_tupa, int cantidad, int id_ofi_crea);
        Response.P_INSERT_UPDATE_MAE_OPERACION_Result Guardar_Operacion(int numero, DateTime fecha, int oficina, decimal abono, string usuario); 
        Response.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA_Result Guardar_det_fac_opera(int id_factura, int id_operacion);
        int genera_reporte_comprobante_x_mes();
        void update_db_general_mae_operacion(ConsultaDbGeneralMaeOperacionResponse ope_rq);
    }
}
