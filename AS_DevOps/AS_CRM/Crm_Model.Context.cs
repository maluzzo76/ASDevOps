﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AS_CRMEntities : DbContext
    {
        public AS_CRMEntities()
            : base("name=AS_CRMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Comprobante> Comprobantes { get; set; }
        public virtual DbSet<GastosFijo> GastosFijos { get; set; }
        public virtual DbSet<GastosVariable> GastosVariables { get; set; }
        public virtual DbSet<TipoGasto> TipoGastoes { get; set; }
        public virtual DbSet<TiposComprobante> TiposComprobantes { get; set; }
        public virtual DbSet<AcuardosComerciale> AcuardosComerciales { get; set; }
    }
}
