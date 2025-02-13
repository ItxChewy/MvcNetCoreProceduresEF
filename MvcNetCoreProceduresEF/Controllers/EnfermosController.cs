using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermosAsync();
            return View(enfermos);
        }

        public async Task< IActionResult> Details(string inscripcion)
        {
            Enfermo enfermo = await this.repo.FindEnfermoAsync(inscripcion);
            return View(enfermo);
        }

        public async Task<IActionResult> Delete(string inscripcion)
        {
            //this.repo.DeleteEnfermos(inscripcion);
            await this.repo.DeleteEnfermosRawAsync(inscripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> Insert
            (string apellido, string direccion, DateTime fechaNacimiento
            , string genero)
        {
            await this.repo.InsertEnfermos(apellido,direccion,fechaNacimiento,genero);
            return RedirectToAction("Index");
        }
        
    }
}
