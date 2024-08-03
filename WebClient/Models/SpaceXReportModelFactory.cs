using Data.Objects.Report;

namespace WebClient.Models
{
	internal static class SpaceXReportModelFactory
	{
		public static SpaceXReportModel View(SpaceXReport report)
		{
			return new SpaceXReportModel
			{
				Report = report,
				Action = "View",
				ShowAction = false,
				Title = "Report details",
				IsReadOnly = true,
			};
		}

		public static SpaceXReportModel Create(SpaceXReport report)
		{
			return new SpaceXReportModel
			{
				Report = report,
			};
		}

		public static SpaceXReportModel Modify(SpaceXReport report)
		{
			return new SpaceXReportModel
			{
				Report = report,
				Action = "Modify",
				Title = "Modify report",
			};
		}

		public static SpaceXReportModel Delete(SpaceXReport report)
		{
			return new SpaceXReportModel
			{
				Report = report,
				Action = "Delete",
				Title = "Delete report",
			};
		}
	}
}
