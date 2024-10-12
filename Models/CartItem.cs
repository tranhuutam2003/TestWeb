namespace TestWeb.Models
{

    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }  // Sản phẩm trong giỏ hàng
        public int Quantity { get; set; }
    }
}
