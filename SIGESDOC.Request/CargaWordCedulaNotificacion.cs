using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.Request
{
   public class CargaWordCedulaNotificacion
    {
        public string NON_DOC { get; set; }
        public string ASUNTO { get; set; }
        public string DIRECCION_CDL_NOTIF { get; set; }
        public string EMPRESA_CDL_NOTIF { get; set; }
        public string FOLIA_CDL_NOTIF { get; set; }
        public string DOC_NOTIFICAR_CDL_NOTIF { get; set; }
        public string EXP_O_HT_N_CDL_NOTIF { get; set; }
    }
}
