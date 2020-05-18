using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"\A[A-Za-z][A-Za-z0-9]+\Z", ErrorMessage = "Invalid username format. Username must consist of only letters and numbers and begin with a letter.")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*\d+)(?=.*[a-zA-Z])[0-9a-zA-Z!@#$%]{8,50}$", ErrorMessage = "Password must be at least 8 and a maximum of 50 characters. Password must contain a letter and a number. Password can only letters, numbers and the special characters !@#$%")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}