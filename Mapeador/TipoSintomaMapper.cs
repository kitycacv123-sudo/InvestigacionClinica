using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class TipoSintomaMapper
    {
        public static TipoSintomaDTO ToDTO(TipoSintoma tipoSintoma)
        {
            return new TipoSintomaDTO
            {
                Codigo = tipoSintoma.Codigo,
                Nombre = tipoSintoma.Nombre,
                Gravedad = tipoSintoma.Gravedad
            };
        }

        
        public static TipoSintoma ToEntity(TipoSintomaDTO dto)
        {
            return new TipoSintoma
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Gravedad = dto.Gravedad,
                Estado = "activo"
            };
        }
    }
}
