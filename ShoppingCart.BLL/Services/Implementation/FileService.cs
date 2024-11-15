using Microsoft.AspNetCore.Http;
using ShoppingCart.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Services.Implementation
{
	public class FileService : IFileService
	{
		public async Task<string> SaveFileAsync(IFormFile file, string rootPath, string folderPath)
		{
			var uploadFolder = Path.Combine(rootPath, folderPath.TrimStart('\\'));

			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			//var filePath = Path.Combine(uploadFolder, fileName).Replace('\\', '/');
			var filePath = Path.Combine(uploadFolder, fileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			return Path.Combine(folderPath, fileName);
		}

		public void DeleteFile(string rootPath, string filePath)
		{
			var oldImage = Path.Combine(rootPath, filePath.TrimStart('\\'));

			if (File.Exists(oldImage)) File.Delete(oldImage);
		}
	}
}
