using InvestigacionClinica.DTO;
using InvestigacionClinica.Soporte;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BioseguridadController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string URL = "";

        public BioseguridadController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.URL = Constantes.URLBio; // Asegúrate que Constantes.URLBio tenga la URL base
        }

        [HttpPost("CrearProtocolo")]
        public async Task<IActionResult> PostProtocolo([FromBody] ProtocoloDTO protocolo)
        {
            // Construir la URL con los parámetros en query string
            var urlCompleta = $"{URL}api/Protocolos/POST/{Uri.EscapeDataString(protocolo.Codigo)}" +
                              $"?titulo={Uri.EscapeDataString(protocolo.Titulo)}" +
                              $"&descripcion={Uri.EscapeDataString(protocolo.Descripcion)}";

            // Enviar POST sin cuerpo (los datos van en la URL)
            var response = await httpClient.PostAsync(urlCompleta, null);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }
        [HttpPost("CrearSolicitud")]
        public async Task<IActionResult> PostSolicitud([FromBody] SolicitudBioseguridadDTO solicitud)
        {
            
            var urlCompleta = $"{URL}api/Solicitudes"; // Cambia la ruta según corresponda

            var response = await httpClient.PostAsJsonAsync(urlCompleta, solicitud);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }
        [HttpPost("AsignarProtocolo")]
        public async Task<IActionResult> PostAsignacion([FromBody] AsignacionProtocoloDTO asignacion)
        {
            // Ajusta la URL del microservicio. Supongamos que es "api/Asignaciones"
            var urlCompleta = $"{URL}api/Asignaciones";

            var response = await httpClient.PostAsJsonAsync(urlCompleta, asignacion);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }

    }
}