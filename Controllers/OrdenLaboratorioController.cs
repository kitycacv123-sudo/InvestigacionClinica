using InvestigacionClinica.DTO;
using InvestigacionClinica.Soporte;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenLaboratorioController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string URL = "";

        public OrdenLaboratorioController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.URL = Constantes.URLDig; // "https://diagnosticoseguro2-3.onrender.com/"
        }

        // GET: api/OrdenLaboratorio/ListarExamenes
        // Devuelve la lista de exámenes (estructura original del servicio)
        [HttpGet("ListarExamenes")]
        public async Task<IActionResult> ListarExamenes()
        {
            var urlCompleta = $"{URL}api/Examen";
            var response = await httpClient.GetAsync(urlCompleta);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }

        // POST: api/OrdenLaboratorio/CrearOrden
        [HttpPost("CrearOrden")]
        public async Task<IActionResult> CrearOrden([FromBody] CrearOrdenLaboratorioDTO nuevaOrden)
        {
            var urlCompleta = $"{URL}api/OrdenLaboratorio/HacerOrdenLaboratorio" +
                              $"?code={Uri.EscapeDataString(nuevaOrden.Code)}" +
                              $"&PacienteCodigo={Uri.EscapeDataString(nuevaOrden.PacienteCodigo)}" +
                              $"&MedicoCodigo={Uri.EscapeDataString(nuevaOrden.MedicoCodigo)}" +
                              $"&FechaOrden={Uri.EscapeDataString(nuevaOrden.FechaOrden)}" +
                              $"&TipoAtencion={Uri.EscapeDataString(nuevaOrden.TipoAtencion)}" +
                              $"&Observaciones={Uri.EscapeDataString(nuevaOrden.Observaciones)}";

            var response = await httpClient.PostAsync(urlCompleta, null);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }

        // GET: api/OrdenLaboratorio/ObtenerOrdenPorDoctor/{code}
        [HttpGet("ObtenerOrdenPorDoctor/{code}")]
        public async Task<IActionResult> ObtenerOrdenPorDoctor(string code)
        {
            var urlCompleta = $"{URL}api/OrdenExamen/Mostrar-Datos-A-Doctores/{Uri.EscapeDataString(code)}";
            var response = await httpClient.GetAsync(urlCompleta);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                // Opcional: deserializar a OrdenExamenDoctorDTO para trabajar con objetos tipados
                // var data = JsonSerializer.Deserialize<OrdenExamenDoctorDTO>(resultado);
                // return Ok(data);
                return Ok(resultado); // O devuelve string JSON directamente
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }
    }
}