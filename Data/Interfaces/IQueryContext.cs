using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
	public interface IQueryContext
	{
		int QueryLimit { get; set; }

		string? SpaceXApiUrl { get; set; }

		string? ReportConnectionStr { get; set; }
	}
}
