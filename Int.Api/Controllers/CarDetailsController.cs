using Int.Domain.Entities;
using Int.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Int.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CarDetailsController : ControllerBase
	{
		private readonly FiElSekkaContext _context;

		public CarDetailsController(FiElSekkaContext context)
		{
			_context = context;
		}

		// GET: api/cardetails/brands
		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable<object>>> GetBrands()
		{
			var brands = await _context.Brands
				.Select(b => new { b.Id, b.Name })
				.ToListAsync();

			return Ok(brands);
		}

		// GET: api/cardetails/colors
		[HttpGet("colors")]
		public async Task<ActionResult<IEnumerable<object>>> GetColors()
		{
			var colors = await _context.Colors
				.Select(c => new { c.Id, c.Name })
				.ToListAsync();

			return Ok(colors);
		}

		// GET: api/cardetails/models/3
		[HttpGet("models/{brandId}")]
		public async Task<ActionResult<IEnumerable<object>>> GetModelsByBrand(int brandId)
		{
			var models = await _context.Models
				.Where(m => m.BrandId == brandId)
				.Select(m => new { m.Id, m.Name })
				.ToListAsync();

			return Ok(models);
		}
	}
}
