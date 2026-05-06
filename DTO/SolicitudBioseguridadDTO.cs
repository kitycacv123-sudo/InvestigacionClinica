using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class SolicitudBioseguridadDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("ci")]
        public string Ci { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}