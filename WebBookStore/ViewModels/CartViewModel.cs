using System.Collections.Generic;
using WebBookStore.Models;

namespace WebBookStore.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }

        public CartViewModel()
        {
            Items = new List<CartItemViewModel>();
        }
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

