using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class RecetaRequestDTO
    {
        [JsonPropertyName("pacienteCodigo")]
        public string PacienteCodigo { get; set; }

        [JsonPropertyName("medicoCodigo")]
        public string MedicoCodigo { get; set; }

        [JsonPropertyName("detalles")]
        public List<RecetaDetalleDTO> Detalles { get; set; }
    }
}
