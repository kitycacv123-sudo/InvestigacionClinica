using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InvestigacionClinica.Dominio
{
    public class Recoleccion_Resultado
    {
        [Key]
        public int IdRecoleccionDetalle { get; set; }
        
        public int IdRecoleccion { get; set; }
        public int IdResultado { get; set; }
        public DateOnly FechaAsignacion { get; set; }
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Estado { get; set; } = "activo";

        [ForeignKey("IdRecoleccion")]
        [JsonIgnore]
        public Recoleccion Recoleccion { get; set; }

        [ForeignKey("IdResultado")]
        [JsonIgnore]
        public Resultado Resultado { get; set; }

    }
}
