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
    using System.Linq;

    public partial class PObjetivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PObjetivo()
        {
            this.PTareas = new HashSet<PTarea>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaIncio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaEntrega { get; set; }
        public string Aprovador { get; set; }
        public Nullable<int> Estado_Id { get; set; }
        public Nullable<int> Proyecto_Id { get; set; }

        public string Cumplimineto
        {
            get {
                try
                {
                    decimal _max = this.PTareas.Count();
                    decimal _completadas = this.PTareas.Where(w => w.PEstado.Nombre == "Finalizado").Count();
                    if (_completadas > 0)
                        return string.Format("{0}%", 100 - decimal.Round((((_max - _completadas) / _max) * 100), 0));
                    else
                        return "0%";
                }
                catch { return "0%"; }
            }
        }
    
        public virtual PEstado PEstado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PTarea> PTareas { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}