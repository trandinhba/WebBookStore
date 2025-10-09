using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookStore.Models
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        public DateTime AddedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        public Wishlist()
        {
            AddedDate = DateTime.Now;
        }
    }
}