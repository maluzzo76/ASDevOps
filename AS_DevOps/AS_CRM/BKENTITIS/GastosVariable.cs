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

    public partial class GastosVariable
    {
        public int Id { get; set; }
        public Nullable<int> TipoGastoId { get; set; }
        public string Descripcion { get; set; }
        public Nullable<decimal> Importe { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<decimal> Iva { get; set; }
        public Nullable<decimal> IIBB { get; set; }
        public Nullable<decimal> Neto { get; set; }
        public Nullable<int> Cuenta_Id { get; set; }
    
        public virtual TipoGasto TipoGasto { get; set; }
        public virtual Plan_Cuentas Plan_Cuentas { get; set; }
    }
}
