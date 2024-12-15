using System;
namespace drunkShop.Models
{
	public class ShoppingCart
	{
		public int ProductId { get; set; }

		public int Quantity { get; set; }

		public double TotalPrice { get; set; }
    }
}

