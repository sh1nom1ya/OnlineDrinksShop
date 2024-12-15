using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace drunkShop.Models
{
    public class Category
    {
        public Category()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string? Naming { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int DisplayOrder { get; set; }
    }
}