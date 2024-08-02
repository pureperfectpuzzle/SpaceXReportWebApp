using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Repository.SpaceX;

namespace WebClient.Utilities
{
	internal class QueryContext : IQueryContext
	{
		public QueryContext(IConfiguration config)
		{
			QueryLimit = Convert.ToInt32((config["QueryLimit"]) ?? "2000");
			SpaceXApiUrl = config["SpaceX:ApiUrl"];
			ReportConnectionStr = config["Report:ConnectionString"];

		}

		public int QueryLimit { get; set; } = 2000;

		public string? SpaceXApiUrl { get; set; } = string.Empty;

		public string? ReportConnectionStr { get; set; } = string.Empty;
	}
}
