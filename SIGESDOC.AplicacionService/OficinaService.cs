using SIGESDOC.Entidades;
using SIGESDOC.IAplicacionService;
using SIGESDOC.IRepositorio;
using SIGESDOC.Request;
using SIGESDOC.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGESDOC.AplicacionService
{
    public class OficinaService : IOficinaService
    {
        /*01*/  private readonly IConsultarOficinaRepositorio _ConsultarOficinaRepositorio;
        /*02*/  private readonly IConsultarSedeOficinaRepositorio _ConsultarSedeOficinaRepositorio;
        /*03*/  private readonly IHojaTramiteRepositorio _hojatramiteRepositorio;
        /*04*/  private readonly IUnitOfWork _unitOfWork;
        /*05*/  private readonly IConsultarDniRepositorio _ConsultarDniRepositorio;

        public OficinaService(
            /*01*/  IConsultarOficinaRepositorio ConsultarOficinaRepositorio,
            /*02*/  IConsultarSedeOficinaRepositorio ConsultarSedeOficinaRepositorio,
            /*03*/  IHojaTramiteRepositorio hojatramiteRepositorio,
            /*04*/  IUnitOfWork unitOfWork,
            /*05*/  IConsultarDniRepositorio ConsultarDniRepositorio
            )
        {
            /*01*/  _ConsultarOficinaRepositorio = ConsultarOficinaRepositorio;
            /*02*/  _hojatramiteRepositorio = hojatramiteRepositorio;
            /*03*/  _unitOfWork = unitOfWork;
            /*04*/  _ConsultarDniRepositorio = ConsultarDniRepositorio;
            /*05*/  _ConsultarSedeOficinaRepositorio = ConsultarSedeOficinaRepositorio;
        }

        /*01*/
        public IEnumerable<ConsultarDireccionResponse> GetAllEmpresa_RUC(string CONSUL_RUC)
        {
            return _hojatramiteRepositorio.GetAllEmpresa_RUC(CONSUL_RUC);
        }
        /*02*/
        public bool Crea_Persona(string persona_num_documento, byte tipo_doc_iden, string paterno, string materno, string nombres, DateTime fecha_nacimiento, string ubigeo, string sexo, string direccion, string ruc, string usuario)
        {
            try
            {
                if (_ConsultarDniRepositorio.CreaPersona( persona_num_documento,  tipo_doc_iden,  paterno,  materno,  nombres,  fecha_nacimiento,  ubigeo,  sexo,  direccion,  ruc, usuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*03*/
        public IEnumerable<ConsultarOficinaResponse> GetallOficina_x_RUC_NOMBRE(int pageIndex, int pageSize, string RUC, string NOMBRE)
        {
            return _hojatramiteRepositorio.OF_GetallOficina_x_RUC_NOMBRE(pageIndex, pageSize, RUC, NOMBRE);
        }
        /*04*/
        public int CountOficina_x_RUC_NOMBRE(string RUC, string NOMBRE)
        {
            return _hojatramiteRepositorio.OF_CountOficina_x_RUC_NOMBRE(RUC, NOMBRE);
        }
        /*05*/
        public IEnumerable<Response.ConsultarDireccionResponse> GetallOficina_DIR_x_RUC(int pageIndex, int pageSize, string CONS_RUC)
        {
            return _hojatramiteRepositorio.OF_GetallOficina_DIR_x_RUC(pageIndex, pageSize, CONS_RUC);
        }
        /*06*/
        public int CountOficina_DIR_x_RUC(string RUC)
        {
            return _hojatramiteRepositorio.OF_CountOficina_DIR_x_RUC(RUC);
        }
        /*07*/
        public bool crea_empresa(string ruc,string nombre_empresa,string siglas,string nombre_sede,string direccion,string referencia,string ubigeo, string usuario)
        {
            try
            {
                if (_ConsultarDniRepositorio.CreaEmpresa(ruc,nombre_empresa,siglas,nombre_sede,direccion,referencia,ubigeo,usuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*08*/
        public IEnumerable<ConsultarOficinaResponse> Consultar_Oficina_x_RUC(string CONS_RUC)
        {

            var result = (from zp in _ConsultarOficinaRepositorio.Listar(x => x.RUC == CONS_RUC)
                         select new ConsultarOficinaResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             id_ofi_padre = zp.ID_OFI_PADRE,
                             nombre = zp.NOMBRE,
                             siglas = zp.SIGLAS
                         }).OrderBy(r => r.nombre);
            return result.ToList();
        }
        /*09*/
        public IEnumerable<ConsultarSedeOficinaResponse> Consultar_direcciones_x_oficina(int CONS_ID_OFICINA)
        {
            var result = (from zp in _ConsultarSedeOficinaRepositorio.Listar(x => x.ID_OFICINA == CONS_ID_OFICINA)
                         select new ConsultarSedeOficinaResponse
                         {
                             id_sede = zp.ID_SEDE,
                             direccion = zp.DIRECCION,
                             nombre = zp.NOMBRE,
                             referencia = zp.REFERENCIA,
                             ubigeo = zp.UBIGEO
                         }).OrderBy(r => r.nombre);
            return result.ToList();
        }
        /*10*/
        public ConsultarSedeOficinaResponse crea_sede_secundaria(string nombre_sede,string direccion,string referencia,string ubigeo,int id_oficina, string usuario)
        {
            return _ConsultarDniRepositorio.crea_sede_secundaria(nombre_sede, direccion, referencia, ubigeo, id_oficina, usuario).First();               
        }
        /*11*/
        public bool crea_oficina_secundaria(string nombre_oficina,int id_ofi_adre,string siglas,string ruc,int id_sede, string usuario)
        {
            try
            {
                if (_ConsultarDniRepositorio.crea_oficina_secundaria(nombre_oficina,id_ofi_adre,siglas,ruc,id_sede, usuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*12*/
        public bool asignar_personal(string persona_num_doc, int id_oficina_dir, string usuario)
        {
            try
            {
                if (_ConsultarOficinaRepositorio.Asigna_oficina_persona(persona_num_doc,id_oficina_dir,usuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*13*/
        public bool quita_oficina_persona(int id_per_emp,string usuario)
        {
            try
            {
                if (_ConsultarOficinaRepositorio.quita_oficina_persona(id_per_emp, usuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*09*/
        public string insertar_actualizar_direccion_legal(int ID_DIRECCION_LEGAL, string RUC, int ID_SEDE, string USUARIO)
        {
            if (_ConsultarOficinaRepositorio.insert_update_direccion_legal(ID_DIRECCION_LEGAL, RUC, ID_SEDE, USUARIO).Count() > 0)
            {
                return "Se registro correctamente";
            }
            else
            {
                return "";
            }
        }
        /*09*/
        public int direccion_legal_id(int ID_DIRECCION_LEGAL, string RUC, int ID_SEDE, string USUARIO)
        {
            return _ConsultarOficinaRepositorio.insert_update_direccion_legal(ID_DIRECCION_LEGAL, RUC, ID_SEDE, USUARIO).First().id_oficina_direccion_legal ?? 0; 
        }
        /*09*/
        public string insertar_actualizar_persona_legal(int id_persona_legal, string documento, string telefono, string correo, string RUC, string USUARIO)
        {
            if (_ConsultarOficinaRepositorio.insert_update_persona_legal(id_persona_legal, documento, telefono, correo, RUC, USUARIO).Count() > 0)
            {
                return "Se registro correctamente";
            }
            else
            {
                return "";
            }
        }
        
        /*09*/
        public string insertar_actualizar_persona_legal_DNI(int id_dni_persona_legal, string documento, string telefono, string correo, string DNI, string USUARIO)
        {
            if (_ConsultarOficinaRepositorio.insertar_actualizar_persona_legal_DNI(id_dni_persona_legal, documento, telefono, correo, DNI, USUARIO).Count() > 0)
            {
                return "Se registro correctamente";
            }
            else
            {
                return "";
            }
        }
        
    }
}
