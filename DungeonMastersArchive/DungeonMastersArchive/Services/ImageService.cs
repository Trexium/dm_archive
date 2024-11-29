using DungeonMastersArchive.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace DungeonMastersArchive.Services
{
    public class ImageService
    {
        private readonly string[] _imageExtensions = new string[]{"apng", "png", "avif", "gif", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "svg", "webp", "bmp", "ico", "cur", "tif", "tiff" };
        private readonly string _fileUploadFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\uploaded_images\\";
        private readonly string _fileUploadUrl = "/uploaded_images/";

        public string AcceptedExtensions { get { return string.Join(", ", _imageExtensions); } }

        public async Task<bool> ValidateFileName(string fileName)
        {
            var extension = fileName.Split('.').Last();
            if (!_imageExtensions.Contains(extension))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateFile(IBrowserFile browserFile)
        {
            return await ValidateFileName(browserFile.Name);
        }

        public string GetImageUrl(ArticleImageMetadata metadata)
        {
            return $"{_fileUploadUrl}{metadata.CampaignId}/{metadata.FileName}";
        }

        public async Task<ArticleImageMetadata> UploadFile(IBrowserFile browserFile, int campaignId)
        {
            if (await ValidateFile(browserFile))
            {
                var metadata = new ArticleImageMetadata();
                metadata.CampaignId = campaignId;
                metadata.Title = browserFile.Name.Split(".").First();
                var extension = browserFile.Name.Split('.').Last();
                metadata.FileName = $"{Guid.NewGuid().ToString()}.{extension}";

                var path = $"{_fileUploadFolder}{campaignId}\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                await using FileStream fs = new($"{path}{metadata.FileName}", FileMode.Create);
                await browserFile.OpenReadStream().CopyToAsync(fs);

                return metadata;
            }
            else
            {
                return null;
            }
            
        }
    }
}
