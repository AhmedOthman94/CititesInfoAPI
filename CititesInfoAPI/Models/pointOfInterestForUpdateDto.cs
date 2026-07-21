using System.ComponentModel.DataAnnotations;

namespace CititesInfoAPI.Models
{
	public class PointOfInterestForUpdateDto
	{
		[Required(ErrorMessage = "You must provide a name.")]
		[MaxLength(50)]
		public string Name { get; set; } = string.Empty;
		[MaxLength(200)]
		public string? Description { get; set; }
	}
}
