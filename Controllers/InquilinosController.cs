using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria23.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino reProp;

        public InquilinosController()
        {
            reProp = new RepositorioInquilino();
        }
        // GET: Inquilinos
  
        public ActionResult Index()
        {
            var lista = reProp.GetInquilinos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
            // List<Inquilino> Inquilinos = reProp.GetInquilinos();
            // return View(Inquilinos);
        }

        // GET: Inquilinos/Details/5
     
        public ActionResult Details(int id)
        {
            var inquilino = reProp.GetInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
       [HttpGet]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]   
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {                
                if (ModelState.IsValid) {
                    reProp.Alta(inquilino);
                    TempData["Id"] = inquilino.Id;
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return View(inquilino);
                }
            }
            catch
            {
                throw;
            }
        }

        // GET: Inquilinos/Edit/5
       [HttpGet]
        public ActionResult Edit(int id)
        {
            var prop = reProp.GetInquilino(id);
            return View(prop);
        }


        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(int id, Inquilino collection) //, Inquilino collection
        {
            Inquilino p = new Inquilino();
            try
            {
                p = reProp.GetInquilino(id);
                p.Nombre = collection.Nombre;
                p.Apellido = collection.Apellido;
                p.DNI = collection.DNI;
                p.Telefono = collection.Telefono;
                p.Email = collection.Email;
                if (reProp.Modificacion(p) > 0)
                {
                    TempData["Mensaje"] = "Datos guardados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception )
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // GET: Inquilinos/Delete/5
       
        public ActionResult Delete(int id)
        {
            try
            {
                var prop = reProp.GetInquilino(id);
                return View(prop);
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {
                
                if (reProp.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Eliminación realizada correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}