using Data.Interfaces;
using Data.Objects.Report;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Data.Enums;
using System.Net.Http.Headers;

namespace WebClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IReportRepository _reportRepository;

        public AdminController(IConfiguration config, IReportRepository reportRepository)
        {
            this._config = config;
            this._reportRepository = reportRepository;
        }

        public async Task<IActionResult> SpaceXReports(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
        {
            ReportCollectionModel model = new ReportCollectionModel(_config, _reportRepository);
            await model.InitializeAsync(pageIndex, sortField, sortDirection, searchString);
            return View(model);
        }

        public async Task<IActionResult> ViewReport(string id)
        {
            SpaceXReport? report = await this._reportRepository.GetSpaceXReportAsync(id);
            if (report != null)
            {
                SpaceXReportModel model = SpaceXReportModelFactory.View(report);
                return View("SpaceXReportEditor", model);                
            }
            else
            {
                return NotFound();
            }
        }

		public IActionResult CreateReport()
		{
            SpaceXReport report = new SpaceXReport();
			SpaceXReportModel model = SpaceXReportModelFactory.Create(report);
			return View("SpaceXReportEditor", model);
		}

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]SpaceXReport report)
        {
            if (ModelState.IsValid)
            {
                report.Id = Guid.NewGuid();
                await this._reportRepository.AddSpaceXReportAsync(report);
                return RedirectToAction(nameof(SpaceXReports));
            }
            else
            {
				SpaceXReportModel model = SpaceXReportModelFactory.Create(report);
			    return View("SpaceXReportEditor", model);
			}
        }

		public async Task<IActionResult> ModifyReport(string id)
		{
			SpaceXReport? report = await this._reportRepository.GetSpaceXReportAsync(id);
            if (report != null)
            {
			    SpaceXReportModel model = SpaceXReportModelFactory.Modify(report);
			    return View("SpaceXReportEditor", model);
            }
            else
            {
                return NotFound();
            }
		}

        [HttpPost]
        public async Task<IActionResult> Modify([FromForm]SpaceXReport report)
        {
            if (ModelState.IsValid)
            {
                await this._reportRepository.ModifySpaceXReportAsync(report);
                return RedirectToAction(nameof(SpaceXReports));
            }
            else
            {
				SpaceXReportModel model = SpaceXReportModelFactory.Modify(report);
				return View("SpaceXReportEditor", model);
			}
		}

        public async Task<IActionResult> DeleteReport(string id)
		{
			SpaceXReport? report = await this._reportRepository.GetSpaceXReportAsync(id);
			if (report != null)
			{
				SpaceXReportModel model = SpaceXReportModelFactory.Delete(report);
				return View("SpaceXReportEditor", model);
			}
			else
			{
				return NotFound();
			}
		}

        [HttpPost]
        public async Task<IActionResult> Delete(SpaceXReport report)
        {
            await this._reportRepository.DeleteSpaceXReportAsync(report);
            return RedirectToAction(nameof(SpaceXReports));
        }

		public async Task<IActionResult> UserAccounts(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
		{
			UserAccountCollectionModel model = new UserAccountCollectionModel(_config, _reportRepository);
			await model.InitializeAsync(pageIndex, sortField, sortDirection, searchString);
			return View(model);
		}
	}
}
