using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class EmpleadoDTO
    {
        [JsonPropertyName("idEmpleado")]
        public int IdEmpleado { get; set; }

        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; }

        [JsonPropertyName("ci")]
        public string Ci { get; set; }

        [JsonPropertyName("cargo")]
        public string Cargo { get; set; }

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }
    }
}