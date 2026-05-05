using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InvestigacionClinica.Dominio
{
    public class Recoleccion
    {
        [Key]
        public int IdRecoleccion { get; set; }
        public int IdInvestigacion { get; set; }
        public string Codigo { get; set; }
        public string CodigoProtocolo { get; set; } //codigoInvestigacion
        public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly Fechafin { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Descripcion { get; set; }
        public int Total { get; set; }
        public string Estado { get; set; } = "activo";

        [ForeignKey("IdInvestigacion")]
        [JsonIgnore]
        public Investigacion Investigacion { get; set; }


    }
}
