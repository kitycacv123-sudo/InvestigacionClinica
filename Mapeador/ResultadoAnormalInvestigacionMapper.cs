using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoAnormalInvestigacionMapper
    {
        public static ResultadoAnormalInvestigacionDTO ToDto (string titulo, string codigoPaciente, string valorObtenido)
        {
            return new ResultadoAnormalInvestigacionDTO
            {
                Titulo = titulo,
                CodigoPaciente = codigoPaciente,
                ValorObtenido = valorObtenido,
            };
        }
    }
}
