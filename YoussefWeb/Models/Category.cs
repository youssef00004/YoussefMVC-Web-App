using System.ComponentModel.DataAnnotations;

namespace YoussefWeb.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
