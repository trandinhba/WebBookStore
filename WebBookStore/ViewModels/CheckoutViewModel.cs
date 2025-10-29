using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBookStore.ViewModels
{
    /// <summary>
    /// ViewModel cho trang thanh toán - chứa thông tin đơn hàng và thanh toán
    /// </summary>
    public class CheckoutViewModel
    {
        // Thông tin khách hàng
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        // Phương thức thanh toán
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        // Thông tin giỏ hàng
        public List<CartItem> CartItems { get; set; }

        // Tổng tiền
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Mã giảm giá
        [Display(Name = "Mã giảm giá")]
        public string CouponCode { get; set; }

        // Điều khoản
        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản")]
        [Display(Name = "Đồng ý với điều khoản")]
        public bool AgreeToTerms { get; set; }
    }

    /// <summary>
    /// Mục trong giỏ hàng
    /// </summary>
    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Phương thức thanh toán
    /// </summary>
    public class PaymentMethod
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}