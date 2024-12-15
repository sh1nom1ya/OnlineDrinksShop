using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace drunkShop.Models
{
    public class AppOrderProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppOrderId { get; set; }
        [ForeignKey("AppOrderId")]
        public virtual AppOrder AppOrder { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Неправильное количество")]
        public int Quantity { get; set; }
    }
}