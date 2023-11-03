using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;
using Microsoft.Azure.Pipelines.WebApi;

namespace AS_CRM.Controllers
{
    public class AsientoesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: Asientoes
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _r = from _o in db.Asientos.Include(a => a.Lineas_Asiento).OrderByDescending(o => o.Id)
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Id.ToString().Contains(SearchString)
                     select _o;
            }

            Pagination<Asiento> _page = new Pagination<Asiento>();

            return View(_page.paginado(_r, pagina));
            
        }

        // GET: Asientoes/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiento asiento = db.Asientos.Find(id);
            if (asiento == null)
            {
                return HttpNotFound();
            }
            return View(asiento);
        }

        // GET: Asientoes/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Asiento _asEntity = new Asiento();
            _asEntity.Fecha = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Asientos.Add(_asEntity);
                db.SaveChanges();
                return RedirectToAction("edit","Asientoes",new { id=_asEntity.Id });
            }

            return View();
        }

        // POST: Asientoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Concepto,Fecha")] Asiento asiento)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Asientos.Add(asiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asiento);
        }

        public ActionResult Createla(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Create", "Lineas_Asiento", new { id = id});
        }

        // GET: Asientoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiento asiento = db.Asientos.Find(id);
            if (asiento == null)
            {
                return HttpNotFound();
            }
            return View(asiento);
        }

        // POST: Asientoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cuenta_Id,Concepto,Fecha")] Asiento asiento)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(asiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asiento);
        }

        // GET: Asientoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiento asiento = db.Asientos.Find(id);
            if (asiento == null)
            {
                return HttpNotFound();
            }
            return View(asiento);
        }

        // POST: Asientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Asiento asiento = db.Asientos.Find(id);
            db.Asientos.Remove(asiento);
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
