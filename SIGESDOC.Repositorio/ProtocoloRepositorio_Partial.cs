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
    public partial class ProtocoloRepositorio : IProtocoloRepositorio
    {
        public IEnumerable<ProtocoloResponse> GetAllProtocolo_x_planta(int id_planta)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from PROT in _dataContext.MAE_PROTOCOLO

                          from DPPL in _dataContext.DAT_PROTOCOLO_PLANTA
                          .Where(DPPL => PROT.ID_PROTOCOLO == DPPL.ID_PROTOCOLO && DPPL.ACTIVO=="1")
                          .DefaultIfEmpty() // <== makes join left join

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                          .Where(MSEG => PROT.ID_SEGUIMIENTO == MSEG.ID_SEGUIMIENTO)

                        where MSEG.ID_HABILITANTE==id_planta

                          select new ProtocoloResponse
                          {
                              id_protocolo = PROT.ID_PROTOCOLO,
                              id_seguimiento = PROT.ID_SEGUIMIENTO,
                              nombre = PROT.NOMBRE,
                              fecha_inicio = PROT.FECHA_INICIO,
                              fecha_fin  = PROT.FECHA_FIN,
                              fecha_registro = PROT.FECHA_REGISTRO,
                              ind_concha_abanico = DPPL.IND_CONCHA_ABANICO == "1" ? "SI" : "NO",
                              ind_crustaceos = DPPL.IND_CRUSTACEOS == "1" ? "SI" : "NO",
                              ind_otros = DPPL.IND_OTROS == "1" ? "SI" : "NO",
                              ind_peces = DPPL.IND_PECES == "1" ? "SI" : "NO",
                              activo = PROT.ACTIVO,
                              id_ind_pro_esp = PROT.ID_IND_PRO_ESP,
                              id_est_pro = PROT.ID_EST_PRO
                          }).OrderByDescending(r => r.id_protocolo).AsEnumerable();
            return result;
        }

        public int Generar_numero_protocolo_transporte(int anno)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            return _dataContext.SP_GENERA_PROTOCOLO_TRANSPORTE(anno).First().ID_NUM_TRAN ?? 0;
        }


    }
}
