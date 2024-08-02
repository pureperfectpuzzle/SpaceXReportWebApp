using Data.Interfaces;
using Data.Objects.SpaceX;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Utilities.Enums;
using WebClient.Utilities;

namespace WebClient.Pages.SpaceX
{
    public class UpcomingLaunchesModel : PageModel
    {
		private readonly IConfiguration config;
		private readonly ISpaceXRepository spaceXRepository;
		private List<Launch> _launches = new List<Launch>();
		private Pagination? _pagination;

		public UpcomingLaunchesModel(ISpaceXRepository repository, IConfiguration config)
		{
			this.config = config;
			this.spaceXRepository = repository;
		}

		public IEnumerable<Launch> Launches => _launches;

		internal Pagination? Pagination => _pagination;

		internal SortingDirection GetNextSortingDirection(string fieldName)
		{
			if (string.Compare(fieldName, this.Pagination?.SortingField ?? "ID", true) == 0)
			{
				return this.Pagination?.SortingDirection == SortingDirection.Ascending ? SortingDirection.Descending : SortingDirection.Ascending;
			}
			else
			{
				return SortingDirection.Ascending;
			}
		}

		public async Task<ActionResult> OnGetAsync(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
		{
			IEnumerable<Launch> launches = await this.spaceXRepository.GetUpcomingLaunchesAsync();

			if (string.IsNullOrEmpty(searchString) == false)
			{
				// filter based on string field(s)
				launches = launches.Where(l => (l.Id?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											   (l.Launch_Year?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											   (l.Mission_Name?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault());
			}

			if (string.Compare(sortField, "ID", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launches = launches.OrderBy(l => l.Id);
				}
				else
				{
					launches = launches.OrderByDescending(l => l.Id);
				}
			}
			else if (string.Compare(sortField, "LaunchYear", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launches = launches.OrderBy(l => l.Launch_Year);
				}
				else
				{
					launches = launches.OrderByDescending(l => l.Launch_Year);
				}
			}
			else if (string.Compare(sortField, "MissionName", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launches = launches.OrderBy(l => l.Mission_Name);
				}
				else
				{
					launches = launches.OrderByDescending(l => l.Mission_Name);
				}
			}

			this._pagination = new Pagination(config, launches.Count(), pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
			IEnumerable<Launch> foundLaunches = this._pagination.GetPagedObjects(launches);

			this._launches.Clear();
			this._launches.AddRange(foundLaunches);
			return Page();
		}
	}
}
