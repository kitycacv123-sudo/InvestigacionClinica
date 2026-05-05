using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class GestionLegalDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        [JsonPropertyName("tipoSolicitud")]
        public string TipoSolicitud { get; set; }
        [JsonPropertyName("motivo")]
        public string Motivo { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("fechaSolicitud")]
        public string FechaSolicitud { get; set; }
        

    }
}
