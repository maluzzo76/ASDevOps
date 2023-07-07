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
    public class GastosFijoesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: GastosFijoes
        public ActionResult Index(string SearchString, int pagina = 1)
        {

            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            var _r = from _o in db.GastosFijos.Include(g => g.TipoGasto)
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Descripcion.Contains(SearchString) 
                     select _o;
            }

            Pagination<GastosFijo> _page = new Pagination<GastosFijo>();

            return View(_page.paginado(_r, pagina));
        }

        // GET: GastosFijoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosFijo gastosFijo = db.GastosFijos.Find(id);
            if (gastosFijo == null)
            {
                return HttpNotFound();
            }
            return View(gastosFijo);
        }

        // GET: GastosFijoes/Create
        public ActionResult Create()
        {
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Nombre");
            return View();
        }

        // POST: GastosFijoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,FechaRegistro")] GastosFijo gastosFijo)
        {
            if (ModelState.IsValid)
            {
                db.GastosFijos.Add(gastosFijo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosFijo.TipoGastoId);
            return View(gastosFijo);
        }

        // GET: GastosFijoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosFijo gastosFijo = db.GastosFijos.Find(id);
            if (gastosFijo == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosFijo.TipoGastoId);
            return View(gastosFijo);
        }

        // POST: GastosFijoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,FechaRegistro")] GastosFijo gastosFijo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gastosFijo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosFijo.TipoGastoId);
            return View(gastosFijo);
        }

        // GET: GastosFijoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosFijo gastosFijo = db.GastosFijos.Find(id);
            if (gastosFijo == null)
            {
                return HttpNotFound();
            }
            return View(gastosFijo);
        }

        // POST: GastosFijoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GastosFijo gastosFijo = db.GastosFijos.Find(id);
            db.GastosFijos.Remove(gastosFijo);
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
