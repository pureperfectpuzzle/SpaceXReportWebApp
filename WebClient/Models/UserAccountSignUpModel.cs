﻿using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
	public class UserAccountSignUpModel
	{
		[Required]
		public string UserName { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;
	}
}
