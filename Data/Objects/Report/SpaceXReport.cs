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
		public Guid Id { get; set; } = Guid.Empty;

		[Required]
		public string? Title { get; set; }

		[Required]
		public string? Description { get; set; }

		public string? LaunchId { get; set; }

		public Guid CreatorId { get; set; } = Guid.Empty;

		public DateTime DateOfCreation { get; set; }

		public Guid InvestigatorId { get; set; } = Guid.Empty;

		public string? InvestigationComments { get; set; }

		public string? Solution { get; set; }

		public Guid QaId { get; set; } = Guid.Empty;

		public string? QaComments { get; set; }

		public void Copy(SpaceXReport other)
		{
			this.Title = other.Title;
			this.Description = other.Description;
			this.LaunchId = other.LaunchId;
			this.CreatorId = other.CreatorId;
			this.DateOfCreation = other.DateOfCreation;
			this.InvestigatorId = other.InvestigatorId;
			this.InvestigationComments = other.InvestigationComments;
			this.Solution = other.Solution;
			this.QaId = other.QaId;
			this.QaComments = other.QaComments;
		}
	}
}
