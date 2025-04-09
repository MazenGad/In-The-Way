using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.DTOs.Cars
{
	public class UploadCarDTO
    {
        public string? Description { get; set; }
        public string Location { get; set; } = null!;
        public string? PlateNumber { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public List<IFormFile> CarPhotos { get; set; }

    }
}
