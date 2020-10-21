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
    public class RecepcionProductoesController : Controller
    {

        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: RecepcionProductoes
        //public ActionResult Index(int? idEm)
        //{
        //    var recepcionProducto = db.RecepcionProducto.Include(r => r.Empleado).Include(r => r.Producto).Include(r => r.Proveedor).Where(r => r.IdEmpleado == idEm);
        //    return View(recepcionProducto.ToList());
        //}
        // GET: RecepcionProductoes
        public ActionResult Index2(int? idEm)
        {

            var idUsuario = (int) Session["UserId"];
            var recepcionProducto = db.RecepcionProducto.Include(r => r.Empleado).Include(r => r.Producto).Include(r => r.Proveedor).Where(r => r.IdEmpleado == idUsuario);
            return View(recepcionProducto.ToList());

            //ViewBag.IdEm = idEm;
            //var recepcionProducto = db.RecepcionProducto.Include(r => r.Producto).Include(r => r.Proveedor).Where(r => r.IdEmpleado == idEm);
            //return View(recepcionProducto.ToList());



            //if (ModelState.IsValid) {

            //}


            //if (idEm != null)
            //{
            //    return View(db.RecepcionProducto.ToList().Where(r => r.IdEmpleado == idEm));
            //}
            //return View(recepcionProducto.ToList());
            //return RedirectToAction("Index2", new { idEm });
            //RecepcionProducto recepcion = db.RecepcionProducto;
            //var list = recepcion.IdEmpleado == idEm;
            //if (idEm != null)
            //{
            //    var list = db.RecepcionProducto.Include(r => r.Empleado).Include(r => r.Producto).Include(r => r.Proveedor).Where(r => r.IdEmpleado == idEm);
            //    return View(list);

            //}
            //return View(list);
            //return RedirectToAction("Index", new { idEm });

        }

        // GET: RecepcionProductoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            if (recepcionProducto == null)
            {
                return HttpNotFound();
            }
            return View(recepcionProducto);
        }

        // GET: RecepcionProductoes/Create
        public ActionResult Create(int? can, int? idProveedores, int idEmp)
        {


            

            ViewBag.Cantidad = can;
            ViewBag.Prov = idProveedores;

            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre");
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo");
            ViewBag.IdEmp = idEmp;
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre");
            return View();
        }

        // POST: RecepcionProductoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cantidad,IdProducto,IdEmpleado,IdProveedor,Estado")] RecepcionProducto recepcionProducto)
        {
            if (ModelState.IsValid)
            {
                db.RecepcionProducto.Add(recepcionProducto);
                db.SaveChanges();
                var idpor = recepcionProducto.IdProveedor;
                //return RedirectToAction("Exito", idpor);
                return RedirectToAction("Exito", new { idpor });
            }

            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", recepcionProducto.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", recepcionProducto.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", recepcionProducto.IdProveedor);
            return View(recepcionProducto);
        }

        // GET: RecepcionProductoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            if (recepcionProducto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", recepcionProducto.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", recepcionProducto.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", recepcionProducto.IdProveedor);
            return View(recepcionProducto);
        }

        // POST: RecepcionProductoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cantidad,IdProducto,IdEmpleado,IdProveedor,Estado")] RecepcionProducto recepcionProducto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recepcionProducto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", recepcionProducto.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", recepcionProducto.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", recepcionProducto.IdProveedor);
            return View(recepcionProducto);
        }

        // GET: RecepcionProductoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            if (recepcionProducto == null)
            {
                return HttpNotFound();
            }
            return View(recepcionProducto);
        }

        // POST: RecepcionProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            db.RecepcionProducto.Remove(recepcionProducto);
            
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

        public ActionResult Confirmar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            if (recepcionProducto == null)
            {
                return HttpNotFound();
            }
            var idUsuario = (int)Session["UserId"];
            ViewBag.Nombre = idUsuario;

            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", recepcionProducto.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", recepcionProducto.IdProducto);
            ViewBag.IdProd = recepcionProducto.IdProducto;
            ViewBag.IdProducto2 = recepcionProducto.IdProducto;
            ViewBag.Cantidad = recepcionProducto.Cantidad;
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", recepcionProducto.IdProveedor);
            return View(recepcionProducto);
        }

        // POST: RecepcionProductoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmar([Bind(Include = "Id,Cantidad,IdProducto,IdEmpleado,IdProveedor,Estado")] RecepcionProducto recepcionProducto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recepcionProducto).State = EntityState.Modified;
                db.SaveChanges();
           
                var can = recepcionProducto.Cantidad;
                var id = recepcionProducto.IdProducto;
                //return RedirectToAction("Create", "RecepcionProductoes");
                Eliminar(recepcionProducto.Id);
                return RedirectToAction("Create2", "Productoes", new { can, id });


                //return RedirectToAction("Exito");



            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", recepcionProducto.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", recepcionProducto.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", recepcionProducto.IdProveedor);
            return View(recepcionProducto);
        }



        public ActionResult Exito(int? idpor)
        {
            ViewBag.IdProveedor = idpor;
            //return View();

            int? id = idpor;
            return RedirectToAction("Index", "OrdenPedidoes", new { id });

        }



        public void Eliminar(int id)
        {
            RecepcionProducto recepcionProducto = db.RecepcionProducto.Find(id);
            db.RecepcionProducto.Remove(recepcionProducto);
            db.SaveChanges();
            
        }
    }
}
