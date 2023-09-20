using Microsoft.AspNetCore.Authorization;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria23.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly InquilinoRepositorio repositorio;

        public InquilinosController()
        {
            repositorio = new InquilinoRepositorio();
        }
        // GET: Inquilinos
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.GetInquilinos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
            // List<Inquilino> Inquilinos = repositorio.GetInquilinos();
            // return View(Inquilinos);
        }

        // GET: Inquilinos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inquilino = repositorio.GetInquilino(id);
            inquilino.Contratos = repositorio.ObtenerContratosDelInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
       [HttpGet]
       [Authorize]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]   
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {                
                //if (ModelState.IsValid) {
                if (inquilino.Nombre == null || inquilino.Apellido == null || inquilino.DNI == null ||inquilino.Telefono == null || inquilino.Email == null)
                {
                    repositorio.Alta(inquilino);
                    TempData["Id"] = inquilino.Id;
                    return RedirectToAction(nameof(Index));
                } else {
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
       [Authorize]
        public ActionResult Edit(int id)
        {
            var prop = repositorio.GetInquilino(id);
            return View(prop);
        }


        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino collection)
        {
            Inquilino p = new Inquilino();
            try
            {
                p = repositorio.GetInquilino(id);
                p.Nombre = collection.Nombre;
                p.Apellido = collection.Apellido;
                p.DNI = collection.DNI;
                p.Telefono = collection.Telefono;
                p.Email = collection.Email;
                if (repositorio.Modificacion(p) > 0)
                {
                    TempData["Mensaje"] = "Datos guardados!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception )
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // GET: Inquilinos/Delete/5
       [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var prop = repositorio.GetInquilino(id);
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
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, Inquilino inmueble)
        {
            try
            {
                
                if (repositorio.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Se eliminó correctamente";
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