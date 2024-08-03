using Data.Enums;
using Data.Interfaces;
using Data.Objects.Report;
using WebClient.Utilities;

namespace WebClient.Models
{
	public class UserAccountCollectionModel
	{
		private readonly IConfiguration _config;
		private readonly IReportRepository _reportRepository;
		private readonly List<UserAccount> _userAccounts = new List<UserAccount>();
		private Pagination? _pagination;

		public UserAccountCollectionModel(IConfiguration config, IReportRepository reportRepository)
		{
			this._config = config;
			this._reportRepository = reportRepository;
		}

		public async Task InitializeAsync(int pageIndex, string sortField,
			SortingDirection sortDirection, string searchString)
		{
			int itemCount = await this._reportRepository.GetUserAcountCountAsync();

			IEnumerable<UserAccount> userAccounts = await this._reportRepository.GetUserAccountsAsync(pageIndex,
				sortField, sortDirection, searchString);
			this._userAccounts.Clear();
			this._userAccounts.AddRange(userAccounts);

			this._pagination = new Pagination(_config, itemCount, pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
		}

		public IEnumerable<UserAccount> UserAccounts => _userAccounts;

		internal Pagination? Pagination => _pagination;
	}
}
