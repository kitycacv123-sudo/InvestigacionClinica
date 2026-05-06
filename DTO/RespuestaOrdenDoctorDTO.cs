using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class RespuestaOrdenDoctorDTO
    {
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        [JsonPropertyName("data")]
        public List<OrdenDoctorItemDTO> Data { get; set; }
    }
}
