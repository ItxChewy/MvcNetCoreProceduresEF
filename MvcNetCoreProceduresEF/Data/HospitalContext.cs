using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Models;

namespace MvcNetCoreProceduresEF.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        { }

        public DbSet<Enfermo>Enfermos { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<VistaEmpleado> VistaEmpleados { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}
