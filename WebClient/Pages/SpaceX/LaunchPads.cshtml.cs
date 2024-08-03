using Data.Interfaces;
using Data.Objects.SpaceX;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Utilities;
using Data.Enums;

namespace WebClient.Pages.SpaceX
{
    public class LaunchPadsModel : PageModel
    {
		private readonly IConfiguration config;
		private readonly ISpaceXRepository spaceXRepository;
        private List<LaunchPad> _launchPads = new List<LaunchPad>();
		private Pagination? _pagination;

		public LaunchPadsModel(ISpaceXRepository repository, IConfiguration config)
        {
			this.config = config;
			this.spaceXRepository = repository;
        }

        public IEnumerable<LaunchPad> LaunchPads => _launchPads;

		internal Pagination? Pagination => _pagination;

		public async Task<ActionResult> OnGetAsync(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
        {
			IEnumerable<LaunchPad> launchPads = await this.spaceXRepository.GetLaunchPadsAsync();

			if (string.IsNullOrEmpty(searchString) == false)
			{
				// filter based on string field(s)
				launchPads = launchPads.Where(l => (l.Id?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											       (l.Name?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											       (l.Status?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
												   (l.Wikipedia?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault());
			}

			if (string.Compare(sortField, "ID", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launchPads = launchPads.OrderBy(l => l.Id);
				}
				else
				{
					launchPads = launchPads.OrderByDescending(l => l.Id);
				}
			}
			else if (string.Compare(sortField, "Name", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launchPads = launchPads.OrderBy(l => l.Name);
				}
				else
				{
					launchPads = launchPads.OrderByDescending(l => l.Name);
				}
			}
			else if (string.Compare(sortField, "Status", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launchPads = launchPads.OrderBy(l => l.Status);
				}
				else
				{
					launchPads = launchPads.OrderByDescending(l => l.Status);
				}
			}
			else if (string.Compare(sortField, "Wikipedia", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					launchPads = launchPads.OrderBy(l => l.Wikipedia);
				}
				else
				{
					launchPads = launchPads.OrderByDescending(l => l.Wikipedia);
				}
			}

			this._pagination = new Pagination(config, launchPads.Count(), pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
			IEnumerable<LaunchPad> foundLaunchPads = this._pagination.GetPagedObjects(launchPads);

			this._launchPads.Clear();
            this._launchPads.AddRange(foundLaunchPads);
            return Page();            
        }
    }
}
