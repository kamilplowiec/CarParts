using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CarParts.Models.Account
{
    public class RegisterModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }

        public bool ChangePassword { get; set; }

        [Display(Name = "Wybierz rolę użytkownika")]
        [Range(1, 3, ErrorMessage = "Wybierz rolę użytkownika")]
        public int AccountRole { get; set; }
    }
}