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
    public partial class ConsultaEmbarcacionesRepositorio : IConsultaEmbarcacionesRepositorio
    {
        public IEnumerable<Response.ConsultaEmbarcacionesResponse> Guarda_Embarcacion(string matricula, string nombre,int id_tipo_embarcacion, string usuario, int codigo_hab, int num_cod_hab, string nom_cod_hab, int id_tipo_act_emb, string fecha_const)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            
            if (fecha_const != null)
            {
                DateTime var_fec_const = Convert.ToDateTime(fecha_const);
                var result = from r in _dataContext.P_CREA_EMBARCACIONES(matricula, nombre, id_tipo_embarcacion, usuario, codigo_hab, num_cod_hab, nom_cod_hab, id_tipo_act_emb, var_fec_const)
                             select new ConsultaEmbarcacionesResponse()
                             {
                                 id_embarcacion = r.ID_EMBARCACION,
                                 matricula = r.MATRICULA,
                                 nombre = r.NOMBRE
                             };
                return result;
            }
            else
            {
                var result = from r in _dataContext.P_CREA_EMBARCACIONES(matricula, nombre, id_tipo_embarcacion, usuario, codigo_hab, num_cod_hab, nom_cod_hab, id_tipo_act_emb, null)
                             select new ConsultaEmbarcacionesResponse()
                             {
                                 id_embarcacion = r.ID_EMBARCACION,
                                 matricula = r.MATRICULA,
                                 nombre = r.NOMBRE
                             };
                return result;
            }
        }

        public ConsultaEmbarcacionesResponse Recupera_Embarcacion(int id_seguimiento, int id_embarcacion)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCE in _dataContext.VW_CONSULTA_EMBARCACIONES

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                               .Where(MSEG => VCE.ID_EMBARCACION == MSEG.ID_HABILITANTE && MSEG.ID_TIPO_SEGUIMIENTO == 2)
                           .DefaultIfEmpty() // <== makes join left join

                          where (id_seguimiento == 0 || (MSEG.ID_SEGUIMIENTO == id_seguimiento && id_seguimiento != 0))
                          && (id_embarcacion == 0 || (MSEG.ID_HABILITANTE == id_embarcacion && id_embarcacion != 0))
                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = VCE.ID_EMBARCACION,
                              id_tipo_embarcacion = VCE.ID_TIPO_EMBARCACION,
                              //activo = VCP.ACTIVO,
                              nombre_estado = VCE.ACTIVO == "0" ? "Desactivado" : " Activo"
                          }).OrderBy(r => r.id_embarcacion).Distinct().AsEnumerable();

            if (result.Count() > 0)
            {
                return result.First();
            }
            else
            {
                ConsultaEmbarcacionesResponse res_cons = new ConsultaEmbarcacionesResponse();
                return res_cons;
            }
        }


        public IEnumerable<Response.ConsultaEmbarcacionesResponse> GetAllEmbarcaciones_sin_paginado(string matricula, string nombre, int cmb_actividad)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCE in _dataContext.VW_CONSULTA_EMBARCACIONES

                          from MTEMB in _dataContext.VW_CONSULTA_TIPO_EMBARCACIONES
                              .Where(MTEMB => VCE.ID_TIPO_EMBARCACION == MTEMB.ID_TIPO_EMBARCACION)
                           .DefaultIfEmpty() // <== makes join left join

                          from MACTV in _dataContext.vw_CONSULTAR_ACTV_EMBARCACION
                             .Where(MACTV => VCE.ID_TIPO_ACT_EMB == MACTV.ID_TIPO_ACT_EMB)
                          .DefaultIfEmpty() // <== makes join left join

                          from MCEMB in _dataContext.vw_CONSULTAR_COD_HAB_EMBARCACION
                             .Where(MCEMB => VCE.CODIGO_HABILITACION == MCEMB.ID_COD_HAB_EMB)
                          .DefaultIfEmpty() // <== makes join left join

                          where VCE.MATRICULA.Contains(matricula) && VCE.NOMBRE.Contains(nombre)
                          && (cmb_actividad == 0 || (cmb_actividad != 0 && VCE.ID_TIPO_ACT_EMB==cmb_actividad))
                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = VCE.ID_EMBARCACION,
                              id_tipo_embarcacion = VCE.ID_TIPO_EMBARCACION,
                              matricula = VCE.MATRICULA,
                              nombre = VCE.NOMBRE,
                              nombre_tipo_embarcacion = MTEMB.NOMBRE,
                              nombre_actividad = MACTV.NOMBRE,
                              cod_habilitante = (VCE.CODIGO_HABILITACION == null || VCE.NUM_COD_HABILITACION == null) ? "" : MCEMB.CODIGO + "-" + VCE.NUM_COD_HABILITACION.ToString() + "-" + VCE.NOM_COD_HABILITACION
                          }).OrderByDescending(r => r.id_embarcacion).Distinct().AsEnumerable();
            return result;
        }



        public ConsultaEmbarcacionesResponse buscar_embarcacion_x_seguimiento(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCE in _dataContext.VW_CONSULTA_EMBARCACIONES

                          from MTEMB in _dataContext.VW_CONSULTA_TIPO_EMBARCACIONES
                              .Where(MTEMB => VCE.ID_TIPO_EMBARCACION == MTEMB.ID_TIPO_EMBARCACION)
                           .DefaultIfEmpty() // <== makes join left join

                          from MACTV in _dataContext.vw_CONSULTAR_ACTV_EMBARCACION
                             .Where(MACTV => VCE.ID_TIPO_ACT_EMB == MACTV.ID_TIPO_ACT_EMB)
                          .DefaultIfEmpty() // <== makes join left join

                          from MCEMB in _dataContext.vw_CONSULTAR_COD_HAB_EMBARCACION
                             .Where(MCEMB => VCE.CODIGO_HABILITACION == MCEMB.ID_COD_HAB_EMB)
                          .DefaultIfEmpty() // <== makes join left join

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                             .Where(MSEG => VCE.ID_EMBARCACION == MSEG.ID_HABILITANTE)
                          .DefaultIfEmpty() // <== makes join left join

                          where MSEG.ID_SEGUIMIENTO==id_seguimiento

                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = VCE.ID_EMBARCACION,
                              id_tipo_embarcacion = VCE.ID_TIPO_EMBARCACION,
                              matricula = VCE.MATRICULA,
                              nombre = VCE.NOMBRE,
                              nombre_tipo_embarcacion = MTEMB.NOMBRE,
                              nombre_actividad = MACTV.NOMBRE,
                              cod_habilitante = (VCE.CODIGO_HABILITACION == null || VCE.NUM_COD_HABILITACION == null) ? "" : MCEMB.CODIGO + "-" + VCE.NUM_COD_HABILITACION.ToString() + "-" + VCE.NOM_COD_HABILITACION
                          }).OrderBy(r => r.id_embarcacion).Distinct().AsEnumerable().First();
            return result;
        }


        public IEnumerable<ConsultaEmbarcacionesResponse> genera_protocolo_embarcacion()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_EMBARCACION()
                         select new ConsultaEmbarcacionesResponse()
                         {
                             genera_data_matricula = r.MATRICULA,
                             genera_data_actividad = r.ACTIVIDAD,
                             genera_data_tipo_embarcacion = r.TIPO,
                             genera_data_nombre = r.NOMBRE,
                             genera_data_codigo_habilitacion = r.CODIGO_HABILITACION,
                             genera_data_archivos = r.ARCHIVOS
                         };
            return result;
        }

    }
}
