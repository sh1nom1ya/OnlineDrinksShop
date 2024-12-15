using System;
using Microsoft.AspNetCore.Identity;

namespace drunkShop.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
	}
}

