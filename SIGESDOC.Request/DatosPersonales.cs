using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.Request
{
    public class DatosPersonales
    {
       public string coResultado { get; set; }
       public List<DatosPersona> datosPersona { get; set; }
        public string deResultado { get; set; }
    }
}
