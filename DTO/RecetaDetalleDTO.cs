using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class RecetaDetalleDTO
    {
        [JsonPropertyName("medicamentoCodigo")]
        public string MedicamentoCodigo { get; set; }

        [JsonPropertyName("cantidadSolicitada")]
        public int CantidadSolicitada { get; set; }

        [JsonPropertyName("posologia")]
        public PosologiaDTO Posologia { get; set; }
    }
}
