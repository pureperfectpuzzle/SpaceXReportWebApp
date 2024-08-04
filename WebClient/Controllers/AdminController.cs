using Data.Interfaces;
using Data.Objects.Report;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Data.Enums;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IReportRepository _reportRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IConfiguration config, IReportRepository reportRepository, 
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._config = config;
            this._reportRepository = reportRepository;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

		[Authorize]
		public async Task<IActionResult> SpaceXReports(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
        {
            ReportCollectionModel model = new ReportCollectionModel(_config, _reportRepository);
            await model.InitializeAsync(pageIndex, sortField, sortDirection, searchString);
            return View(model);
        }

		[Authorize(Roles = "Admins")]
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

		[Authorize(Roles = "Admins")]
		public IActionResult CreateReport()
		{
            SpaceXReport report = new SpaceXReport();
			SpaceXReportModel model = SpaceXReportModelFactory.Create(report);
			return View("SpaceXReportEditor", model);
		}

        [HttpPost]
 		[Authorize(Roles = "Admins")]
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

 		[Authorize(Roles = "Admins")]
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
 		[Authorize(Roles = "Admins")]
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

 		[Authorize(Roles = "Admins")]
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
 		[Authorize(Roles = "Admins")]
        public async Task<IActionResult> Delete(SpaceXReport report)
        {
            await this._reportRepository.DeleteSpaceXReportAsync(report);
            return RedirectToAction(nameof(SpaceXReports));
        }

 		[Authorize(Roles = "Admins")]
		public IActionResult UserAccounts(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
		{
			UserAccountCollectionModel model = new UserAccountCollectionModel(_config, _userManager);
			model.Initialize(pageIndex, sortField, sortDirection, searchString);
			return View(model);
		}

        [Authorize(Roles = "Admins")]
        public IActionResult Roles(int pageIndex = 1, string sortField = "ID", SortingDirection sortDirection = SortingDirection.Ascending, string searchString = "")
        {
            RoleCollectionModel model = new RoleCollectionModel(_config, _userManager, _roleManager);
            model.Initialize(pageIndex, sortField, sortDirection, searchString);
			return View(model);
        }

		[Authorize(Roles = "Admins")]
		public async Task<IActionResult> EditRole(string id)
		{
			IdentityRole? role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				return View("RoleEdit", new RoleEditModel(_userManager, _roleManager, role));
			}
			else
			{
				return RedirectToAction(nameof(Roles));
			}
		}

		[Authorize(Roles = "Admins")]
		[HttpPost]
		public async Task<IActionResult> ChangeRoleOfUser(string userid, string rolename)
		{
			IdentityRole? role = await _roleManager.FindByNameAsync(rolename);
			IdentityUser? user = await _userManager.FindByIdAsync(userid);
			if (role == null || user == null)
			{
				return RedirectToAction(nameof(Roles));
			}
			else
			{
				IdentityResult result;
				if (await _userManager.IsInRoleAsync(user!, rolename))
				{
					result = await _userManager.RemoveFromRoleAsync(user!, rolename);
				}
				else
				{
					result = await _userManager.AddToRoleAsync(user!, rolename);
				}

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Roles));
				}
				else
				{
					foreach (IdentityError err in result.Errors)
					{
						ModelState.AddModelError("", err.Description);
					}
					return View("RoleEdit", new RoleEditModel(_userManager, _roleManager, role!));
				}
			}
		}
	}
}
