using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Customer
{
    public class CustomerModel
    {
        public int? Id { get; set; }

        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Display(Name = "NIP")]
        public string NIP { get; set; }

        [Display(Name = "REGON")]
        public string REGON { get; set; }

        [Display(Name = "PESEL")]
        public string PESEL { get; set; }

        [Required(ErrorMessage ="Pole {0} jest wymagane")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }


        public AddressModel Address { get; set; } = new AddressModel();
    }

    public class AddressModel
    {
        public int? AddressId { get; set; }

        [Required(ErrorMessage ="Pole {0} jest wymagane")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage ="Pole {0} jest wymagane")]
        [Display(Name = "Nr budynku")]
        public string HouseNo { get; set; }

        [Display(Name = "Nr lokalu")]
        public string FlatNo { get; set; }

        [Required(ErrorMessage ="Pole {0} jest wymagane")]
        [Display(Name = "Kod pocztowy")]
        public string PostCode { get; set; }

        [Required(ErrorMessage ="Pole {0} jest wymagane")]
        [Display(Name = "Miejscowość")]
        public string Place { get; set; }

        public string Data
        {
            get
            {
                return string.Format("{0} {1}{2}, {3} {4}", Street, HouseNo, (string.IsNullOrEmpty(FlatNo) ? "" : "/" + FlatNo), PostCode, Place);
            }
        }
    }
}