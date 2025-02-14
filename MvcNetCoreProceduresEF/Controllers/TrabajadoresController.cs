using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class TrabajadoresController : Controller
    {
        private RepositoryTrabajadores repo;
        public TrabajadoresController(RepositoryTrabajadores repo)
        {
            this.repo = repo;
        }
        public async Task< IActionResult> Index()
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelAsync();
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelOficioAsync(oficio);
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
    }
}
