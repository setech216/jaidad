﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class propertyDbEntities : DbContext
    {
        public propertyDbEntities()
            : base("name=propertyDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<block> block { get; set; }
        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<inquiry> inquiry { get; set; }
        public virtual DbSet<phase> phase { get; set; }
        public virtual DbSet<property> property { get; set; }
        public virtual DbSet<property_type> property_type { get; set; }
        public virtual DbSet<society> society { get; set; }
        public virtual DbSet<userRole> userRole { get; set; }
    }
}