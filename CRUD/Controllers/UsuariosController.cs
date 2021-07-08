using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            IEnumerable<Usuario> listUsuario = _context.Usuario;
            return View(listUsuario);
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
            return View();
        }
        //Https Get Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
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
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
    }
}
