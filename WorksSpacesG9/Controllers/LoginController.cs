using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WorksSpacesG9.Controllers
{
    public class LoginController : Controller
    {

        private WorkSpacesG9Entities context=new WorkSpacesG9Entities();
        [HttpGet]
        public ActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuarios usuario)
        {

            if (ModelState.IsValid)
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string contrasena)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var usuario = context.Usuarios.FirstOrDefault(u => u.email == email && u.contrasena == contrasena);
            if (usuario != null)
            {
                Session["idUsuario"]=usuario.id_usuario;
                Session["UserName"] = usuario.nombre; 
                Session["UserEmail"] = usuario.email; 
                Session["IsAuthenticated"] = true;
                Session["IsAdmin"] = usuario.administrador;

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;

            Session.Clear(); 
            Session.Abandon(); 

            

            return RedirectToAction("Index", "Home");
        }
    }
}