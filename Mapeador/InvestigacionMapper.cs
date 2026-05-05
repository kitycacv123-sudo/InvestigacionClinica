using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class InvestigacionMapper
    {
        public static InvestigacionDTO ToDTO(Investigacion inv)
        {
            return new InvestigacionDTO
            {
                Codigo = inv.Codigo,
                Titulo = inv.Titulo,
                TipoEstudio = inv.TipoEstudio,
                Fase = inv.Fase,
                FechaInicio = inv.FechaInicio,
                FechaFin = inv.FechaFin
            };
        }

        public static void UpdateEntity(Investigacion entidad, InvestigacionDTO dto)
        {
            entidad.Codigo = dto.Codigo;
            entidad.Titulo = dto.Titulo;
            entidad.TipoEstudio = dto.TipoEstudio;
            entidad.Fase = dto.Fase;
            entidad.FechaInicio = dto.FechaInicio;
            entidad.FechaFin = dto.FechaFin;
        }
    }
}
