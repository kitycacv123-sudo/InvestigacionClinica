using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoPorRecoleccionMapper
    {
        public static ResultadoPorRecoleccionDTO ToDTO(string codigoRecoleccion, string codigoResultado, string tipoPrueba, string valorObtenido, string tieneValorAnormal)
        {
            return new ResultadoPorRecoleccionDTO
            {
                CodigoRecoleccion = codigoRecoleccion,
                CodigoResultado = codigoResultado,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido,
                TieneValorAnormal = tieneValorAnormal
            };
        }
    }
}
