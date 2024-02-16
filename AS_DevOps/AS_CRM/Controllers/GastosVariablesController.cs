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
    public class GastosVariablesController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();
      

        public ActionResult Index(string SearchString, int pagina = 1)
        {

            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _r = from _o in db.GastosVariables.Include(g => g.TipoGasto).OrderByDescending(o => o.Id).ToList<GastosVariable>()
            select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Descripcion.Contains(SearchString)
                     select _o;
            }

            Pagination<GastosVariable> _page = new Pagination<GastosVariable>();

            return View(_page.paginado(_r, pagina));
        }

        // GET: GastosVariables/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            return View(gastosVariable);
        }

        // GET: GastosVariables/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _planCuenta = db.Plan_Cuentas.Where(w => w.IsImputable == true).ToDictionary(s => s.Id, s => (s.Numero + " - " + s.Nombre)).OrderBy(o => o.Value);
            ViewBag.Cuenta_Id = new SelectList(_planCuenta, "Key", "Value");
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Nombre");
            return View();
        }

        // POST: GastosVariables/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,Neto,Iva,IIBB,FechaRegistro, Cuenta_Id")] GastosVariable gastosVariable)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            int CuentaOrigenId = gastosVariable.Cuenta_Id.Value;
            int CuentaDestinoId = 0;
           

            if (ModelState.IsValid)
            {
                db.GastosVariables.Add(gastosVariable);
                db.SaveChanges();


                if (db.TipoGastoes.Find(gastosVariable.TipoGastoId).Cuenta_Id != null)
                {
                    CuentaDestinoId = db.TipoGastoes.Find(gastosVariable.TipoGastoId).Cuenta_Id.Value;

                    //Crea asiento
                    Asiento _asiento = new Asiento();
                    _asiento.Fecha = gastosVariable.FechaRegistro;
                    _asiento.Concepto = "Asiento automatico generado desde gastos";
                    db.Asientos.Add(_asiento);
                    db.SaveChanges();

                    //Crear Linea de asiento
                    Lineas_Asiento _laOrigen = new Lineas_Asiento();
                    _laOrigen.Asiento_Id = _asiento.Id;
                    _laOrigen.Cuenta_Id = CuentaOrigenId;
                    _laOrigen.Debe = 0;
                    _laOrigen.Haber = gastosVariable.Importe;
                    _laOrigen.Concepto = string.Format("Gasto de {0} - Nro Comprobante {1}", gastosVariable.TipoGasto.Nombre, gastosVariable.Descripcion);
                    db.Lineas_Asiento.Add(_laOrigen);
                    db.SaveChanges();

                    Lineas_Asiento _laDestino = new Lineas_Asiento();
                    _laDestino.Asiento_Id = _asiento.Id;
                    _laDestino.Cuenta_Id = CuentaDestinoId;
                    _laDestino.Debe = gastosVariable.Importe;
                    _laDestino.Haber = 0;
                    _laDestino.Concepto = string.Format("Gasto de {0} - Nro Comprobante {1}", gastosVariable.TipoGasto.Nombre, gastosVariable.Descripcion);
                    db.Lineas_Asiento.Add(_laDestino);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Nombre", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // GET: GastosVariables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            var _planCuenta = db.Plan_Cuentas.Where(w => w.IsImputable == true).ToDictionary(s => s.Id, s => (s.Numero + " - " + s.Nombre)).OrderBy(o => o.Value);
            ViewBag.Cuenta_Id = new SelectList(_planCuenta, "Key", "Value", gastosVariable.Cuenta_Id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Nombre", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // POST: GastosVariables/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoGastoId,Descripcion,Importe,Neto,Iva,IIBB,FechaRegistro, Cuenta_Id")] GastosVariable gastosVariable)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(gastosVariable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoGastoId = new SelectList(db.TipoGastoes, "Id", "Nombre", gastosVariable.TipoGastoId);
            return View(gastosVariable);
        }

        // GET: GastosVariables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            if (gastosVariable == null)
            {
                return HttpNotFound();
            }
            return View(gastosVariable);
        }

        // POST: GastosVariables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            GastosVariable gastosVariable = db.GastosVariables.Find(id);
            db.GastosVariables.Remove(gastosVariable);
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
