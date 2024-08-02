using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Objects.Report;

namespace Repository.Report
{
	public class ReportRepository : IReportRepository
	{
		public ReportRepository()
		{
		
		}

		#region IReportRepository

		public async Task<IEnumerable<SpaceXReport>> GetReportsAsync()
		{
			throw new NotImplementedException();
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
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		#endregion IDisposable

		#endregion IReportRepository
	}
}
