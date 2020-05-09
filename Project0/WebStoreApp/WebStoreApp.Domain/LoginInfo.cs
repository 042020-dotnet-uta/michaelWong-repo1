using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Domain
{
    public class LoginInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}