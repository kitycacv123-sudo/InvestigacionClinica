using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace InvestigacionClinica.DTO
{
    public class OrdenExamenDoctorDTO
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

    // DTO que envuelve la respuesta completa del endpoint
    public class RespuestaOrdenExamenDoctorDTO
    {
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        [JsonPropertyName("data")]
        public List<OrdenExamenDoctorDTO> Data { get; set; }
    }
}