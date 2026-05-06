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
        [HttpPost("POST/{codigo}/{documento}/{descripcion}")]
        public async Task<IActionResult> PostSolicitud(string codigo, string documento, string descripcion)
        {
            // Construimos la URL con los parámetros que recibimos en la ruta
            var urlCompleta = $"{URL}api/Solicitudes/POST/{codigo}/{documento}/{Uri.EscapeDataString(descripcion)}";

            // Si el servicio de destino NO espera un JSON en el body, usa PostAsync en lugar de PostAsJsonAsync
            var response = await httpClient.PostAsync(urlCompleta, null);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }
        [HttpPost("{codigoProtocolo}/{codigoSolicitud}/{fechaInicio}/{fechaFin}")]
        public async Task<IActionResult> PostAsignacion(string codigoProtocolo, string codigoSolicitud, string fechaInicio, string fechaFin)
        {
            // Construimos la URL destino usando los parámetros de la ruta
            // Ejemplo: .../api/SolicitudProtocolos/PRO-001/SOL-450/2026-12-01/2026-12-10
            var urlCompleta = $"{URL}api/SolicitudProtocolos/{codigoProtocolo}/{codigoSolicitud}/{fechaInicio}/{fechaFin}";

            // Como los datos ya van en la URL, enviamos el contenido como null o vacío
            var response = await httpClient.PostAsync(urlCompleta, null);

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