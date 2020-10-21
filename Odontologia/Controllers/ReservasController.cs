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
    public class ReservasController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: Reservas
        public ActionResult Index()
        {
            var reserva = db.Reserva.Include(r => r.Cliente).Include(r => r.Servicios);
            return View(reserva.ToList());
        }

        // GET: Reservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        public ActionResult Create()
        {
            var idUsuario = (int)Session["UserId"];

            Cliente cliente = db.Cliente.Find(idUsuario);



            var name = db.Cliente.Where(c => c.Id == idUsuario).Select(c => c.Nombre);

            var time = DateTime.Now;
            ViewBag.time = time;

            ViewBag.Cliente = cliente.Nombre + " " + cliente.Apellido;
            ViewBag.idcli = cliente.Id;
            ViewBag.name = name;
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre");
            ViewBag.IdServicio = new SelectList(db.Servicios, "Id", "Descripcion");
            return View();
        }

        // POST: Reservas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,IdCliente,IdServicio")] Reserva reserva)
        {
            //using(Models.OdontologicaEntities3 db = new Models.OdontologicaEntities3())
            //{
            //    List<Models.ViewModel.TablaView> lst = (from d in db.Servicios select new Models.Servicios
            //    {
            //        Id = d.Id,
            //        Descripcion=d.Descripcion
            //    }).ToList();
            //}

            if (ModelState.IsValid)
            {
                if (reserva.Fecha == null)
                {
                    DateTime time = DateTime.Now;
                    reserva.Fecha = time;
                }

                Servicios servicios = db.Servicios.Find(reserva.IdServicio);
                Producto producto = db.Producto.Find(servicios.IdProducto);

                producto.Stock = descontarStock(reserva.IdServicio);
                db.Reserva.Add(reserva);
                db.SaveChanges();
                return RedirectToAction("InicioSession", "Cliente");
            }


            if (reserva.Fecha == null || reserva.IdServicio == null)
            {
                ViewBag.Message = "Debe ingresar campos obligatorios *";
                return View();
            }

            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", reserva.IdCliente);
            ViewBag.IdServicio = new SelectList(db.Servicios, "Id", "Descripcion", reserva.IdServicio);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", reserva.IdCliente);
            ViewBag.IdServicio = new SelectList(db.Servicios, "Id", "Descripcion", reserva.IdServicio);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,IdCliente,IdServicio")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InicioSession","Empleado");
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "Id", "Nombre", reserva.IdCliente);
            ViewBag.IdServicio = new SelectList(db.Servicios, "Id", "Descripcion", reserva.IdServicio);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reserva.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reserva.Find(id);
            db.Reserva.Remove(reserva);
            db.SaveChanges();
            return RedirectToAction("InicioSession", "Empleado");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool BuscarServicios(int servicio)
        {

            OdontologicaEntities3 bd = new OdontologicaEntities3();
            int ser = 0;
            var user = bd.Servicios.FirstOrDefault(S => S.Id == ser);
            var fin = bd.Reserva.FirstOrDefault(r => r.IdServicio == servicio);
            return true;
        }

        public ActionResult InicioSession()
        {
            return View();
        }
        public ActionResult ReservaExitosa()
        {
            return View();
        }


        public int? descontarStock(int? id)
        {
            Producto producto = db.Producto.Find(id);

             var tot = producto.Stock - 1;


            return tot;
        }

    }
}
