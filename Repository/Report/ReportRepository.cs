using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Objects.Report;
using Microsoft.EntityFrameworkCore;
using Repository.Report.DataAccess;
using Data.Enums;

namespace Repository.Report
{
	public class ReportRepository : IReportRepository
	{
		private readonly IQueryContext queryContext;
		private readonly ReportDbContext dbContext;

		public ReportRepository(IQueryContext queryContext)
		{
			this.queryContext = queryContext;

			var builder = new DbContextOptionsBuilder<ReportDbContext>();
			builder.UseInMemoryDatabase(databaseName: "SpaceXReportDB");
			builder.EnableSensitiveDataLogging();
			var options = builder.Options;
			this.dbContext = new ReportDbContext(options);
		}

		#region IReportRepository

		public async Task<int> GetReportCountAsync()
		{
			return await this.dbContext.SpaceXReports.CountAsync();
		}

		public async Task<IEnumerable<SpaceXReport>> GetReportsAsync(int pageIndex, 
			string sortField, SortingDirection sortingDirection, string searchString)
		{
			IQueryable<SpaceXReport> query = this.dbContext.SpaceXReports;

			// Filter
			string trimmedSearchString = searchString.Trim();
			if (string.IsNullOrEmpty(trimmedSearchString) == false)
			{
				query = query.Where(r => r.Title != null && r.Title.Contains(trimmedSearchString));
			}

			// Sort
			if (string.Compare(sortField, "ID", true) == 0)
			{
				if (sortingDirection == SortingDirection.Ascending)
				{
					query = query.OrderBy(r => r.Id);
				}
				else
				{
					query = query.OrderByDescending(r => r.Id);
				}
			}
			else if (string.Compare(sortField, "Title", true) == 0)
			{
				if (sortingDirection == SortingDirection.Ascending)
				{
					query = query.OrderBy(r => r.Title);
				}
				else
				{
					query = query.OrderByDescending(r => r.Title);
				}
			}

			// Pagination
			int pageSize = this.queryContext.PageSize;
			query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

			List<SpaceXReport> reportList = new();
			IEnumerable<SpaceXReport> reports = await query.ToArrayAsync();
			reportList.AddRange(reports);
			return reportList;
		}

		public async Task<SpaceXReport?> GetSpaceXReportAsync(string id)
		{
			SpaceXReport? spaceXReport = await this.dbContext.SpaceXReports.Where(r => r.Id == new Guid(id)).FirstOrDefaultAsync();
			return spaceXReport;
		}

		public async Task AddSpaceXReportAsync(SpaceXReport spaceXReport)
		{
			if (spaceXReport != null)
			{
				await this.dbContext.AddAsync(spaceXReport);
				await this.dbContext.SaveChangesAsync();
			}
			return;
		}

		public async Task ModifySpaceXReportAsync(SpaceXReport spaceXReport)
		{
			if (spaceXReport != null)
			{
				SpaceXReport? foundReport = await this.dbContext.SpaceXReports.Where(r => r.Id == spaceXReport.Id).FirstOrDefaultAsync();
				if (foundReport != null)
				{
					foundReport.Copy(spaceXReport);
					await this.dbContext.SaveChangesAsync();
				}
			}
		}

		public async Task DeleteSpaceXReportAsync(SpaceXReport spaceXReport)
		{
			if (spaceXReport != null)
			{
				SpaceXReport? foundReport = await this.dbContext.SpaceXReports.Where(r => r.Id == spaceXReport.Id).FirstOrDefaultAsync();
				if (foundReport != null)
				{
					this.dbContext.SpaceXReports.Remove(foundReport);
					await this.dbContext.SaveChangesAsync();
				}
			}
		}

		#region IDisposable

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private bool disposedValue;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.dbContext.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		#endregion IDisposable

		public void SeedTestData<T>(IEnumerable<T> data)
		{
			if (typeof(T) == typeof(SpaceXReport))
			{
				IEnumerable<SpaceXReport> reports = data.Cast<SpaceXReport>();
				this.dbContext.AddRange(reports);
			}

            this.dbContext.SaveChanges();
		}

		#endregion IReportRepository
	}
}
