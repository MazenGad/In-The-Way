using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Int.Domain.Services.Contrct;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InT.Application.Services
{

    public class CloudinaryServices : ICloudinaryServices
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }
        private string GetPublicIdfromUrl(string url)
        {
            Uri uri = new Uri(url);
            string publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            return publicId;
        }

        public async Task<List<(string imageUrl, string publicId)>> UploadImagesAsync(List<IFormFile> files)
        {
            var imageList = new List<(string imageUrl, string publicId)>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;

                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(800).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageList.Add((uploadResult.SecureUrl.ToString(), uploadResult.PublicId));
            }

            return imageList;
        }

    }
}