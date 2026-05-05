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
    public class TipoSintomasController : ControllerBase
    {
        private readonly InvestigacionClinicaContext _context;

        public TipoSintomasController(InvestigacionClinicaContext context)
        {
            _context = context;
        }

        // GET: api/TipoSintomas/Lista
        [HttpGet("Lista")]
        public async Task<IActionResult> GetTipoSintomas()
        {
            var lista = await (from t in _context.TipoSintoma
                               where t.Estado == "activo"
                               select new TipoSintomaDTO
                               {
                                   Codigo = t.Codigo,
                                   Nombre = t.Nombre,
                                   Gravedad = t.Gravedad
                               }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("3.SintomaFrecuenteInvestigacion")]
        public async Task<IActionResult> GetSintomaFrecuente()
        {
            var lista = await(from i in _context.Investigacion
                              where i.Estado == "activo"
                              join r in _context.Recoleccion
                              on i.IdInvestigacion equals r.IdInvestigacion
                              where r.Estado == "activo"
                              join rr in _context.Recoleccion_Resultado
                              on r.IdRecoleccion equals rr.IdRecoleccion
                              where rr.Estado == "activo"
                              join re in _context.Resultado
                              on rr.IdResultado equals re.IdResultado
                              join rs in _context.Resultado_Sintoma
                              on re.IdResultado equals rs.IdResultado
                              where rs.Estado == "activo"
                              join s in _context.TipoSintoma
                              on rs.IdResultadoSintoma equals s.IdTipoSintoma
                              where s.Estado == "activo"
                              group s by s.Nombre into g
                              orderby g.Count() descending 
                              select SintomaFrecuenteMapper.ToDTO(
                                 g.Key,
                                 g.Count()
                                 )).ToListAsync();
            if(lista == null || lista.Count == 0)
                return NotFound("No se encontraron síntomas frecuentes");
            return Ok(lista);
        }

        // GET: api/TipoSintomas/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetTipoSintoma(string codigo)
        {
            var tipoSintoma = await (from t in _context.TipoSintoma
                                     where t.Codigo == codigo && t.Estado == "activo"
                                     select new TipoSintomaDTO
                                     {
                                         Codigo = t.Codigo,
                                         Nombre = t.Nombre,
                                         Gravedad = t.Gravedad
                                     }).FirstOrDefaultAsync();

            if (tipoSintoma == null)
                return NotFound();

            return Ok(tipoSintoma);
        }

        // PUT: api/TipoSintomas/{codigo}
        [HttpPut("Actualizar/{codigo}")]
        public async Task<IActionResult> PutTipoSintoma(string codigo, [FromBody] TipoSintomaDTO dto)
        {
            var tipoSintoma = await (from t in _context.TipoSintoma
                                     where t.Codigo == codigo && t.Estado == "activo"
                                     select t).FirstOrDefaultAsync();

            if (tipoSintoma == null)
                return BadRequest("El Tipo de Síntoma no existe");

           
            tipoSintoma.Codigo = dto.Codigo;
            tipoSintoma.Nombre = dto.Nombre;
            tipoSintoma.Gravedad = dto.Gravedad;

            _context.TipoSintoma.Update(tipoSintoma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/TipoSintomas
        [HttpPost("Crear")]
        public async Task<IActionResult> PostTipoSintoma([FromBody] TipoSintomaDTO dto)
        {
            
            var existente = await (from t in _context.TipoSintoma
                                   where t.Codigo == dto.Codigo
                                   select t).FirstOrDefaultAsync();

            if (existente != null)
                return BadRequest("El código ya existe");

            var nuevo = TipoSintomaMapper.ToEntity(dto);

            _context.TipoSintoma.Add(nuevo);
            await _context.SaveChangesAsync();

            var dtoCreado = TipoSintomaMapper.ToDTO(nuevo);

            return CreatedAtAction(nameof(GetTipoSintoma), new { codigo = nuevo.Codigo }, dtoCreado);
        }

        // DELETE: api/TipoSintomas/{codigo} (Soft Delete)
        [HttpDelete("Eliminar/{codigo}")]
        public async Task<IActionResult> DeleteTipoSintoma(string codigo)
        {
            var tipoSintoma = await (from t in _context.TipoSintoma
                                     where t.Codigo == codigo
                                     select t).FirstOrDefaultAsync();

            if (tipoSintoma == null)
                return NotFound();

            tipoSintoma.Estado = "inactivo";
            _context.TipoSintoma.Update(tipoSintoma);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}