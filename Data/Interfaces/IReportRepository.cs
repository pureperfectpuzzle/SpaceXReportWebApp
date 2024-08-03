using Data.Objects.Report;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
	public interface IReportRepository : IDisposable
	{
		Task<int> GetReportCountAsync();
		Task<IEnumerable<SpaceXReport>> GetReportsAsync(int pageIndex, string sortField, SortingDirection sortingDirection, string searchString);
		Task<SpaceXReport?> GetSpaceXReportAsync(string id);
		Task AddSpaceXReportAsync(SpaceXReport spaceXReport);
		Task ModifySpaceXReportAsync(SpaceXReport spaceXReport);
		Task DeleteSpaceXReportAsync(SpaceXReport spaceXReport);
		Task<int> GetUserAcountCountAsync();
		Task<IEnumerable<UserAccount>> GetUserAccountsAsync(int pageIndex, string sortField, SortingDirection sortingDirection, string searchString);
		void SeedTestData<T>(IEnumerable<T> data);
	}
}
