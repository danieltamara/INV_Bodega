using INV_Bodega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace INV_Bodega.Controllers
{
    public class CuentaController : Controller
    {
        private tecnologiaEntities db = new tecnologiaEntities();
        // GET: Cuenta
        public ActionResult Login()
        {
            var micookie = ControllerContext.HttpContext.Request.Cookies["TIKECTCOOKIE"];
            if (micookie != null)
            {
                ViewBag.miCookie = micookie.Value;
            }

            return View();
        }
        [HttpPost]
        public ActionResult Login(string Nombre, string Contraseña)
        {
            var user = db.Usuario.FirstOrDefault(e => e.NombreUsuario == Nombre && e.Contraseña == Contraseña);
            HttpCookie cookie = new HttpCookie("TIKECTCOOKIE", Nombre);
            ControllerContext.HttpContext.Response.SetCookie(cookie);
            Response.Cookies.Add(cookie);
            ViewBag.cookie = Nombre;

            if (user!=null)
            {
                FormsAuthentication.SetAuthCookie(user.Nombre, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", new { message = "No reconocimos sus datos" });
            }
            
        }
    }
}