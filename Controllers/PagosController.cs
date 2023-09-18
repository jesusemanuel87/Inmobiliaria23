using Microsoft.AspNetCore.Mvc;
using Inmobiliaria23.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria23.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagoRepositorio rePago;
        private readonly ContratoRepositorio reContrato;
        public PagosController()
        {
            rePago = new PagoRepositorio();
            reContrato = new ContratoRepositorio();
        }
        // GET: Pagos
        [Authorize]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            var lista = rePago.GetPagos();
            return View(lista);
        }

        [Authorize]
        public ActionResult Contrato(int id)
        {
            var lista = rePago.PagosPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Contrato = id;
            if (lista.Count() != 0)
            {
                ViewBag.Mensaje = "Cupones del contrato: " + lista[0].contrato.ToString();
            }
            return View("Index", lista);
        }


        // GET: Pagos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var deuda = rePago.ObtenerDeuda(id);
            ViewBag.Deuda = deuda;
            var lista = rePago.GetPago(id);
            return View(lista);
        }

        [Authorize]
        // GET: Pagos/Create
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (id == 0)
                {
                    ViewBag.Contratos = reContrato.GetContratos();
                }
                else
                {
                    ViewBag.Contrato = reContrato.GetContrato(id);
                }

                return View();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago collection)
        {
            Pago pago = new Pago();
            try
            {
                if (collection.FechaPagado == null)
                {
                    TempData["Mensaje"] = "Debe ingresar la fecha de pago";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Importe == null)
                {
                    TempData["Mensaje"] = "Debe ingresar un importe mayor a cero";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Mes == 0)
                {
                    TempData["Mensaje"] = "Debe ingresar N° de cuota";
                    return RedirectToAction(nameof(Create));
                }
                pago.FechaPagado = collection.FechaPagado;
                pago.Importe = collection.Importe;
                pago.ContratoId = (collection.ContratoId == 0) ? collection.Id : collection.ContratoId;
                pago.Mes = collection.Mes;
                if (rePago.Alta(pago) > 0)
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

        // GET: Pagos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            var lista = rePago.GetPago(id);
            return View(lista);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Pago collection)
        {
            Pago pago = new Pago();
            try
            {
                // TODO: Add update logic here
                if (collection.FechaPagado == null)
                {
                    TempData["Mensaje"] = "Debe ingresar la fecha de pago";
                    return RedirectToAction(nameof(Edit), new { id = collection.Id });
                }
                if (collection.Importe <= 0)
                {
                    TempData["Mensaje"] = "Debe ingresar un importe mayor a cero";
                    return RedirectToAction(nameof(Edit), new { id = collection.Id });
                }
                pago = rePago.GetPago(collection.Id);
                pago.FechaPagado = collection.FechaPagado;
                pago.Importe = collection.Importe;
                if (rePago.Modificacion(pago) > 0)
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

        // GET: Pagos/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var lista = rePago.GetPago(id);
            return View(lista);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (rePago.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Datos eliminados correctamente";
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