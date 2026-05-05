using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Mapeador
{
    public class ResultadoPendienteAsignacionMapper
    {
        public static ResultadoPendienteAsignacionDTO ToDTO(string codigoResultado, string codigoPaciente, string tipoPrueba, string valorObtenido)
        {
            return new ResultadoPendienteAsignacionDTO
            {
                CodigoResultado = codigoResultado,
                CodigoPaciente = codigoPaciente,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido
            };
        }
    }
}
