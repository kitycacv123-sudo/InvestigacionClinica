using System.ComponentModel.DataAnnotations;

namespace InvestigacionClinica.Dominio
{
    public class Resultado
    {
        [Key]
        public int IdResultado { get; set; }
        public string Codigo { get; set; }
        public string CodigoOrdenLaboratorio { get; set; }
        public string CodigoPaciente { get; set; }
        public string TipoPrueba { get; set; }
        public string ValorObtenido { get; set; }
        public DateOnly FechaRecepcion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string TieneValorAnormal { get; set; } = "no";
        public string Estado { get; set; } = "activo";


    }
}
