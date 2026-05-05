using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoMapper
    {
        public static ResultadoDTO ToDTO(Resultado resultado)
        {
            return new ResultadoDTO
            {
                Codigo = resultado.Codigo,
                CodigoOrdenLaboratorio = resultado.CodigoOrdenLaboratorio,
                CodigoPaciente = resultado.CodigoPaciente,
                TipoPrueba = resultado.TipoPrueba,
                ValorObtenido = resultado.ValorObtenido,
                FechaRecepcion = resultado.FechaRecepcion,
                TieneValorAnormal = resultado.TieneValorAnormal
            };
        }

       
        public static Resultado ToEntity(ResultadoDTO dto)
        {
            return new Resultado
            {
                Codigo = dto.Codigo,
                CodigoOrdenLaboratorio = dto.CodigoOrdenLaboratorio,
                CodigoPaciente = dto.CodigoPaciente,
                TipoPrueba = dto.TipoPrueba,
                ValorObtenido = dto.ValorObtenido,
                FechaRecepcion = dto.FechaRecepcion,
                TieneValorAnormal = dto.TieneValorAnormal,
                Estado = "activo"
            };
        }
    }

}
