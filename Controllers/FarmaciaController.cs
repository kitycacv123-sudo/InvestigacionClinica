using InvestigacionClinica.DTO;
using InvestigacionClinica.Soporte;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Text.Json;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmaciaController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string URL = "";

        public FarmaciaController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.URL = Constantes.URLFarm; // https://hospital3ernivel-farmacia.onrender.com/
        }

        // POST: api/Farmacia/CrearReceta
        [HttpPost("CrearReceta")]
        public async Task<IActionResult> CrearReceta([FromBody] RecetaRequestDTO receta)
        {
            var urlCompleta = $"{URL}api/Recetas";
            var json = JsonSerializer.Serialize(receta);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(urlCompleta, content);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest($"Error {response.StatusCode}: {error}");
        }

        // GET: api/Farmacia/ListarMedicamentos
        [HttpGet("ListarMedicamentos")]
        public async Task<IActionResult> ListarMedicamentos()
        {
            var urlCompleta = $"{URL}api/Medicamentos/catalogo";

            var response = await httpClient.GetAsync(urlCompleta);

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