using Data.Objects.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
	public interface IReportRepository : IDisposable
	{
		Task<IEnumerable<SpaceXReport>> GetReportsAsync();
	}
}
