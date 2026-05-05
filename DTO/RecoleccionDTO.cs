namespace InvestigacionClinica.DTO
{
    public class RecoleccionDTO
    {
        public string Codigo { get; set; }
        public string CodigoProtocolo { get; set; }
        public DateOnly FechaInicio { get; set; } 
        public DateOnly Fechafin { get; set; }
        public string Descripcion { get; set; }
        public int Total { get; set; }
    }
}
