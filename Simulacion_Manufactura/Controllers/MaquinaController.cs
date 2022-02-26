using Microsoft.AspNetCore.Mvc;
using Simulacion_Manufactura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Controllers
{
    public class MaquinaController : Controller
    {
        private readonly ICosmosDBServiceMaquina _cosmosDB;

        public MaquinaController(ICosmosDBServiceMaquina cosmosDBService)
        {
            this._cosmosDB = cosmosDBService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _cosmosDB.GetMaquinasAsync("SELECT * FROM c")).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> CreateMaquina(Maquina maquina)
        {
            Random random = new Random();
            maquina.Id = Guid.NewGuid().ToString();
            maquina.ProbabilidadFallo = random.Next(0, 11) / 10.00;
            await _cosmosDB.AddMaquinaAsync(maquina);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Maquina maquina)
        {
            return View(maquina);
        }

        public async Task<ActionResult> EditMaquina(Maquina maquina)
        {
            await _cosmosDB.UpdateMaquinaAsync(maquina.Id, maquina);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(Maquina maquina)
        {
            return View(maquina);
        }

        public async Task<ActionResult> DeleteMaquina(Maquina maquina)
        {
            await _cosmosDB.DeleteMaquinaAsync(maquina.Id);
            return RedirectToAction("Index");
        }

    }
}
