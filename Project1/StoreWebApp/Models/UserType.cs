using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StoreWebApp.Models
{
    public class UserType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}