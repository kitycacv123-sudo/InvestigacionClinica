namespace InvestigacionClinica.DTO
{
    public class InvestigacionDTO
    {
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string TipoEstudio { get; set; }
        public string Fase { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
