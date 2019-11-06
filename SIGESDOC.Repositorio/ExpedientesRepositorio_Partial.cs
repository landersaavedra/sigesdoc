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
    public partial class ExpedientesRepositorio : IExpedientesRepositorio
    {


        public ExpedientesResponse GetExpediente_x_id(int id_exp)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MEX in _dataContext.MAE_EXPEDIENTES

                          from MTE in _dataContext.MAE_TIPO_EXPEDIENTE
                               .Where(MTE => MEX.ID_TIPO_EXPEDIENTE == MTE.ID_TIPO_EXPEDIENTE)

                          where MEX.ID_EXPEDIENTE == id_exp

                          select new ExpedientesResponse
                          {
                              
                              tipo_expediente = new TipoExpedienteResponse
                              {
                                  nombre = MTE.NOMBRE
                              },
                              id_expediente = MEX.ID_EXPEDIENTE,
                              id_tipo_expediente = MEX.ID_TIPO_EXPEDIENTE,
                              numero_expediente = MEX.NUMERO_EXPEDIENTE,
                              fecha_registro = MEX.FECHA_REGISTRO,
                              usuario_registro = MEX.USUARIO_REGISTRO,
                              fecha_modifico = MEX.FECHA_MODIFICO,
                              usuario_modifico = MEX.USUARIO_MODIFICO,
                              indicador_seguimiento = MEX.INDICADOR_SEGUIMIENTO,
                              nom_expediente = MEX.NOM_EXPEDIENTE,
                              año_crea = MEX.AÑO_CREA
                          }).AsEnumerable();
            return result.ToList().First();
        }

        
        public IEnumerable<SubTupaResponse> recuperatupa(decimal monto)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (monto == 0)
            {

                var result = (from MTUP in _dataContext.MAE_TUPA

                              from MTTUP in _dataContext.MAE_TIPO_TUPA
                                   .Where(MTTUP => MTTUP.ID_TIPO_TUPA == MTUP.ID_TIPO_TUPA)

                              from MSTUP in _dataContext.MAE_SUB_TUPA
                                   .Where(MSTUP => MSTUP.ID_TUPA == MTUP.ID_TUPA)

                              select new SubTupaResponse
                              {
                                  id_sub_tupa = MSTUP.ID_SUB_TUPA,
                                  indice = MSTUP.INDICE,
                                  nombre = MSTUP.NOMBRE,
                                  precio = MSTUP.PRECIO,
                                  tupa = new TupaResponse
                                  {
                                      asunto = MTUP.ASUNTO,
                                      numero = MTUP.NUMERO,
                                      dias_tupa = MTUP.DIAS_TUPA,
                                      tipo_tupa = new TipoTupaResponse
                                      {
                                          nombre = MTTUP.NOMBRE
                                      }
                                  }
                              }).AsEnumerable();
                return result.ToList();
            }
            else
            {
                var result = (from MTUP in _dataContext.MAE_TUPA

                              from MTTUP in _dataContext.MAE_TIPO_TUPA
                                   .Where(MTTUP => MTTUP.ID_TIPO_TUPA == MTUP.ID_TIPO_TUPA)

                              from MSTUP in _dataContext.MAE_SUB_TUPA
                                   .Where(MSTUP => MSTUP.ID_TUPA == MTUP.ID_TUPA)

                              where MSTUP.PRECIO == monto

                              select new SubTupaResponse
                              {
                                  id_sub_tupa = MSTUP.ID_SUB_TUPA,
                                  indice = MSTUP.INDICE,
                                  nombre = MSTUP.NOMBRE,
                                  precio = MSTUP.PRECIO,
                                  tupa = new TupaResponse
                                  {
                                      asunto = MTUP.ASUNTO,
                                      numero = MTUP.NUMERO,
                                      dias_tupa = MTUP.DIAS_TUPA,
                                      tipo_tupa = new TipoTupaResponse
                                      {
                                          nombre = MTTUP.NOMBRE
                                      }
                                  }
                              }).AsEnumerable();
                return result.ToList();
            }
            
        }
        
        public IEnumerable<ExpedientesResponse> GetAllExpediente_sin_paginado(string numero_exp, int id_oficina_dir, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (id_oficina_dir == 28)
            {
                var result = (from MEX in _dataContext.MAE_EXPEDIENTES

                              from MTE in _dataContext.MAE_TIPO_EXPEDIENTE
                                   .Where(MTE => MEX.ID_TIPO_EXPEDIENTE == MTE.ID_TIPO_EXPEDIENTE)

                              from VWDNI in _dataContext.vw_CONSULTAR_DNI
                              .Where(VWDNI => MEX.USUARIO_REGISTRO.Replace("20565429656 - ", "") == VWDNI.persona_num_documento)
                              .DefaultIfEmpty() // <== makes join left join

                              where (MEX.NOM_EXPEDIENTE + "." + MTE.NOMBRE).Contains(numero_exp)

                              select new ExpedientesResponse
                              {
                                  id_expediente = MEX.ID_EXPEDIENTE,
                                  id_tipo_expediente = MEX.ID_TIPO_EXPEDIENTE,
                                  nom_expediente = MEX.NOM_EXPEDIENTE,
                                  año_crea = MEX.AÑO_CREA,
                                  usuario_registro = MEX.USUARIO_REGISTRO,
                                  fecha_registro = MEX.FECHA_REGISTRO,
                                  nom_usuario = VWDNI.paterno + " " + VWDNI.materno + " " + VWDNI.nombres,
                                  tipo_expediente = new TipoExpedienteResponse
                                  {
                                      nombre = MTE.NOMBRE
                                  },
                                  numero_expediente = MEX.NUMERO_EXPEDIENTE,
                                  indicador_seguimiento = MEX.INDICADOR_SEGUIMIENTO,
                                  estado_seguimiento = (MEX.INDICADOR_SEGUIMIENTO == "0" ? "SIN SEGUIMIENTO" : "CON SEGUIMIENTO")
                              }).OrderByDescending(r => r.año_crea).ThenByDescending(r => r.fecha_registro).ThenByDescending(r => r.id_tipo_expediente).ThenByDescending(r => r.numero_expediente).AsEnumerable().Take(500);
                return result;
            }
            else {
                var result = (from MEX in _dataContext.MAE_EXPEDIENTES

                              from MTE in _dataContext.MAE_TIPO_EXPEDIENTE
                                   .Where(MTE => MEX.ID_TIPO_EXPEDIENTE == MTE.ID_TIPO_EXPEDIENTE)
                              
                              from VWDNI in _dataContext.vw_CONSULTAR_DNI
                                     .Where(VWDNI => MEX.USUARIO_REGISTRO.Replace("20565429656 - ","") == VWDNI.persona_num_documento)
                                     .DefaultIfEmpty() // <== makes join left join

                              where (MEX.NOM_EXPEDIENTE + "." + MTE.NOMBRE).Contains(numero_exp) && MEX.USUARIO_REGISTRO == "20565429656 - " + usuario

                              select new ExpedientesResponse
                              {
                                  id_expediente = MEX.ID_EXPEDIENTE,
                                  id_tipo_expediente = MEX.ID_TIPO_EXPEDIENTE,
                                  nom_expediente = MEX.NOM_EXPEDIENTE,
                                  año_crea = MEX.AÑO_CREA,
                                  usuario_registro = MEX.USUARIO_REGISTRO,
                                  fecha_registro = MEX.FECHA_REGISTRO,
                                  nom_usuario = VWDNI.paterno +" "+ VWDNI.materno + " "+ VWDNI.nombres,
                                  tipo_expediente = new TipoExpedienteResponse
                                  {
                                      nombre = MTE.NOMBRE
                                  },
                                  numero_expediente = MEX.NUMERO_EXPEDIENTE,
                                  indicador_seguimiento = MEX.INDICADOR_SEGUIMIENTO,
                                  estado_seguimiento = (MEX.INDICADOR_SEGUIMIENTO == "0" ? "SIN SEGUIMIENTO" : "CON SEGUIMIENTO")
                              }).OrderByDescending(r => r.año_crea).ThenByDescending(r => r.fecha_registro).ThenByDescending(r => r.id_tipo_expediente).ThenByDescending(r => r.numero_expediente).AsEnumerable().Take(500);
                return result;
            }
            
        }
        
        public IEnumerable<ExpedientesResponse> Lista_expediente_sin_seguimiento()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MEX in _dataContext.MAE_EXPEDIENTES

                          from MTE in _dataContext.MAE_TIPO_EXPEDIENTE
                               .Where(MTE => MEX.ID_TIPO_EXPEDIENTE == MTE.ID_TIPO_EXPEDIENTE)

                          where MEX.INDICADOR_SEGUIMIENTO=="0"

                          select new ExpedientesResponse
                          {
                              id_expediente = MEX.ID_EXPEDIENTE,
                              id_tipo_expediente = MEX.ID_TIPO_EXPEDIENTE,
                              nom_expediente = MEX.NOM_EXPEDIENTE,
                              tipo_expediente = new TipoExpedienteResponse
                              {
                                  nombre = MTE.NOMBRE
                              },
                              numero_expediente = MEX.NUMERO_EXPEDIENTE,
                              indicador_seguimiento = MEX.INDICADOR_SEGUIMIENTO
                          }).OrderByDescending(r => r.id_expediente).AsEnumerable();
            return result;
        }
    }
}
