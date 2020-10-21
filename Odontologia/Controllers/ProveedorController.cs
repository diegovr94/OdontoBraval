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
    public class ProveedorController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: Proveedor
        public ActionResult Index()
        {
            return View(db.Proveedor.ToList());
        }

        // GET: Proveedor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Correo,Rubro,Usuario,Contrasena")] Proveedor proveedor)
        {
            OdontologicaEntities3 bd = new OdontologicaEntities3();
            var user = bd.Empleado.FirstOrDefault();
            var user1 = bd.Cliente.FirstOrDefault();
            var user2 = bd.Proveedor.FirstOrDefault();
            var user3 = bd.Administrador.FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (proveedor.Usuario == user1.Usuario || user.Usuario == user2.Usuario || user.Usuario == user3.Usuario)
                {
                    return RedirectToAction("InicioSession", "Login", new { message = "El usuario ya esta creado" });
                }
                if (proveedor.Usuario == null || proveedor.Rubro == null || proveedor.Nombre == null || proveedor.Contrasena == null || proveedor.Correo == null)
                {
                    ViewBag.Message = "Debe ingresar campos obligatorios *";
                    return View();
                }

                proveedor.Contrasena = Encriptar.GetSHA1(proveedor.Contrasena);
                db.Proveedor.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("InicioSession", "Login");
            }

            return View(proveedor);
        }

        // GET: Proveedor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Correo,Rubro,Usuario,Contrasena")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InicioSession");
            }
            return View(proveedor);
        }

        // GET: Proveedor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedor.Find(id);
            db.Proveedor.Remove(proveedor);
            db.SaveChanges();
            return RedirectToAction("InicioSession");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //public ActionResult InicioSession(Proveedor provedor)
        //{
        //    ViewBag.Id = provedor.Id;
        //    return View();
        //}
        public ActionResult InicioSession(Proveedor provedor)
        {
            ViewBag.Id = provedor.Id;
            var id = provedor.Id;
            return RedirectToAction("Index", "OrdenPedidoes", new { id });
        }
        public ActionResult Exito()
        {
            return View();
        }
    }
}
