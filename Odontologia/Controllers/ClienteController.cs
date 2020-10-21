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
    public class ClienteController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();



        public ActionResult Inicio(Cliente cliente)
        {
            return View(db.Cliente.ToList());
        }


        // GET: Cliente
        public ActionResult Index()
        {
            var idUsuario = (int)Session["UserId"];



            var list = db.Cliente.Where(c => c.Id == idUsuario);

            return View(list.ToList());
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        // POST: Cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Usuario,Contrasena,Certificado,Liquidacion")] Cliente cliente)
        {

            OdontologicaEntities3 bd = new OdontologicaEntities3();
            var user = bd.Empleado.FirstOrDefault();
            var user1 = bd.Cliente.FirstOrDefault();
            var user2 = bd.Proveedor.FirstOrDefault();
            var user3 = bd.Administrador.FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (cliente.Usuario == user1.Usuario || user.Usuario == user2.Usuario || user.Usuario == user3.Usuario)
                {
                    return RedirectToAction("InicioSession", "Login", new { message = "El usuario ya esta creado" });
                }
                if (cliente.Usuario == null || cliente.Apellido == null || cliente.Nombre == null || cliente.Contrasena == null)
                {
                    ViewBag.Message = "Debe ingresar campos obligatorios *";
                    return View();
                }


                cliente.Contrasena = Encriptar.GetSHA1(cliente.Contrasena);
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("InicioSession", "Login");
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Usuario,Contrasena,Certificado,Liquidacion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InicioSession");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
        public ActionResult InicioSession(Cliente cliente)
        {
            ViewBag.Nombre = cliente.Nombre;
            //return RedirectToAction("VistaParcial");

            var idUsuario = (int)Session["UserId"];

            var list = db.Cliente.Where(c => c.Id == idUsuario);

            ViewBag.List = list.ToList();
            return View();
        }
        public ActionResult ReservaExitosa()
        {
            return View();
        }


        public ActionResult VistaParcial()
        {
            var idUsuario = (int)Session["UserId"];

            var listt = db.Reserva.Where(c => c.IdCliente == idUsuario);

            return PartialView(listt);
        }
    }
}
