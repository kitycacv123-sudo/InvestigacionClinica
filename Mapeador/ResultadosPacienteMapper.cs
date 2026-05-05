using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadosPacienteMapper
    {
        public static ResultadosPacienteDTO ToDTO(string codigoInvestigacion,
                                                  string codigoPaciente,
                                                  string tipoPrueba,
                                                  string valorObtenido)
        {
            return new ResultadosPacienteDTO
            {
                CodigoInvestigacion = codigoInvestigacion,
                CodigoPaciente = codigoPaciente,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido
            };
        }
    }
}
