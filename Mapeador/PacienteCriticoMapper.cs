using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class PacienteCriticoMapper
    {
        public static PacienteCriticoDTO toDTO(string codigoPaciente, string nombre, string valorObtenido)
        {
            return new PacienteCriticoDTO
            {
                CodigoPaciente = codigoPaciente,
                Nombre = nombre,
                ValorObtenido = valorObtenido,
            };
        }
    }
}
