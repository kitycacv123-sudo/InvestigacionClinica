// Mapeadores/ResultadoSintomaMapper.cs
using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ResultadoSintoma2Mapper
    {
        // Para GET: entidad + códigos → DTO de salida
        public static ResultadoSintomas2DTO ToDTO(Resultado_Sintoma detalle, string codigoResultado, string codigoTipoSintoma)
        {
            return new ResultadoSintomas2DTO
            {
                CodigoResultado = codigoResultado,
                CodigoTipoSintoma = codigoTipoSintoma,
                FechaRegistro = detalle.FechaRegistro
            };
        }

        // Para POST/PUT: DTO de entrada + IDs → entidad lista para guardar
        public static Resultado_Sintoma ToEntity(ResultadoSintomas2PostDTO dto, int idResultado, int idTipoSintoma)
        {
            return new Resultado_Sintoma
            {
                IdResultado = idResultado,
                IdTipoSintoma = idTipoSintoma,
                FechaRegistro = DateOnly.FromDateTime(DateTime.Now),
                Estado = "activo"
            };
        }
    }
}