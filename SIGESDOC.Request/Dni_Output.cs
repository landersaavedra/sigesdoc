using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.Request
{
    public class Dni_Output
    {
        public string apPrimer { get; set; }
        public string apSegundo { get; set; }
        public string direccion { get; set; }
        public string estadoCivil { get; set; }
        public string foto { get; set; }
        public string prenombres { get; set; }
        public string restriccion { get; set; }
        public string ubigeo { get; set; }
        public string msg { get; set; }

        public string coResultado { get; set; }

        //public List<DatosPersonales> datosPersona { get; set; }
    }
}
