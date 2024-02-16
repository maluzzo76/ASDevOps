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
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

/*
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
[Display(Name = "Descripcion Reparacion")]
*/

namespace AS_CRM.Controllers
{
    public class CustomController : Controller
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
                return RedirectToAction("Index", "Home");
            }
            
        }*/

        public bool validarLoggin()
        {

            ViewData["Menu"] = db.MenuSecurities.Include(u => u.AspNetUser).Include(i => i.ItemMenuSecurities).Where(w => w.AspNetUser.UserName == User.Identity.Name && w.IsActivo == true).OrderBy(o => o.Orden).ToList<MenuSecurity>();


            if (User.Identity.Name == "")
                return false;

            return true;
        }

        public AspNetUser GetUsert()
        {
            var _users = db.AspNetUsers.Where(w => w.Email == User.Identity.Name);
            if (_users.Count() > 0)
                return (AspNetUser)_users.ToList<AspNetUser>().ElementAt(0);

            return new AspNetUser();
        }

        public DataSet SqlExecute(string query)
        { 
            DataSet _ds = new DataSet();
            SqlConnection _sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using(SqlCommand _sqlCommand = new SqlCommand(query, _sqlConn))
            {
                new SqlDataAdapter(_sqlCommand).Fill(_ds);
                _sqlCommand.Connection.Close();
                _sqlCommand.Connection.Dispose();
            }

            return _ds;
        }

        public void CotizacionDolar()
        {
            string _uriApi = "https://www.dolarsi.com/api/api.php?type=valoresprincipales";
            HttpWebRequest request = WebRequest.Create(_uriApi) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Metodo modificado
           /* string postData = "username=miUsuraio&password=MiClave&grant_type=password&client_id=Miclient_id"; 
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            

            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(dataStream))
                {
                    stmw.Write(postData);
                }
                dataStream.Write(byteArray, 0, byteArray.Length);
            }*/


            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            List<CotizacionDolar> foos = new List<CotizacionDolar>();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string resp = reader.ReadToEnd();
                var cotizaciones = Regex.Split(resp, "casa");
                foreach (var item in cotizaciones)
                {
                    var _cor = item;
                }

            }

            
        }
    }
}