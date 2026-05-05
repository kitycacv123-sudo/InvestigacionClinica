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
    public class Resultado_SintomasController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public Resultado_SintomasController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/Resultado_Sintomas/Lista
        [HttpGet("Lista")]
        public async Task<IActionResult> GetResultado_Sintoma()
        {
            var lista = await (from rs in _context.Resultado_Sintoma
                               where rs.Estado == "activo"
                               join res in _context.Resultado on rs.IdResultado equals res.IdResultado
                               join ts in _context.TipoSintoma on rs.IdTipoSintoma equals ts.IdTipoSintoma
                               select new ResultadoSintomas2DTO
                               {
                                   CodigoResultado = res.Codigo,
                                   CodigoTipoSintoma = ts.Codigo,
                                   FechaRegistro = rs.FechaRegistro
                               }).ToListAsync();

            return Ok(lista);
        }

        // GET: api/Resultado_Sintomas/{codigoResultado}/{codigoTipoSintoma}
        [HttpGet("{codigoResultado}/{codigoTipoSintoma}")]
        public async Task<IActionResult> GetResultado_Sintoma(string codigoResultado, string codigoTipoSintoma)
        {
            var entidad = await (from rs in _context.Resultado_Sintoma
                                 where rs.Estado == "activo"
                                 join res in _context.Resultado on rs.IdResultado equals res.IdResultado
                                 where res.Codigo == codigoResultado
                                 join ts in _context.TipoSintoma on rs.IdTipoSintoma equals ts.IdTipoSintoma
                                 where ts.Codigo == codigoTipoSintoma
                                 select new ResultadoSintomas2DTO
                                 {
                                     CodigoResultado = res.Codigo,
                                     CodigoTipoSintoma = ts.Codigo,
                                     FechaRegistro = rs.FechaRegistro
                                 }).FirstOrDefaultAsync();

            if (entidad == null)
                return NotFound();

            return Ok(entidad);
        }

        // PUT: api/Resultado_Sintomas/{codigoResultado}/{codigoTipoSintoma}
        [HttpPut("Actualizar/{codigoResultado}/{codigoTipoSintoma}")]
        public async Task<IActionResult> PutResultado_Sintoma(
            string codigoResultado,
            string codigoTipoSintoma,
            [FromBody] ResultadoSintomas2PostDTO dto)
        {
            // 1. Buscar la asignación actual por los códigos antiguos
            var entidadExistente = await (from rs in _context.Resultado_Sintoma
                                          where rs.Estado == "activo"
                                          join res in _context.Resultado on rs.IdResultado equals res.IdResultado
                                          where res.Codigo == codigoResultado
                                          join ts in _context.TipoSintoma on rs.IdTipoSintoma equals ts.IdTipoSintoma
                                          where ts.Codigo == codigoTipoSintoma
                                          select rs).FirstOrDefaultAsync();

            if (entidadExistente == null)
                return BadRequest("La asignación no existe");

            // 2. Buscar las nuevas entidades por sus códigos
            var nuevoResultado = await (from r in _context.Resultado
                                        where r.Codigo == dto.CodigoResultado && r.Estado == "activo"
                                        select r).FirstOrDefaultAsync();

            var nuevoTipoSintoma = await (from t in _context.TipoSintoma
                                          where t.Codigo == dto.CodigoTipoSintoma && t.Estado == "activo"
                                          select t).FirstOrDefaultAsync();

            if (nuevoResultado == null || nuevoTipoSintoma == null)
                return BadRequest("El Resultado o el Tipo de Síntoma no existe");

            // 3. Validar que la nueva combinación no exista ya (excluyendo la actual)
            var yaExiste = await (from r in _context.Resultado_Sintoma
                                  where r.Estado == "activo"
                                     && r.IdResultado == nuevoResultado.IdResultado
                                     && r.IdTipoSintoma == nuevoTipoSintoma.IdTipoSintoma
                                     && !(r.IdResultado == entidadExistente.IdResultado && r.IdTipoSintoma == entidadExistente.IdTipoSintoma)
                                  select r).FirstOrDefaultAsync();

            if (yaExiste != null)
                return BadRequest("El Resultado ya tiene asignado ese Tipo de Síntoma");

            // 4. Actualizar los IDs
            entidadExistente.IdResultado = nuevoResultado.IdResultado;
            entidadExistente.IdTipoSintoma = nuevoTipoSintoma.IdTipoSintoma;

            _context.Resultado_Sintoma.Update(entidadExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Resultado_Sintomas
        [HttpPost("Crear")]
        public async Task<IActionResult> PostResultado_Sintoma([FromBody] ResultadoSintomas2PostDTO dto)
        {
            // 1. Buscar las entidades relacionadas por sus códigos
            var resultadoBuscado = await (from r in _context.Resultado
                                          where r.Codigo == dto.CodigoResultado && r.Estado == "activo"
                                          select r).FirstOrDefaultAsync();

            var tipoSintomaBuscado = await (from t in _context.TipoSintoma
                                            where t.Codigo == dto.CodigoTipoSintoma && t.Estado == "activo"
                                            select t).FirstOrDefaultAsync();

            if (resultadoBuscado == null || tipoSintomaBuscado == null)
                return BadRequest("El Resultado o el Tipo de Síntoma no existe");

            // 2. Validar que no exista ya la misma asignación
            var yaExiste = await (from r in _context.Resultado_Sintoma
                                  where r.IdResultado == resultadoBuscado.IdResultado
                                     && r.IdTipoSintoma == tipoSintomaBuscado.IdTipoSintoma
                                     && r.Estado == "activo"
                                  select r).FirstOrDefaultAsync();

            if (yaExiste != null)
                return BadRequest("El Resultado ya tiene asignado ese Tipo de Síntoma");

            // 3. Crear la entidad usando el mapeador (pasando los IDs obtenidos)
            var nuevoDetalle = ResultadoSintoma2Mapper.ToEntity(
                dto,
                resultadoBuscado.IdResultado,
                tipoSintomaBuscado.IdTipoSintoma);

            _context.Resultado_Sintoma.Add(nuevoDetalle);
            await _context.SaveChangesAsync();

            // 4. Devolver el DTO de salida
            var dtoSalida = ResultadoSintoma2Mapper.ToDTO(
                nuevoDetalle,
                resultadoBuscado.Codigo,
                tipoSintomaBuscado.Codigo);

            return CreatedAtAction(nameof(GetResultado_Sintoma), new { codigoResultado = dtoSalida.CodigoResultado, codigoTipoSintoma = dtoSalida.CodigoTipoSintoma }, dtoSalida);
        }

        // DELETE: api/Resultado_Sintomas/{codigoResultado}/{codigoTipoSintoma}
        [HttpDelete("Eliminar/{codigoResultado}/{codigoTipoSintoma}")]
        public async Task<IActionResult> DeleteResultado_Sintoma(string codigoResultado, string codigoTipoSintoma)
        {
            var entidad = await (from rs in _context.Resultado_Sintoma
                                 where rs.Estado == "activo"
                                 join res in _context.Resultado on rs.IdResultado equals res.IdResultado
                                 where res.Codigo == codigoResultado
                                 join ts in _context.TipoSintoma on rs.IdTipoSintoma equals ts.IdTipoSintoma
                                 where ts.Codigo == codigoTipoSintoma
                                 select rs).FirstOrDefaultAsync();

            if (entidad == null)
                return NotFound();

            entidad.Estado = "inactivo";
            _context.Resultado_Sintoma.Update(entidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}