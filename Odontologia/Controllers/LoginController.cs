using Odontologia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Odontologia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InicioSession(string message = "") {
            ViewBag.Message = message;
            return View();
        }


        [HttpPost]
        public ActionResult Inicio(string username, string password)
        {

            OdontologicaEntities3 bd = new OdontologicaEntities3();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {

                var pass = Encriptar.GetSHA1(password);

               var user = bd.Empleado.FirstOrDefault(c => c.Usuario == username && c.Contrasena == pass);
               var user1 = bd.Cliente.FirstOrDefault(c => c.Usuario == username && c.Contrasena == pass);
                var user2 = bd.Proveedor.FirstOrDefault(c => c.Usuario == username && c.Contrasena == pass);
                var user3 = bd.Administrador.FirstOrDefault(c => c.Usuario == username && c.Contrasena == pass);


                //var userEmp = bd.Empleado.FirstOrDefault(c => c.Usuario1 == username && c.Contrasena == pass);               
                if (user != null ) 
                {

                    //FormsAuthentication.SetAuthCookie(user.Usuario, true);
                    //return RedirectToAction("InicioSession", "Empleado");

                    FormsAuthentication.SetAuthCookie(user.Usuario, true);
                    ViewBag.id = user.Id;

                    Session["UserId"] = user.Id;



                    return RedirectToAction("InicioSession", "Empleado", new Cliente { Id = user.Id });


                }
                else if (user1 != null)
                {
                    FormsAuthentication.SetAuthCookie(user1.Usuario, true);
                    ViewBag.Nombre = user1.Nombre;
                    //return RedirectToAction("ACTION", "CONTROLLER", new
                    //{
                    //    id = 99,
                    //    otherParam = "Something",
                    //    anotherParam = "OtherStuff"
                    //});
                    Session["UserId"] = user1.Id;

                    return RedirectToAction("InicioSession", "Cliente", new Cliente {Nombre  = user1.Nombre } );
                }
                else if (user2 != null)
                {
                    ViewBag.id = user2.Id;

                    Session["UserId"] = user2.Id;

                    FormsAuthentication.SetAuthCookie(user2.Usuario, true);

                    Session["UserId"] = user2.Id;

                    return RedirectToAction("InicioSession", "Proveedor", new Cliente { Id = user2.Id });
                }
                else if (user3 != null)
                {
                    FormsAuthentication.SetAuthCookie(user3.Usuario, true);
                    Session["UserId"] = user3.Id;
          
                    return RedirectToAction("Dashboard", "Administrador");

                }
                else
                {
                    return RedirectToAction("InicioSession", new {message= "Usuario o Clave Incorrecta" });
                }
            }
            else
            {
                return RedirectToAction("InicioSession", new { message = "Completa Los datos" });
            }
            
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("InicioSession");
        }


        public ActionResult Registros()
        {
           
            return View();
        }

    }
}