using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;

#region PROCEDURES
/* 
 CREATE PROCEDURE SP_GET_DOCTORES
AS
	SELECT * FROM DOCTOR
GO
create procedure SP_GET_ESPECIALIDADES
AS
	select distinct especialidad from DOCTOR
GO
CREATE PROCEDURE SP_UPDATE_AND_SELECT_DOCTORES
( @Especialidad NVARCHAR(50),@IncrementoSalario INT)
AS
    UPDATE DOCTOR
    SET SALARIO = SALARIO + @IncrementoSalario
    WHERE ESPECIALIDAD = @Especialidad;

    SELECT * FROM DOCTOR
    WHERE ESPECIALIDAD = @Especialidad;
GO
 
 */
#endregion
namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryDoctores
    {
        EnfermosContext context;

        public RepositoryDoctores(EnfermosContext context)
        {
            this.context = context;
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string sql = "SP_GET_DOCTORES";

            var consulta = await this.context.Doctores.FromSqlRaw(sql).ToListAsync();
            return consulta;
            //using(DbCommand com =
            //    this.context.Database.GetDbConnection().CreateCommand())
            //{
            //    string sql = "SP_GET_DOCTORES";
            //    com.CommandType = CommandType.StoredProcedure;
            //    com.CommandText = sql;
            //    await com.Connection.OpenAsync();
            //    DbDataReader reader = await com.ExecuteReaderAsync();
            //    List<Doctor> doctores = new List<Doctor>();
            //    while(await reader.ReadAsync())
            //    {
            //        Doctor doctor = new Doctor
            //        {
            //            Codigo = int.Parse(reader["HOSPITAL_COD"].ToString()),
            //            IdDoctor = int.Parse(reader["DOCTOR_NO"].ToString()),
            //            Apellido = reader["APELLIDO"].ToString(),
            //            Especialidad = reader["ESPECIALIDAD"].ToString(),
            //            Salario = int.Parse(reader["SALARIO"].ToString())

            //        };
            //        doctores.Add(doctor);
            //    }
            //    await reader.CloseAsync();
            //    await com.Connection.CloseAsync();
            //    return doctores;

            //}
        }
        public async Task<List<string>> GetEspecialidadesAsync()
        {
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_GET_ESPECIALIDADES";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    especialidades.Add(reader["especialidad"].ToString());
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;

            }
        }

        public async Task<List<Doctor>> UpdateDoctoresAsync(string especialidad,int incremento)
        {
            string sql = "SP_UPDATE_AND_SELECT_DOCTORES @Especialidad,@IncrementoSalario";
            SqlParameter pamEsp = new SqlParameter("@Especialidad", especialidad);
            SqlParameter pamInc = new SqlParameter("@IncrementoSalario", incremento);

            var consulta = await this.context.Doctores.FromSqlRaw(sql, pamEsp, pamInc).ToListAsync();
            return consulta;
        }
    }
}
