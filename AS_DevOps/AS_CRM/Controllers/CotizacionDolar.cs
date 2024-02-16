using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_CRM.Controllers
{
    public class CotizacionDolar
    {
        public decimal compra { get; set; }
        public decimal venta { get; set; }
        public int agencia { get; set; }    
        public string nombre { get; set; }    
        public decimal variacion { get; set; }
        public string ventaCero { get; set; }
        public string decimales { get; set; }

    }
}