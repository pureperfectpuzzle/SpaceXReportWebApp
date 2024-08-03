using Data.Interfaces;
using Data.Objects.Report;
using Data.Objects.SpaceX;
using WebClient.Pages;
using WebClient.Utilities;
using Data.Enums;

namespace WebClient.Models
{
	public class ReportCollectionModel
	{
		private readonly IConfiguration _config;
		private readonly IReportRepository _reportRepository;
		private readonly List<SpaceXReport> _reports = new List<SpaceXReport>();
		private Pagination? _pagination;

		public ReportCollectionModel(IConfiguration config, IReportRepository reportRepository)
		{
			this._config = config;
			this._reportRepository = reportRepository;
		}

		public async Task InitializeAsync(int pageIndex, string sortField, 
			SortingDirection sortDirection, string searchString)
		{
			int itemCount = await this._reportRepository.GetReportCountAsync();

			IEnumerable<SpaceXReport> reports = await this._reportRepository.GetReportsAsync(pageIndex, 
				sortField, sortDirection, searchString);
			this._reports.Clear();
			this._reports.AddRange(reports);

			this._pagination = new Pagination(_config, itemCount, pageIndex)
			{
				SearchString = searchString,
				SortingField = sortField,
				SortingDirection = sortDirection,
			};
		}

		public IEnumerable<SpaceXReport> Reports => _reports;

		internal Pagination? Pagination => _pagination;
	}
}
