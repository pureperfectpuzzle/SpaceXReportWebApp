using Data.Interfaces;
using Data.Objects.SpaceX;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using WebClient.Utilities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Pages.SpaceX
{
	[Authorize]
    public class CapsulesModel : PageModel
    {
		private readonly IConfiguration config;
		private readonly ISpaceXRepository spaceXRepository;
		private List<Capsule> _capsules = new List<Capsule>();
		private Pagination? _pagination;

		public CapsulesModel(ISpaceXRepository repository, IConfiguration config)
		{
			this.config = config;
			this.spaceXRepository = repository;
		}

		public IEnumerable<Capsule> Capsules => _capsules;

		internal Pagination? Pagination => _pagination;

		public async Task<ActionResult> OnGetAsync(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
		{
			IEnumerable<Capsule> capsules = await this.spaceXRepository.GetCapsulesAsync();

			if (string.IsNullOrEmpty(searchString) == false)
			{
				// filter based on string field(s)
				capsules = capsules.Where(c => (c.Id?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											   (c.Type?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault() ||
											   (c.Status?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault());
			}

			if (string.Compare(sortField, "ID", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					capsules = capsules.OrderBy(c => c.Id);
				}
				else
				{
					capsules = capsules.OrderByDescending(c => c.Id);
				}
			}
			else if (string.Compare(sortField, "Type", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					capsules = capsules.OrderBy(c => c.Type);
				}
				else
				{
					capsules = capsules.OrderByDescending(c => c.Type);
				}
			}
			else if (string.Compare(sortField, "Status", true) == 0)
			{
				if (sortDirection == SortingDirection.Ascending)
				{
					capsules = capsules.OrderBy(c => c.Status);
				}
				else
				{
					capsules = capsules.OrderByDescending(c => c.Status);
				}
			}

			this._pagination = new Pagination(config, capsules.Count(), pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
			IEnumerable<Capsule> foundCapsules = this._pagination.GetPagedObjects(capsules);

			this._capsules.Clear();
			this._capsules.AddRange(foundCapsules);
			return Page();
		}
	}
}
