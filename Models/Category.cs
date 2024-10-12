using System.ComponentModel.DataAnnotations;

namespace TestWeb.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Books> Books { get; set; }
    }


}
