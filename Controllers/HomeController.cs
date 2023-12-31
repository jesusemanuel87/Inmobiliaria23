﻿using System.Diagnostics;
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
         ViewBag.Id = TempData["Id"];
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.Mensaje = TempData["Mensaje"];
        }
        return View();
    }

     public IActionResult Login()
    {
        return View();
    }
    public IActionResult Restringido(){
        return View();
    }

    [Authorize]
    public ActionResult Seguro()
    {
        // var identity = (ClaimsIdentity)User.Identity;
        // IEnumerable<Claim> claims = identity.Claims;
        // return View(claims);
        var identity = User.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity?.Claims ?? Enumerable.Empty<Claim>();
        return View(claims);
    }

   [Authorize(Policy = "Admin")]
    public ActionResult Admin()
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
