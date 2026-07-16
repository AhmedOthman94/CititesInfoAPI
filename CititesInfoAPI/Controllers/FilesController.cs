using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;



namespace CititesInfoAPI.Controllers
{
	[Route("api/files")]
	[ApiController]
	public class FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
		: ControllerBase
	{
		[HttpGet("{fileId}")]
		public async Task<ActionResult> GetFile(string fileId)
		{
			var pathToFile = "simple-file.pdf";

			if (!System.IO.File.Exists(pathToFile))
			{
				return NotFound();
			}

			if (!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
			{
				contentType = "application/octet-stream"; // binary stream download bot open in broswer
			}

			var bytes = await System.IO.File.ReadAllBytesAsync(pathToFile);

			return File(bytes, 
						contentType,
						Path.GetFileName(pathToFile));

		}

		[HttpPost]
		public async Task<ActionResult> CreateFile(IFormFile file)
		{
			if (file.Length is 0 ||
				file.Length > 20971520 ||
				file.ContentType != "application/pdf")
			{
				return BadRequest("No file or an invalid one has been inputed.");
			}

			var path = Path.Combine(
				Directory.GetCurrentDirectory(),
				$"uploaded_file{Guid.NewGuid()}.pdf");

			await using var stream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(stream);


			return Ok("Your file has been uploaded successfully");
		}
	}
}
