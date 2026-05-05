using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class GestionDocumentacionDTO
    {
        [JsonPropertyName("codigoDocumento")]
        public string codigoDocumento { get; set; }
        [JsonPropertyName("documento")]
        public string documento { get; set; }
    }
}
