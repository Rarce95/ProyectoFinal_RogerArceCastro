using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Models
{
    public class Simaulacion
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "cantHoras")]
        public string CantidadHoras { get; set; }

        [JsonProperty(PropertyName = "cantProduccionHorasDia")]
        public int CantProduccionHorasDia { get; set; }

        [JsonProperty(PropertyName = "cantProductosDia")]
        public int cantProductosDia { get; set; }

        [JsonProperty(PropertyName = "idMaquina1")]
        public string IdMaquina1 { get; set; }

        [JsonProperty(PropertyName = "cantidadProductoMaquina1")]
        public int ProductoHoraMaquina1 { get; set; }

        [JsonProperty(PropertyName = "gananciaRealMaquina1")]
        public double GananciaRealMaquina1 { get; set; }

        [JsonProperty(PropertyName = "gananciaBrutoMaquina1")]
        public double GananciaBrutoMaquina1 { get; set; }

        [JsonProperty(PropertyName = "idMaquina2")]
        public string IdMaquina2 { get; set; }

        [JsonProperty(PropertyName = "cantidadProductoMaquina2")]
        public int ProductoHoraMaquina2 { get; set; }

        [JsonProperty(PropertyName = "gananciaRealMaquina2")]
        public double GananciaRealMaquina2 { get; set; }

        [JsonProperty(PropertyName = "gananciaBrutoMaquina2")]
        public double GananciaBrutoMaquina2 { get; set; }

    }
}
