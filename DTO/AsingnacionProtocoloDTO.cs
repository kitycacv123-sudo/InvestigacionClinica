using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class AsignacionProtocoloDTO
    {
        [JsonPropertyName("codigo_protocolo")]
        public string CodigoProtocolo { get; set; }

        [JsonPropertyName("codigo_solicitud")]
        public string CodigoSolicitud { get; set; }

        [JsonPropertyName("fecha_inicio")]
        public string FechaInicio { get; set; }  // Formato "yyyy-MM-dd"

        [JsonPropertyName("fecha_fin")]
        public string FechaFin { get; set; }     // Formato "yyyy-MM-dd"
    }
}