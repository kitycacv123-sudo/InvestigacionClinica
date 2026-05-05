using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class ConteoSintomasMapper
    {
        public static List<ConteoSintomasDTO> ToListaDTO(
        List<(string CodigoRecoleccion, string Descripcion, int TotalSintomas)> lista)
        {
            return lista.Select(item => new ConteoSintomasDTO
            {
                CodigoRecoleccion = item.CodigoRecoleccion,
                Descripcion = item.Descripcion,
                TotalSintomas = item.TotalSintomas
            }).ToList();
        }
    }
}
