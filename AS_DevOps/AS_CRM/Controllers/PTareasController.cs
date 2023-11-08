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
    public class PTareasController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PTareas
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pTareas = db.PTareas.Include(p => p.AspNetUser).Include(p => p.PEstado).Include(p => p.PObjetivo).Include(p => p.PSprint);
            return View(pTareas.ToList());
        }

        // GET: PTareas/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTarea pTarea = db.PTareas.Find(id);
            if (pTarea == null)
            {
                return HttpNotFound();
            }
            return View(pTarea);
        }

        // GET: PTareas/Create
        public ActionResult Create(int? ido)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            int _Proyecto_id = db.PObjetivos.Find(ido).Proyecto_Id.Value;
            ViewBag.ido = _Proyecto_id;

            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre");
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos.Where(w=>w.Id == ido), "Id", "Nombre");
            ViewBag.Sprint_Id = new SelectList(db.PSprints, "Id", "Nombre");
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre");
            return View();
        }

        // POST: PTareas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Usuario_Id,Estado_Id,Objetivo_Id,Sprint_Id,FechaIncio,FechaFinalizado,FechaEntrega,HorasEstimadas,Detalle,Prioridad_Id")] PTarea pTarea)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            int _proyecto_id = db.PObjetivos.Find(pTarea.Objetivo_Id).Proyecto_Id.Value;

            if (ModelState.IsValid)
            {
                db.PTareas.Add(pTarea);
                db.SaveChanges();
                var _idp = db.PObjetivos.Find(pTarea.Objetivo_Id).Proyecto_Id.Value;
                return RedirectToAction("Index", "PObjetivoes", new { idp = _idp });
            }

            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "UserName", pTarea.Usuario_Id);
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pTarea.Estado_Id);
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos.Where(w=> w.Id == pTarea.Objetivo_Id), "Id", "Nombre", pTarea.Objetivo_Id);
            ViewBag.Sprint_Id = new SelectList(db.PSprints.Where(w => w.Proyecto_Id == _proyecto_id), "Id", "Nombre", pTarea.Sprint_Id);
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre", pTarea.Prioridad_Id);
            return View(pTarea);
        }

        // GET: PTareas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTarea pTarea = db.PTareas.Find(id);
            if (pTarea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "UserName", pTarea.Usuario_Id);
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pTarea.Estado_Id);
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos, "Id", "Nombre", pTarea.Objetivo_Id);
            ViewBag.Sprint_Id = new SelectList(db.PSprints, "Id", "Nombre", pTarea.Sprint_Id);
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre", pTarea.Prioridad_Id);
            return View(pTarea);
        }

        // POST: PTareas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Usuario_Id,Estado_Id,Objetivo_Id,Sprint_Id,FechaIncio,FechaFinalizado,FechaEntrega,HorasEstimadas,Detalle, Prioridad_Id")] PTarea pTarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pTarea).State = EntityState.Modified;
                db.SaveChanges();
                var _idp = db.PObjetivos.Find(pTarea.Objetivo_Id).Proyecto_Id;
                return RedirectToAction("Index","PObjetivoes", new { idp = _idp });
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "Email", pTarea.Usuario_Id);
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pTarea.Estado_Id);
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos, "Id", "Nombre", pTarea.Objetivo_Id);
            ViewBag.Sprint_Id = new SelectList(db.PSprints.Where(w=>w.Proyecto_Id == pTarea.PObjetivo.Proyecto_Id), "Id", "Nombre", pTarea.Sprint_Id);
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre", pTarea.Prioridad_Id);
            return View(pTarea);
        }

        public ActionResult EditModal(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTarea pTarea = db.PTareas.Find(id);
            if (pTarea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "UserName", pTarea.Usuario_Id);
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pTarea.Estado_Id);
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos, "Id", "Nombre", pTarea.Objetivo_Id);
            ViewBag.Sprint_Id = new SelectList(db.PSprints, "Id", "Nombre", pTarea.Sprint_Id);
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre", pTarea.Prioridad_Id);
            return View(pTarea);
        }

        // POST: PTareas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModal([Bind(Include = "Id,Nombre,Usuario_Id,Estado_Id,Objetivo_Id,Sprint_Id,FechaIncio,FechaFinalizado,FechaEntrega,HorasEstimadas,Detalle, Prioridad_Id")] PTarea pTarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pTarea).State = EntityState.Modified;
                db.SaveChanges();
                var _ids = pTarea.Sprint_Id;
                return RedirectToAction("Details", "PSPrints", new { id = _ids });
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "Email", pTarea.Usuario_Id);
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pTarea.Estado_Id);
            ViewBag.Objetivo_Id = new SelectList(db.PObjetivos, "Id", "Nombre", pTarea.Objetivo_Id);
            ViewBag.Sprint_Id = new SelectList(db.PSprints.Where(w => w.Proyecto_Id == pTarea.PObjetivo.Proyecto_Id), "Id", "Nombre", pTarea.Sprint_Id);
            ViewBag.Prioridad_Id = new SelectList(db.PPrioridades, "Id", "Nombre", pTarea.Prioridad_Id);
            return View(pTarea);
        }


        // GET: PTareas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTarea pTarea = db.PTareas.Find(id);
            if (pTarea == null)
            {
                return HttpNotFound();
            }
            return View(pTarea);
        }

        // POST: PTareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");
                         

            PTarea pTarea = db.PTareas.Find(id);
            var _idp = pTarea.PObjetivo.Proyecto_Id;
            db.PTareas.Remove(pTarea);
            db.SaveChanges();
            return RedirectToAction("Index","Pobjetivoes", new { idp = _idp });
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
