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
    public class PObjetivoesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PObjetivoes
        public ActionResult Index(int? idp)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pObjetivos = db.PObjetivos.Include(p => p.PEstado).Where(w=>w.Proyecto_Id == idp).OrderBy(o=>o.FechaEntrega);
            ViewBag.proyecto = db.Proyectos.Find(idp).Nombre;
            ViewBag.idProyecto = idp;

            return View(pObjetivos.ToList());
        }

        // GET: PObjetivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Create
        public ActionResult Create(int? idp)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre");
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos.Where(w=> w.Id == idp), "Id", "Nombre");
            
            return View();
        }

        // POST: PObjetivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,FechaIncio,FechaEntrega,Aprovador,Estado_Id,Proyecto_Id")] PObjetivo pObjetivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _ipd = pObjetivo.Proyecto_Id;

            if (ModelState.IsValid)
            {
                db.PObjetivos.Add(pObjetivo);
                db.SaveChanges();
                return RedirectToAction("Index", "PObjetivoes", new { idp = _ipd });
            }

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            return View(pObjetivo);
        }

        // POST: PObjetivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,FechaIncio,FechaEntrega,Aprovador,Estado_Id,Proyecto_Id")] PObjetivo pObjetivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _ipd = pObjetivo.Proyecto_Id;

            if (ModelState.IsValid)
            {
                db.Entry(pObjetivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "PObjetivoes", new { idp = _ipd });
            }

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            return View(pObjetivo);
        }

        // POST: PObjetivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            db.PObjetivos.Remove(pObjetivo);
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
