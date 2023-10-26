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
    public class CertificacionHorasController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: CertificacionHoras
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            var certificacionHoras = db.CertificacionHoras.Include(c => c.Cliente).Include(c => c.Comprobante);


            if (!string.IsNullOrEmpty(SearchString))
            {
                certificacionHoras = from _o in certificacionHoras
                                     where _o.Cliente.RazonSocial.Contains(SearchString)
                                     select _o;
            }

            Pagination<CertificacionHora> _page = new Pagination<CertificacionHora>();

            return View(_page.paginado(certificacionHoras, pagina));
        }

        // GET: CertificacionHoras/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificacionHora certificacionHora = db.CertificacionHoras.Find(id);
            if (certificacionHora == null)
            {
                return HttpNotFound();
            }
            return View(certificacionHora);
        }

        // GET: CertificacionHoras/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial");
            ViewBag.ComprobanteId = new SelectList(db.Comprobantes, "Id", "Id");
            return View();
        }

        // POST: CertificacionHoras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,ClienteId,ComprobanteId,HorasACertificar,HorasCertificadas,Saldo")] CertificacionHora certificacionHora)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (ModelState.IsValid)
            {
                db.CertificacionHoras.Add(certificacionHora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", certificacionHora.ClienteId);
            ViewBag.ComprobanteId = new SelectList(db.Comprobantes, "Id", "Id", certificacionHora.ComprobanteId);
            return View(certificacionHora);
        }

        // GET: CertificacionHoras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificacionHora certificacionHora = db.CertificacionHoras.Find(id);
            if (certificacionHora == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", certificacionHora.ClienteId);
            ViewBag.ComprobanteId = new SelectList(db.Comprobantes, "Id", "Id", certificacionHora.ComprobanteId);
            return View(certificacionHora);
        }

        // POST: CertificacionHoras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,ClienteId,ComprobanteId,HorasACertificar,HorasCertificadas,Saldo")] CertificacionHora certificacionHora)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(certificacionHora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", certificacionHora.ClienteId);
            ViewBag.ComprobanteId = new SelectList(db.Comprobantes, "Id", "Id", certificacionHora.ComprobanteId);
            return View(certificacionHora);
        }

        // GET: CertificacionHoras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificacionHora certificacionHora = db.CertificacionHoras.Find(id);
            if (certificacionHora == null)
            {
                return HttpNotFound();
            }
            return View(certificacionHora);
        }

        // POST: CertificacionHoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            CertificacionHora certificacionHora = db.CertificacionHoras.Find(id);
            db.CertificacionHoras.Remove(certificacionHora);
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
