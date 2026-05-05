using InvestigacionClinica.DTO;
using InvestigacionClinica.Soporte;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionLegalController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string URL = "";

        public GestionLegalController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.URL = Constantes.URLLegal; 
        }

        [HttpPost("Solicitar")]
        public async Task<IActionResult> PostSolicitud([FromBody] GestionLegalDTO nuevaSolicitud)
        {
            
            var urlCompleta = $"{URL}api/Solicituds?Codigo={Uri.EscapeDataString(nuevaSolicitud.Codigo)}&TipoSolicitud={Uri.EscapeDataString(nuevaSolicitud.TipoSolicitud)}" +
                $"&Motivo={Uri.EscapeDataString(nuevaSolicitud.Motivo)}&Descripcion={Uri.EscapeDataString(nuevaSolicitud.Descripcion)}" +
                $"&FechaSolicitud={Uri.EscapeDataString(nuevaSolicitud.FechaSolicitud)}";

            
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