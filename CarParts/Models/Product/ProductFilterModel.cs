using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Product
{
    public class ProductFilterModel
    {
        public string FilterName { get; set; }

        public string FilterDetails { get; set; }
    }
}