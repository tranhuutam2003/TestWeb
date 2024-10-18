using System.ComponentModel.DataAnnotations;

namespace TestWeb.Models
{
    public class User
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }

        [Key]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string? Password { get; set; }
        public int Role { get; set; }  // 0: Khách hàng, 1: Admin

        public ICollection<Order>? Orders { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}