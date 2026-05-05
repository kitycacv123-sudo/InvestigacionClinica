using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class SintomaFrecuenteMapper
    {
        public static SintomaFrecuenteDTO ToDTO(string nombreSintoma, int frecuencia)
        {
            return new SintomaFrecuenteDTO
            {
                NombreSintoma = nombreSintoma,
                Frecuencia = frecuencia
            };
        }
    }
}
