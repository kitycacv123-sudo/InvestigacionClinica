using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Dominio;

namespace InvestigacionClinica.Data
{
    public class InvestigacionClinicaContext : DbContext
    {
        public InvestigacionClinicaContext (DbContextOptions<InvestigacionClinicaContext> options)
            : base(options)
        {
        }

        public DbSet<InvestigacionClinica.Dominio.Recoleccion> Recoleccion { get; set; } = default!;
        public DbSet<InvestigacionClinica.Dominio.Resultado> Resultado { get; set; } = default!;
        public DbSet<InvestigacionClinica.Dominio.Recoleccion_Resultado> Recoleccion_Resultado { get; set; } = default!;
        public DbSet<InvestigacionClinica.Dominio.Investigacion> Investigacion { get; set; } = default!;
        public DbSet<InvestigacionClinica.Dominio.TipoSintoma> TipoSintoma { get; set; } = default!;
        public DbSet<InvestigacionClinica.Dominio.Resultado_Sintoma> Resultado_Sintoma { get; set; } = default!;


    }
}
