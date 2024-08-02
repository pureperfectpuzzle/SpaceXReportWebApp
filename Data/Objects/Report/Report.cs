using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects.Report
{
	public class SpaceXReport
	{
		public Guid Id { get; set; }

		[Required]
		public string? Title { get; set; }

		[Required]
		public string? Description { get; set; }

		public string? LaunchId { get; set; }

		public Guid CreatorId { get; set; }

		public DateTime DateOfCreation { get; set; }

		public Guid InvestigatorId { get; set; }

		public string? InvestigationComments { get; set; }

		public string? Solution { get; set; }

		public Guid QaId { get; set; }

		public string? QaComments { get; set; }
	}
}
