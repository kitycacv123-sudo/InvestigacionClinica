using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class FarmaciaMedicinaDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        [JsonPropertyName("nombreGenerico")]
        public string NombreGenerico { get; set; }
        [JsonPropertyName("cnombreComercial")]
        public string NombreComercial { get; set; }
        [JsonPropertyName("unidadMedida")]
        public string UnidadMedida { get; set; }
        [JsonPropertyName("formaNombre")]
        public string FormaNombre { get; set; }
        [JsonPropertyName("valorConcentracion")]
        public string ValorConcentracion { get; set; }
        
    }
}
