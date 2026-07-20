using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CititesInfoAPI.Models
{
	public class PointsOfInterestBulkUpdateDto
	{
		public string NewDescription { get; set; } = string.Empty;
	}
}
