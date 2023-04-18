using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Customer
{
    public class CustomerFilterModel
    {
        public string FilterName { get; set; }

        public string FilterNIP { get; set; }
        public string FilterREGON { get; set; }
        public string FilterPESEL { get; set; }
        public string FilterEmail { get; set; }
        public string FilterPhone { get; set; }
        public string FilterAddress { get; set; }
    }
}