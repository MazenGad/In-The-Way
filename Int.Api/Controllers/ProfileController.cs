using AutoMapper;
using Int.Domain.DTOs.Auth;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
using Int.Domain.Services.Contrct;
using Int.Infrastructure.Entities;
using InT.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Int.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly FiElSekkaContext _context;
		private readonly ICloudinaryServices _cloudinaryServices;

		public ProfileController(UserManager<User> userManager, IMapper mapper , FiElSekkaContext context , ICloudinaryServices cloudinaryServices)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
			_cloudinaryServices = cloudinaryServices;
		}

        [HttpGet("Profile")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        
        [HttpPut("update")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO profileDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            user.firstName = profileDto.FirstName;
            user.lastName = profileDto.LastName;
            user.PhoneNumber = profileDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update profile");

            return Ok("Profile updated successfully");
        }

		[Authorize]
		[HttpPost("reset-my-password")]
		public async Task<IActionResult> ResetMyPassword([FromBody] ConfirmResetPasswordDTO dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
				return Unauthorized("User not authenticated.");

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return NotFound("User not found.");

			if (user.Email != dto.Email)
				return BadRequest("Email does not match the authenticated user.");

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

			if (!result.Succeeded)
				return BadRequest(result.Errors.Select(e => e.Description));

			return Ok("Password changed successfully.");
		}
    

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to delete account");

            return Ok("Account deleted successfully");
        }

        [HttpPost("AddSearch")]
        public async Task<IActionResult> AddSearchHistory([FromBody] string query)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var Tquery = new SearchHistory
            {
                UserId = userId,
                Query = query
            };

            await _context.SearchHistories.AddAsync(Tquery);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetSearch")]
        public async Task<IActionResult> GetSearchHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            return NotFound("User not found");
            var history = await _context.SearchHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.SearchedAt)
            .Select(h => new
                {
                    h.Query,
                    h.SearchedAt
                })
                .ToListAsync();

            return Ok(history);
        }

		[HttpPost("upload-profile-image")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> UploadProfileImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var user = await _userManager.GetUserAsync(User);

			string imageUrl;
			try
			{
				imageUrl = await _cloudinaryServices.UploadImageAsync(file);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Image upload failed: {ex.Message}");
			}

			user.imageUrl = imageUrl;
			await _userManager.UpdateAsync(user);

			return Ok(new { imageUrl });
		}
	}
}
