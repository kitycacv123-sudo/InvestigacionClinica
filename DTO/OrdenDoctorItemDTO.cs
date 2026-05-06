using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class OrdenDoctorItemDTO
    {
        [JsonPropertyName("ordenLaboratorioCodigo")]
        public string OrdenLaboratorioCodigo { get; set; }

        [JsonPropertyName("nombreExamen")]
        public string NombreExamen { get; set; }

        [JsonPropertyName("tipoMuestra")]
        public string TipoMuestra { get; set; }

        [JsonPropertyName("estadoOrdenExamen")]
        public string EstadoOrdenExamen { get; set; }
    }
}
