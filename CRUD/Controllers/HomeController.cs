using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewModels modelo = new ViewModels();
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(model: modelo);
        }
        public IActionResult Logout()
        {
           HttpContext.Session.Clear();
           return RedirectToAction("Index", "Login");
        }
        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewModels modelo = new ViewModels();
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(modelo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
