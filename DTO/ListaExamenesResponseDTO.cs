using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace InvestigacionClinica.DTO
{
    public class ListaExamenesResponseDTO
    {
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        [JsonPropertyName("examenes")]
        public List<ExamenItemDTO> Examenes { get; set; }
    }

    
}