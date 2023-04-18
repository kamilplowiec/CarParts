using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CarParts.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}