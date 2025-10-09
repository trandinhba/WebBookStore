using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookStore.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                var unit = Book != null ? Book.FinalPrice : 0;
                return unit * Quantity;
            }
        }
    }
}