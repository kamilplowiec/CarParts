using CarParts.Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Shipment
{
    public class ShipmentModel
    {
        public int? Id { get; set; }

        [Display(Name = "Status zamówienia")]
        public int ShipmentStatus { get; set; }

        /// <summary>
        /// Czy zamówienie jest na podanym statusie
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ShipmentHaveStatus(Enums.ShipmentStatus status)
        {
            return status == (Enums.ShipmentStatus)ShipmentStatus;
        }

        public IEnumerable<SelectListItem> ShipmentStatusList { get; set; }

        [Required]
        [Display(Name = "Data utworzenia")]
        public DateTime CreateDate { get; set; }

        public string CreatedByName { get; set; }

        [Display(Name = "Data odbioru")]
        public DateTime? ReceiptDate { get; set; }

        [Display(Name = "Data dostarczenia")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Ubezpieczenie")]
        public bool Insurance { get; set; }

        [Display(Name = "Koszt transportu")]
        public string DeliveryCost
        {
            get
            {
                return _deliveryCost.ToString();
            }
            set
            {
                decimal p;
                if (decimal.TryParse(value.Replace(".", ","), out p))
                    _deliveryCost = p;
            }
        }
        private decimal _deliveryCost { get; set; }

        [Display(Name = "Numer listu przewozowego")]
        public string ShippingNumber { get; set; }

        public string ShipmentTrackingLink { get; set; }

        [Display(Name = "Dostawca")]
        public string DeliveryName { get; set; }


        [Display(Name = "Waga")]
        public string Weight
        {
            get
            {
                return _weight.ToString();
            }
            set
            {
                decimal p;
                if (decimal.TryParse(value.Replace(".", ","), out p))
                    _weight = p;
            }
        }
        private decimal _weight { get; set; }

        public int Size { get; set; }

        [Display(Name = "Wartość")]
        public string Price
        {
            get
            {
                return _price.ToString();
            }
            set
            {
                decimal p;
                if (decimal.TryParse(value.Replace(".", ","), out p))
                    _price = p;
            }
        }
        private decimal _price { get; set; }

        public int CustomerId { get; set; }

        public CustomerModel Customer { get; set; }

        public int DeliveryId { get; set; }

        public IEnumerable<SelectListItem> DeliveryList { get; set; }


        public int AllProductsInShipment { get; set; }
        public int PackedProductsInShipment { get; set; }
    }
}