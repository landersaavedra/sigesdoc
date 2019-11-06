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
    public partial class ConsultaFacturasRepositorio : IConsultaFacturasRepositorio
    {
        public Response.ConsultaFacturasResponse Guardar_Factura(string num1, string num2, DateTime fecha, decimal importe_total, string usuario, int id_tipo_factura,string ruc_dni,string nombre,string direccion,int id_sub_tupa, int cantidad, int id_ofi_crea)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.P_CREA_FACTURAS(num1, num2, fecha, importe_total, usuario, id_tipo_factura, ruc_dni, nombre, direccion, id_sub_tupa, cantidad, id_ofi_crea)
                         select new ConsultaFacturasResponse()
                         {
                             id_factura = r.ID_FACTURA,
                             num1_fact = r.NUM1_FACT,
                             num2_fact = r.NUM2_FACT,
                             fecha_fact = r.FECHA_FACT,
                             importe_total = r.IMPORTE_TOTAL,
                             id_tipo_factura = r.ID_TIPO_FACTURA
                         }).ToList().First();
            return result;
        }

        public int genera_reporte_comprobante_x_mes()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var rx = _dataContext.SP_EJECUTA_COMPROBANTE_X_MES_CONSULTA();

            return 1;
        }

        public Response.P_INSERT_UPDATE_MAE_OPERACION_Result Guardar_Operacion(int numero, DateTime fecha, int oficina, decimal abono,string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.P_INSERT_UPDATE_MAE_OPERACION(0, numero, fecha, oficina, abono, 0, usuario)
                         select new Response.P_INSERT_UPDATE_MAE_OPERACION_Result()
                         {
                             id_operacion = r.ID_OPERACION,
                             numero = r.NUMERO,
                             fecha_deposito = r.FECHA_DEPOSITO,
                             oficina = r.OFICINA,
                             abono = r.ABONO,
                             cargo = r.CARGO,
                             usuario_crea = r.USUARIO_CREA,
                             usuario_modifica = r.USUARIO_MODIFICA,
                             fecha_crea = r.FECHA_CREA,
                             fecha_modifica = r.FECHA_MODIFICA
                         }).ToList().First();                         
            return result;
        }


        public void update_db_general_mae_operacion(ConsultaDbGeneralMaeOperacionResponse ope_rq)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            _dataContext.P_UPDATE_MAE_OPERACION(ope_rq.id_operacion, ope_rq.numero, ope_rq.fecha_deposito, ope_rq.oficina, ope_rq.abono, 0, ope_rq.usuario_modifica, ope_rq.ruta_pdf);
            
        }


        public Response.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA_Result Guardar_det_fac_opera(int id_factura, int id_operacion)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA(0,id_operacion,id_factura)
                         select new Response.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA_Result()
                         {
                             id_det_ope_fact = r.ID_DET_OPE_FACT,
                             id_operacion = r.ID_OPERACION,
                             id_factura = r.ID_FACTURA
                         }).ToList().First();                         
            return result;
        }
        
    }
}
