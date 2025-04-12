using Microsoft.AspNetCore.Http;
namespace Int.Domain.Services.Contrct
{
	public interface ICloudinaryServices
    {
        Task<List<(string imageUrl, string publicId)>> UploadImagesAsync(List<IFormFile> files);
    }
}
