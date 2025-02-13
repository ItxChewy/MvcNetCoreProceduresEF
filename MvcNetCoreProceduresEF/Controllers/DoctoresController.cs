using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctores repo;
        public DoctoresController(RepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public async Task< IActionResult> Index()
        {
            List<Doctor> doctores = await this.repo.GetDoctoresAsync();
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["Especialidades"] = especialidades;
            return View(doctores);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string especialidad,int incremento)
        {
            List<Doctor> doctores = await this.repo.UpdateDoctoresAsync(especialidad,incremento);
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["Especialidades"] = especialidades;
            return View(doctores);
        }

    }
}
