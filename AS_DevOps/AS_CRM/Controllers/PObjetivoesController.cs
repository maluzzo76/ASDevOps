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
    public class PObjetivoesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: PObjetivoes
        public ActionResult Index(int? idp, int? Estado_Id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var pObjetivos = db.PObjetivos.Include(p => p.PEstado).Where(w=>w.Proyecto_Id == idp).OrderBy(o=>o.FechaIncio).ToList<PObjetivo>();

            PEstado _newpr = new PEstado();
            _newpr.Id = 0;
            _newpr.Nombre = "Todos";

            var festados = db.PEstados.ToList<PEstado>();
            festados.Add(_newpr);

            ViewBag.Estado_Id = new SelectList(festados.OrderBy(o=>o.Id), "Id", "Nombre");
            ViewBag.proyecto = db.Proyectos.Find(idp).Nombre;
            ViewBag.idProyecto = idp;
            ViewBag.filterEstado_Id = (Estado_Id == null) ? 0: Estado_Id ;

          /*  if (Estado_Id >0)
            {
                pObjetivos = pObjetivos.Where(w => w.Estado_Id == Estado_Id).ToList<PObjetivo>();
            }*/

            return View(pObjetivos.ToList());
        }

        public ActionResult Tareas(int idp, int? Estado_Id = 1)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PEstado _newpr = new PEstado();
            _newpr.Id = 0;
            _newpr.Nombre = "Todos";

            var festados = db.PEstados.ToList<PEstado>();
            festados.Add(_newpr);

            ViewBag.Estado_Id = new SelectList(festados.OrderBy(o => o.Id), "Id", "Nombre",Estado_Id);
            ViewBag.SelEstado_Id = Estado_Id;

           var pTareas = db.PTareas.Where(w => w.PObjetivo.Proyecto_Id == idp).ToList<PTarea>().OrderBy(o=>o.FechaIncio).ToList<PTarea>();

            if (Estado_Id >0)
            {
                pTareas = pTareas.Where(w => w.Estado_Id == Estado_Id).ToList<PTarea>();
            }

            return View(pTareas);
        }

        public ActionResult UpdateTaskDetails(string Detalle, int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PTarea _t = db.PTareas.Find(id);
            _t.Detalle = Detalle;

            db.Entry(_t).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Tareas", "PObjetivoes", new { idp = _t.PObjetivo.Proyecto_Id });
        }

        public ActionResult TareaSetting(int? id, string Usuario_Id, string FechaIncio, string FechaEntrega,int Prioridad_Id = 1,int Objetivo_Id =1, int Estado_Id =1, int Horas = 0)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

                      

            DateTime _incio = DateTime.Parse(FechaIncio);
            DateTime _entrega = DateTime.Parse(FechaEntrega);

            var _sprint = db.PSprints.Max(m => m.Id);

            try
            {
                _sprint = db.PSprints.Where(w => w.FechaFin >= _entrega).Min(m => m.Id);
            }
            catch { }
            

            PTarea _t = db.PTareas.Find(id);
            _t.Usuario_Id = Usuario_Id;
            _t.FechaIncio = _incio;
            _t.FechaEntrega = _entrega;
            _t.HorasEstimadas = Horas;
            _t.Prioridad_Id = Prioridad_Id;
            _t.Sprint_Id = _sprint;
            _t.Estado_Id = Estado_Id;
            _t.Objetivo_Id = Objetivo_Id;

            db.Entry(_t).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Tareas", "PObjetivoes", new { idp= _t.PObjetivo.Proyecto_Id});
        }

        // GET: PObjetivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Create
        public ActionResult Create(int? idp)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre");
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos.Where(w=> w.Id == idp), "Id", "Nombre");
            ViewBag.idp = idp;
            
            return View();
        }

        // POST: PObjetivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,FechaIncio,FechaEntrega,Aprovador,Estado_Id,Proyecto_Id")] PObjetivo pObjetivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _ipd = pObjetivo.Proyecto_Id;

            if (ModelState.IsValid)
            {
                db.PObjetivos.Add(pObjetivo);
                db.SaveChanges();
                return RedirectToAction("Index", "PObjetivoes", new { idp = _ipd });
            }

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            ViewBag.idp = pObjetivo.Proyecto_Id;
            return View(pObjetivo);
        }

        // POST: PObjetivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,FechaIncio,FechaEntrega,Aprovador,Estado_Id,Proyecto_Id")] PObjetivo pObjetivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _ipd = pObjetivo.Proyecto_Id;

            if (ModelState.IsValid)
            {
                db.Entry(pObjetivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "PObjetivoes", new { idp = _ipd });
            }

            ViewBag.Estado_Id = new SelectList(db.PEstados, "Id", "Nombre", pObjetivo.Estado_Id);
            ViewBag.Proyecto_Id = new SelectList(db.Proyectos, "Id", "Nombre", pObjetivo.Proyecto_Id);
            return View(pObjetivo);
        }

        // GET: PObjetivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            if (pObjetivo == null)
            {
                return HttpNotFound();
            }
            return View(pObjetivo);
        }

        // POST: PObjetivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            PObjetivo pObjetivo = db.PObjetivos.Find(id);
            db.PObjetivos.Remove(pObjetivo);
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
