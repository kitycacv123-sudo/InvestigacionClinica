using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoPorTipoDePruebaMapper
    {
        public static ResultadoPorTipoDePruebaDTO ToDTO(
            string codigoInvestigacion,
            string tituloInvestigacion,
            string codigoResultado,
            string tipoPrueba,
            string valorObtenido,
            string codigoPaciente,
            string tieneValorAnormal)
        {
            return new ResultadoPorTipoDePruebaDTO
            {
                CodigoInvestigacion = codigoInvestigacion,
                TituloInvestigacion = tituloInvestigacion,
                CodigoResultado = codigoResultado,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido,
                CodigoPaciente = codigoPaciente,
                TieneValorAnormal = tieneValorAnormal
            };
        }
    }
}
