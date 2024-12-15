using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace drunkShop.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Naming { get; set; }
    }
}