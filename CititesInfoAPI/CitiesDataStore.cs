using CititesInfoAPI.Models;

namespace CititesInfoAPI
{
	public class CitiesDataStore
	{
		public List<CityDto> Cities { get; set; }

		public CitiesDataStore() 
		{
			Cities =
			[
				// 1. New York City
				new()
				{
					Id = 1,
					Name = "New York City",
					Description = "The one with that big park.",
					PointsOfInterest = [
						new() {
							Id = 1,
							Name = "Central Park",
							Description = "The most visited urban park in the United States."
						},
						new() {
							Id = 2,
							Name = "Empire State Building",
							Description = "A 102-story skyscraper located in Midtown Manhattan."
						}
					]
				},

				// 2. Cairo
				new()
				{
					Id = 2,
					Name = "Cairo",
					Description = "The city of a thousand minarets.",
					PointsOfInterest = [
						new() {
							Id = 3,
							Name = "The Great Pyramids of Giza",
							Description = "One of the Seven Wonders of the Ancient World."
						},
						new() {
							Id = 4,
							Name = "Khan el-Khalili",
							Description = "A famous historic bazaar and souq in the historic center."
						}
					]
				},

				// 3. Paris
				new()
				{
					Id = 3,
					Name = "Paris",
					Description = "The City of Light and romance.",
					PointsOfInterest = [
						new() {
							Id = 5,
							Name = "Eiffel Tower",
							Description = "A locally nicknamed iron lady, located on the Champ de Mars."
						},
						new() {
							Id = 6,
							Name = "Louvre Museum",
							Description = "The world's largest art museum and a historic monument."
						}
					]
				},

				// 4. Tokyo
				new()
				{
					Id = 4,
					Name = "Tokyo",
					Description = "A bustling metropolis blending ultra-modern technology with ancient temples.",
					PointsOfInterest = [
						new() {
							Id = 7,
							Name = "Senso-ji Temple",
							Description = "Tokyo's oldest and one of its most significant Buddhist temples."
						},
						new() {
							Id = 8,
							Name = "Shibuya Crossing",
							Description = "The world's busiest pedestrian intersection."
						}
					]
				}
			];
		}
	}
}
