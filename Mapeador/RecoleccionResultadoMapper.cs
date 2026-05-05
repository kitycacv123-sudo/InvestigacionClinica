using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class RecoleccionResultadoMapper
    {
        // Para GET: entidad + códigos → DTO de salida
        public static RecoleccionResultadoDTO ToDTO(Recoleccion_Resultado detalle, string codigoRecoleccion, string codigoResultado)
        {
            return new RecoleccionResultadoDTO
            {
                
                CodigoRecoleccion = codigoRecoleccion,
                CodigoResultado = codigoResultado,
                FechaAsignacion = detalle.FechaAsignacion
            };
        }

        // Para POST/PUT: DTO de entrada + IDs → entidad lista para guardar
        public static Recoleccion_Resultado ToEntity(RecoleccionResultadoPostDTO dto, int idRecoleccion, int idResultado)
        {
            return new Recoleccion_Resultado
            {
                
                IdRecoleccion = idRecoleccion,
                IdResultado = idResultado,
                FechaAsignacion = DateOnly.FromDateTime(DateTime.Now),
                FechaRegistro = DateOnly.FromDateTime(DateTime.Now),
                Estado = "activo"
            };
        }
    }
}