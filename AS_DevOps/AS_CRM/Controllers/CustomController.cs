using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Security;
using System.Security.Principal;

namespace AS_CRM.Controllers
{
    public class CustomController:Controller
    {
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
            if (User.Identity.Name == "")
                return false;

            return true;
        }
    }
}