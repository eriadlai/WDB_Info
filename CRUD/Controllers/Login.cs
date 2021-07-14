using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    public class Login : Controller
    {
        private readonly ApplicationDbContext _context;
        public Login(ApplicationDbContext context) 
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if(HttpContext.Session.GetString("usuario") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(string User, string Pass)
        {
            try
            {
                IEnumerable<Usuario> listUsuario = _context.Usuario;
                
               foreach(var item in listUsuario.ToArray())
                {
                   if(item.email.Equals(User.Trim()) && item.password.Equals(Pass.Trim()))
                    {
                        HttpContext.Session.SetString("usuario",item.email);
                        HttpContext.Session.SetString("rol", item.profesion);
                       
                    }
                   
                }
                

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        public ActionResult Perfil()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
           
            ViewModels modelo = new ViewModels();
            modelo.listaUsuarios = _context.Usuario.Where(usu => usu.email.Equals(HttpContext.Session.GetString("usuario")));
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(modelo);
        }
    }
}
