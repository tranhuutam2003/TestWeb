using TestWeb.Models;

namespace TestWeb.ViewModel
{
    public class ProductSearchViewModel
    {
        public string Query { get; set; }
        public List<Product> Products { get; set; }
    }
}
