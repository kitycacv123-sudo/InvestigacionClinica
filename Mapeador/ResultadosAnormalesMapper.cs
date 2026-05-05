using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Mapeador
{
    public class ResultadosAnormalesMapper
    {
        public static ResultadosAnormalesDTO ToDTO(
            string codigoProtocolo,
            string descripcion,
            string codigoResultado,
            string tipoPrueba,
            string valorObtenido)
        {
            return new ResultadosAnormalesDTO
            {
                CodigoProtocolo = codigoProtocolo,
                Descripcion = descripcion,
                CodigoResultado = codigoResultado,
                TipoPrueba = tipoPrueba,
                ValorObtenido = valorObtenido,

            };               
    
        }
    }
}
