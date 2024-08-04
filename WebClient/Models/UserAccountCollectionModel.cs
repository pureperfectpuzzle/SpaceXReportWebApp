using Data.Enums;
using Data.Interfaces;
using Data.Objects.Report;
using Data.Objects.SpaceX;
using Microsoft.AspNetCore.Identity;
using WebClient.Utilities;

namespace WebClient.Models
{
	public class UserAccountCollectionModel
	{
		private readonly IConfiguration _config;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly List<IdentityUser> _userAccounts = new List<IdentityUser>();
		private Pagination? _pagination;

		public UserAccountCollectionModel(IConfiguration config, UserManager<IdentityUser> userManager)
		{
			this._config = config;
			this._userManager = userManager;
		}

		public void Initialize(int pageIndex, string sortField,
			SortingDirection sortDirection, string searchString)
		{
			int itemCount = this._userManager.Users.Count();
            IEnumerable<IdentityUser> userAccounts = this._userManager.Users;

            if (string.IsNullOrEmpty(searchString) == false)
            {
                // filter based on string field(s)
                userAccounts = userAccounts.Where(c => (c.Id?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
                                                 (c.UserName?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
                                                 (c.Email?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault());
            }

            if (string.Compare(sortField, "ID", true) == 0)
            {
                if (sortDirection == SortingDirection.Ascending)
                {
                    userAccounts = userAccounts.OrderBy(u => u.Id);
                }
                else
                {
                    userAccounts = userAccounts.OrderByDescending(u => u.Id);
                }
            }
            else if (string.Compare(sortField, "Name", true) == 0)
            {
                if (sortDirection == SortingDirection.Ascending)
                {
                    userAccounts = userAccounts.OrderBy(u => u.UserName);
                }
                else
                {
                    userAccounts = userAccounts.OrderByDescending(u => u.UserName);
                }
            }
            else if (string.Compare(sortField, "Email", true) == 0)
            {
                if (sortDirection == SortingDirection.Ascending)
                {
                    userAccounts = userAccounts.OrderBy(u => u.Email);
                }
                else
                {
                    userAccounts = userAccounts.OrderByDescending(u => u.Email);
                }
            }

			this._pagination = new Pagination(_config, itemCount, pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
            IEnumerable<IdentityUser> foundUserAccounts = this._pagination.GetPagedObjects(userAccounts);

            this._userAccounts.Clear();
			this._userAccounts.AddRange(foundUserAccounts);
		}

		public IEnumerable<IdentityUser> UserAccounts => _userAccounts;

		internal Pagination? Pagination => _pagination;
	}
}
