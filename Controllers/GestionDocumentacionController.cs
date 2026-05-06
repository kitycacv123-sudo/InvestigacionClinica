using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InvestigacionClinica.Soporte;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionDocumentacionController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string URL = "";

        public GestionDocumentacionController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.URL = Constantes.URLDoc; // "https://gestiondocumental-1.onrender.com/"
        }

        /// <summary>
        /// Sube un archivo al microservicio de gestión documental.
        /// </summary>
        /// <param name="archivo">Archivo a subir (binario)</param>
        /// <param name="datos">Datos adicionales del documento</param>
        [HttpPost("SubirArchivo")]
        public async Task<IActionResult> SubirArchivo(
            IFormFile archivo,
            [FromForm] SolicitudSubirDocumentoDTO datos)
        {
            // Validar que el archivo exista
            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "Debe enviar un archivo." });

            // Construir el contenido multipart/form-data
            using var formData = new MultipartFormDataContent();

            // Agregar el archivo (con el nombre de campo "Archivo")
            var fileContent = new StreamContent(archivo.OpenReadStream());
            formData.Add(fileContent, "Archivo", archivo.FileName);

            // Agregar los demás campos (usar los nombres exactos que espera el microservicio)
            formData.Add(new StringContent(datos.NombreArchivo ?? ""), "NombreArchivo");
            formData.Add(new StringContent(datos.DescripcionContenido ?? ""), "DescripcionContenido");
            formData.Add(new StringContent(datos.NombreMedico ?? ""), "NombreMedico");
            formData.Add(new StringContent(datos.NombreDepartamento ?? ""), "NombreDepartamento");
            formData.Add(new StringContent(datos.CodigoPaciente ?? ""), "CodigoPaciente");
            formData.Add(new StringContent(datos.CodigoSolicitud ?? ""), "CodigoSolicitud");
            formData.Add(new StringContent(datos.CodigoTipoDoc ?? ""), "CodigoTipoDoc");

            var urlCompleta = $"{URL}api/DocumentoSolicitadoes/SubirArchivo";
            var response = await httpClient.PostAsync(urlCompleta, formData);

            // Aunque no te interese la respuesta, podemos devolver un simple éxito o el mensaje del servicio
            if (response.IsSuccessStatusCode)
            {
                // Opcional: leer respuesta si la necesitas
                // var resultado = await response.Content.ReadAsStringAsync();
                return Ok(new { mensaje = "Archivo subido exitosamente al microservicio." });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(new { mensaje = "Error al subir archivo", detalle = error });
            }
        }
    }
}