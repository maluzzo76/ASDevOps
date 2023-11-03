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
    public class Lineas_AsientoController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: Lineas_Asiento
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var lineas_Asiento = db.Lineas_Asiento.Include(l => l.Plan_Cuentas).Include(l => l.Asiento);
            return View(lineas_Asiento.ToList());
        }

        // GET: Lineas_Asiento/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas_Asiento lineas_Asiento = db.Lineas_Asiento.Find(id);
            if (lineas_Asiento == null)
            {
                return HttpNotFound();
            }
            return View(lineas_Asiento);
        }

        // GET: Lineas_Asiento/Create
        public ActionResult Create(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _planCuenta = db.Plan_Cuentas.Where(w=>w.IsImputable==true).ToDictionary(s => s.Id, s => (s.Numero + " - " + s.Nombre)).OrderBy(o=>o.Value);

            ViewBag.id = id;
            ViewBag.Cuenta_Id = new SelectList(_planCuenta, "Key", "Value");
            ViewBag.Asiento_Id = new SelectList(db.Asientos.Where(a=>a.Id == id), "Id", "Id");

            return View();
        }

        // POST: Lineas_Asiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Asiento_Id,Cuenta_Id,Concepto,Debe,Haber")] Lineas_Asiento lineas_Asiento)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Lineas_Asiento.Add(lineas_Asiento);
                db.SaveChanges();
            }

            return RedirectToAction("edit", "Asientoes", new { id = lineas_Asiento.Asiento_Id });
        }

        // GET: Lineas_Asiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas_Asiento lineas_Asiento = db.Lineas_Asiento.Find(id);
            if (lineas_Asiento == null)
            {
                return HttpNotFound();
            }

            var _planCuenta =   db.Plan_Cuentas.ToDictionary(s=>s.Id,s=>(s.Numero + " - " + s.Nombre));
            var _asiento = db.Asientos.ToDictionary(s => s.Id, s => (s.Id.ToString().PadLeft(8,char.Parse("0"))));

            ViewBag.Cuenta_Id = new SelectList(_planCuenta, "Key", "Value", lineas_Asiento.Cuenta_Id);
            ViewBag.Asiento_Id = new SelectList(_asiento, "Key", "Value", lineas_Asiento.Asiento_Id);
            return View(lineas_Asiento);
        }

        // POST: Lineas_Asiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Asiento_Id,Cuenta_Id,Concepto,Debe,Haber")] Lineas_Asiento lineas_Asiento)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(lineas_Asiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var _planCuenta = db.Plan_Cuentas.ToDictionary(s => s.Id, s => (s.Numero + " - " + s.Nombre));
            var _asiento = db.Asientos.ToDictionary(s => s.Id, s => (s.Id.ToString().PadLeft(8, char.Parse("0"))));

            ViewBag.Cuenta_Id = new SelectList(_planCuenta, "Key", "Value", lineas_Asiento.Cuenta_Id);
            ViewBag.Asiento_Id = new SelectList(_asiento, "Key", "Value", lineas_Asiento.Asiento_Id); 

            return RedirectToAction("edit", "Asientoes", new { id = lineas_Asiento.Asiento_Id });
        }

        // GET: Lineas_Asiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas_Asiento lineas_Asiento = db.Lineas_Asiento.Find(id);
            if (lineas_Asiento == null)
            {
                return HttpNotFound();
            }
            return View(lineas_Asiento);
        }

        // POST: Lineas_Asiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Lineas_Asiento lineas_Asiento = db.Lineas_Asiento.Find(id);
            db.Lineas_Asiento.Remove(lineas_Asiento);
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
