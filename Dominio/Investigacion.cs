using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace InvestigacionClinica.Dominio
{
    public class Investigacion
    {
        [Key]
        public int IdInvestigacion {  get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string TipoEstudio { get; set; }
        public string Fase { get; set; }
        public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly FechaFin { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Estado { get; set; } = "activo";
    }
}
