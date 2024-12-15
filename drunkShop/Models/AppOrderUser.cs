using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace drunkShop.Models
{
    public class AppOrderUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppOrderId { get; set; }
        [ForeignKey("AppOrderId")]
        public virtual AppOrder AppOrder { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}