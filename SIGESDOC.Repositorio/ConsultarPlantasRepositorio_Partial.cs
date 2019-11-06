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
    public partial class ConsultarPlantasRepositorio : IConsultarPlantasRepositorio
    {
        public IEnumerable<Response.ConsultarPlantasResponse> Guarda_Plantas(int id_sede, int id_tipo_planta,int numero_planta, string nombre_planta, int id_tipo_actividad, int id_filial, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_CREA_PLANTA(id_sede,id_tipo_planta,numero_planta,nombre_planta,id_tipo_actividad,id_filial,usuario)
                         select new ConsultarPlantasResponse()
                         {
                             id_planta = r.ID_PLANTA,
                             id_sede_oficina = r.ID_SEDE_OFICINA,
                             id_tipo_planta = r.ID_TIPO_PLANTA,
                             numero_planta = r.NUMERO_PLANTA,
                             nombre_planta = r.NOMBRE_PLANTA
                         };
            return result;
        }

        public bool Actualiza_habilitacion_planta(DateTime fecha_habilitacion_final, int id_planta)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            _dataContext.SP_HABILITA_PLANTA(id_planta, fecha_habilitacion_final, 1);
            
            return true;
        }
        
        public IEnumerable<ConsultarPlantasResponse> Consulta_planta(int id_direccion, string activo)
        {
                DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

                var result = (from VCP in _dataContext.vw_CONSULTAR_PLANTAS

                              from VCTP in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                   .Where(VCTP => VCP.ID_TIPO_PLANTA == VCTP.ID_TIPO_PLANTA)

                              from VSED in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VSED => VCP.ID_SEDE_OFICINA == VSED.ID_SEDE)

                              from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOF => VSED.ID_OFICINA == VCOF.ID_OFICINA)

                              from VDIR in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VDIR => VCOF.ID_OFICINA == VDIR.ID_OFICINA)
                              
                              where VDIR.ID_OFICINA_DIRECCION == id_direccion && VCP.ACTIVO == activo

                              select new ConsultarPlantasResponse
                              {
                                  id_planta = VCP.ID_PLANTA,
                                  id_tipo_planta = VCP.ID_TIPO_PLANTA,
                                  siglas_tipo_planta = VCTP.SIGLAS,
                                  nombre_planta = VCP.NOMBRE_PLANTA,
                                  numero_planta = VCP.NUMERO_PLANTA,
                                  activo = VCP.ACTIVO,
                                  nombre_estado = VCP.ACTIVO == "0" ? "Desactivado" : " Activo"
                              }).Distinct().OrderBy(r => r.id_planta).Distinct().AsEnumerable();
                return result;
        }


        public IEnumerable<ConsultarPlantasResponse> GetAllPlantas_sin_paginado(string id_tipo_planta, string var_numero, string var_nombre, int var_id_filial, int var_id_actividad, string var_entidad)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCP in _dataContext.vw_CONSULTAR_PLANTAS

                          from VCTP in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                               .Where(VCTP => VCP.ID_TIPO_PLANTA == VCTP.ID_TIPO_PLANTA)

                          from VCDIR in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                          .Where(VCDIR => VCP.ID_SEDE_OFICINA == VCDIR.ID_SEDE)

                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(VCOF => VCDIR.ID_OFICINA == VCOF.ID_OFICINA)

                          from VCTAC in _dataContext.vw_CONSULTAR_TIPO_ACTIVIDAD_PLANTA
                                  .Where(VCTAC => VCP.ID_TIPO_ACTIVIDAD == VCTAC.ID_TIPO_ACTIVIDAD)
                                  .DefaultIfEmpty() // <== makes join left join

                          from VCOF_PADRE in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(VCOF_PADRE => VCOF.RUC == VCOF_PADRE.RUC && VCOF_PADRE.ID_OFI_PADRE == null)

                          where (id_tipo_planta == "" || (VCP.ID_TIPO_PLANTA.ToString() == id_tipo_planta && id_tipo_planta != ""))
                          && VCP.NUMERO_PLANTA.ToString().Contains(var_numero)
                          && VCP.NOMBRE_PLANTA.Contains(var_nombre)
                          && (var_id_filial == 0 || (var_id_filial == VCP.ID_FILIAL && var_id_filial != 0))
                          && (var_id_actividad == 0 || (var_id_actividad == VCP.ID_TIPO_ACTIVIDAD && var_id_actividad != 0))
                          && VCOF_PADRE.NOMBRE.Contains(var_entidad)

                          select new ConsultarPlantasResponse
                          {
                              id_planta = VCP.ID_PLANTA,
                              id_tipo_planta = VCP.ID_TIPO_PLANTA,
                              siglas_tipo_planta = VCTP.SIGLAS,
                              nombre_actividad = VCTAC.NOMBRE,
                              nombre_planta = VCP.NOMBRE_PLANTA,
                              numero_planta = VCP.NUMERO_PLANTA,
                              nombre_entidad = VCOF_PADRE.NOMBRE,
                              direccion_entidad = VCDIR.DIRECCION,
                              nombre_estado = VCP.ACTIVO == "0" ? "Desactivado" : " Activo",
                              cond_protocolo = VCP.IND_HABILITACION == 1 ? "True" : "False"
                          }).Distinct().OrderBy(r => r.id_planta).AsEnumerable();
            return result;

        }

        public ConsultarPlantasResponse Recupera_Planta(int id_seguimiento, int id_planta)
        {            
                DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
                
                    var result = (from VCP in _dataContext.vw_CONSULTAR_PLANTAS

                                  from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                                       .Where(MSEG => VCP.ID_PLANTA == MSEG.ID_HABILITANTE && MSEG.ID_TIPO_SEGUIMIENTO==1)
                                   .DefaultIfEmpty() // <== makes join left join

                                  from VCTP in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                  .Where(VCTP => VCP.ID_TIPO_PLANTA == VCTP.ID_TIPO_PLANTA)

                                  where (id_seguimiento == 0 || (MSEG.ID_SEGUIMIENTO == id_seguimiento && id_seguimiento!=0))
                                  && (id_planta == 0 || (MSEG.ID_HABILITANTE == id_planta && id_planta != 0))
                                  select new ConsultarPlantasResponse
                                  {
                                      id_planta = VCP.ID_PLANTA,
                                      id_tipo_planta = VCP.ID_TIPO_PLANTA,
                                      id_tipo_actividad = VCP.ID_TIPO_ACTIVIDAD,
                                      siglas_tipo_planta = VCTP.SIGLAS,
                                      nombre_planta = VCP.NOMBRE_PLANTA,
                                      numero_planta = VCP.NUMERO_PLANTA,
                                      //activo = VCP.ACTIVO,
                                      nombre_estado = VCP.ACTIVO == "0" ? "Desactivado" : " Activo"
                                  }).OrderBy(r => r.id_planta).Distinct().AsEnumerable();

                    if (result.Count() > 0)
                    {
                        return result.First();
                    }
                    else
                    {
                        ConsultarPlantasResponse res_cons = new ConsultarPlantasResponse();
                        return res_cons;
                    }
        }


        public IEnumerable<ConsultarPlantasResponse> genera_protocolo_planta()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.p_GENERA_DATA_PROTOCOLO_PLANTA()
                         select new ConsultarPlantasResponse()
                         {
                             genera_data_externo = r.externo,
                             genera_data_codigo_planta = r.codigo_planta,
                             genera_data_actividad = r.actividad,
                             genera_data_direccion = r.direccion,
                             genera_data_archivos = r.archivos
                         };
            return result;
        }


    }
}
