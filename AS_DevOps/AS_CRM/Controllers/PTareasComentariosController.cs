using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;
using Microsoft.VisualStudio.Services.Identity;

namespace AS_CRM.Controllers
{
    public class PTareasComentariosController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PTareasComentarios
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pTareasComentarios = db.PTareasComentarios.Include(p => p.AspNetUser).Include(p => p.PTarea);
            return View(pTareasComentarios.ToList());
        }

        // GET: PTareasComentarios/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTareasComentario pTareasComentario = db.PTareasComentarios.Find(id);
            if (pTareasComentario == null)
            {
                return HttpNotFound();
            }
            return View(pTareasComentario);
        }

        // GET: PTareasComentarios/Create
        public ActionResult Create(int? idt, string viewreturn)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers.Where(w => w.Id == GetUsert().Id), "Id", "Email", GetUsert().Id);
            ViewBag.Tarea_Id = new SelectList(db.PTareas.Where(w => w.Id == idt), "Id", "Nombre", idt);
            ViewBag.viewreturn = viewreturn;

            return View();
        }

        // POST: PTareasComentarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tarea_Id,Usuario_Id,Descripcion,Fecha")] PTareasComentario pTareasComentario, string viewreturn)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                pTareasComentario.Usuario_Id = GetUsert().Id;
                pTareasComentario.Fecha = DateTime.Now;
                db.PTareasComentarios.Add(pTareasComentario);
                db.SaveChanges();


                switch (viewreturn)
                {
                    case "EditModal":
                        return RedirectToAction("EditModal", "PTareas", new { id = pTareasComentario.Tarea_Id });

                    case "PTareas":
                        return RedirectToAction("Edit", "PTareas", new { id = pTareasComentario.Tarea_Id });

                }
            }

            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers.Where(w => w.Id == GetUsert().Id), "Id", "Email", GetUsert().Id);
            ViewBag.Tarea_Id = new SelectList(db.PTareas.Where(w => w.Id == pTareasComentario.Tarea_Id), "Id", "Nombre", pTareasComentario.Tarea_Id);
            ViewBag.viewreturn = viewreturn;
            return View(pTareasComentario);
        }

        // GET: PTareasComentarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTareasComentario pTareasComentario = db.PTareasComentarios.Find(id);
            if (pTareasComentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "Email", pTareasComentario.Usuario_Id);
            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pTareasComentario.Tarea_Id);
            return View(pTareasComentario);
        }

        // POST: PTareasComentarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tarea_Id,Usuario_Id,Descripcion,Fecha")] PTareasComentario pTareasComentario)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(pTareasComentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usuario_Id = new SelectList(db.AspNetUsers, "Id", "Email", pTareasComentario.Usuario_Id);
            ViewBag.Tarea_Id = new SelectList(db.PTareas, "Id", "Nombre", pTareasComentario.Tarea_Id);
            return View(pTareasComentario);
        }

        // GET: PTareasComentarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PTareasComentario pTareasComentario = db.PTareasComentarios.Find(id);
            if (pTareasComentario == null)
            {
                return HttpNotFound();
            }
            return View(pTareasComentario);
        }

        // POST: PTareasComentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PTareasComentario pTareasComentario = db.PTareasComentarios.Find(id);
            db.PTareasComentarios.Remove(pTareasComentario);
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
