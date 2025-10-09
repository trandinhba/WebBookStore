using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookStore.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        [StringLength(20)]
        public string Role { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        public User()
        {
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
            Carts = new HashSet<Cart>();
            CreatedDate = DateTime.Now;
            IsActive = true;
            Role = "Customer";
        }
    }
}