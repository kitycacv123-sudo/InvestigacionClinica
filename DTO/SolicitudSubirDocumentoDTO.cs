using System.Text.Json.Serialization;

namespace InvestigacionClinica.DTO
{
    public class SolicitudSubirDocumentoDTO
    {
        public string NombreArchivo { get; set; }
        public string DescripcionContenido { get; set; }
        public string NombreMedico { get; set; }
        public string NombreDepartamento { get; set; }
        public string CodigoPaciente { get; set; }
        public string CodigoSolicitud { get; set; }
        public string CodigoTipoDoc { get; set; }
    }
}