using System;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApp.Web.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UsernameLogin { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string PasswordLogin { get; set; }
    }
}