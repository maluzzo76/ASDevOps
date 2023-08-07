//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AS_CRM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Comprobante
    {
        public int Id { get; set; }
        public Nullable<int> TipoComprobanteId { get; set; }
        public Nullable<int> ClienteId { get; set; }
        public Nullable<decimal> Numero { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaRegistracion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaVencimiento { get; set; }
        public Nullable<decimal> TotalNeto { get; set; }
        public Nullable<decimal> Iva { get; set; }
        public Nullable<decimal> IIBB { get; set; }
        public Nullable<decimal> TotalBruto { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual TiposComprobante TiposComprobante { get; set; }
    }
}
