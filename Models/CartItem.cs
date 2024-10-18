namespace TestWeb.Models
{

    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public Books? Book { get; set; }
        public int Quantity { get; set; }
    }
}