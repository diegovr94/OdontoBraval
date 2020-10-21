using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Odontologia.Models;

namespace Odontologia.Controllers
{
    public class EmpleadoController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: Empleado
        public ActionResult Index()
        {
            return View(db.Empleado.ToList());
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create(string username)
        {
            OdontologicaEntities3 bd = new OdontologicaEntities3();

            if (!string.IsNullOrEmpty(username))
            {
                var user = bd.Empleado.FirstOrDefault(c => c.Usuario == username);
                var user1 = bd.Cliente.FirstOrDefault(c => c.Usuario == username);
                var user2 = bd.Proveedor.FirstOrDefault(c => c.Usuario == username);
                var user3 = bd.Administrador.FirstOrDefault(c => c.Usuario == username);


                //var userEmp = bd.Empleado.FirstOrDefault(c => c.Usuario1 == username && c.Contrasena == pass);               
                //if (user.Usuario == username )
                //{
                //    return RedirectToAction("Create", new { message = "Usuario Ya Creado" });
                //}

                if (user.Usuario == user1.Usuario || user.Usuario == user2.Usuario || user.Usuario == user3.Usuario)
                {
                    return RedirectToAction("InicioSession", new { message = "esadasd" });
                }

                else
                {
                    return RedirectToAction("Index", "Empleado");
                }
            }
            else
            {
                return View();
            }


        }

        // POST: Empleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Nivel,Usuario,Contrasena")] Empleado empleado)
        {
            OdontologicaEntities3 bd = new OdontologicaEntities3();
            var user = bd.Empleado.FirstOrDefault();
            var user1 = bd.Cliente.FirstOrDefault();
            var user2 = bd.Proveedor.FirstOrDefault();
            var user3 = bd.Administrador.FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (empleado.Usuario == user1.Usuario || user.Usuario == user2.Usuario || user.Usuario == user3.Usuario)
                {
                    return RedirectToAction("InicioSession", "Login", new { message = "El usuario ya esta creado" });
                }
                if (empleado.Usuario == null || empleado.Apellido == null || empleado.Nombre == null || empleado.Contrasena == null|| empleado.Nivel == null)
                {
                    ViewBag.Message = "Debe ingresar campos obligatorios *";
                    return View();
                }

                empleado.Contrasena = Encriptar.GetSHA1(empleado.Contrasena);
                db.Empleado.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("InicioSession", "Login");
            }

            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Nivel,Usuario,Contrasena")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.Empleado.Find(id);
            db.Empleado.Remove(empleado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ValidarUsuario(string username)
        {
            OdontologicaEntities3 bd = new OdontologicaEntities3();

            if (!string.IsNullOrEmpty(username))
            {
                var user = bd.Empleado.FirstOrDefault(c => c.Usuario == username);
                var user1 = bd.Cliente.FirstOrDefault(c => c.Usuario == username);
                var user2 = bd.Proveedor.FirstOrDefault(c => c.Usuario == username);
                var user3 = bd.Administrador.FirstOrDefault(c => c.Usuario == username);


                //var userEmp = bd.Empleado.FirstOrDefault(c => c.Usuario1 == username && c.Contrasena == pass);               
                if (user.Usuario == username)
                {
                    return RedirectToAction("Create", new { message = "Usuario Ya Creado" });
                }
                else
                {
                    return RedirectToAction("Index", "Empleado");
                }
            }
            else
            {
                return RedirectToAction("Create", new { message = "Usu" });
            }

            return View();
        }

        public ActionResult InicioSession(Empleado empleado)
        {
            var val = Validar();
            ViewBag.val = val;
            ViewBag.Id = empleado.Id;
            //    var id = empleado.Id;
            //    return RedirectToAction("InicioSession", "RecepcionProductoes", new { id });
            return View();
        }
        public ActionResult Exito()
        {
            return View();
        }
        public int Validar()
        {
            var lstUsers = (from p in db.Producto
                            where p.Stock <= p.StockCritico
                            select p).Count();

            return lstUsers;

        }

    }
}
