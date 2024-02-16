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
    public class AcuardosComercialesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: AcuardosComerciales
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _r = db.AcuardosComerciales.Include(a => a.Cliente);

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Cliente.RazonSocial.Contains(SearchString)
                     select _o;
            }

            Pagination<AcuardosComerciale> _page = new Pagination<AcuardosComerciale>();
            return View(_page.paginado(_r.OrderByDescending(o=>o.Fecha),pagina));
        }

       

        // GET: AcuardosComerciales/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcuardosComerciale acuardosComerciale = db.AcuardosComerciales.Find(id);
            if (acuardosComerciale == null)
            {
                return HttpNotFound();
            }
            return View(acuardosComerciale);
        }

        // GET: AcuardosComerciales/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial");
            return View();
        }

        // POST: AcuardosComerciales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClienteId,ValorHora,HorasVendidas,ImporteTotal,Fecha,IsActiva")] AcuardosComerciale acuardosComerciale)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.AcuardosComerciales.Add(acuardosComerciale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", acuardosComerciale.ClienteId);
            return View(acuardosComerciale);
        }

        // GET: AcuardosComerciales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcuardosComerciale acuardosComerciale = db.AcuardosComerciales.Find(id);
            if (acuardosComerciale == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", acuardosComerciale.ClienteId);
            return View(acuardosComerciale);
        }

        // POST: AcuardosComerciales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClienteId,ValorHora,HorasVendidas,ImporteTotal,Fecha,IsActiva")] AcuardosComerciale acuardosComerciale)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(acuardosComerciale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", acuardosComerciale.ClienteId);
            return View(acuardosComerciale);
        }

        // GET: AcuardosComerciales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcuardosComerciale acuardosComerciale = db.AcuardosComerciales.Find(id);
            if (acuardosComerciale == null)
            {
                return HttpNotFound();
            }
            return View(acuardosComerciale);
        }

        // POST: AcuardosComerciales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            AcuardosComerciale acuardosComerciale = db.AcuardosComerciales.Find(id);
            db.AcuardosComerciales.Remove(acuardosComerciale);
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
