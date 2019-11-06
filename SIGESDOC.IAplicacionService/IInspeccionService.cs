using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Request;
using SIGESDOC.Response;

namespace SIGESDOC.IAplicacionService
{
    [ServiceContract]
    public interface IInspeccionService
    {

        [OperationContract]
        IEnumerable<ConsultaSolicitudInspeccionOdResponse> Recupera_lista_si_od(int id_ofi_dir);

        [OperationContract]
        IEnumerable<ConsultaSolicitudInspeccionOdResponse> Recupera_lista_si_od_x_inspector(int id_ofi_dir, string inspector);

        [OperationContract]
        IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp(string usuario, int id_ofi_dir);

        [OperationContract]
        IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp_x_inspector(string usuario, int id_ofi_dir, string inspector);

        [OperationContract]
        int asigna_inspector(int id_sol_insp, string inspector, string fec_inspeccion);
        
        [OperationContract]
        int atender_inspector(int id_sol_insp_od);
    }
}
