using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarParts.Models.Account
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Stare hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Stare hasło *")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nowe hasło")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło *")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Powtórz nowe hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Powtórz nowe hasło *")]
        [Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }
}