using SIGESDOC.Request;
using SIGESDOC.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.IAplicacionService
{
    [ServiceContract]
    public interface IOficinaService
    {
        /*01*/
        [OperationContract]
        IEnumerable<ConsultarDireccionResponse> GetAllEmpresa_RUC(string CONSUL_RUC);
        /*02*/
        [OperationContract]
        bool Crea_Persona(string persona_num_documento, byte tipo_doc_iden, string paterno, string materno, string nombres, DateTime fecha_nacimiento, string ubigeo, string sexo, string direccion, string ruc, string usuario);
        /*03*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> GetallOficina_x_RUC_NOMBRE(int pageIndex, int pageSize, string RUC, string NOMBRE);
        /*04*/
        [OperationContract]
        int CountOficina_x_RUC_NOMBRE(string RUC, string NOMBRE);
        /*05*/
        [OperationContract]
        IEnumerable<Response.ConsultarDireccionResponse> GetallOficina_DIR_x_RUC(int pageIndex, int pageSize, string RUC);
        /*06*/
        [OperationContract]
        int CountOficina_DIR_x_RUC(string RUC);
        /*07*/
        [OperationContract]
        bool crea_empresa(string ruc, string nombre_empresa, string siglas, string nombre_sede, string direccion, string referencia, string ubigeo, string usuario);
        /*08*/
        [OperationContract]
        IEnumerable<ConsultarOficinaResponse> Consultar_Oficina_x_RUC(string CONS_RUC);
        /*09*/
        [OperationContract]
        IEnumerable<ConsultarSedeOficinaResponse> Consultar_direcciones_x_oficina(int CONS_ID_OFICINA);
        /*10*/
        [OperationContract]
        ConsultarSedeOficinaResponse crea_sede_secundaria(string nombre_sede, string direccion, string referencia, string ubigeo, int id_oficina, string usuario);
        /*11*/
        [OperationContract]
        bool crea_oficina_secundaria(string nombre_oficina, int id_ofi_adre, string siglas, string ruc, int id_sede,string usuario);
        
        /*12*/
        [OperationContract]
        bool asignar_personal(string persona_num_doc, int id_oficina_dir, string usuario);
        /*13*/
        [OperationContract]
        bool quita_oficina_persona(int id_per_emp, string usuario); 
        /*14*/
        [OperationContract]
        string insertar_actualizar_direccion_legal(int ID_DIRECCION_LEGAL, string RUC, int ID_SEDE, string USUARIO);
        /*09*/
        [OperationContract]
        int direccion_legal_id(int ID_DIRECCION_LEGAL, string RUC, int ID_SEDE, string USUARIO);
        /*14*/
        [OperationContract]
        string insertar_actualizar_persona_legal(int id_persona_legal, string documento, string telefono, string correo, string RUC, string USUARIO);
        /*14*/
        [OperationContract]
        string insertar_actualizar_persona_legal_DNI(int id_dni_persona_legal, string documento, string telefono, string correo, string DNI, string USUARIO);
        
    }
}
