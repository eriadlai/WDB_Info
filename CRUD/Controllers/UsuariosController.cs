using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context) {
            _context = context;
        }
        //Https Get Index
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if(HttpContext.Session.GetString("rol") != "Administrador")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewModels modelo = new ViewModels();
            modelo.listaUsuarios= _context.Usuario.Where(usu => usu.email != HttpContext.Session.GetString("usuario"));
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            
            return View(model:modelo);
        }
        //Https Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario) {
            if (ModelState.IsValid)
            {
               
                _context.Usuario.Add(usuario);
                _context.SaveChanges();
                TempData["message"] = "El usuario ha sido creado";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Https Get Create

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("rol") != "Administrador")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewModels modelo = new ViewModels();
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(modelo);
        }
        //Https Get Edit
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("rol") != "Administrador")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewModels modelo = new ViewModels();
            modelo.usuario = usuario;
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");

            return View(modelo);
        }
        //Https Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuario.Update(usuario);
                _context.SaveChanges();
                TempData["message"] = "El usuario se ha actualizado";
                return RedirectToAction("Index");
            }
            return View();
        }

        //Https Post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUsuario(int? id)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
           
                _context.Usuario.Remove(usuario);
                _context.SaveChanges();
            TempData["message"] = "El usuario se ha Eliminado";
         
            return RedirectToAction("Index");
        }
        //Https Get Delete
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("rol") != "Administrador")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewModels modelo = new ViewModels();
            modelo.usuario = usuario;
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(modelo);
        }
    }
}
