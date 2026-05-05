using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Mapeador
{
    public class SinSintomaMapeador
    {
        public static SinSintomaDTO ToDTO(
                string codigo,
                string codigoPaciente,
                string tipoPrueba,
                string valorObtenido,
                string tieneValorAnormal)
        {
            return new SinSintomaDTO
            {
                Codigo = codigo,
                CodigoPaciente = codigoPaciente,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido,
                TieneValorAnormal = tieneValorAnormal
            };
        }
    }
}
