using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Security;
using System.Security.Principal;
using AS_CRM;
using Microsoft.Azure.Pipelines.WebApi;
using System.Data.Entity;

/*
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
[Display(Name = "Descripcion Reparacion")]
*/

namespace AS_CRM.Controllers
{
    public class CustomController:Controller
    {
        private AS_CRMEntities db = new AS_CRMEntities();
        /*
         * ejemplo de validacion loggin
         public ActionResult Index()
        {
            if (validarLoggin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Account");
            }
            
        }*/

        public bool validarLoggin()
        {

            ViewData["Menu"] = db.MenuSecurities.Include(u => u.AspNetUser).Include(i => i.ItemMenuSecurities).Where(w => w.AspNetUser.UserName == User.Identity.Name && w.IsActivo==true).OrderBy(o=>o.Orden).ToList<MenuSecurity>();


            if (User.Identity.Name == "")
                return false;
                        
            return true;
        }

        public AspNetUser GetUsert()
        {
            var _users = db.AspNetUsers.Where(w => w.Email == User.Identity.Name);
            if(_users.Count()>0)
                return (AspNetUser) _users.ToList<AspNetUser>().ElementAt(0) ;

            return new AspNetUser();
        }

    }
}