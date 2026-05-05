using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InvestigacionClinica.Dominio
{
    public class Resultado_Sintoma
    {
        [Key]
        public int IdResultadoSintoma { get; set; }
        public int IdResultado { get; set; }
        public int IdTipoSintoma { get; set; }
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Estado { get; set; } = "activo";

        [ForeignKey("IdResultado")]
        [JsonIgnore]
        public Resultado Resultado { get; set; }

        [ForeignKey("IdTipoSintoma")]
        [JsonIgnore]
        public TipoSintoma TipoSintoma { get; set; }

    }
}
