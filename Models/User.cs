using System.ComponentModel.DataAnnotations;
using TestWeb.Models;
namespace TestWeb.Models
{
    public class User
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        [Key]
        public string PhoneNumber { get; set; }  // Primary key
        public string Address { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }  // 0: Khách hàng, 1: Admin
        public int MaUser { get; set; } = 0;  // Mặc định là 0 cho khách hàng

        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCartItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }


}
