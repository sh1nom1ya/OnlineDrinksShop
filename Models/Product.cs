using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace drunkShop.Models
{
	public class Product
	{

		[Key]
		public int Id { get; set; }

		[Required]
		public string? Naming { get; set; }

		public string? Description { get; set; }

		[Required]
		[Range(50, 800, ErrorMessage = "Неправильно задано значение")]
		public int Volume { get; set; }

		[Required]
		[Range(1, 15000, ErrorMessage = "Неправильно задано значение")]
		public double Price { get; set; }

		public string? Image { get; set; }

		[Display(Name = "Category Type")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
    }
}

