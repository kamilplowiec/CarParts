using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Product
{
    public class ShipmentProductModel : ProductModel
    {
        new public int? Id { get; set; }
        public int ShipmentId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public bool InShipment { get; set; }

        public string Sum
        {
            get
            {
                return (Count * decimal.Parse(Price)).ToString().Replace(".", ",");
            }
        }
    }
}