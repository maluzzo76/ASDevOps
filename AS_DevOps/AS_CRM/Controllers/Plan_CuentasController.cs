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
    public class Plan_CuentasController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        // GET: Plan_Cuentas
        public ActionResult Index(string SearchString,string SearchStringAccount, int pagina = 1)
        {

            if (!validarLoggin())
                return RedirectToAction("login", "Account");

            var _r = from _o in db.Plan_Cuentas.OrderBy(o => o.Codigo).ToList()
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Codigo.ToLower().StartsWith(SearchString.ToLower()) || _o.Numero.Equals(SearchString)|| _o.Nombre.ToLower().Contains(SearchString.ToLower())
                     select _o;
            }

            if (!string.IsNullOrEmpty(SearchStringAccount))
            {
                _r = from _o in _r
                     where _o.Nombre.ToLower().Contains(SearchStringAccount.ToLower())
                     select _o;
            }

            ViewData["LA"] = db.Lineas_Asiento.Where(l=>l.Plan_Cuentas.IsImputable==true).ToList<Lineas_Asiento>();


            return View(_r);
        }

        // GET: Plan_Cuentas/Details/5
        public ActionResult Details(int? id)
        {
            validarLoggin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan_Cuentas plan_Cuentas = db.Plan_Cuentas.Find(id);

            ViewData["PCA"] = db.Plan_Cuentas.Include(l => l.Lineas_Asiento).Where(w => w.Codigo.StartsWith(plan_Cuentas.Codigo)).OrderBy(o => o.Codigo).ToList<Plan_Cuentas>();
            ViewData["LA"] = db.Lineas_Asiento.Where(l => l.Plan_Cuentas.Codigo.StartsWith(plan_Cuentas.Codigo) && l.Plan_Cuentas.IsImputable == true).OrderBy(o=>o.Plan_Cuentas.Codigo).ToList<Lineas_Asiento>();

            if (plan_Cuentas == null)
            {
                return HttpNotFound();
            }
            return View(plan_Cuentas);
        }

        // GET: Plan_Cuentas/Create
        public ActionResult Create()
        {
            validarLoggin();
            return View();
        }

        // POST: Plan_Cuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,Numero,IsImputable,IsResultado")] Plan_Cuentas plan_Cuentas)
        {
            validarLoggin();
            if (ModelState.IsValid)
            {
                db.Plan_Cuentas.Add(plan_Cuentas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(plan_Cuentas);
        }

        // GET: Plan_Cuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            validarLoggin();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan_Cuentas plan_Cuentas = db.Plan_Cuentas.Find(id);
            if (plan_Cuentas == null)
            {
                return HttpNotFound();
            }
            return View(plan_Cuentas);
        }

        // POST: Plan_Cuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,Numero,IsImputable,IsResultado")] Plan_Cuentas plan_Cuentas)
        {
            validarLoggin();
            if (ModelState.IsValid)
            { 
                db.Entry(plan_Cuentas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plan_Cuentas);
        }

        // GET: Plan_Cuentas/Delete/5
        public ActionResult Delete(int? id)
        {
            validarLoggin();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan_Cuentas plan_Cuentas = db.Plan_Cuentas.Find(id);
            if (plan_Cuentas == null)
            {
                return HttpNotFound();
            }
            return View(plan_Cuentas);
        }

        // POST: Plan_Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            validarLoggin();
            Plan_Cuentas plan_Cuentas = db.Plan_Cuentas.Find(id);
            db.Plan_Cuentas.Remove(plan_Cuentas);
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
