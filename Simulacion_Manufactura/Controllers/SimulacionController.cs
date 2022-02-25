using Microsoft.AspNetCore.Mvc;
using Simulacion_Manufactura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Controllers
{
    public class SimulacionController : Controller
    {
        private readonly ICosmosDBServiceSimulacion _cosmosDB;

        public SimulacionController(ICosmosDBServiceSimulacion cosmosDBService)
        {
            this._cosmosDB = cosmosDBService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _cosmosDB.GetSimulacionesAsync("SELECT * FROM c")).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> CreateSimulacion(Simaulacion sim)
        {
            sim.Id = Guid.NewGuid().ToString();
            await _cosmosDB.AddSimulacionAsync(sim);
            return RedirectToAction("Create");
        }

        public ActionResult Details(Simaulacion sim)
        {
            return View(sim);
        }
    }
}
