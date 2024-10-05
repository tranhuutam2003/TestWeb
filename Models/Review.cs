using System.ComponentModel.DataAnnotations;

namespace TestWeb.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public int BookID { get; set; }
        public string? PhoneNumber { get; set; }
        public int Rating { get; set; }  // Rating phải nằm trong khoảng 1 đến 5
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public Books? Book { get; set; }
        public User? User { get; set; }
    }


}
