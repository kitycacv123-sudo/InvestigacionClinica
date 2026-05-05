using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;
using InvestigacionClinica.Dominio;
using InvestigacionClinica.Mapeador;
using System.ComponentModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using InvestigacionClinica.DTO;
namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoleccionesController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public RecoleccionesController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/Recolecciones
        [HttpGet("Lista")]
        public async Task<IActionResult> GetRecoleccion()
        {
            var lista = await(from r in _context.Recoleccion
                             where r.Estado == "activo"
                             select new
                             {
                                 r.Codigo,
                                 r.CodigoProtocolo,
                                 r.FechaInicio,
                                 r.Fechafin,
                                 r.Descripcion,
                                 r.Total
                             }).ToListAsync();
            var dto = lista.Select(item=>RecoleccionMapper.Todto(item.Codigo, item.CodigoProtocolo, item.FechaInicio, item.Fechafin, item.Descripcion,item.Total));
            return Ok(dto);
        }

        [HttpGet("4.VerificarReclutadosVsObtenidos")]
        public async Task<IActionResult> GetReclutadosVsObtenidos()
        {
            
            var conteoResultados = await (from det in _context.Recoleccion_Resultado
                                          where det.Estado == "activo"
                                          group det by det.IdRecoleccion into g
                                          select new
                                          {
                                              IdRecoleccion = g.Key,
                                              Obtenidos = g.Count()
                                          }).ToListAsync();

            
            var recolecciones = await (from rec in _context.Recoleccion
                                       where rec.Estado == "activo"
                                       select rec).ToListAsync();

            
            var progreso = recolecciones.Select(rec =>
            {
                var conteo = conteoResultados.FirstOrDefault(c => c.IdRecoleccion == rec.IdRecoleccion);
                int obtenidos = conteo?.Obtenidos ?? 0;
                double porcentaje = rec.Total > 0
                    ? Math.Round((double)obtenidos / rec.Total * 100, 2)
                    : 0;

                return ComparacionRecoleccionMapper.ToDTO(
                    rec.Codigo,
                    rec.Total,
                    obtenidos,
                    porcentaje
                );
            }).ToList();

            return Ok(progreso);
        }

        // GET: api/Recolecciones/5
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Recoleccion>> GetRecoleccion(string codigo)
        {
            var recoleccion = await (from r in _context.Recoleccion
                                     where r.Codigo == codigo && r.Estado == "activo"
                                     select new
                                     {
                                         r.Codigo,
                                         r.CodigoProtocolo,
                                         r.FechaInicio,
                                         r.Fechafin,
                                         r.Descripcion,
                                         r.Total
                                     }).FirstOrDefaultAsync();

            if (recoleccion == null)
            {
                return NotFound();
            }

            var dto = RecoleccionMapper.Todto(
                recoleccion.Codigo,
                recoleccion.CodigoProtocolo,
                recoleccion.FechaInicio,
                recoleccion.Fechafin,
                recoleccion.Descripcion,
                recoleccion.Total
            );

            return Ok(dto); 
        }

        [HttpGet("ResultadosARecoleccion/{codigo}")]
        public async Task<IActionResult> GetResultados(string codigo)
        {
            var resultados = await (from rec in _context.Recoleccion
                                    where rec.Codigo == codigo && rec.Estado == "activo"
                                    join det in _context.Recoleccion_Resultado
                                        on rec.IdRecoleccion equals det.IdRecoleccion
                                    where det.Estado == "activo"
                                    join res in _context.Resultado
                                        on det.IdResultado equals res.IdResultado
                                    where res.Estado == "activo"
                                    select new
                                    {
                                        rec.CodigoProtocolo,
                                        rec.Descripcion,
                                        CodigoResultado = res.Codigo,
                                        res.TipoPrueba,
                                        res.ValorObtenido
                                    }).ToListAsync();

            if (resultados == null)
                return NotFound($"No se encontraron resultados para la recolección: {codigo}");

            return Ok(resultados);
        }

        [HttpGet("ResultadosAnormales")]
        public async Task<IActionResult> GetAnormales()
        {
            var resultados = await (from rec in _context.Recoleccion
                                    where rec.Estado == "activo"
                                    join det in _context.Recoleccion_Resultado
                                        on rec.IdRecoleccion equals det.IdRecoleccion
                                    where det.Estado == "activo"
                                    join res in _context.Resultado
                                        on det.IdResultado equals res.IdResultado
                                    where res.Estado == "activo"
                                       && (res.TieneValorAnormal == "si")
                                    select ResultadosAnormalesMapper.ToDTO(
                                        rec.CodigoProtocolo,
                                        rec.Descripcion,
                                        res.Codigo,           
                                        res.TipoPrueba,
                                        res.ValorObtenido
                                    )).ToListAsync();

            if (!resultados.Any())
                return Ok(new { mensaje = "No se encontraron resultados con valores anormales" });

            return Ok(resultados);
        }
        //1. Listado general con JOIN entre al menos 2 tablas
        [HttpGet("1.RecoleccionesConInvestigacion")]
        public async Task<ActionResult<Recoleccion>> GetRecoleccionInvestigacion()
        {
            var recoleccionInvestigacion = await (from r in _context.Recoleccion
                                     where r.Estado == "activo"
                                     join i in  _context.Investigacion
                                     on r.IdInvestigacion equals i.IdInvestigacion
                                     where i.Estado == "activo"
                                     select new
                                     {
                                         TituloInvestigacion = i.Titulo,
                                         CodigoRecoleccion = r.Codigo,

                                     }).FirstOrDefaultAsync();

            if (recoleccionInvestigacion == null)
            {
                return NotFound();
            }

            var dto = RecoleccionConInvestigacionMapper.toDTO(recoleccionInvestigacion.TituloInvestigacion, recoleccionInvestigacion.CodigoRecoleccion);

            return Ok(dto);
        }

        // PUT: api/Recolecciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Actualizar/{codigo}")]
        public async Task<IActionResult> PutRecoleccion(string codigo, [FromBody] RecoleccionDTO dto)
        {
            Recoleccion recoleccion = await (from r in _context.Recoleccion
                                             where r.Codigo == codigo && r.Estado == "activo"
                                             select r).FirstOrDefaultAsync();
            if (recoleccion == null)
            {
                return BadRequest("La Recoleccion no Existe");
            }

            // Solo se actualizan los campos permitidos
            recoleccion.Codigo = dto.Codigo;
            
            recoleccion.Descripcion = dto.Descripcion;
            recoleccion.Total = dto.Total;

            _context.Recoleccion.Update(recoleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Recolecciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<IActionResult> PostRecoleccion([FromBody] RecoleccionDTO dto)
        {
            // 1. Validar que la investigación exista por su código de protocolo
            var investigacion = await _context.Investigacion
                .Where(i => i.Codigo == dto.CodigoProtocolo && i.Estado == "activo") // si usas estado
                .FirstOrDefaultAsync();

            if (investigacion == null)
            {
                return BadRequest($"No existe una investigación activa con el código de protocolo '{dto.CodigoProtocolo}'");
            }

            // 2. Validar que el código de recolección no esté repetido
            var recoleccionExistente = await _context.Recoleccion
                .Where(r => r.Codigo == dto.Codigo && r.Estado == "activo")
                .FirstOrDefaultAsync();

            if (recoleccionExistente != null)
            {
                return BadRequest($"El código de recolección '{dto.Codigo}' ya existe");
            }

            // 3. Crear la nueva recolección con los datos del DTO
            Recoleccion recoleccion = new Recoleccion()
            {
                Codigo = dto.Codigo,
                CodigoProtocolo = dto.CodigoProtocolo,
                Descripcion = dto.Descripcion,
                Total = dto.Total,
                FechaInicio = dto.FechaInicio,       
                Fechafin = dto.Fechafin,              
                IdInvestigacion = investigacion.IdInvestigacion,   
                Estado = "activo"
            };

            _context.Recoleccion.Add(recoleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecoleccion", new { mensaje = "Recoleccion Ingresada con Exito" });
        }

        // DELETE: api/Recolecciones/5
        [HttpDelete("Eliminar/{codigo}")]
        public async Task<IActionResult> DeleteRecoleccion(string codigo)
        {
            var recoleccion = await (from r in _context.Recoleccion
                                     where r.Codigo == codigo
                                     select r).FirstOrDefaultAsync();
            if (recoleccion == null)
            {
                return NotFound();
            }

            recoleccion.Estado = "inactivo";
            _context.Recoleccion.Update(recoleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecoleccionExists(int id)
        {
            return _context.Recoleccion.Any(e => e.IdRecoleccion == id);
        }
    }
}
