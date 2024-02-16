using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;
using static System.Net.WebRequestMethods;

namespace AS_CRM.Controllers
{
    public class ComprobantesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

   
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _r = from _o in db.Comprobantes.Include(c => c.Cliente).Include(c => c.TiposComprobante)
            select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Cliente.RazonSocial.Contains(SearchString)
                     select _o;
            }

            Pagination<Comprobante> _page = new Pagination<Comprobante>();

            return View(_page.paginado(_r.OrderByDescending(o=>o.FechaRegistracion), pagina));
        }

        public ActionResult FileUpload(int? Id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Comprobante _comp = new Comprobante();

            if (Id == null)
            {
                _comp.FechaRegistracion = DateTime.Now;
                _comp.FechaVencimiento = DateTime.Now;
                _comp.TotalNeto = 0;
                _comp.Iva = 0;
                _comp.IIBB = 0;
                _comp.TotalBruto = 0;
                _comp.Numero = 0;


                db.Comprobantes.Add(_comp);
                db.SaveChanges();
                return View(_comp);
            }
            else
            {
                _comp = db.Comprobantes.Find(Id);
            }
            return View(_comp);
        }

        public ActionResult upFile(HttpPostedFileBase file, int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string _fileId = string.Format("C_{0}.{1}", file.FileName.Split(char.Parse("."))[0], file.FileName.Split(char.Parse("."))[1]);

                    string _server = Request.Path;
                    string path = Path.Combine(Server.MapPath("~/files/pdf"), _fileId);
                    file.SaveAs(path);

                    Comprobante _comp = db.Comprobantes.Find(id);
                    _comp.FileName = _fileId;
                    db.Entry(_comp).State = EntityState.Modified;
                    db.SaveChanges();

                    if(_comp.Numero==0)
                        return RedirectToAction("Edit", "Comprobantes", new { id = _comp.Id});
                }
            }

          
            return RedirectToAction("Index");
        }

        // GET: Comprobantes/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            return View(comprobante);
        }

        // GET: Comprobantes/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial");
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo");
            return View();
        }

        // POST: Comprobantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoComprobanteId,ClienteId,Numero,FechaRegistracion,FechaVencimiento,TotalNeto,Iva,IIBB,TotalBruto")] Comprobante comprobante)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Comprobantes.Add(comprobante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial",comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // GET: Comprobantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }

            var _clientes = db.Clientes.ToList<Cliente>();
            Cliente _cNew = new Cliente();
            _cNew.RazonSocial = "N/A";
            _cNew.Id = 0;
            _clientes.Add(_cNew);

            ViewBag.ClienteId = new SelectList(_clientes.ToList<Cliente>() , "Id", "RazonSocial",(comprobante.ClienteId==null)?0:comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // POST: Comprobantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoComprobanteId,ClienteId,Numero,FechaRegistracion,FechaVencimiento,TotalNeto,Iva,IIBB,TotalBruto,FileName")] Comprobante comprobante)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(comprobante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "RazonSocial", comprobante.ClienteId);
            ViewBag.TipoComprobanteId = new SelectList(db.TiposComprobantes, "Id", "Acronimo", comprobante.TipoComprobanteId);
            return View(comprobante);
        }

        // GET: Comprobantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = db.Comprobantes.Find(id);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            return View(comprobante);
        }

        // POST: Comprobantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Comprobante comprobante = db.Comprobantes.Find(id);
            db.Comprobantes.Remove(comprobante);
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
