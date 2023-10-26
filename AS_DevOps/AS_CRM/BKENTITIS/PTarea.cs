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

    public partial class PTarea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PTarea()
        {
            this.PParteHoras = new HashSet<PParteHora>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario_Id { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> Estado_Id { get; set; }
        [Display(Name = "Objetivo")]
        public Nullable<int> Objetivo_Id { get; set; }
        [Display(Name = "Sprint")]
        public Nullable<int> Sprint_Id { get; set; }
        [Display(Name = "Fecha Incio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaIncio { get; set; }
        [Display(Name = "Ficha Finalizada")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaFinalizado { get; set; }
        [Display(Name = "Fecha de Entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaEntrega { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual PEstado PEstado { get; set; }
        public virtual PObjetivo PObjetivo { get; set; }
        public virtual PSprint PSprint { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PParteHora> PParteHoras { get; set; }
    }
}