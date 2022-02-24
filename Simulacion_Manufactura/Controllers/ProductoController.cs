using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simulacion_Manufactura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ICosmosDBServiceProducto _cosmosDB;

        public ProductoController(ICosmosDBServiceProducto cosmosDBServiceProducto)
        {
            this._cosmosDB = cosmosDBServiceProducto;
        }
        public async Task<ActionResult> Producto()
        {
            return View((await _cosmosDB.GetProductosAsync("SELECT * FROM c")).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> CreateProduct(Producto producto)
        {
            producto.Id = Guid.NewGuid().ToString();
            await _cosmosDB.AddProductoAsync(producto);
            return RedirectToAction("Producto");
        }

        public ActionResult Edit(Producto producto)
        {
            return View(producto);
        }

        public async Task<ActionResult> EditProduct(Producto producto)
        {
            await _cosmosDB.UpdateProductoAsync(producto.Id, producto);
            return RedirectToAction("Producto");
        }

        public ActionResult Delete(Producto producto)
        {
            return View(producto);
        }

        public async Task<ActionResult> DeleteProduct(Producto producto)
        {
            await _cosmosDB.DeleteProductoAsync(producto.Id);
            return RedirectToAction("Producto");
        }
    }
}
