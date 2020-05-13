using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreApp.Domain
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public OrderInfo OrderInfo { get; set; }
    }
}