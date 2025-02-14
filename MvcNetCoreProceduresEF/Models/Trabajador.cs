using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreProceduresEF.Models
{
    [Table("V_WORKERS")]
    public class Trabajador
    {
        [Key]
        [Column("IDWORKER")]
        public int IdTrabajador { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
    }
}
