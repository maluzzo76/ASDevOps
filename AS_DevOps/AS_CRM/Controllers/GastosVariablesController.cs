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
    public class GastosVariablesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: GastosVariables
        public ActionResult Index()
        {
            var gastosVariables = db.GastosVariables.Include(g => g.TipoGasto);
            return View(gastosVariables.ToList());
        }

        // GET: GastosVariables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            return View(gastosVariable);
        }

        // GET: GastosVariables/Create
        public ActionResult Create()
        {
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo");
            return View();
        }

        // POST: GastosVariables/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,FechaRegistro")] GastosVariable gastosVariable)
        {
            if (ModelState.IsValid)
            {
                db.GastosVariables.Add(gastosVariable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // GET: GastosVariables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // POST: GastosVariables/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,FechaRegistro")] GastosVariable gastosVariable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gastosVariable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Acronimo", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // GET: GastosVariables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            return View(gastosVariable);
        }

        // POST: GastosVariables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            db.GastosVariables.Remove(gastosVariable);
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
