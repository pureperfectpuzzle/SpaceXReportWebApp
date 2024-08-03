using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
	/// <summary>
	/// Interface to retrive query related settings from application setting file
	/// </summary>
	public interface IQueryContext
	{
		/// <summary>
		/// The page size of pagination
		/// </summary>
		int PageSize { get; set; }

		/// <summary>
		/// The maximum items to retrieve from data source when no condition specified
		/// </summary>
		int QueryLimit { get; set; }

		/// <summary>
		/// The web api URL for SpaceX GrpahQL api server
		/// </summary>
		string? SpaceXApiUrl { get; set; }

		/// <summary>
		/// The connection string for local database
		/// </summary>
		string? ReportConnectionStr { get; set; }
	}
}
