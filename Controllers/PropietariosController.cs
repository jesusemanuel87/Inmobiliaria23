using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria23.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly PropietarioRepositorio reProp;

        public PropietariosController()
        {
            reProp = new PropietarioRepositorio();
        }
        // GET: Propietarios
  
        public ActionResult Index()
        {
            var lista = reProp.GetPropietarios();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
            // List<Propietario> propietarios = reProp.GetPropietarios();
            // return View(propietarios);
        }

        // GET: Propietarios/Details/5
     
        public ActionResult Details(int id)
        {
            var propietario = reProp.GetPropietario(id);
            return View(propietario);
        }

        // GET: Propietarios/Create
       [HttpGet]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]   
        public ActionResult Create(Propietario propietario)
        {
            try
            {                
                if (ModelState.IsValid) {
                    reProp.Alta(propietario);
                    TempData["Id"] = propietario.Id;
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return View(propietario);
                }
            }
            catch
            {
                throw;
            }
        }

        // GET: Propietarios/Edit/5
       [HttpGet]
        public ActionResult Edit(int id)
        {
            var prop = reProp.GetPropietario(id);
            return View(prop);
        }


        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(int id, Propietario collection) //, Propietario collection
        {
            Propietario p = new Propietario();
            try
            {
                p = reProp.GetPropietario(id);
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

        // GET: Propietarios/Delete/5
       
        public ActionResult Delete(int id)
        {
            try
            {
                var prop = reProp.GetPropietario(id);
                return View(prop);
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Delete(int id, Propietario entidad)
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