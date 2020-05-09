using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        [Required]
        public Guid LoginInfoId { get; set; }
        public LoginInfo LoginInfo { get; set; }
    }
}
