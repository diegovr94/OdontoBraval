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
    public class ProductoesController : Controller
    {
        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: Productoes
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Familia).Include(p => p.Proveedor);
            return View(producto.ToList());
        }

        // GET: Productoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productoes/Create
        public ActionResult Create()
        {
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Id");
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Id");
            return View();
        }

        // POST: Productoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,FechaVencimiento,Descripcion,PrecioCompra,PrecioVenta,Stock,StockCritico,IdProveedor,IdFamilia")] Producto producto )
        {
            Familia familia = db.Familia.Find(producto.IdFamilia);


            if (ModelState.IsValid)
            {
                producto.Codigo = producto.IdProveedor + "/" + producto.FechaVencimiento + "/" + familia.Descripcion + "";
                db.Producto.Add(producto);
                db.SaveChanges();
                
                //return RedirectToAction("InicioSession", "Cliente", new Cliente { Nombre =  });
                return RedirectToAction("Index");
            }
            ViewBag.IDPro = producto.Id;
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Descripcion", producto.IdFamilia);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", producto.IdProveedor);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Descripcion", producto.IdFamilia);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", producto.IdProveedor);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,FechaVencimiento,Descripcion,PrecioCompra,PrecioVenta,Stock,StockCritico,IdProveedor,IdFamilia")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Descripcion", producto.IdFamilia);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", producto.IdProveedor);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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

        public string Codificar()
        {

            return "";
        }


        public int? Actualizarstock(  int? idOrden, int can)
        {
            Producto producto = db.Producto.Find(idOrden);
            int? stock = producto.Stock;
            var total = stock + can;

            return total;
        }


        public ActionResult Create2(int? id,  int can)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Descripcion", producto.IdFamilia);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", producto.IdProveedor);
            ViewBag.can = can + producto.Stock;
            return View(producto);
        }


        // POST: Productoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "Id,Codigo,FechaVencimiento,Descripcion,PrecioCompra,PrecioVenta,Stock,StockCritico,IdProveedor,IdFamilia")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InicioSession","Empleado");
            }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Descripcion", producto.IdFamilia);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", producto.IdProveedor);
            return View(producto);
        }
       
    }

    

}
