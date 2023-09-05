using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria23.Models;

namespace Inmobiliaria23.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Titulo = "Página de Inicio";
        return View();
    }

    [Authorize]
    public ActionResult Seguro()
    {
        var identity = (ClaimsIdentity)User.Identity;
        IEnumerable<Claim> claims = identity.Claims;
        return View(claims);
    }

    [Authorize(Policy = "Admin")]
    public ActionResult Admin()
    {
        return View();
    }

    public ActionResult Restringido()
    {
        return View();
    }

    //Colocar código de CAMBIAR CLAIMS !!!!!!!!!!!!!!!!!!!!!!!!!!

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
