using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;

#region POCEDURES
/* 
 create procedure SP_TODOS_ENFERMOS
as
	select * from ENFERMO
go
alter procedure SP_FIND_ENFERMO
(@inscripcion nvarchar(50))
as
	select * from ENFERMO WHERE INSCRIPCION=@inscripcion
go
alter PROCEDURE SP_DELETE_ENFERMO
(@INSCRIPCION nvarchar(50))
AS
	DELETE FROM ENFERMO WHERE INSCRIPCION=@INSCRIPCION
GO
CREATE PROCEDURE SP_INSERTAR_ENFERMO
(@APELLIDO NVARCHAR(50),@DIRECCION NVARCHAR(50), @FECHA DATETIME
,@GENERO NVARCHAR(50))
AS
	DECLARE @ID INT
	SELECT @ID=CAST(MAX(INSCRIPCION)AS INT) FROM ENFERMO
	INSERT INTO ENFERMO VALUES(@ID +1,@APELLIDO,@DIRECCION,@FECHA,@GENERO,888)

GO
 
 */
#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEnfermos
    {
        HospitalContext context;

        public RepositoryEnfermos(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            using(DbCommand com = 
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await  com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }

        //public Enfermo FindEnfermo(string inscripcion)
        //{
        //    string sql = "SP_FIND_ENFERMO @INSCRIPCION";
        //    SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
        //    var consulta = this.context.Enfermos.FromSqlRaw(sql, pamInscripcion);
        //    Enfermo enfermo = consulta.AsEnumerable().FirstOrDefault();
        //    return enfermo;
        //}
        public async Task<Enfermo> FindEnfermoAsync(string inscripcion)
        {
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);

            // Realizamos la consulta asincrónica
            var consulta = await this.context.Enfermos.FromSqlRaw(sql, pamInscripcion).ToListAsync();

            // Ahora realizamos la operación de FirstOrDefault en el cliente (en memoria)
            Enfermo enfermo = consulta.FirstOrDefault();
            return enfermo;
        }


        public void DeleteEnfermos(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            using(DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamInscripcion);
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                com.Parameters.Clear();
            }
        }

        public async Task DeleteEnfermosRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamInscripcion);
        }

        public async Task InsertEnfermos(string apellido, string direccion, DateTime fechaNacimiento
            ,string genero)
        {
            string sql = "SP_INSERTAR_ENFERMO @APELLIDO, @DIRECCION, @FECHA, @GENERO";
            SqlParameter pamApe = new SqlParameter("@APELLIDO", apellido);
            SqlParameter pamDir = new SqlParameter("@DIRECCION", direccion);
            SqlParameter pamFecha = new SqlParameter("@FECHA", fechaNacimiento);
            SqlParameter pamGenero = new SqlParameter("@GENERO", genero);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe,pamDir,pamFecha,pamGenero);
        }
    }
}
