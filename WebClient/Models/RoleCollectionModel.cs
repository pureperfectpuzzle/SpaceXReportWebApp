using Data.Enums;
using Microsoft.AspNetCore.Identity;
using WebClient.Utilities;

namespace WebClient.Models
{
    public class RoleCollectionModel
	{
		private readonly IConfiguration _config;
		private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly List<IdentityRole> _roles = new List<IdentityRole>();
		private Pagination? _pagination;

		public RoleCollectionModel(IConfiguration config, UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
			_config = config;
            _userManager = usermanager;
			_roleManager = roleManager;
        }

        public void Initialize(int pageIndex, string sortField,
			SortingDirection sortDirection, string searchString)
        {
			int itemCount = this._roleManager.Roles.Count();
			IEnumerable<IdentityRole> roles = this._roleManager.Roles;

			if (string.IsNullOrEmpty(searchString) == false)
			{
				// filter based on string field(s)
				roles = roles.Where(c => (c.Id?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
										 (c.Name?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault());
			}

			if (string.Compare(sortField, "ID", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					roles = roles.OrderBy(r => r.Id);
				}
				else
				{
					roles = roles.OrderByDescending(r => r.Id);
				}
			}
			else if (string.Compare(sortField, "Name", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					roles = roles.OrderBy(r => r.Name);
				}
				else
				{
					roles = roles.OrderByDescending(r => r.Name);
				}
			}

			this._pagination = new Pagination(_config, itemCount, pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
			IEnumerable<IdentityRole> foundRoles = this._pagination.GetPagedObjects(roles);

			this._roles.Clear();
			this._roles.AddRange(foundRoles);
		}

		public IEnumerable<IdentityRole> Roles => _roles; 
        
        internal Pagination? Pagination => _pagination;
	}
}
