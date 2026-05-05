namespace InvestigacionClinica.DTO
{
    public class ComparacionRecoleccionDTO
    {
        public string CodigoRecoleccion { get; set; }
        public int TotalEsperado { get; set; }
        public int TotalObtenido { get; set; }
        public double Porcentaje { get; set; }
    }

}
