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
    public partial class ConsultarOficinaRepositorio : IConsultarOficinaRepositorio
    {


        public IEnumerable<Response.SP_ACTUALIZA_NOM_EMPRESA_Result> Edita_db_general_nom_empresa(string nombres,string ruc, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_ACTUALIZA_NOM_EMPRESA( nombres, ruc,  usuario)
                         select new Response.SP_ACTUALIZA_NOM_EMPRESA_Result()
                         {
                             ruc = r.RUC,
                             razon_social = r.RAZON_SOCIAL
                         };
            return result;
        }
        
        public IEnumerable<Response.SP_EDITA_DB_GENERAL_MAE_SEDE_Result> Edita_db_general_mae_sede(int id_sede, string direccion, string ubigeo, string sede, string referencia)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.SP_EDITA_DB_GENERAL_MAE_SEDE(id_sede, sede, direccion, referencia, ubigeo)
                         select new Response.SP_EDITA_DB_GENERAL_MAE_SEDE_Result()
                         {
                             id_sede = r.ID_SEDE,
                             direccion = r.DIRECCION
                         };
            return result;
        }
        
        /*
        public IEnumerable<Response.ConsultarOficinaResponse> OF_GetallOficina_x_RUC_NOMBRE(int pageIndex, int pageSize, string RUC, string NOMBRE)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join
                               
                          from VCO_OFI in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO_OFI => VCO.RUC == VCO_OFI.RUC && VCO_OFI.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join
                               
                          from VCSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VCSO => VCD.ID_SEDE == VCSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC.Contains(RUC) && VCO.NOMBRE.Contains(NOMBRE) 

                         select new ConsultarOficinaResponse()
                         {
                             ruc = VCO.RUC,
                             nombre = VCO.ID_OFI_PADRE == null ? VCO.NOMBRE : VCO_OFI.SIGLAS + " - " + VCO.NOMBRE + (VCSO.NOMBRE.ToString().Trim() == "" ? " ": " - " + VCSO.NOMBRE),
                             siglas = VCO.SIGLAS,
                             activo_direccion = VCD.ACTIVO,
                             nombre_direccion = VCSO.NOMBRE.ToString().Trim() == "" ? VCSO.DIRECCION : VCSO.DIRECCION
                         }).OrderByDescending(r => r.ruc).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return result;
        }

        public int OF_CountOficina_x_RUC_NOMBRE(string RUC, string NOMBRE)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCO_OFI in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO_OFI => VCO.RUC == VCO_OFI.RUC && VCO_OFI.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VCSO => VCD.ID_SEDE == VCSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC.Contains(RUC) && VCO.NOMBRE.Contains(NOMBRE)
                          select new ConsultarOficinaResponse()
                          {
                              ruc = VCO.RUC,
                              nombre = VCO.ID_OFI_PADRE == null ? VCO.NOMBRE : VCO_OFI.SIGLAS + " - " + VCO.NOMBRE,
                              siglas = VCO.SIGLAS,
                              activo_direccion = VCD.ACTIVO,
                              nombre_direccion = VCSO.NOMBRE.ToString().Trim() == "" ? VCSO.DIRECCION : VCSO.NOMBRE + " - " + VCSO.DIRECCION
                          }).OrderByDescending(r => r.ruc).AsEnumerable();
            return result.Count();
        }
        
        public IEnumerable<Response.ConsultarDireccionResponse> OF_GetallOficina_DIR_x_RUC(int pageIndex, int pageSize, string RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION
                          
                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join
                               
                          from VSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VSO => VCD.ID_SEDE == VSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join
                               
                          from VCU in _dataContext.vw_CONSULTAR_UBIGEO
                               .Where(VCU => VSO.UBIGEO == VCU.UBIGEO)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC == RUC 

                         select new ConsultarDireccionResponse()
                         {
                             id_oficina_direccion = VCD.ID_OFICINA_DIRECCION,
                             nom_oficina = VSO.NOMBRE.ToString().Trim() == "" ? VCO.NOMBRE : VCO.NOMBRE+"-"+VSO.NOMBRE,
                             direccion = VSO.DIRECCION,
                             nom_ubigeo = VCU.DEPARTAMENTO + '-' + VCU.PROVINCIA + '-' + VCU.DISTRITO,
                             activo = VCD.ACTIVO
                         }).OrderByDescending(r => r.id_oficina_direccion).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return result;
        }

        public int OF_CountOficina_DIR_x_RUC(string RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VSO => VCD.ID_SEDE == VSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCU in _dataContext.vw_CONSULTAR_UBIGEO
                               .Where(VCU => VSO.UBIGEO == VCU.UBIGEO)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC == RUC

                          select new ConsultarDireccionResponse()
                          {
                              id_oficina_direccion = VCD.ID_OFICINA_DIRECCION,
                              nom_oficina = VSO.NOMBRE.ToString().Trim() == "" ? VCO.NOMBRE : VCO.NOMBRE + "-" + VSO.NOMBRE,
                              direccion = VSO.DIRECCION,
                              nom_ubigeo = VCU.DEPARTAMENTO + '-' + VCU.PROVINCIA + '-' + VCU.DISTRITO,
                              activo = VCD.ACTIVO
                          }).OrderByDescending(r => r.id_oficina_direccion).AsEnumerable();
            return result.Count();
        }
        */

        public IEnumerable<ConsultarPersonalResponse> Asigna_oficina_persona(string persona_num_documento, int id_oficina_dir, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_ASIGNA_OFICINA_PERSONA(persona_num_documento, id_oficina_dir, usuario)
                         select new ConsultarPersonalResponse()
                         {
                             persona_num_documento = r.persona_num_documento,
                             id_oficina_direccion = r.ID_OFICINA_DIRECCION
                         };
            return result;
        }
        public IEnumerable<ConsultarPersonalResponse> quita_oficina_persona(int id_per_empresa, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_DESACTIVA_OFICINA_PERSONA(id_per_empresa, usuario)
                         select new ConsultarPersonalResponse()
                         {
                             persona_num_documento = r.persona_num_documento,
                             id_oficina_direccion = r.ID_OFICINA_DIRECCION
                         };
            return result;
        }
        
        public IEnumerable<ConsultarOficinaDireccionLegalResponse> insert_update_direccion_legal(int id_direccion_legal, string ruc, int id_sede, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_INSERT_UPDATE_MAE_OFICINA_DIRECCION_LEGAL(id_direccion_legal,ruc,id_sede,usuario)
                         select new ConsultarOficinaDireccionLegalResponse()
                         {
                             id_oficina_direccion_legal = r.ID_OFICINA_DIRECCION_LEGAL
                         };
            return result;
        }
        public IEnumerable<ConsultarEmpresaPersonaLegalResponse> insert_update_persona_legal(int id_persona_legal, string documento, string telefono, string correo, string RUC, string USUARIO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_INSERT_UPDATE_MAE_ENTIDAD_PERSONA_LEGAL(id_persona_legal, documento, telefono, correo, RUC, USUARIO)
                         select new ConsultarEmpresaPersonaLegalResponse()
                         {
                             id_persona_legal = r.ID_PERSONA_LEGAL
                         };
            return result;
        }

        public IEnumerable<ConsultarDniPersonalLegalResponse> insertar_actualizar_persona_legal_DNI(int id_dni_persona_legal, string documento, string telefono, string correo, string DNI, string USUARIO)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_INSERT_UPDATE_MAE_DNI_PERSONA_LEGAL(id_dni_persona_legal, documento, telefono, correo, DNI, USUARIO)
                         select new ConsultarDniPersonalLegalResponse()
                         {
                             id_dni_persona_legal = r.ID_DNI_PERSONA_LEGAL
                         };
            return result;
        }
        public string Recupera_RUC_x_ID_OFI_DIR(int id_ofi_dir)
        {
            DB_GESDOCEntities _dataContext = new DB_GESDOCEntities();

            var result = (from OFI_DIR in _dataContext.vw_CONSULTAR_DIRECCION

                          from OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(OFICINA => OFI_DIR.ID_OFICINA == OFICINA.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join
                          where OFI_DIR.ID_OFICINA_DIRECCION ==id_ofi_dir
                          select new ConsultarOficinaResponse
                          {
                              ruc = OFICINA.RUC
                          }).First();

            return result.ruc;
        }
    }
}
