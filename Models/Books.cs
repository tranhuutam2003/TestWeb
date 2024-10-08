using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWeb.Models
{
    public class Books
    {
        [Key] 
        public int BookID { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public DateTime PublishedDate { get; set; }
        public int CategoryID { get; set; }
        public decimal? Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }

        [NotMapped] // Không lưu thuộc tính này vào cơ sở dữ liệu
        public IFormFile? ImageFile { get; set; }
        public string? ImageURL { get; set; }

        public Category? Category { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<ShoppingCart>? ShoppingCartItems { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
