using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoSintomaMapper
    {
        public static ResultadoSintomaDTO ToDTO(
            string codigoSintoma,
            string nombreSintoma,
            string valorObtenido,
            string gravedad)
        {
            return new ResultadoSintomaDTO
            {
                CodigoSintoma = codigoSintoma,
                NombreSintoma = nombreSintoma,
                ValorObtenido = valorObtenido,
                Gravedad = gravedad
            };
        }
    }
}
