using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;

namespace AS_CRM.Controllers
{
    public class ComprobantesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

   
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            var _r = from _o in db.Comprobantes.Include(c => c.Cliente).Include(c => c.TiposComprobante)
            select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Cliente.RazonSocial.Contains(SearchString)
                     select _o;
            }

            Pagination<Comprobante> _page = new Pagination<Comprobante>();

            return View(_page.paginado(_r, pagina));
        }

        // GET: Comprobantes/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            return View(comprobante);
        }

        // GET: Comprobantes/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial");
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo");
            return View();
        }

        // POST: Comprobantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoComprobanteId,ClienteId,Numero,FechaRegistracion,FechaVencimiento,TotalNeto,Iva,IIBB,TotalBruto")] Comprobante comprobante)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (ModelState.IsValid)
            {
                db.Comprobantes.Add(comprobante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // GET: Comprobantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // POST: Comprobantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoComprobanteId,ClienteId,Numero,FechaRegistracion,FechaVencimiento,TotalNeto,Iva,IIBB,TotalBruto")] Comprobante comprobante)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(comprobante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // GET: Comprobantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            return View(comprobante);
        }

        // POST: Comprobantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            Comprobante comprobante = db.Comprobantes.Find(id);
            db.Comprobantes.Remove(comprobante);
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
    }
}
