using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Product
{
    public class ProductModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Cechy")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Ilość w magazynie")]
        public int QuantityInWarehouse { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Pole Waga 1 sztuki jest wymagane")]
        [Display(Name = "Waga 1 sztuki")]
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

        [Required]
        [MinLength(1, ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Cena")]
        public string Price 
        { 
            get
            {
                return _price.ToString();
            }
            set
            {
                decimal p;
                if(decimal.TryParse(value.Replace(".", ","), out p))
                    _price = p;
            }
        }
        private decimal _price { get; set; }
    }
}