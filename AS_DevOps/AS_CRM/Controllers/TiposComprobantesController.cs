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
    public class TiposComprobantesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: TiposComprobantes
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            var _r = from _o in db.TiposComprobantes
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Nombre.Contains(SearchString)
                     select _o;
            }

            Pagination<TiposComprobante> _page = new Pagination<TiposComprobante>();

            return View(_page.paginado(_r, pagina));
        }

        // GET: TiposComprobantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposComprobante tiposComprobante = db.TiposComprobantes.Find(id);
            if (tiposComprobante == null)
            {
                return HttpNotFound();
            }
            return View(tiposComprobante);
        }

        // GET: TiposComprobantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposComprobantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Acronimo,Nombre")] TiposComprobante tiposComprobante)
        {
            if (ModelState.IsValid)
            {
                db.TiposComprobantes.Add(tiposComprobante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposComprobante);
        }

        // GET: TiposComprobantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposComprobante tiposComprobante = db.TiposComprobantes.Find(id);
            if (tiposComprobante == null)
            {
                return HttpNotFound();
            }
            return View(tiposComprobante);
        }

        // POST: TiposComprobantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Acronimo,Nombre")] TiposComprobante tiposComprobante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposComprobante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposComprobante);
        }

        // GET: TiposComprobantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposComprobante tiposComprobante = db.TiposComprobantes.Find(id);
            if (tiposComprobante == null)
            {
                return HttpNotFound();
            }
            return View(tiposComprobante);
        }

        // POST: TiposComprobantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposComprobante tiposComprobante = db.TiposComprobantes.Find(id);
            db.TiposComprobantes.Remove(tiposComprobante);
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
