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
    public class PParteHorasController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PParteHoras
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pParteHoras = db.PParteHoras.Include(p => p.PTarea);
            return View(pParteHoras.ToList());
        }

        // GET: PParteHoras/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PParteHora pParteHora = db.PParteHoras.Find(id);
            if (pParteHora == null)
            {
                return HttpNotFound();
            }
            return View(pParteHora);
        }

        // GET: PParteHoras/Create
        public ActionResult Create(int idt)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.ido = db.PTareas.Find(idt).PObjetivo.Proyecto_Id;
            ViewBag.user = User.Identity.Name;

            ViewBag.Tarea_Id = new SelectList(db.PTareas.Where(w=>w.Id == idt), "Id", "Nombre");
            return View();
        }

        // POST: PParteHoras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tarea_Id,Fecha,Horas,UserName")] PParteHora pParteHora)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                pParteHora.UserName = User.Identity.Name;
                db.PParteHoras.Add(pParteHora);
                db.SaveChanges();
                var _idp = db.PTareas.Find(pParteHora.Tarea_Id).PObjetivo.Proyecto_Id;
                return RedirectToAction("Index", "PObjetivoes", new { idp = _idp }); ;
            }

            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pParteHora.Tarea_Id);
            return View(pParteHora);
        }


        public ActionResult CreateParteHoras(int idt)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.task = db.PTareas.Find(idt).Nombre;
            ViewBag.ido = db.PTareas.Find(idt).Sprint_Id;
            ViewBag.user = User.Identity.Name;

            ViewBag.Tarea_Id = new SelectList(db.PTareas.Where(w => w.Id == idt), "Id", "Nombre");
            return View();
        }

        // POST: PParteHoras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParteHoras([Bind(Include = "Id,Tarea_Id,Fecha,Horas,UserName")] PParteHora pParteHora)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                pParteHora.UserName = User.Identity.Name;
                db.PParteHoras.Add(pParteHora);
                db.SaveChanges();
                var _idp = db.PTareas.Find(pParteHora.Tarea_Id).Sprint_Id;
                return RedirectToAction("Details", "PSprints", new { id = _idp }); ;
            }

            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pParteHora.Tarea_Id);
            return View(pParteHora);
        }


        // GET: PParteHoras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PParteHora pParteHora = db.PParteHoras.Find(id);
            if (pParteHora == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pParteHora.Tarea_Id);
            return View(pParteHora);
        }

        // POST: PParteHoras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tarea_Id,Fecha,Horas,UserName")] PParteHora pParteHora)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(pParteHora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pParteHora.Tarea_Id);
            return View(pParteHora);
        }

        // GET: PParteHoras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PParteHora pParteHora = db.PParteHoras.Find(id);
            if (pParteHora == null)
            {
                return HttpNotFound();
            }
            return View(pParteHora);
        }

        // POST: PParteHoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PParteHora pParteHora = db.PParteHoras.Find(id);
            db.PParteHoras.Remove(pParteHora);
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
