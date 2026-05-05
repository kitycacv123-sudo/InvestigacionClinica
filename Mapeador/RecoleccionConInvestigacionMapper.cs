using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Mapeador
{
    public class RecoleccionConInvestigacionMapper
    {
        public static RecoleccionConInvestigacionDTO toDTO(string tituloInvestigacion, string codigoRecoleccion)
        {
            return new RecoleccionConInvestigacionDTO
            {
                TituloInvestigacion = tituloInvestigacion,
                CodigoRecoleccion = codigoRecoleccion
            };
        }

    }
}
