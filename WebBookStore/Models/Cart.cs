using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebBookStore.Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new HashSet<CartItem>();
            CreatedDate = DateTime.Now;
        }

        [NotMapped]
        public decimal TotalAmount
        {
            get
            {
                return CartItems?.Sum(item => item.TotalPrice) ?? 0;
            }
        }

        [NotMapped]
        public int TotalItems
        {
            get
            {
                return CartItems?.Count ?? 0;
            }
        }

        [NotMapped]
        public int TotalQuantity
        {
            get
            {
                return CartItems?.Sum(item => item.Quantity) ?? 0;
            }
        }
    }
}