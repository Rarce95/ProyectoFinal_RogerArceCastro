using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Models
{
    public class Maquina
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "cantidadProdHoras")]
        public int CantidadProdHoras { get; set; }

        [JsonProperty(PropertyName = "costoHora")]
        public double CostoHora { get; set; }

        [JsonProperty(PropertyName = "probabilidadFallo")]
        public double ProbabilidadFallo { get; set; }

        [JsonProperty(PropertyName = "horasMantenimiento")]
        public int HorasMantenimiento { get; set; }

        [JsonProperty(PropertyName = "fechaCompra")]
        public DateTime FechaCompra { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public bool Estado { get; set; }
    }
}

