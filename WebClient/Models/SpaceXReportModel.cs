using Data.Objects.Report;
using Data.Objects.SpaceX;
using Microsoft.AspNetCore.Identity;

namespace WebClient.Models
{
	public class SpaceXReportModel
	{
		public SpaceXReport? Report { get; set; }

		public string Action { get; set; } = "Create";

		public bool ShowAction { get; set; } = true;

		public string Title { get; set; } = "Create new report";

		public bool IsReadOnly { get; set; } = false;

		public IEnumerable<IdentityUser> Users { get; set; } = Array.Empty<IdentityUser>();

		public IEnumerable<Launch> Launches { get; set; } = Array.Empty<Launch>();
	}
}
