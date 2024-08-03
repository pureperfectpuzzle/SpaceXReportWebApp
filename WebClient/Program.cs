using Data.Enums;
using Data.Interfaces;
using Data.Objects.Report;
using Repository.Report;
using Repository.SpaceX;
using WebClient.Utilities;

namespace WebClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddSingleton<IQueryContext, QueryContext>();
            builder.Services.AddSingleton<ISpaceXRepository, SpaceXRepository>();
			builder.Services.AddSingleton<IReportRepository, ReportRepository>();

			builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

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

			app.UseAuthorization();

			app.MapControllers();
			app.MapControllerRoute("reports", "reports/{controller=Admin}/{action=Index}/{id?}");
			app.MapRazorPages();

			SeedTestData(app);

			app.Run();
		}

		private static void SeedTestData(WebApplication app)
		{
			IReportRepository reportRep = app.Services.GetRequiredService<IReportRepository>();

			var reports = new List<SpaceXReport>()
				{
					new SpaceXReport()
					{
						Id = Guid.NewGuid(),
						Title = "Rocket engine failed to ignite",
						Description = "Rocket was not lifted because its engine was not ignited for some reason. We need to find out why ASAP.",
						LaunchId = string.Empty,
						DateOfCreation = new DateTime(2022, 2, 21, 8, 8, 8),
						InvestigationComments = "I checked the procedure of fuel and found out it was not cold enough.",
						Solution = "We need to add more stricter QA on fuel preparation.",
						QaComments = "I did the same as the investigator and agree with him"
					},
					new SpaceXReport()
					{
						Id = Guid.NewGuid(),
						Title = "Rocket exploded in air",
						Description = "Rocket exploded for some reason. Find out what caused this.",
						LaunchId = string.Empty,
						DateOfCreation = DateTime.Now,
						InvestigationComments = "I checked the procedure of rocket assembly.",
						Solution = "We need to add more stricter QA on rocket assembly.",
						QaComments = "I did the same as the investigator and agree with him."
					},
				};
			reportRep.SeedTestData(reports);

			var userAccounts = new List<UserAccount>()
			{
				new UserAccount()
				{
					Id= Guid.NewGuid(),
					FirstName = "Flynn",
					LastName = "Xu",
					Email = "flynnxu@yahoo.com",
					AccountType = (int)UserAccountType.Regular,
					Password = "Password", // Encryption!
				},
				new UserAccount()
				{
					Id= Guid.NewGuid(),
					FirstName = "Admin",
					LastName = "Admin",
					Email = "admin@google.com",
					AccountType = (int)UserAccountType.Administrator,
					Password = "Password", // Encryption!
				},
			};
			reportRep.SeedTestData(userAccounts);
		}
	}
}
