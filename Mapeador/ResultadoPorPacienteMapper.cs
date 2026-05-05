using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoPorPacienteMapper
    {
        public static ResultadoPorPacienteDTO ToDTO(string codigoResultado, string codigoPaciente, string tipoPrueba, string valorObtenido, string tieneValorAnormal)
        {
            return new ResultadoPorPacienteDTO
            {
                CodigoResultado = codigoResultado,
                CodigoPaciente = codigoPaciente,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido,
                TieneValorAnormal = tieneValorAnormal
            };
        }
    }
}
