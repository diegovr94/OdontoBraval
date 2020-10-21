using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Odontologia.Models;
using System.Data.SqlClient;

namespace Odontologia.Controllers
{
    public class AdministradorController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: Administrador
        public ActionResult Index()
        {
            var administrador = db.Administrador.Include(a => a.Cliente).Include(a => a.Empleado).Include(a => a.Proveedor);
            return View(administrador.ToList());
        }

        // GET: Administrador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // GET: Administrador/Create
        public ActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre");
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre");
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre");
            return View();
        }

        // POST: Administrador/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Usuario,Contrasena,IdProveedor,IdEmpleado,IdCliente")] Administrador administrador)
        {
            OdontologicaEntities3 bd = new OdontologicaEntities3();
            var user = bd.Empleado.FirstOrDefault();
            var user1 = bd.Cliente.FirstOrDefault();
            var user2 = bd.Proveedor.FirstOrDefault();
            var user3 = bd.Administrador.FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (administrador.Usuario == user1.Usuario || user.Usuario == user2.Usuario || user.Usuario == user3.Usuario)
                {
                    return RedirectToAction("InicioSession", "Login", new { message = "El usuario ya esta creado" });
                }
                if (administrador.Usuario == null || administrador.Apellido == null || administrador.Nombre == null || administrador.Contrasena == null)
                {
                    ViewBag.Message = "Debe ingresar campos obligatorios *";
                    return View();
                }
                administrador.Contrasena = Encriptar.GetSHA1(administrador.Contrasena);
                db.Administrador.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("InicioSession", "Login");
            }

            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", administrador.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", administrador.IdEmpleado);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", administrador.IdProveedor);
            return View(administrador);
        }

        // GET: Administrador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", administrador.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", administrador.IdEmpleado);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", administrador.IdProveedor);
            return View(administrador);
        }

        // POST: Administrador/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Usuario,Contrasena,IdProveedor,IdEmpleado,IdCliente")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", administrador.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", administrador.IdEmpleado);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", administrador.IdProveedor);
            return View(administrador);
        }

        // GET: Administrador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: Administrador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrador administrador = db.Administrador.Find(id);
            db.Administrador.Remove(administrador);
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


        public ActionResult Dashboard()
        {
            var recepcionProducto = db.RecepcionProducto.Include(r => r.Empleado).Include(r => r.Producto).Include(r => r.Proveedor);

           


            return View(recepcionProducto.ToList());
        }


        public ActionResult ParcialOrden() {

            var ordenes = db.OrdenPedido.Include(o => o.Empleado).Include(o => o.Producto).Include(o => o.Proveedor);

            return PartialView( ordenes.ToList());
        }

        public ActionResult ParcialRecepcion()
        {
            var recepcionProducto = db.RecepcionProducto.Include(r => r.Empleado).Include(r => r.Producto).Include(r => r.Proveedor);

            return View(recepcionProducto.ToList());
        }

        public ActionResult ParcialReserva()
        {

            var reserva = db.Reserva.Include(r => r.Cliente).Include(r => r.Servicios);

            return PartialView(reserva.ToList());
        }
    }
}
