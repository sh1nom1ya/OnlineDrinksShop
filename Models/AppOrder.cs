using System;
using System.ComponentModel.DataAnnotations;

namespace drunkShop.Models
{
    public class AppOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

    }
}