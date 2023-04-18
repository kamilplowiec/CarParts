using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Delivery
{
    public class DeliveryModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string PriceForWeight
        {
            get
            {
                return _priceForWeight.ToString();
            }
            set
            {
                decimal p;
                if (decimal.TryParse(value.Replace(".", ","), out p))
                    _priceForWeight = p;
            }
        }
        private decimal _priceForWeight { get; set; }

        public int InsurancePercentCost { get; set; }

        public string TrackUrl { get; set; }
    }
}