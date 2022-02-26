using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ICosmosDBServiceProducto _cosmosDBProduct;
        private readonly ICosmosDBServiceMaquina _cosmosDBMaquina;

        public SimulacionController(ICosmosDBServiceSimulacion cosmosDBService, ICosmosDBServiceProducto _cosmosDBProduct, ICosmosDBServiceMaquina _cosmosDBMaquina)
        {
            this._cosmosDB = cosmosDBService;
            this._cosmosDBMaquina = _cosmosDBMaquina;
            this._cosmosDBProduct = _cosmosDBProduct;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<Producto> productList = (await _cosmosDBProduct.GetProductosAsync("SELECT * FROM c")).ToList();
            var list = (from p in productList
                        select new SelectListItem()
                        {
                            Text = p.Nombre,
                            Value = p.Id
                        }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Text = "---Select---",
                Value = string.Empty
            });

            IEnumerable<Maquina> maquinaList = (await _cosmosDBMaquina.GetMaquinasAsync("SELECT * FROM c")).ToList();
            var maquinas = (from m in maquinaList
                            select new SelectListItem()
                            {
                                Text = m.Nombre,
                                Value = m.Id
                            }).ToList();
            maquinas.Insert(0, new SelectListItem()
            {
                Text = "---Select---",
                Value = string.Empty
            });

            ViewBag.ListaProductos = list;
            ViewBag.ListaMaquinas = maquinas;
            return View();
        }

        public async Task<ActionResult> CreateSimulacion(Simaulacion simulacion)
        {
            simulacion.Id = Guid.NewGuid().ToString();

            int totalDiasSemanalesEfectivos = (simulacion.CantidadDias * 4) * (simulacion.CantidadMeses); //20 dias 

            int contador_dias = 1;
            int resta_dias = 7 - simulacion.CantidadDias;

            for (int j = 1; j <= simulacion.cantProductosDia; j++)
            {
                if (contador_dias == simulacion.CantidadDias)
                {
                    totalDiasSemanalesEfectivos++;
                    j += resta_dias;
                    contador_dias = 1;
                }
                else
                {
                    if (contador_dias <= simulacion.CantidadDias)
                    {
                        totalDiasSemanalesEfectivos++;
                        contador_dias++;
                    }
                }
            }
            int totalHorasDiariasEfectivas = (simulacion.CantProduccionHorasDia * totalDiasSemanalesEfectivos);
            int contador_horas = 1;
            int resta_horas = 24 - simulacion.CantProduccionHorasDia;

            for (int j = 1; j <= simulacion.CantidadHoras; j++)
            {
                if (contador_horas == simulacion.CantProduccionHorasDia)
                {
                    totalHorasDiariasEfectivas++;
                    j += resta_horas;
                    contador_horas = 1;
                }
                else
                {
                    if (contador_horas <= simulacion.CantProduccionHorasDia)
                    {
                        totalHorasDiariasEfectivas++;
                        contador_horas++;
                    }
                }
            }

            Maquina maquina1 = this._cosmosDBMaquina.GetMaquinaAsync(simulacion.IdMaquina1).Result;
            Maquina maquina2 = this._cosmosDBMaquina.GetMaquinaAsync(simulacion.IdMaquina1).Result;

            int contadorMa1 = 1;
            int contadorMa2 = 1;

            for (int i = 1; i <= totalHorasDiariasEfectivas; i++)
            {
                if (contadorMa1 <= simulacion.CantProduccionHorasDia) //maquina 1
                {
                    simulacion.ProductoHoraMaquina1 += maquina1.CantidadProdHoras;
                    contadorMa1 = 1;
                }

                if (contadorMa2 <= simulacion.CantProduccionHorasDia) //maquina 2
                {
                    simulacion.ProductoHoraMaquina2 += maquina2.CantidadProdHoras;
                    contadorMa2 = 1;
                }
                contadorMa1++;
                contadorMa2++;
            }
            
            Producto producto = this._cosmosDBProduct.GetProductoAsync(simulacion.IdProducto).Result;

            simulacion.GananciaBrutoMaquina1 = (simulacion.ProductoHoraMaquina1 * producto.Precio);
            simulacion.GananciaBrutoMaquina2 = (simulacion.ProductoHoraMaquina2 * producto.Precio);

            simulacion.GananciaRealMaquina1 = (simulacion.GananciaBrutoMaquina1 - (simulacion.ProductoHoraMaquina1 * simulacion.PrecioFabricacionProducto));
            simulacion.GananciaRealMaquina2 = (simulacion.GananciaBrutoMaquina2 - (simulacion.ProductoHoraMaquina2 * simulacion.PrecioFabricacionProducto));
            simulacion.MaquinaRecomnedada = simulacion.GananciaRealMaquina1 > simulacion.GananciaRealMaquina2 ? maquina1.Nombre : maquina2.Nombre;
            await _cosmosDB.AddSimulacionAsync(simulacion);
            return RedirectToAction("Create");
        }

        public async Task<ActionResult> Details()
        {
            IEnumerable<Simaulacion> simulacion = (await _cosmosDB.GetSimulacionesAsync("SELECT * FROM c")).ToList();
            return View(simulacion.Last());
        }
    }
}
