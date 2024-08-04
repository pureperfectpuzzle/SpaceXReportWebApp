using Data.Objects.Report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Report.DataAccess
{
	internal class ReportDbContext : DbContext
	{
		public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
		{ }

		public DbSet<SpaceXReport> SpaceXReports { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// TODO: add some schema constraints here!!!
			modelBuilder.Entity<SpaceXReport>()
						.HasKey(r => r.Id);
		}
	}
}
