using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using As_PDF;
///https://www.youtube.com/watch?v=XvtQsJxhYts hosting

namespace AS_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            As_PDF.Pdf.CrearCertificacionHoras();
            /*
           DataTable _dt = getCuits().Tables[0];

            foreach (DataRow _c in _dt.Rows)
            {
                printArba(_c["CUIT"].ToString());
            }
            */
            Console.WriteLine("FIN");
            
            Console.ReadLine();
        }

        static void printArba(object cuit)
        {
            try
            {
                var _arbalLocalidad = "";
                var _arbalProvincia = "";

                var url = string.Format("https://www.cuitonline.com/detalle/{0}/herpaco-s-a.html", cuit);


                var textFromFile = (new WebClient()).DownloadString(url);
                int _indexOffProvincia = textFromFile.IndexOf("Provincia:");
                int _indexOffLocalidad = textFromFile.IndexOf("Localidad:");



                if (_indexOffLocalidad > 0)
                {
                    int _indexFinLocalidad = textFromFile.Substring(_indexOffLocalidad).Replace("<span itemprop=\"addressRegion\" class=\"p_cuit c_black\">", "").IndexOf("·");
                    _arbalLocalidad = textFromFile.Substring(_indexOffLocalidad).Replace("<span itemprop=\"addressRegion\" class=\"p_cuit c_black\">", "").Substring(0, _indexFinLocalidad).Replace("Localidad: ", "");
                }

                if (_indexOffProvincia > 0)
                {
                    int _indexProvincia = textFromFile.Substring(_indexOffProvincia).Replace("<span itemprop=\"addressRegion\" class=\"p_cuit c_black\">", "").IndexOf("</span> -");

                    _arbalProvincia = textFromFile.Substring(_indexOffProvincia).Replace("<span itemprop=\"addressRegion\" class=\"p_cuit c_black\">", "").Substring(0, _indexProvincia).Replace("Provincia: ", "");
                }

                if (_arbalProvincia.Equals(""))
                {
                    _arbalProvincia = _arbalLocalidad;
                }


                Console.WriteLine(("Provincia:{0}", _arbalProvincia));
                Console.WriteLine(("Localidad:{0}", _arbalLocalidad));

                UpdateProspect(cuit, _arbalProvincia, _arbalLocalidad);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR");
            }
        }

        static DataSet getCuits()
        { 
            DataSet _ds  = new DataSet();
            string _query = "select * from prospects";
            string _strConnexion = "Password=Mml1609;Persist Security Info=True;User ID=sa;Initial Catalog=AS_CRM;Data Source=(local)";

            try
            {
                new SqlDataAdapter(_query, _strConnexion).Fill(_ds);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { }
            

            return _ds;
        }

        static void UpdateProspect(object cuit, string provincia, string localidad)
        {
            string _query = string.Format("update prospects set provincia = '{1}', localidad ='{2}' where cuit = '{0}'",cuit,provincia,localidad);
            string _strConnexion = "Password=Mml1609;Persist Security Info=True;User ID=sa;Initial Catalog=AS_CRM;Data Source=(local)";

            SqlCommand _sqlcomman = new SqlCommand(_query, new SqlConnection(_strConnexion));
            try
            {
                _sqlcomman.Connection.Open();
                _sqlcomman.ExecuteNonQuery();
                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally {
                _sqlcomman.Connection.Close();
                _sqlcomman.Connection.Dispose();
            }
        }
    }
}
