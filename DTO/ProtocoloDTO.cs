using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class ProtocoloDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}