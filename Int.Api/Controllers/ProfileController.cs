using AutoMapper;
using Int.Domain.DTOs.Auth;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
using Int.Infrastructure.Entities;
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

        public ProfileController(UserManager<User> userManager, IMapper mapper , FiElSekkaContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("Profile")]
        [Authorize]
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
        [Authorize]
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
    }
}
