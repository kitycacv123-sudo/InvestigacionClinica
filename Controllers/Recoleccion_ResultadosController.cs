using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class Recoleccion_ResultadosController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public Recoleccion_ResultadosController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/Recoleccion_Resultados/Lista
        [HttpGet("Lista")]
        public async Task<IActionResult> GetRecoleccion_Resultado()
        {
            var lista = await (from det in _context.Recoleccion_Resultado
                               where det.Estado == "activo"
                               join rec in _context.Recoleccion on det.IdRecoleccion equals rec.IdRecoleccion
                               join res in _context.Resultado on det.IdResultado equals res.IdResultado
                               select new RecoleccionResultadoDTO
                               {
                                   CodigoRecoleccion = rec.Codigo,
                                   CodigoResultado = res.Codigo,
                                   FechaAsignacion = det.FechaAsignacion
                               }).ToListAsync();

            return Ok(lista);
        }

        // GET: api/Recoleccion_Resultados/{codigoRecoleccion}/{codigoResultado}
        [HttpGet("{codigoRecoleccion}/{codigoResultado}")]
        public async Task<IActionResult> GetRecoleccion_Resultado(string codigoRecoleccion, string codigoResultado)
        {
            var resultado = await (from det in _context.Recoleccion_Resultado
                                   where det.Estado == "activo"
                                   join rec in _context.Recoleccion on det.IdRecoleccion equals rec.IdRecoleccion
                                   where rec.Codigo == codigoRecoleccion
                                   join res in _context.Resultado on det.IdResultado equals res.IdResultado
                                   where res.Codigo == codigoResultado
                                   select new RecoleccionResultadoDTO
                                   {
                                       CodigoRecoleccion = rec.Codigo,
                                       CodigoResultado = res.Codigo,
                                       FechaAsignacion = det.FechaAsignacion
                                   }).FirstOrDefaultAsync();

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpGet("6.PendientesAsignacion")]
        public async Task<IActionResult> GetPendientesAsignacion()
        {
            var pendientes = await (from res in _context.Resultado
                                    where res.Estado == "activo"
                                    join det in _context.Recoleccion_Resultado
                                        on res.IdResultado equals det.IdResultado into detJoin
                                    from det in detJoin.DefaultIfEmpty()
                                    where det == null
                                    select ResultadoPendienteAsignacionMapper.ToDTO(
                                        res.Codigo,
                                        res.CodigoPaciente,
                                        res.TipoPrueba,
                                        res.ValorObtenido
                                    )).ToListAsync();

            return Ok(pendientes);
        }

        // PUT: api/Recoleccion_Resultados/{codigoRecoleccion}/{codigoResultado}
        [HttpPut("Actualizar/{codigoRecoleccion}/{codigoResultado}")]
        public async Task<IActionResult> PutRecoleccion_Resultado(
            string codigoRecoleccion,
            string codigoResultado,
            [FromBody] RecoleccionResultadoPostDTO dto)
        {
            // 1. Buscar la asignación actual por los códigos antiguos
            var detalleExistente = await (from det in _context.Recoleccion_Resultado
                                          where det.Estado == "activo"
                                          join rec in _context.Recoleccion on det.IdRecoleccion equals rec.IdRecoleccion
                                          where rec.Codigo == codigoRecoleccion
                                          join res in _context.Resultado on det.IdResultado equals res.IdResultado
                                          where res.Codigo == codigoResultado
                                          select det).FirstOrDefaultAsync();

            if (detalleExistente == null)
                return BadRequest("La asignación no existe");

            // 2. Buscar las nuevas entidades por sus códigos
            var nuevaRecoleccion = await (from r in _context.Recoleccion
                                          where r.Codigo == dto.CodigoRecoleccion && r.Estado == "activo"
                                          select r).FirstOrDefaultAsync();

            var nuevoResultado = await (from r in _context.Resultado
                                        where r.Codigo == dto.CodigoResultado && r.Estado == "activo"
                                        select r).FirstOrDefaultAsync();

            if (nuevaRecoleccion == null || nuevoResultado == null)
                return BadRequest("La Recolección o el Resultado no existe");

            // 3. Validar que la nueva combinación no exista ya (excluyendo la actual)
            var yaExiste = await (from r in _context.Recoleccion_Resultado
                                  where r.Estado == "activo"
                                     && r.IdRecoleccion == nuevaRecoleccion.IdRecoleccion
                                     && r.IdResultado == nuevoResultado.IdResultado
                                     && !(r.IdRecoleccion == detalleExistente.IdRecoleccion && r.IdResultado == detalleExistente.IdResultado)
                                  select r).FirstOrDefaultAsync();

            if (yaExiste != null)
                return BadRequest("La nueva Recolección ya tiene asignado ese Resultado");

            // 4. Actualizar los IDs
            detalleExistente.IdRecoleccion = nuevaRecoleccion.IdRecoleccion;
            detalleExistente.IdResultado = nuevoResultado.IdResultado;

            _context.Recoleccion_Resultado.Update(detalleExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Recoleccion_Resultados
        [HttpPost("Asignar")]
        public async Task<IActionResult> PostRecoleccion_Resultado([FromBody] RecoleccionResultadoPostDTO dto)
        {
            // 1. Buscar las entidades relacionadas por sus códigos
            var recoleccionBuscada = await (from r in _context.Recoleccion
                                            where r.Codigo == dto.CodigoRecoleccion && r.Estado == "activo"
                                            select r).FirstOrDefaultAsync();

            var resultadoBuscado = await (from r in _context.Resultado
                                          where r.Codigo == dto.CodigoResultado && r.Estado == "activo"
                                          select r).FirstOrDefaultAsync();

            if (recoleccionBuscada == null || resultadoBuscado == null)
                return BadRequest("La Recolección o el Resultado no existe");

            // 2. Validar que no exista ya la misma asignación
            var yaExiste = await (from r in _context.Recoleccion_Resultado
                                  where r.IdRecoleccion == recoleccionBuscada.IdRecoleccion
                                     && r.IdResultado == resultadoBuscado.IdResultado
                                     && r.Estado == "activo"
                                  select r).FirstOrDefaultAsync();

            if (yaExiste != null)
                return BadRequest("La Recolección ya tiene asignado ese Resultado");

            // 3. Crear la entidad usando el mapeador (pasando los IDs obtenidos)
            var nuevoDetalle = RecoleccionResultadoMapper.ToEntity(
                dto,
                recoleccionBuscada.IdRecoleccion,
                resultadoBuscado.IdResultado);

            _context.Recoleccion_Resultado.Add(nuevoDetalle);
            await _context.SaveChangesAsync();

            // 4. Devolver el DTO de salida
            var dtoSalida = RecoleccionResultadoMapper.ToDTO(
                nuevoDetalle,
                recoleccionBuscada.Codigo,
                resultadoBuscado.Codigo);

            return CreatedAtAction(nameof(GetRecoleccion_Resultado), new { codigoRecoleccion = dtoSalida.CodigoRecoleccion, codigoResultado = dtoSalida.CodigoResultado }, dtoSalida);
        }

        // DELETE: api/Recoleccion_Resultados/{codigoRecoleccion}/{codigoResultado}
        [HttpDelete("Eliminar/{codigoRecoleccion}/{codigoResultado}")]
        public async Task<IActionResult> DeleteRecoleccion_Resultado(string codigoRecoleccion, string codigoResultado)
        {
            var detalle = await (from det in _context.Recoleccion_Resultado
                                 where det.Estado == "activo"
                                 join rec in _context.Recoleccion on det.IdRecoleccion equals rec.IdRecoleccion
                                 where rec.Codigo == codigoRecoleccion
                                 join res in _context.Resultado on det.IdResultado equals res.IdResultado
                                 where res.Codigo == codigoResultado
                                 select det).FirstOrDefaultAsync();

            if (detalle == null)
                return NotFound();

            detalle.Estado = "inactivo";
            _context.Recoleccion_Resultado.Update(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}