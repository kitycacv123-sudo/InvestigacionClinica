using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class PosologiaDTO
    {
        [JsonPropertyName("dosis")]
        public decimal Dosis { get; set; }  

        [JsonPropertyName("unidadAbreviatura")]
        public string UnidadAbreviatura { get; set; }

        [JsonPropertyName("viaAdministracion")]
        public string ViaAdministracion { get; set; }

        [JsonPropertyName("frecuencia")]
        public string Frecuencia { get; set; }

        [JsonPropertyName("frecuenciaValor")]
        public int FrecuenciaValor { get; set; }

        [JsonPropertyName("duracion")]
        public string Duracion { get; set; }

        [JsonPropertyName("indicacionesAdicionales")]
        public string IndicacionesAdicionales { get; set; }
    }
}
