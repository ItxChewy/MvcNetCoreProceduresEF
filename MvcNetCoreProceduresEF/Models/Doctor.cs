using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreProceduresEF.Models
{
    [Table("DOCTOR")]
    public class Doctor
    {
        [Column("HOSPITAL_COD")]
        public int Codigo { get; set; }

        [Key]
        [Column("DOCTOR_NO")]
        public int IdDoctor { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("ESPECIALIDAD")]
        public string Especialidad { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }



    }
}
