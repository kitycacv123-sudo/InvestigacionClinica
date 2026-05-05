using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Mapeador
{
    public class RecoleccionMapper
    {
        public static RecoleccionDTO Todto(
                string codigo,
                string codigoProtocolo,
                DateOnly fechaInicio,
                DateOnly fechaFin,
                string descripcion,
                int total)
        {
            return new RecoleccionDTO
            {
                Codigo = codigo,
                CodigoProtocolo = codigoProtocolo,
                FechaInicio = fechaInicio,
                Fechafin = fechaFin,
                Descripcion = descripcion,
                Total = total
            };
        }

    }
}
