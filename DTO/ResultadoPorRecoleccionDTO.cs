namespace InvestigacionClinica.DTO
{
    
    public class ResultadoPorRecoleccionDTO
    {
        public string CodigoRecoleccion { get; set; }
        public string CodigoResultado { get; set; }
        public string TipoPrueba { get; set; }
        public string ValorObtenido { get; set; }
        public string TieneValorAnormal { get; set; }
    }
}
