﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Ten kod źródłowy został wygenerowany na podstawie szablonu.
//
//    Ręczne modyfikacje tego pliku mogą spowodować nieoczekiwane zachowanie aplikacji.
//    Ręczne modyfikacje tego pliku zostaną zastąpione w przypadku ponownego wygenerowania kodu.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarParts
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarPartsEntities : DbContext
    {
        public CarPartsEntities()
            : base("name=CarPartsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Shipments> Shipments { get; set; }
        public DbSet<ShipmentProducts> ShipmentProducts { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<ShipmentStatuses> ShipmentStatuses { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Customers> Customers { get; set; }
    }
}
