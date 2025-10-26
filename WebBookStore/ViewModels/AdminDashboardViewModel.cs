using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBookStore.ViewModels
{
    /// <summary>
    /// ViewModel cho Admin Dashboard - chứa dữ liệu hiển thị trên trang quản lý
    /// </summary>
    public class AdminDashboardViewModel
    {
        // Thống kê tổng quan
        public DashboardStats Stats { get; set; }
        
        // Danh sách yêu cầu chưa giải quyết
        public RequestStats Requests { get; set; }
        
        // Công việc hôm nay
        public List<TaskItem> TodayTasks { get; set; }
        
        // Thông tin user hiện tại
        public UserInfo CurrentUser { get; set; }
        
        // Danh sách sách mới nhất
        public List<BookSummary> RecentBooks { get; set; }
        
        // Danh sách đơn hàng chờ xử lý
        public List<OrderSummary> PendingOrders { get; set; }
    }

    /// <summary>
    /// Thống kê dashboard
    /// </summary>
    public class DashboardStats
    {
        public int Unresolved { get; set; }
        public int Overdue { get; set; }
        public int InProgress { get; set; }
        public int PendingApproval { get; set; }
    }

    /// <summary>
    /// Thống kê yêu cầu
    /// </summary>
    public class RequestStats
    {
        public int PendingResolution { get; set; }
        public int CustomerBugReports { get; set; }
        public int WaitingForDevFix { get; set; }
        public int PendingApproval { get; set; }
    }

    /// <summary>
    /// Mục công việc
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
        public string Type { get; set; } // "radio" hoặc "checkbox"
        public string Tag { get; set; }
    }

    /// <summary>
    /// Thông tin user
    /// </summary>
    public class UserInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Tóm tắt sách
    /// </summary>
    public class BookSummary
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }

    /// <summary>
    /// Tóm tắt đơn hàng
    /// </summary>
    public class OrderSummary
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}