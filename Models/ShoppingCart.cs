using System.ComponentModel.DataAnnotations;

namespace TestWeb.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        public string PhoneNumber { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }

        public User User { get; set; }
        public Books Book { get; set; }
    }


}
