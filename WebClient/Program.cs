using Data.Enums;
using Data.Interfaces;
using Data.Objects.Report;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Report;
using Repository.Report.DataAccess;
using Repository.SpaceX;
using WebClient.Utilities;

namespace WebClient
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddSingleton<IQueryContext, QueryContext>();
            builder.Services.AddSingleton<ISpaceXRepository, SpaceXRepository>();
			builder.Services.AddSingleton<IReportRepository, ReportRepository>();

			builder.Services.AddDbContext<ReportIdentityDbContext>(options =>
			{
				options.UseInMemoryDatabase(databaseName: "ReportIdentityDb");
			});
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ReportIdentityDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/reports/Identity/DisplayLoginView";
                options.AccessDeniedPath = "/reports/Identity/AccessDenied";
                options.SlidingExpiration = true;
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
			app.MapControllerRoute("reports", "reports/{controller=Admin}/{action=Index}/{id?}");
			app.MapRazorPages();

			await SeedTestData(app);

			app.Run();
		}

		private static async Task SeedTestData(WebApplication app)
		{
			IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;

            // Add some user accounts and roles for testing
			IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			var usersInfo = config.GetSection("UserAccounts").GetChildren();
			foreach (var userInfo in usersInfo)
			{
				string userName = userInfo["Name"]?? string.Empty;
				string password = userInfo["Password"]?? string.Empty;
				string email = userInfo["Email"] ?? string.Empty;
				string roleName = userInfo["Role"] ?? string.Empty;

				if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password) &&
					string.IsNullOrEmpty(email) && string.IsNullOrEmpty(roleName))
				{
					continue;
				}

				if (await userManager.FindByNameAsync(userName) != null)
				{
					continue;
				}

				if (await roleManager.FindByNameAsync(roleName) == null)
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}

				IdentityUser user = new IdentityUser
				{
					UserName = userName,
					Email = email,
				};

				IdentityResult result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, roleName);
				}
			}

			// Add some SpaceX sample reports
			IReportRepository reportRep = serviceProvider.GetRequiredService<IReportRepository>();
			IdentityUser? testUser = await userManager.Users.FirstOrDefaultAsync();
			Guid testUserId = testUser == null ? Guid.Empty : new Guid(testUser.Id);

			var reports = new List<SpaceXReport>()
				{
					new SpaceXReport()
					{
						Id = Guid.NewGuid(),
						Title = "Rocket engine failed to ignite",
						Description = "Rocket was not lifted because its engine was not ignited for some reason. We need to find out why ASAP.",
						LaunchId = string.Empty,
						CreatorId = testUserId,
						DateOfCreation = new DateTime(2022, 2, 21, 8, 8, 8),
						InvestigatorId = testUserId,
						InvestigationComments = "I checked the procedure of fuel and found out it was not cold enough.",
						Solution = "We need to add more stricter QA on fuel preparation.",
						QaId = testUserId,
						QaComments = "I did the same as the investigator and agree with him"
					},
					new SpaceXReport()
					{
						Id = Guid.NewGuid(),
						Title = "Rocket exploded in air",
						Description = "Rocket exploded for some reason. Find out what caused this.",
						LaunchId = string.Empty,
						CreatorId = testUserId,
						DateOfCreation = DateTime.Now,
						InvestigatorId = testUserId,
						InvestigationComments = "I checked the procedure of rocket assembly.",
						Solution = "We need to add more stricter QA on rocket assembly.",
						QaId = testUserId,
						QaComments = "I did the same as the investigator and agree with him."
					},
				};
			reportRep.SeedTestData(reports);
		}
	}
}
