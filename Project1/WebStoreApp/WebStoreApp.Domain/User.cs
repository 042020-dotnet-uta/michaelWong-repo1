using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebStoreApp.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserTypeId { get; set; }
        public UserType UserType { get; set; }

        [Required]
        public UserInfo UserInfo { get; set; }

        [Required]
        public LoginInfo LoginInfo { get; set; }

        // public ICollection<Order> Orders { get; set; }
    }
}
