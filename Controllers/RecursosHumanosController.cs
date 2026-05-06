using Microsoft.AspNetCore.Mvc;
using InvestigacionClinica.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosHumanosController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string URL = "";

        // Inyectamos HttpClient en el constructor
        public RecursosHumanosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // La URL base de bioseguridad
            this.URL = "https://bycs-production.up.railway.app/";
        }

        /// <summary>
        /// Obtiene una lista de empleados (datos estáticos de ejemplo)
        /// </summary>
        [HttpGet("ListarEmpleados")]
        public IActionResult ListarEmpleados()
        {
            var empleados = new List<EmpleadoDTO>
            {
                new EmpleadoDTO
                {
                    
                    NombreCompleto = "Juan Pérez",
                    Ci = "12345678",
                    Cargo = "Psicólogo Clínico",
                    Departamento = "Psicología",
                    Estado = "Activo"
                },
                new EmpleadoDTO
                {
                    
                    NombreCompleto = "María López",
                    Ci = "87654321",
                    Cargo = "Trabajadora Social",
                    Departamento = "Trabajo Social",
                    Estado = "Activo"
                },
                new EmpleadoDTO
                {
                    
                    NombreCompleto = "Carlos Gómez",
                    Ci = "11223344",
                    Cargo = "Médico General",
                    Departamento = "Medicina",
                    Estado = "Inactivo"
                }
            };

            return Ok(empleados);
        }

        [HttpGet("ListarPacientes")]
        public async Task<IActionResult> ListarPacientes()
        {
            // Construimos la URL completa para obtener los usuarios
            var urlCompleta = $"{URL}api/Usuarios/GET";

            var response = await _httpClient.GetAsync(urlCompleta);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                // Devolvemos el mismo resultado, que es un array de usuarios en formato JSON.
                return Ok(resultado);
            }

            var error = await response.Content.ReadAsStringAsync();
            // En caso de error, devolvemos el código y el mensaje del microservicio.
            return BadRequest($"Error {response.StatusCode}: {error}");
        }
    }
}