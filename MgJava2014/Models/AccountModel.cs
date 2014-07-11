using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MgJava2014.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Моля въведете потребителско име")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Моля въведете парола")]
        [Display(Name = "Парола")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "Потребителско име")]
        [Required(ErrorMessage = "Моля въведете потребителско име")]
        public string Username { get; set; }
        
        [Required(ErrorMessage="Моля въведете email")]
        [EmailAddress(ErrorMessage="Невалиден email")]
        public string Email { get; set; }   

        [Display(Name="Парола")]
        [Required(ErrorMessage = "Моля въведете парола")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Минималната дължина на паролата е 3 символа.")]
        public string Password { get; set; }
    }
}