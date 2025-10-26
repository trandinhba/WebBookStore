using System.ComponentModel.DataAnnotations;

namespace WebBookStore.ViewModels
{
    /// <summary>
    /// ViewModel cho trang đăng nhập - chứa dữ liệu form đăng nhập
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email hoặc tên đăng nhập không được để trống")]
        [Display(Name = "Email hoặc Tên đăng nhập")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }

        // URL để redirect sau khi đăng nhập thành công
        public string ReturnUrl { get; set; }
    }
}