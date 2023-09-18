using Microsoft.AspNetCore.Authorization;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria23.Controllers
{
    public class InmueblesController : Controller
    {
        // GET: Inmuebles
        private readonly InmuebleRepositorio inmuebleRepositorio;
        private readonly PropietarioRepositorio propietarioRepositorio;
        public InmueblesController()
        {
            inmuebleRepositorio = new InmuebleRepositorio();
            propietarioRepositorio = new PropietarioRepositorio();
        }
        // GET: Inmuebles
        [Authorize]
        public ActionResult Index()
        {
            var lista = inmuebleRepositorio.GetInmuebles();
			if (TempData.ContainsKey("Id"))
				ViewBag.Id = TempData["Id"];
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			return View(lista);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Disponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var FechaInicio = fechaInicio;
                var FechaFin = fechaFin;

                var lista = inmuebleRepositorio.GetInmueblesDisponiblesPorFechas(FechaInicio, FechaFin);

                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (lista.Count != 0)
                {
                    ViewBag.Mensaje = "Inmuebles disponibles entre: " + FechaInicio.ToString("dd/MM/yyyy") + " y " + FechaFin.ToString("dd/MM/yyyy");
                }
                return View("Index", lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
         public ActionResult Propietario(int id)
        {
            var lista = inmuebleRepositorio.BuscarPorPropietario(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Propietario = id;
            if (lista.Count != 0)
            {
                ViewBag.Mensaje = "Inmuebles del propietario: " + lista[0].Propietario?.Nombre + " " + lista[0].Propietario?.Apellido;
            }
            return View("Index", lista);
        }

        // GET: Inmuebles/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.Propietarios = propietarioRepositorio.GetPropietarios();
            var inmueble = inmuebleRepositorio.GetInmueble(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Mensaje")) {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                ViewBag.Propietarios = propietarioRepositorio.GetPropietarios();
                ViewBag.Propietario = id;

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                if (inmueble.Direccion == null || inmueble.Ambientes == null
                || inmueble.Superficie == null || inmueble.Latitud == null
                || inmueble.Precio == null || inmueble.Longitud == null)
                {
                    TempData["Mensaje"] = "Debe llenar todos los campos";
                    return RedirectToAction(nameof(Create));
                }
                ViewBag.Propietarios = propietarioRepositorio.GetPropietarios();
                if (inmuebleRepositorio.Alta(inmueble) > 0)
                {
                    TempData["Mensaje"] = "Inmueble creado con exito! Id: " + inmueble.Id;
                }
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inmueble);
            }
        }

        // GET: Inmuebles/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inmueble = inmuebleRepositorio.GetInmueble(id);
            ViewBag.Propietarios = propietarioRepositorio.GetPropietarios();
            ViewBag.PropietarioActual = propietarioRepositorio.GetPropietario(inmueble.PropietarioId);
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble collection)
        {
            Inmueble inmueble = new Inmueble();

            try
            {
                inmueble = inmuebleRepositorio.GetInmueble(id);
                inmueble.Ambientes = collection.Ambientes;
                inmueble.Direccion = collection.Direccion;
                inmueble.Id = collection.Id;
                inmueble.Latitud = collection.Latitud;
                inmueble.PropietarioId = collection.PropietarioId;
                inmueble.Longitud = collection.Longitud;
                inmueble.Superficie = collection.Superficie;
                inmueble.Tipo = collection.Tipo;
                inmueble.Precio = collection.Precio;
                inmueble.Estado = collection.Estado;
                if (inmuebleRepositorio.Modificacion(inmueble) > 0)
                {
                    TempData["Mensaje"] = "Datos guardados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var inmueble = inmuebleRepositorio.GetInmueble(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (inmuebleRepositorio.Baja(id) > 0)
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