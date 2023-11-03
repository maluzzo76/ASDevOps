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
    public class LeadsController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: Leads
        public ActionResult Index(string SearchString,string SearchProv, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _r = from _o in db.Leads                   
                     select _o;
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.CUIT.Contains(SearchString) || _o.Razon_Social.Contains(SearchString) || _o.Nombre_Contacto.StartsWith(SearchString)
                     select _o;
            }

            if (!string.IsNullOrEmpty(SearchProv))
            {
                _r = from _o in _r
                     where  _o.Provincia.Contains(SearchProv)
                     select _o;
            }

            ViewData["Leads"] = _r.ToList<Lead>();
            ViewBag.buscar = SearchString;
            ViewBag.buscarProv = SearchProv;

            Pagination<Lead> _page = new Pagination<Lead>();            
            return View(_page.paginado(_r.OrderBy(o=>o.Razon_Social), pagina));
        }

        // GET: Leads/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // GET: Leads/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Leads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Razon_Social,CUIT,Telefono_Razon_Social,Email_Razon_Social,Nombre_Contacto,Cargo_Contacto,Telefono_Contacto,Telefono2_Contacto,Email_Contacto,Email2_Contacto,Informacion_Fiscal,Provincia,Localidad")] Lead lead)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (db.Leads.Where(w => w.Razon_Social == lead.Razon_Social).Count() == 0)
            {
                db.Leads.Add(lead);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lead);
        }

        // GET: Leads/Edit/5
        public ActionResult Edit(int? id, string SearchString, string SearchProv)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.buscar = SearchString;
            ViewBag.buscarProv = SearchProv;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // POST: Leads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Razon_Social,CUIT,Telefono_Razon_Social,Email_Razon_Social,Nombre_Contacto,Cargo_Contacto,Telefono_Contacto,Telefono2_Contacto,Email_Contacto,Email2_Contacto,Informacion_Fiscal,Provincia,Localidad")] Lead lead)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(lead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lead);
        }

        // GET: Leads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Lead lead = db.Leads.Find(id);
            db.Leads.Remove(lead);
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
