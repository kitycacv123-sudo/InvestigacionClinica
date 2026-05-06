using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class CrearOrdenLaboratorioDTO
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("PacienteCodigo")]
        public string PacienteCodigo { get; set; }

        [JsonPropertyName("MedicoCodigo")]
        public string MedicoCodigo { get; set; }

        [JsonPropertyName("FechaOrden")]
        public string FechaOrden { get; set; }

        [JsonPropertyName("TipoAtencion")]
        public string TipoAtencion { get; set; }

        [JsonPropertyName("Observaciones")]
        public string Observaciones { get; set; }
    }
}