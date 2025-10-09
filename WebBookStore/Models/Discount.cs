using System;
using System.ComponentModel.DataAnnotations;

namespace WebBookStore.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Code { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public bool IsPercentage { get; set; }

        [Range(0, 100)]
        public decimal Amount { get; set; }

        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }

        public bool IsActive { get; set; }
    }
}