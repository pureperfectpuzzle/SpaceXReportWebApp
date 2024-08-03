using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects.Report
{
	public class UserAccount
	{
		public Guid Id { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

		public string? Password { get; set; }

		public DateTime DateOfCreation { get; set; }

		public int AccountType { get; set; }
	}
}
