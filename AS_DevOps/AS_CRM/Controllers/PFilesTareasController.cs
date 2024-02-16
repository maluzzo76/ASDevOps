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

namespace AS_CRM.Controllers
{
    public class PFilesTareasController : CustomController
    {
        private AS_CRMEntities db = new AS_CRMEntities();

        public ActionResult Index(int? idt, string viewreturn)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.viewreturn = viewreturn;
            return View(db.PTareas.Find(idt));
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file, int idt, string viewreturn)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");



            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string _fileId = string.Format("{0}.{1}", Guid.NewGuid().ToString(), file.FileName.Split(char.Parse("."))[1]);

                    string _server = Request.Path;
                    string path = Path.Combine(Server.MapPath("~/files"), _fileId);
                    file.SaveAs(path);

                    PFilesTarea pFilesTarea = new PFilesTarea();
                    pFilesTarea.Tarea_Id = idt;
                    pFilesTarea.Nombre = file.FileName;
                    pFilesTarea.LinkName = _fileId;

                    db.PFilesTareas.Add(pFilesTarea);
                    db.SaveChanges();
                }


            }

            switch (viewreturn)
            {
                case "EditModal":
                    return RedirectToAction("EditModal", "PTareas", new { id = idt });

                case "PTareas":
                    return RedirectToAction("Edit", "PTareas", new { id = idt });

            }

            return RedirectToAction("Index", "PFilesTareas", new { idt = idt, viewreturn = viewreturn });
        }
    }
}
