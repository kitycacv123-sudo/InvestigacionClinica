using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class InvestigacionTotalRecoleccionMapper
    {
        public static InvestigacionTotalRecoleccionDTO ToDTO (string codigoInvestigacion, string titulo, int totalRecolecciones)
        {
            return new InvestigacionTotalRecoleccionDTO
            {
                CodigoInvestigacion = codigoInvestigacion,
                Titulo = titulo,
                TotalRecolecciones = totalRecolecciones,
            };
        }
    }
}
