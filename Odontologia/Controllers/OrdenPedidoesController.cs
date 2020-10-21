using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using Odontologia.Models;

namespace Odontologia.Controllers
{
    public class OrdenPedidoesController : Controller
    {
        public int? idProveedores;

        private OdontologicaEntities3 db = new OdontologicaEntities3();

        // GET: OrdenPedidoes
        public ActionResult Index(int id)
        {
            var idP = id;
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);

            return View(db.OrdenPedido.Include(o => o.Empleado).Include(o => o.Producto).Include(o => o.Proveedor).Where(o => o.IdProveedor == id));
            //var ordenPedido = db.OrdenPedido.Include(o => o.Empleado).Include(o => o.Producto).Include(o => o.Proveedor).Where(o => o.IdProveedor == id);
            //return View(ordenPedido.ToList());
        }

        // GET: OrdenPedidoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);
            if (ordenPedido == null)
            {
                return HttpNotFound();
            }
            return View(ordenPedido);
        }

        // GET: OrdenPedidoes/Create
        public ActionResult Create()
        {
            var idUsuario = (int)Session["UserId"];
            ViewBag.Nombre = idUsuario;
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre");
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo");
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre");
            return View();
        }

        // POST: OrdenPedidoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Cantidad,IdProducto,IdEmpleado,IdProveedor")] OrdenPedido ordenPedido)
        {
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre");
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo");
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre");
            if (ModelState.IsValid)
            {
                if (ordenPedido.Descripcion == null || ordenPedido.Cantidad == null )
                {
                    ViewBag.Message = "Debe ingresar campos obligatorios *";
                    return View();
                }
                db.OrdenPedido.Add(ordenPedido);
                db.SaveChanges();
                var id = ordenPedido.IdEmpleado;
                return RedirectToAction("InicioSession", "Empleado", new { id });
                //return RedirectToAction("Create", "RecepcionProductoes", new { id });
            }

            var idUsuario = (int)Session["UserId"];
            ViewBag.Nombre = idUsuario;
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre");
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo");
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre");
            return View(ordenPedido);
        }

        // GET: OrdenPedidoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);
            if (ordenPedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", ordenPedido.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", ordenPedido.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
            ViewBag.Cantidad = ordenPedido.Cantidad;
            return View(ordenPedido);
        }

        // POST: OrdenPedidoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Cantidad,IdProducto,IdEmpleado,IdProveedor")] OrdenPedido ordenPedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordenPedido).State = EntityState.Modified;
                db.SaveChanges();
                int can = int.Parse(ordenPedido.Cantidad);
            
                //return RedirectToAction("Create", "RecepcionProductoes");
                return RedirectToAction("Create", "RecepcionProductoes", new { can });

            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", ordenPedido.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", ordenPedido.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
            return View(ordenPedido);
        }

        // GET: OrdenPedidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);
            if (ordenPedido == null)
            {
                return HttpNotFound();
            }
            return View(ordenPedido);
        }

        // POST: OrdenPedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);
            db.OrdenPedido.Remove(ordenPedido);
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
        public ActionResult InicioSession()
        {
            return View();
        }
        public ActionResult Exito()
        {
            return View();
        }


        public ActionResult Confirmar(int? id)
        {
           
            ViewBag.IdOrden = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenPedido ordenPedido = db.OrdenPedido.Find(id);
            if (ordenPedido == null)
            {
                return HttpNotFound();
            }
            var idUsuario = (int)Session["UserId"];

            idProveedores = idUsuario;
            
            var  idEmpleado = ordenPedido.IdEmpleado;
            ViewBag.IdEmp = idEmpleado;
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", ordenPedido.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", ordenPedido.IdProducto);
            ViewBag.IdProd = ordenPedido.IdProducto;
            ViewBag.IdProducto2 = ordenPedido.IdProducto;
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
            ViewBag.IdProveedor2 = ordenPedido.IdProveedor;
            var can = ordenPedido.Cantidad;
            ViewBag.Cantidad = ordenPedido.Cantidad;

            var idEmp = ordenPedido.IdEmpleado;
            if (ModelState.IsValid)
            {
                db.Entry(ordenPedido).State = EntityState.Modified;
                db.OrdenPedido.Remove(ordenPedido);
                db.SaveChanges();
                int? idProv = ordenPedido.IdProveedor;

                return RedirectToAction("Create", "RecepcionProductoes", new { can, idProveedores, idEmp });

            }

          
          
            //return View(ordenPedido);
            db.OrdenPedido.Remove(ordenPedido);
            return RedirectToAction("Create", "RecepcionProductoes", new { can, idProveedores });
        }

        // POST: OrdenPedidoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmar([Bind(Include = "Id,Descripcion,Cantidad,IdProducto,IdEmpleado,IdProveedor")] OrdenPedido ordenPedido, int? id)
        {
            ordenPedido = db.OrdenPedido.Find(id);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
            ViewBag.IdProveedor2 = ordenPedido.IdProveedor;

            if (ModelState.IsValid)
            {
                db.Entry(ordenPedido).State = EntityState.Modified;
                db.OrdenPedido.Remove(ordenPedido);
                db.SaveChanges();
                int can = int.Parse(ordenPedido.Cantidad);
                int? idProv = ordenPedido.IdProveedor;

                return RedirectToAction("Create", "RecepcionProductoes", new { can, idProveedores });

            }
            ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", ordenPedido.IdEmpleado);
            ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", ordenPedido.IdProducto);
            ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
            return View(ordenPedido);
        }


      
        //public ActionResult Confirmar([Bind(Include = "Id,Descripcion,Cantidad,IdProducto,IdEmpleado,IdProveedor")] OrdenPedido ordenPedido)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ordenPedido).State = EntityState.Modified;
        //        db.SaveChanges();
        //        int can = int.Parse(ordenPedido.Cantidad);
        //        var id = ordenPedido.IdProducto;
        //        //return RedirectToAction("Create", "RecepcionProductoes");
        //        return RedirectToAction("Create2", "Productoes", new { can, id });

        //    }
        //    ViewBag.IdEmpleado = new SelectList(db.Empleado, "Id", "Nombre", ordenPedido.IdEmpleado);
        //    ViewBag.IdProducto = new SelectList(db.Producto, "Id", "Codigo", ordenPedido.IdProducto);
        //    ViewBag.IdProveedor = new SelectList(db.Proveedor, "Id", "Nombre", ordenPedido.IdProveedor);
        //    return View(ordenPedido);
        //}


        //     public static void ExportaExcel(IEnumerable<> accounts)
        //     {
        //         var excelApp = new Excel.Application();
        //         excelApp.Visible = true;
        //         excelApp.Workbooks.Add();
        //         Excel._Worksheet workSheet = excelApp.ActiveSheet;

        //         // En versiones anteriores de C# se requiere una conversión
        //         explícita //Excel._Worksheet workSheet =   
        //Excel.Worksheet)excelApp.ActiveSheet;

        //         workSheet.Cells[3, "A"] = "ID Number";
        //         workSheet.Cells[3, "B"] = "Nombre";
        //         workSheet.Cells[3, "C"] = "Salario";

        //         workSheet.Cells[1, "A"] = "Listado de Empleados";
        //         workSheet.Range["A1", "C1"].Merge();
        //         workSheet.Range["A1", "C1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //         workSheet.Range["A1", "C1"].Font.Bold = true;
        //         workSheet.Range["A1", "C1"].Font.Size = 20;

        //         var row = 3;
        //         foreach (var acct in accounts)
        //         {
        //             row++;
        //             workSheet.Cells[row, "A"] = acct.ID;
        //             workSheet.Cells[row, "B"] = acct.Nombre;
        //             workSheet.Cells[row, "C"] = acct.Salario;
        //         }
        //         workSheet.Columns[1].AutoFit();
        //         workSheet.Columns[2].AutoFit();
        //         workSheet.Columns[3].AutoFit();
        //         workSheet.Range["A3", "C5"].AutoFormat(Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic2);
        //     }




    }
}
