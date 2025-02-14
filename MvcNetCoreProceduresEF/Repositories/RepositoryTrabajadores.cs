using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;

namespace MvcNetCoreProceduresEF.Repositories
{
    #region PROCEDURES
    /* 
     CREATE PROCEDURE SP_WORKERS_OFICIO
    (@OFICIO NVARCHAR(50),@PERSONAS INT OUT, @MEDIA INT OUT
    , @SUMA INT OUT)
    AS

        SELECT * FROM V_WOKERS
        WHERE OFICIO=@OFICIO

        SELECT @PERSONAS= COUNT(IDWORKER),
        @MEDIA = AVG(SALARIO),@SUMA = SUM(SALARIO)
        FROM V_WORKERS WHERE OFICIO=@OFICIO
    GO
     */
    #endregion
    public class RepositoryTrabajadores
    {
        private HospitalContext context;

        public RepositoryTrabajadores(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<TrabajadoresModel> GetTrabajadoresModelAsync()
        {
            var consulta = from datos in this.context.Trabajadores
                           select datos;
            TrabajadoresModel model = new TrabajadoresModel();
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = await  consulta.CountAsync();
            model.SumaSalarial = await consulta.SumAsync(i => i.Salario);
            model.MediaSalarial = (int) await consulta.AverageAsync(i => i.Salario);
            return model;
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Trabajadores
                            select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }

        public async Task<TrabajadoresModel> GetTrabajadoresModelOficioAsync(string oficio)
        {
            string sql = "SP_WORKERS_OFICIO @OFICIO, @PERSONAS OUT " +
                " , @MEDIA OUT , @SUMA OUT";
            SqlParameter pamOficio = new SqlParameter("@OFICIO", oficio);

            SqlParameter pamPersonas = new SqlParameter("@PERSONAS", -1);
            pamPersonas.Direction = ParameterDirection.Output;

            SqlParameter pamMedia = new SqlParameter("@MEDIA", -1);
            pamMedia.Direction = ParameterDirection.Output;

            SqlParameter pamSuma = new SqlParameter("@SUMA", -1);
            pamSuma.Direction = ParameterDirection.Output;

            var consulta = this.context.Trabajadores.FromSqlRaw(sql, pamOficio, 
                pamPersonas, pamMedia, pamSuma);

            TrabajadoresModel model = new TrabajadoresModel();
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = int.Parse(pamPersonas.Value.ToString());
            model.MediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.SumaSalarial = int.Parse(pamSuma.Value.ToString());
            return model;
        }
    }
}
