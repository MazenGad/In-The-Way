using AutoMapper;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
using Int.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Int.Api.Controllers
{
	[ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
		private readonly FiElSekkaContext _context;


		public DashboardController(UserManager<User> userManager, IMapper mapper, FiElSekkaContext context)
		{
			_userManager = userManager;
			_mapper = mapper;
			_context = context;
		}

		[HttpGet("GetAll")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
		public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDTO>>(users);
            return Ok(userDtos);
        }

		[HttpDelete("Delete/{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
				return NotFound(new { message = "User not found" });

			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded)
				return BadRequest(new { message = "Failed to delete user", errors = result.Errors });

			return Ok(new { message = "User deleted successfully" });
		}

		[HttpGet("DashboardStats")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
		public async Task<IActionResult> GetDashboardStats()
		{
			var totalUsers = _userManager.Users.Count();
			var totalCars = await _context.Cars.CountAsync(c => !c.IsDeleted);
			var deletedCarsCount = await _context.Cars.CountAsync(c => c.IsDeleted);

			var recentCars = await _context.Cars
				.Where(c => !c.IsDeleted )
				.OrderByDescending(c => c.CreateAt)
				.Take(10)
				.ToListAsync();

			var result = new
			{
				TotalUsers = totalUsers,
				TotalCars = totalCars,
				DeletedCarsCount = deletedCarsCount,
				RecentCars = recentCars
			};

			return Ok(result);
		}


	}
}
