using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Models
{
    public class Maquina
    {
        public string id { get; set; }
        public int cantProdHoras { get; set; }
        public decimal costoHora { get; set; }
        public decimal probabilidadFallo { get; set; }
    }
}
