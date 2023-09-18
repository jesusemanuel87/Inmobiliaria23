using Microsoft.AspNetCore.Authorization;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria23.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly PropietarioRepositorio repositorio;
        public PropietariosController()
        {
            repositorio = new PropietarioRepositorio();
        }
        // GET: Propietarios
   [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.GetPropietarios();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
             if (TempData.ContainsKey("MensajeError"))
                ViewBag.Error = TempData["MensajeError"];
            return View(lista);
            // List<Propietario> propietarios = repositorio.GetPropietarios();
            // return View(propietarios);
        }

        // GET: Propietarios/Details/5
     [Authorize]
        public ActionResult Details(int id)
        {
            var propietario = repositorio.GetPropietario(id);
            // Cargar explícitamente los inmuebles relacionados
            propietario.Inmuebles = repositorio.ObtenerInmueblesDelPropietario(id);
            return View(propietario);
        }


        // GET: Propietarios/Create
       [HttpGet]
       [Authorize]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                else if (TempData.ContainsKey("Error"))
                {
                    ViewBag.Error = TempData["Error"];
                }
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]   
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {
            try
            {                
                if (ModelState.IsValid) {
                    repositorio.Alta(propietario);
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
       [Authorize]
        public ActionResult Edit(int id)
        {
            var prop = repositorio.GetPropietario(id);
            return View(prop);
        }


        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        
        public ActionResult Edit(int id, Propietario collection) //, Propietario collection
        {
            Propietario p = new Propietario();
            try
            {
                p = repositorio.GetPropietario(id);
                p.Nombre = collection.Nombre;
                p.Apellido = collection.Apellido;
                p.DNI = collection.DNI;
                p.Telefono = collection.Telefono;
                p.Email = collection.Email;
                if (repositorio.Modificacion(p) > 0)
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
       [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var prop = repositorio.GetPropietario(id);
                return View(prop);
                
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize(Policy = "Admin")]     
        public ActionResult Delete(int id, IFormCollection collection)

        {
            try
            {
                repositorio.Baja(id);
                
                    TempData["Mensaje"] = "Se eliminó correctamente";
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                //throw; //return View();
                TempData["MensajeError"] = "No se puede eliminar el propietario debido a inmuebles vinculados.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}