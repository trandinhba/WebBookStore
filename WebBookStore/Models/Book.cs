using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookStore.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(100)]
        public string Publisher { get; set; }

        public int? PublishYear { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        public int? PageCount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }

        public Book()
        {
            Reviews = new HashSet<Review>();
            OrderItems = new HashSet<OrderItem>();
            CartItems = new HashSet<CartItem>();
            Wishlists = new HashSet<Wishlist>();
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return 0;

                double sum = 0;
                foreach (var review in Reviews)
                {
                    sum += review.Rating;
                }
                return Math.Round(sum / Reviews.Count, 1);
            }
        }

        [NotMapped]
        public int ReviewCount
        {
            get { return Reviews?.Count ?? 0; }
        }

        [NotMapped]
        public decimal FinalPrice
        {
            get { return DiscountPrice ?? Price; }
        }
    }
}