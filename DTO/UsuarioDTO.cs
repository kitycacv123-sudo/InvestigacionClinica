using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class UsuarioDTO
    {
        [JsonPropertyName("ci")]
        public string Ci { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido_p")]
        public string ApellidoPaterno { get; set; }

        [JsonPropertyName("apellido_m")]
        public string ApellidoMaterno { get; set; }
    }
}