using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_CRM;
using Newtonsoft.Json;

namespace AS_CRM.Controllers
{
    public class ProyectoesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: Proyectoes
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");
            

            ViewData["Task"] = db.PTareas.Include(i => i.PObjetivo).ToList<PTarea>();

            return View(db.Proyectos.OrderBy(o => o.Nombre).ToList());
        }

        public ActionResult GenerarCertificación(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Cliente _cli = db.Proyectos.Find(id).Cliente;
            string _query = string.Format("exec dbo.Sp_GetHorasCertificacionByProyectoId {0}", id);

            DataTable pDt = SqlExecute(_query).Tables[0];
            DateTime _maxDate = Convert.ToDateTime(pDt.Compute("max(Fecha)", null));
            int _horas = Convert.ToInt32(pDt.Compute("sum(Horas)", ""));

            
            string _leyenda = string.Format("Detalle de horas a certificar correspondientes al periodo {0}-{1},Total horas {2}hs.\nLa certificación corresponde solo a tareas en estado finalizadas.",_maxDate.Date.Month,_maxDate.Date.Year,_horas);
           
            string _pdfName = string.Format("{0}_{1}_{2}.pdf", "Certificación Horas", _cli.RazonSocial, DateTime.Now.Date.ToShortDateString().Replace("/", ""));

            As_PDF.Pdf.CrearCertificacionHoras(_cli.NombreContacto, _cli.RazonSocial, _cli.Direccion, _cli.Provincia, _cli.Pais, _leyenda,pDt,_pdfName);

            //Registra la certificación
            CertificacionHora _newCert = new CertificacionHora();
            _newCert.Fecha = DateTime.Now;
            _newCert.ClienteId = _cli.Id;
            _newCert.HorasACertificar =Convert.ToDecimal(pDt.Compute("sum(Horas)", ""));
            _newCert.DocumentoNombre =  _pdfName;
            _newCert.HorasCertificadas = 0;
            _newCert.Saldo = _newCert.HorasACertificar;
            _newCert.ValorHora = _cli.ValorHora;

            db.CertificacionHoras.Add(_newCert);
            db.SaveChanges();

            //Marca las tareas como certificadas
            foreach (DataRow _r in pDt.Rows)
            {
                PTarea _task = db.PTareas.Find(_r["Id"]);
                _task.Certificada = true;
                db.Entry(_task).State= EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Details", "Proyectoes", new { id=id, _err = true });
        }

        public ActionResult gannt()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult CertificarHora(int? idp, int? idt)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PTarea pTarea = db.PTareas.Find(idt);

            pTarea.Certificada = (pTarea.Certificada.Value) ? false : true;

            db.Entry(pTarea).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Proyectoes", new { id = idp });

        }

        // GET: Proyectoes/Details/5
        public ActionResult Details(int? id, bool? _err = false)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (_err.Value)
                ViewBag.Message = "La Certificación se creo con exito";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // GET: Proyectoes/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "RazonSocial");

            return View();
        }

        // POST: Proyectoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,logo,Cliente_Id")] Proyecto proyecto)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Proyectos.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(proyecto);
        }

        // GET: Proyectoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyectos.Find(id);
            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "RazonSocial", proyecto.Cliente_Id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: Proyectoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,logo,Cliente_Id")] Proyecto proyecto)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proyecto);
        }

        // GET: Proyectoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: Proyectoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Proyecto proyecto = db.Proyectos.Find(id);
            db.Proyectos.Remove(proyecto);
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
