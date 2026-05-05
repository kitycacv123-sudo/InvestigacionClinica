using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Mapeador
{
    public class ComparacionRecoleccionMapper
    {
        public static ComparacionRecoleccionDTO ToDTO(
            string codigoRecoleccion,
            int totalEsperado,
            int totalObtenido,
            double porcentaje)
        {
            return new ComparacionRecoleccionDTO
            {
                CodigoRecoleccion = codigoRecoleccion,
                TotalEsperado = totalEsperado,
                TotalObtenido = totalObtenido,
                Porcentaje = porcentaje
            };
        }
    }
}
