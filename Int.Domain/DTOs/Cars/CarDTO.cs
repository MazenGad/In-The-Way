using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.DTOs.Cars
{
    public class CarDTO
    {
		public int Id { get; set; }
		public string? Description { get; set; }
        //  public string? CModel { get; set; }

        public string Location { get; set; }
        public string PlateNumber { get; set; }
        public string? BrandName { get; set; }
        public string? ColorName { get; set; }
		public string? ModelName { get; set; }
		public int? ColorId { get; set; }
		public int? ModelId { get; set; }
		public int? BrandId { get; set; }

		public bool IsDeleted { get; set; } = false;

		public List<string> imageUrls { get; set; } = new List<string>();

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string? UserId { get; set; }

	}
}
