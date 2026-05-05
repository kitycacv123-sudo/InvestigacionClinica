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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using InvestigacionClinica.DTO;

namespace InvestigacionClinica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadosController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public ResultadosController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/Resultados
        [HttpGet("Lista")]
        public async Task<IActionResult> GetResultado()
        {
            var lista = await (from r in _context.Resultado
                               where r.Estado == "activo"
                               select new ResultadoDTO
                               {
                                   Codigo = r.Codigo,
                                   CodigoOrdenLaboratorio = r.CodigoOrdenLaboratorio,
                                   CodigoPaciente = r.CodigoPaciente,
                                   TipoPrueba = r.TipoPrueba,
                                   ValorObtenido = r.ValorObtenido,
                                   FechaRecepcion = r.FechaRecepcion,
                                   TieneValorAnormal = r.TieneValorAnormal
                               }).ToListAsync();
            return Ok(lista);
        }

        // GET: api/Resultados/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetResultado(string codigo)
        {
            var resultado = await (from r in _context.Resultado
                                   where r.Codigo == codigo && r.Estado == "activo"
                                   select new ResultadoDTO
                                   {
                                       Codigo = r.Codigo,
                                       CodigoOrdenLaboratorio = r.CodigoOrdenLaboratorio,
                                       CodigoPaciente = r.CodigoPaciente,
                                       TipoPrueba = r.TipoPrueba,
                                       ValorObtenido = r.ValorObtenido,
                                       FechaRecepcion = r.FechaRecepcion,
                                       TieneValorAnormal = r.TieneValorAnormal
                                   }).FirstOrDefaultAsync();

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        //2. Agrupación con conteo (GROUP BY + COUNT)
        [HttpGet("ConteoSintomas")]
        public async Task<IActionResult> GetConteoSintomasPorRecoleccion()
        {
            var conteo = await (from rec in _context.Recoleccion
                                where rec.Estado == "activo"
                                join det in _context.Recoleccion_Resultado
                                    on rec.IdRecoleccion equals det.IdRecoleccion
                                where det.Estado == "activo"
                                join res in _context.Resultado
                                    on det.IdResultado equals res.IdResultado
                                where res.Estado == "activo"
                                join rs in _context.Resultado_Sintoma
                                    on res.IdResultado equals rs.IdResultado
                                where rs.Estado == "activo"
                                group rec by new { rec.Codigo, rec.Descripcion } into g
                                select new
                                {
                                    g.Key.Codigo,
                                    g.Key.Descripcion,
                                    TotalSintomas = g.Count()
                                }).ToListAsync();
            var listaTuplas = conteo
                .Select(d => (d.Codigo, d.Descripcion, d.TotalSintomas))
                .ToList();
            

            var dto = ConteoSintomasMapper.ToListaDTO(listaTuplas);

            return Ok(dto);
        }
        //3. Agrupación con suma (GROUP BY + SUM)
        [HttpGet("3.ResultadosEsperadosInvestigacion")]
        public async Task<IActionResult> GetResultadoEsperados()
        {
            var listaSum = await (from r in _context.Recoleccion
                               where r.Estado == "activo"
                               join i in _context.Investigacion
                               on r.IdInvestigacion equals i.IdInvestigacion
                               where i.Estado == "activo"
                               group r by new
                               {
                                   i.Codigo,
                                   i.Titulo,
                               } into g 
                               select new
                               {
                                   g.Key.Codigo,
                                   g.Key.Titulo,
                                   TotalResultadosEsperados = g.Sum(x => x.Total)
                               }).ToListAsync();
            var listaTuplas = listaSum.Select(d =>(d.Codigo, d.Titulo, d.TotalResultadosEsperados)).ToList();
            var resultado = ResultadosEsperadosMapper.ToListaDTO(listaTuplas);
            return Ok(resultado);
        }

        //4.Búsqueda filtrada por código o identificador
        [HttpGet("4./{codigo}")]
        /*sin iactionResult*/
        public async Task<ActionResult<Resultado>> GetResultadoSintoma(string codigo)
        {
            var resultadoSintomas = await (from r in _context.Resultado
                                   where r.Codigo == codigo && r.Estado == "activo"
                                   join d in _context.Resultado_Sintoma
                                   on r.IdResultado equals d.IdResultado
                                   where d.Estado == "activo"
                                   join t in _context.TipoSintoma
                                   on d.IdTipoSintoma equals t.IdTipoSintoma
                                   where t.Estado == "activo"
                                   select new
                                   {
                                       t.Codigo,
                                       t.Nombre,
                                       r.ValorObtenido,
                                       t.Gravedad,
                                   }).FirstOrDefaultAsync();

            if (resultadoSintomas == null)
            {
                return NotFound();
            }

            var dto = ResultadoSintomaMapper.ToDTO(resultadoSintomas.Codigo, resultadoSintomas.Nombre, resultadoSintomas.ValorObtenido, resultadoSintomas.Gravedad);

            return Ok(dto);
        }
        //5. Consulta de registros que no tienen relación en otra tabla (NOT EXISTS o equivalente)
        [HttpGet("5.SinSintomas")]
        public async Task<IActionResult> GetSinSintomas()
        {
            var sinSintomas = await (from res in _context.Resultado
                                     where res.Estado == "activo"
                                     join rs in _context.Resultado_Sintoma
                                         on res.IdResultado equals rs.IdResultado into rsJoin
                                     from rs in rsJoin.DefaultIfEmpty()
                                     where rs == null
                                     select new
                                     {
                                         res.Codigo,
                                         res.CodigoPaciente,
                                         res.TipoPrueba,
                                         res.ValorObtenido,
                                         res.TieneValorAnormal
                                     }).ToListAsync();


            var dto = sinSintomas.Select(s => SinSintomaMapeador.ToDTO(
                s.Codigo,
                s.CodigoPaciente,
                s.TipoPrueba,
                s.ValorObtenido,
                s.TieneValorAnormal
            )).ToList();

            return Ok(dto);
        }
        [HttpGet("1.ResultadosAnormalesPorInvestigacion/{codigo}")]
        public async Task<IActionResult> GetResultadoAnormal(string codigo)
        {
            var resultadoAnormalInvestigacion = await (from i in _context.Investigacion
                                                       where i.Codigo == codigo && i.Estado == "activo"
                                                       join r in _context.Recoleccion
                                                       on i.IdInvestigacion equals r.IdInvestigacion
                                                       where r.Estado == "activo"
                                                       join rr in _context.Recoleccion_Resultado
                                                       on r.IdRecoleccion equals rr.IdRecoleccion
                                                       where rr.Estado == "activo"
                                                       join re in _context.Resultado
                                                       on rr.IdResultado equals re.IdResultado
                                                       where re.Estado == "activo" && re.TieneValorAnormal == "si"
                                                       select ResultadoAnormalInvestigacionMapper.ToDto(
                                                           i.Titulo,
                                                           re.CodigoPaciente,
                                                           re.ValorObtenido)
                                                       ).ToListAsync();
            if (resultadoAnormalInvestigacion == null)
            {
                return BadRequest("No se encontraron resultados anormales para la investigación especificada");
            }

            return Ok(resultadoAnormalInvestigacion);
            

        }

        [HttpGet("5.MostrarResultadosPorTipoPrueba/{codigoInvestigacion}/{tipoPrueba}")]
        public async Task<IActionResult> GetResultadosPorTipoPrueba(string codigoInvestigacion,string tipoPrueba)
        {
            var resultados = await (from inv in _context.Investigacion
                                    where inv.Codigo == codigoInvestigacion && inv.Estado == "activo"
                                    join rec in _context.Recoleccion
                                        on inv.IdInvestigacion equals rec.IdInvestigacion
                                    where rec.Estado == "activo"
                                    join det in _context.Recoleccion_Resultado
                                        on rec.IdRecoleccion equals det.IdRecoleccion
                                    where det.Estado == "activo"
                                    join res in _context.Resultado
                                        on det.IdResultado equals res.IdResultado
                                    where res.Estado == "activo" && res.TipoPrueba == tipoPrueba
                                    select ResultadoPorTipoDePruebaMapper.ToDTO(
                                        inv.Codigo,
                                        inv.Titulo,
                                        res.Codigo,
                                        res.TipoPrueba,
                                        res.ValorObtenido,
                                        res.CodigoPaciente,
                                        res.TieneValorAnormal
                                    )).ToListAsync();

            if (resultados == null || resultados.Count == 0)
            {
                return BadRequest("No se encontraron resultados para la investigación y tipo de prueba especificados");
            }

            return Ok(resultados);
        }
        [HttpGet("6.PacienteCritico/{codigo}")]
        public async Task<IActionResult> GetPacienteCritico(string codigo)
        {
            var pacienteCritico = await(from r in _context.Resultado
                                        where r.Estado == "activo" && r.CodigoPaciente == codigo && r.TieneValorAnormal == "si"
                                        join rs in _context.Resultado_Sintoma
                                        on r.IdResultado equals rs.IdResultado
                                        where rs.Estado == "activo"
                                        join s in _context.TipoSintoma
                                        on rs.IdTipoSintoma equals s.IdTipoSintoma
                                        where s.Estado == "activo" && s.Gravedad == "grave" 
                                        select PacienteCriticoMapper.toDTO(
                                            r.CodigoPaciente,
                                            s.Nombre,
                                            r.ValorObtenido)).ToListAsync();
            if (pacienteCritico == null)
            {
                return NotFound("No se encontraron resultados críticos para el paciente especificado");
            }

            return Ok(pacienteCritico);
        }
        [HttpGet("8.PorPaciente/{codigoPaciente}")]
        public async Task<IActionResult> GetResultadosPorPaciente(string codigoPaciente)
        {
            var resultados = await (from res in _context.Resultado
                                    where res.CodigoPaciente == codigoPaciente && res.Estado == "activo"
                                    select ResultadoPorPacienteMapper.ToDTO(
                                        res.Codigo,
                                        res.CodigoPaciente,
                                        res.TipoPrueba,
                                        res.ValorObtenido,
                                        res.TieneValorAnormal
                                    )).ToListAsync();
            if (resultados == null)
            {
                return NotFound();
            }


            return Ok(resultados);
        }
        [HttpGet("TodosLosResultados")]
        public async Task<IActionResult> GetTodosLosResultados()
        {
            var resultados = await (
                from res in _context.Resultado
                join rr in _context.Recoleccion_Resultado on res.IdResultado equals rr.IdResultado
                join rec in _context.Recoleccion on rr.IdRecoleccion equals rec.IdRecoleccion
                join inv in _context.Investigacion on rec.IdInvestigacion equals inv.IdInvestigacion
                where res.Estado == "activo"   
                select ResultadosPacienteMapper.ToDTO(
                    inv.Codigo,              
                    res.CodigoPaciente,
                    res.TipoPrueba,
                    res.ValorObtenido
                )
            ).ToListAsync();

            if (resultados == null || resultados.Count == 0)
            {
                return NotFound("No hay resultados activos con recolección e investigación asociada.");
            }

            return Ok(resultados);
        }

        [HttpGet("AnormalesPorPaciente/{codigoPaciente}")]
        public async Task<IActionResult> GetAnormalesPorPaciente(string codigoPaciente)
        {
            var anormales = await (from res in _context.Resultado
                                   where res.CodigoPaciente == codigoPaciente
                                      && res.Estado == "activo"
                                      && res.TieneValorAnormal == "si"
                                   select ResultadoPorPacienteMapper.ToDTO(
                                       res.Codigo,
                                        res.CodigoPaciente,
                                        res.TipoPrueba,
                                        res.ValorObtenido,
                                        res.TieneValorAnormal
                                   )).ToListAsync();

            if (!anormales.Any())
                return Ok(new { mensaje = "No se encontraron resultados anormales para este paciente." });

            return Ok(anormales);
        }
        [HttpGet("10.PorRecoleccion/{codigoRecoleccion}")]
        public async Task<IActionResult> GetResultadosPorRecoleccion(string codigoRecoleccion)
        {
            var resultados = await (from rec in _context.Recoleccion
                                    where rec.Codigo == codigoRecoleccion && rec.Estado == "activo"
                                    join det in _context.Recoleccion_Resultado
                                        on rec.IdRecoleccion equals det.IdRecoleccion
                                    where det.Estado == "activo"
                                    join res in _context.Resultado
                                        on det.IdResultado equals res.IdResultado
                                    where res.Estado == "activo"
                                    select ResultadoPorRecoleccionMapper.ToDTO(
                                        rec.Codigo,
                                        res.Codigo,
                                        res.TipoPrueba,
                                        res.ValorObtenido,
                                        res.TieneValorAnormal
                                    )).ToListAsync();

            if (!resultados.Any())
                return Ok(new { mensaje = "No se encontraron resultados para esta recolección." });

            return Ok(resultados);
        }
        // PUT: api/Resultados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditarResultado/{codigo}")]
        
        public async Task<IActionResult> PutResultado(string codigo, [FromBody] ResultadoDTO dto)
        {
            Resultado resultado = await (from r in _context.Resultado
                                         where r.Codigo == codigo && r.Estado == "activo"
                                         select r).FirstOrDefaultAsync();

            if (resultado == null)
                return BadRequest("El Resultado no existe");

            // Actualizar campos permitidos
            resultado.Codigo = dto.Codigo;
            resultado.CodigoOrdenLaboratorio = dto.CodigoOrdenLaboratorio;
            resultado.CodigoPaciente = dto.CodigoPaciente;
            resultado.TipoPrueba = dto.TipoPrueba;
            resultado.ValorObtenido = dto.ValorObtenido;
            resultado.FechaRecepcion = dto.FechaRecepcion;
            resultado.TieneValorAnormal = dto.TieneValorAnormal;

            _context.Resultado.Update(resultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Resultados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<IActionResult> PostResultado([FromBody] ResultadoDTO dto)
        {
            // Verificar código único
            Resultado resultadoBuscado = await (from r in _context.Resultado
                                                where r.Codigo == dto.Codigo
                                                select r).FirstOrDefaultAsync();
            if (resultadoBuscado != null)
                return BadRequest("El código ya existe");

            Resultado nuevo = ResultadoMapper.ToEntity(dto);

            _context.Resultado.Add(nuevo);
            await _context.SaveChangesAsync();

            var dtoCreado = ResultadoMapper.ToDTO(nuevo);

            return CreatedAtAction(nameof(GetResultado), new { codigo = nuevo.Codigo }, dtoCreado);
        }

        // DELETE: api/Resultados/5
        [HttpDelete("Eliminar/{codigo}")]
        public async Task<IActionResult> DeleteResultado(string codigo)
        {
            var resultado = await (from r in _context.Resultado
                                   where r.Codigo == codigo
                                   select r).FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound();
            }

            resultado.Estado = "inactivo";
            _context.Resultado.Update(resultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultadoExists(int id)
        {
            return _context.Resultado.Any(e => e.IdResultado == id);
        }
    }
}
