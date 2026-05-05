namespace InvestigacionClinica.DTO
{
    public class ResultadoPorPacienteDTO
    {
        public string CodigoResultado { get; set; }
        public string CodigoPaciente { get; set; }
        public string TipoPrueba { get; set; }
        public string ValorObtenido { get; set; }
        public string TieneValorAnormal { get; set; }
    }
}
