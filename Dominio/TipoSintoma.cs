using System.ComponentModel.DataAnnotations;

namespace InvestigacionClinica.Dominio
{
    public class TipoSintoma
    {
        [Key]
        public int IdTipoSintoma { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Gravedad { get; set; }
        public string Estado { get; set; } = "activo";

    }
}
