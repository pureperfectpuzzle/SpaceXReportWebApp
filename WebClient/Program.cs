using Data.Interfaces;
using Repository.SpaceX;

namespace WebClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<ISpaceXRepository, SpaceXRepository>();
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

			app.MapRazorPages();

			var spaceXRepository = app.Services.GetRequiredService<ISpaceXRepository>();
			var configuration = app.Services.GetService<IConfiguration>();
			spaceXRepository.QueryLimit = Convert.ToInt32((configuration?["QueryLimit"]) ?? "2000");

			app.Run();
		}
	}
}
