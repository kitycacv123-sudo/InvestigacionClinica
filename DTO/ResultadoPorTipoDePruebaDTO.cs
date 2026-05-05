namespace InvestigacionClinica.DTO
{
    public class ResultadoPorTipoDePruebaDTO
    {
        public string CodigoInvestigacion { get; set; }
        public string TituloInvestigacion { get; set; }
        public string CodigoResultado { get; set; }
        public string TipoPrueba { get; set; }
        public string ValorObtenido { get; set; }
        public string CodigoPaciente { get; set; }
        public string TieneValorAnormal { get; set; }
    }
}
