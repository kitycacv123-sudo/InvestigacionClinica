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
            this.URL = Constantes.URLDoc;
        }

        [HttpPost("AñadirDocumento")]
        public async Task<IActionResult> PostDocumento()
        {
            var pacientes = await (httpClient.GetFromJsonAsync<List<ResultadosPacienteDTO>>($"{URL}api/Productos"));
            return Ok(pacientes);
        }
    }
}
