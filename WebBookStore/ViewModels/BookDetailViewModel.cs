using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBookStore.ViewModels
{
    /// <summary>
    /// ViewModel cho trang chi tiết sách - chứa thông tin sách và các sản phẩm liên quan
    /// </summary>
    public class BookDetailViewModel
    {
        // Thông tin sách chính
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }

        // Số lượng mua
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; } = 1;

        // Đánh giá và review
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<BookReview> Reviews { get; set; }

        // Sách liên quan
        public List<RelatedBook> RelatedBooks { get; set; }

        // Sách cùng tác giả
        public List<RelatedBook> SameAuthorBooks { get; set; }

        // Sách cùng thể loại
        public List<RelatedBook> SameCategoryBooks { get; set; }

        // Trạng thái yêu thích
        public bool IsInWishlist { get; set; }

        // Trạng thái trong giỏ hàng
        public bool IsInCart { get; set; }
        public int CartQuantity { get; set; }
    }

    /// <summary>
    /// Review sách
    /// </summary>
    public class BookReview
    {
        public int ReviewId { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsVerified { get; set; }
    }

    /// <summary>
    /// Sách liên quan
    /// </summary>
    public class RelatedBook
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}