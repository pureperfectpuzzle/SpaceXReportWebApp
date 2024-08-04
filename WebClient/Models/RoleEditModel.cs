using Microsoft.AspNetCore.Identity;

namespace WebClient.Models
{
	public class RoleEditModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IdentityRole _role;
		private List<IdentityUser> _members = new List<IdentityUser>();

		public RoleEditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityRole role)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_role = role;
		}

		public IdentityRole Role => _role;

		public async Task LoadMembersAsync()
		{
			if (_role != null && string.IsNullOrEmpty(_role.Name) == false)
			{
				_members.AddRange(await _userManager.GetUsersInRoleAsync(_role.Name));
			}
		}

		public IList<IdentityUser> Members() => _members;

		public IEnumerable<IdentityUser> NonMembers() => _userManager.Users.ToList().Except(_members);
	}
}
