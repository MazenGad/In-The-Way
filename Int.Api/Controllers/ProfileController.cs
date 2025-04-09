using AutoMapper;
using Int.Domain.DTOs.Auth;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Int.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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
    }
}
