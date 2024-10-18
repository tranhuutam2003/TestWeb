using System.ComponentModel.DataAnnotations;
using TestWeb.Models;
namespace TestWeb.Models
{
    public class User
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com.")]
        public string? Email { get; set; }
        [Key]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải đủ 10 số và bắt đầu bằng 0.")]
        public string? PhoneNumber { get; set; }  // Primary key
        public string? Address { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        public string? Password { get; set; }
        public int Role { get; set; }  // 0: Khách hàng, 1: Admin
        public int MaUser { get; set; } = 0;  // Mặc định là 0 cho khách hàng

        public ICollection<Order>? Orders { get; set; }
        public ICollection<ShoppingCart>? ShoppingCartItems { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }


}