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
    
    public partial class Products
    {
        public Products()
        {
            this.ShipmentProducts = new HashSet<ShipmentProducts>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int QuantityInWarehouse { get; set; }
        public decimal Price { get; set; }
        public bool Deleted { get; set; }
        public decimal Weight { get; set; }
    
        public virtual ICollection<ShipmentProducts> ShipmentProducts { get; set; }
    }
}