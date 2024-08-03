using Data.Objects.Report;

namespace WebClient.Models
{
	public class SpaceXReportModel
	{
		public SpaceXReport? Report { get; set; }

		public string Action { get; set; } = "Create";

		public bool ShowAction { get; set; } = true;

		public string Title { get; set; } = "Create new report";

		public bool IsReadOnly { get; set; } = false;
	}
}
