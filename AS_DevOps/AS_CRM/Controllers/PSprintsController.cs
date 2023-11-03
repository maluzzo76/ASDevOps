using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;

namespace AS_CRM.Controllers
{
    public class PSprintsController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PSprints
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pSprints = db.PSprints.Include(p => p.Proyecto).Include(p => p.PEstado);
            return View(pSprints.OrderBy(o=> o.PEstado.Nombre).ToList());
        }

        public ActionResult EditTarea(string nombre,int estado_id, int? sprint_id, int? tarea_id)
        {
            PTarea pTarea = db.PTareas.Find(tarea_id);

            pTarea.Nombre = nombre;
            db.Entry(pTarea).State = EntityState.Modified;
            db.SaveChanges();
            

            PSprint pSprint = db.PSprints.Find(sprint_id);
            return RedirectToAction("Details", "PSprints", new { id= sprint_id});
        }

        // GET: PSprints/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");


            AspNetUser _user = GetUsert();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSprint pSprint = db.PSprints.Find(id) ;

            ViewBag.userfilter = false;

            if (_user.EmailConfirmed == true)
                ViewBag.userfilter = true;

            ViewData["estados"] = db.PEstados.ToList<PEstado>();

            if (pSprint == null)
            {
                return HttpNotFound();
            }
            return View(pSprint);
        }

        // GET: PSprints/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre");
            ViewBag.Estado_id = new SelectList(db.PEstados, "Id", "Nombre");
            return View();
        }

        // POST: PSprints/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Proyecto_Id,Nombre,FechaIncio,FechaFin,Estado_id")] PSprint pSprint)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.PSprints.Add(pSprint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pSprint.Proyecto_Id);
            ViewBag.Estado_id = new SelectList(db.PEstados, "Id", "Nombre", pSprint.Estado_id);
            return View(pSprint);
        }

        // GET: PSprints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSprint pSprint = db.PSprints.Find(id);
            if (pSprint == null)
            {
                return HttpNotFound();
            }
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pSprint.Proyecto_Id);
            ViewBag.Estado_id = new SelectList(db.PEstados, "Id", "Nombre", pSprint.Estado_id);
            return View(pSprint);
        }

        // POST: PSprints/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Proyecto_Id,Nombre,FechaIncio,FechaFin,Estado_id")] PSprint pSprint)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(pSprint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pSprint.Proyecto_Id);
            ViewBag.Estado_id = new SelectList(db.PEstados, "Id", "Nombre", pSprint.Estado_id);
            return View(pSprint);
        }

        // GET: PSprints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSprint pSprint = db.PSprints.Find(id);
            if (pSprint == null)
            {
                return HttpNotFound();
            }
            return View(pSprint);
        }

        // POST: PSprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PSprint pSprint = db.PSprints.Find(id);
            db.PSprints.Remove(pSprint);
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
