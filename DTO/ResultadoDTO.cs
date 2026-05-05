namespace InvestigacionClinica.DTO
{
    public class ResultadoDTO
    {
        public string Codigo { get; set; }
        public string CodigoOrdenLaboratorio { get; set; }
        public string CodigoPaciente { get; set; }
        public string TipoPrueba { get; set; }
        public string ValorObtenido { get; set; }
        public DateOnly FechaRecepcion { get; set; }
        public string TieneValorAnormal { get; set; } 
    }
}
