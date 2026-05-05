using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public static class ResultadosEsperadosMapper
    {
        public static List<ResultadosEsperadosDTO> ToListaDTO(
        List<(string Codigo, string Titulo, int Total)> lista)
        {
            return lista.Select(item => new ResultadosEsperadosDTO
            {
                CodigoInvestigacion = item.Codigo,
                TituloInvestigacion = item.Titulo,
                TotalResultadosEsperados = item.Total
            }).ToList();
        }
    }
}
