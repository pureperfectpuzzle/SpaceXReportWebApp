using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enums
{
	/// <summary>
	/// The type of user accounts
	/// </summary>
	public enum UserAccountType : Int32
	{
		/// <summary>
		/// Disabled user account
		/// </summary>
		Disabled = 0,

		/// <summary>
		/// User account with limited privilege
		/// </summary>
		Regular = 1,

		/// <summary>
		/// User account with highest privilege
		/// </summary>
		Administrator = 2,
	}
}
