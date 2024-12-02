using Microsoft.DotNet.Scaffolding.Shared.T4Templating;

namespace Uniqlo_New.Extensions
{
	public static class FileExtention
	{
		public static bool IsValidType(this IFormFile file,string type)
			=> file.ContentType.StartsWith(type);
		public static bool IsValidSize(this IFormFile file,int kb)
			=>file.Length <= kb*1024;
		public static async Task<string> UploadAsync(this IFormFile file,params string[] paths)
		{
			string uploadPATH = Path.Combine(paths);
			if(!Path.Exists(uploadPATH))
				Directory.CreateDirectory(uploadPATH);
			string fileName = Path.GetRandomFileName()+Path.GetExtension(file.FileName);
			using (Stream stream = File.Create(Path.Combine(uploadPATH, fileName)))
			{
				await file.CopyToAsync(stream);
			}
			return fileName;
		}

	}
}
