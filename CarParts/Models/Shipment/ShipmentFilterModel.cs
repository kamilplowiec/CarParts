using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Shipment
{
    public class ShipmentFilterModel
    {
        public int FilterShipmentStatus { get; set; }

        public DateTime FilterCreateDate { get; set; }

        public string FilterShippingNumber { get; set; }

        public string FilterCustomer { get; set; }
    }
}