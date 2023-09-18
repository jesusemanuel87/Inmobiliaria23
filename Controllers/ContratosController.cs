using Microsoft.AspNetCore.Mvc;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria23.Controllers;

public class ContratosController : Controller
{
        private InmuebleRepositorio inmuebleRepositorio;
        private InquilinoRepositorio inquilinoRepositorio;
        private ContratoRepositorio contratoRepositorio;
        private PropietarioRepositorio propietarioRepositorio;
        private PagoRepositorio pagoRepositorio;
        public ContratosController()
        {
            inmuebleRepositorio = new InmuebleRepositorio();
            contratoRepositorio = new ContratoRepositorio();
            inquilinoRepositorio = new InquilinoRepositorio();
            propietarioRepositorio = new PropietarioRepositorio();
            pagoRepositorio = new PagoRepositorio();

        }
        // GET: Contratos
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var lista = contratoRepositorio.GetContratos();
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("MensajeError"))
                    ViewBag.Error = TempData["MensajeError"];
                return View(lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Vigentes(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var FechaInicio = fechaInicio;
                var FechaFin = fechaFin;

                var lista = contratoRepositorio.GetContratosVigentes(FechaInicio, FechaFin);

                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (lista.Count != 0)
                {
                    ViewBag.Mensaje = "Contratos vigentes desde: " + FechaInicio.ToString("dd/MM/yyyy") + " y " + FechaFin.ToString("dd/MM/yyyy");
                }
                return View("Index", lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        public ActionResult Inmueble(int id)
        {
            var lista = contratoRepositorio.GetContratosPorInmueble(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Inquilino = id;
            if (lista.Count != 0)
            {
                ViewBag.Mensaje = "Contratos del inmueble: " + lista[0].inmueble.ToString();
            }
            return View("Index", lista);
        }

        // GET: Contratos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                var contrato = contratoRepositorio.GetContrato(id);
                ViewBag.InquilinoActual = inquilinoRepositorio.GetInquilino(contrato.InquilinoId);
                ViewBag.InmuebleActual = inmuebleRepositorio.GetInmueble(contrato.InmuebleId);
                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // GET: Contratos/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                ViewBag.Inquilinos = inquilinoRepositorio.GetInquilinos();
                ViewBag.Inmuebles = inmuebleRepositorio.GetInmueblesDisponibles();
                ViewBag.Inmueble = id;
                if (id != 0)
                {
                    var inmueble = inmuebleRepositorio.GetInmueble(id);
                    ViewBag.Precio = inmueble.Precio;
                }
                return View();
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato collection)
        {
            try
            {
                Contrato controlFecha;
                var inmueble = (collection.InmuebleId == 0) ? collection.Id : collection.InmuebleId;
                controlFecha = contratoRepositorio.comprobarFechas(inmueble, collection.FechaInicio, collection.FechaFin);
                if (controlFecha != null)
                {
                    TempData["Mensaje"] = "El inmueble id: " + inmueble + " tiene contrato en la fecha seleccionada";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.FechaFin < collection.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin debe ser mayor a la de inicio";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Precio == null)
                {
                    TempData["Mensaje"] = "El precio no puede ser nulo";
                    return RedirectToAction(nameof(Create));
                }
                if (contratoRepositorio.Alta(collection) > 0)
                {
                    TempData["Mensaje"] = "Contrato creado con exito, id: " + collection.Id;
                    var fechai = collection.FechaInicio;
                    var fechaf = collection.FechaFin;
                    var diferencia = fechaf.Subtract(fechai);
                    var meses = diferencia.Days / 30;
                    if (meses == 0) { meses = 1; }
                    Console.WriteLine("Cantidad de meses: " + meses);
                    for (int i = 1; i < meses; i++)
                    {
                        var pago = new Pago();
                        pago.Mes = i;
                        pago.ContratoId = collection.Id;
                        pagoRepositorio.Alta(pago);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Terminar(int id)
        {
            try
            {
                var deuda = pagoRepositorio.ObtenerDeuda(id);

                var contrato = contratoRepositorio.GetContrato(id);
                ViewBag.InquilinoActual = inquilinoRepositorio.GetInquilino(contrato.InquilinoId);
                ViewBag.InmuebleActual = inmuebleRepositorio.GetInmueble(contrato.InmuebleId);
                ViewBag.deuda = deuda;

                var intervaloF = contrato.FechaFin.Subtract(contrato.FechaInicio) / 30;
                var intervaloA = DateTime.Now.Subtract(contrato.FechaInicio) / 30;
                if ((intervaloF / 2) > intervaloA)
                {
                    ViewBag.multa = contrato.Precio * 2;
                }

                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Terminar(int id, Contrato collection)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = contratoRepositorio.GetContrato(id);
                contrato.FechaFin = DateTime.Now;
                if (contratoRepositorio.Modificacion(contrato) > 0)
                {
                    pagoRepositorio.ModificacionPorContrato(contrato.Id, DateTime.Now);
                    TempData["Mensaje"] = "Contrato terminado con exito";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Delete(int id)
        {
            var contrato = contratoRepositorio.GetContrato(id);
            ViewBag.InquilinoActual = inquilinoRepositorio.GetInquilino(contrato.InquilinoId);
            ViewBag.InmuebleActual = inmuebleRepositorio.GetInmueble(contrato.InmuebleId);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                if (contratoRepositorio.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Eliminación realizada correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //return View();
                TempData["MensajeError"] = "No se puede eliminar el inquilino debido a contratos vinculados.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        public ActionResult Renovar(int id)
        {
            try
            {
                if (TempData["Mensaje"] != null)
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                var contrato = contratoRepositorio.GetContrato(id);
                ViewBag.Inquilinos = inquilinoRepositorio.GetInquilinos();
                ViewBag.InquilinoActual = inquilinoRepositorio.GetInquilino(contrato.InquilinoId);
                ViewBag.Inmuebles = inmuebleRepositorio.GetInmuebles();
                ViewBag.InmuebleActual = inmuebleRepositorio.GetInmueble(contrato.InmuebleId);
                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Renovar(int id, Contrato collection)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = contratoRepositorio.GetContrato(id);
                var contratoAterminar = new Contrato()
                {
                    FechaInicio = contrato.FechaInicio,
                    FechaFin = DateTime.Now,
                    InquilinoId = contrato.InquilinoId,
                    InmuebleId = contrato.InmuebleId,
                    Precio = contrato.Precio,
                    Id = contrato.Id,
                };

                if (collection.Precio <= 0) // || collection.FechaFin == null)
                {
                    TempData["Mensaje"] = "Complete campos fecha y precio";
                    return RedirectToAction(nameof(Renovar));
                }
                if (collection.FechaFin <= contrato.FechaFin)
                {
                    TempData["Mensaje"] = "La fecha de fin debe ser mayor a la fecha fin actual";
                    return RedirectToAction(nameof(Renovar));
                }
                if (collection.FechaInicio >= collection.FechaFin)
                {
                    TempData["Mensaje"] = "La fecha fin debe ser mayor a la fecha inicio";
                    return RedirectToAction(nameof(Renovar));
                }

                var fechai = collection.FechaInicio;
                var fechaf = collection.FechaFin;
                var diferencia = fechaf.Subtract(fechai);
                var meses = diferencia.Days / 30;
                if (meses == 0) { meses = 1; }
                Console.WriteLine("Cantidad de meses: " + meses);

                contrato.FechaFin = collection.FechaFin;
                contrato.FechaInicio = collection.FechaInicio;
                contrato.Precio = collection.Precio;
                contrato.Id = 0;

                if (contratoRepositorio.Alta(contrato) > 0)
                {
                    for (int i = 0; i < meses; i++)
                    {
                        var pago = new Pago();
                        pago.Mes = i;
                        pago.ContratoId = contrato.Id;
                        pagoRepositorio.Alta(pago);
                    }
                    if (contratoRepositorio.Modificacion(contratoAterminar) > 0)
                    {
                        pagoRepositorio.ModificacionPorContrato(contratoAterminar.Id, DateTime.Now);
                    }

                    TempData["Mensaje"] = "Contrato renovado con exito";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }