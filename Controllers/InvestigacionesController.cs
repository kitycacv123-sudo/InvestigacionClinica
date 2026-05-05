using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;
using InvestigacionClinica.Dominio;
using InvestigacionClinica.DTO;
using InvestigacionClinica.Mapeador;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigacionesController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public InvestigacionesController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/Investigaciones/Lista
        [HttpGet("Lista")]
        public async Task<IActionResult> GetInvestigacion()
        {
            var lista = await (from i in _context.Investigacion
                               where i.Estado == "activo"
                               select InvestigacionMapper.ToDTO(i)).ToListAsync();
            return Ok(lista);
        }

        // GET: api/Investigaciones/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetInvestigacion(string codigo)
        {
            var investigacion = await (from i in _context.Investigacion
                                       where i.Codigo == codigo && i.Estado == "activo"
                                       select i).FirstOrDefaultAsync();

            if (investigacion == null)
                return NotFound();

            return Ok(InvestigacionMapper.ToDTO(investigacion));
        }
        [HttpGet("2.InvestigacionesCursoConTotalRecoleccionoes")]
        public async Task<IActionResult> GetRecoleccionTotal()
        {
            var recoleccionTotal = await (from i in _context.Investigacion
                                          where i.Estado == "activo"
                                          join r in _context.Recoleccion 
                                          on i.IdInvestigacion equals r.IdInvestigacion
                                          where r.Estado == "activo"
                                          group r by new { i.Codigo, i.Titulo } into g
                                          select InvestigacionTotalRecoleccionMapper.ToDTO(
                                              g.Key.Codigo,
                                              g.Key.Titulo,
                                              g.Count())
                                          ).ToListAsync();
            return Ok(recoleccionTotal);
        }


        // PUT: api/Investigaciones/{codigo}
        [HttpPut("Actualizar/{codigo}")]
        public async Task<IActionResult> PutInvestigacion(string codigo, [FromBody] InvestigacionDTO dto)
        {
            var investigacion = await (from i in _context.Investigacion
                                       where i.Codigo == codigo && i.Estado == "activo"
                                       select i).FirstOrDefaultAsync();

            if (investigacion == null)
                return BadRequest("La investigación no existe");

            // Actualizar campos usando el mapeador
            InvestigacionMapper.UpdateEntity(investigacion, dto);

            _context.Investigacion.Update(investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Investigaciones
        [HttpPost("Crear")]
        public async Task<IActionResult> PostInvestigacion([FromBody] InvestigacionDTO dto)
        {
            // Validar código único
            if (await _context.Investigacion.AnyAsync(i => i.Codigo == dto.Codigo))
                return BadRequest("El código de la investigación ya existe");

            // Crear nueva entidad (las fechas se asignan automáticamente)
            var nueva = new Investigacion
            {
                Codigo = dto.Codigo,
                Titulo = dto.Titulo,
                TipoEstudio = dto.TipoEstudio,
                Fase = dto.Fase
                // FechaInicio y FechaFin toman el valor por defecto de la entidad (DateTime.Now)
                // Estado se inicializa en "activo" por defecto
            };

            _context.Investigacion.Add(nueva);
            await _context.SaveChangesAsync();

            var dtoCreado = InvestigacionMapper.ToDTO(nueva);
            return CreatedAtAction(nameof(GetInvestigacion), new { codigo = dtoCreado.Codigo }, dtoCreado);
        }

        // DELETE: api/Investigaciones/{codigo}
        [HttpDelete("Eliminar/{codigo}")]
        public async Task<IActionResult> DeleteInvestigacion(string codigo)
        {
            var investigacion = await (from i in _context.Investigacion
                                       where i.Codigo == codigo
                                       select i).FirstOrDefaultAsync();

            if (investigacion == null)
                return NotFound();

            investigacion.Estado = "inactivo";
            _context.Investigacion.Update(investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}