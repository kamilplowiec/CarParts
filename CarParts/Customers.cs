//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Customers
    {
        public Customers()
        {
            this.Shipments = new HashSet<Shipments>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string PESEL { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Deleted { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual ICollection<Shipments> Shipments { get; set; }
    }
}
