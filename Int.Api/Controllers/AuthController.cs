using Int.Domain.DTOs.Auth;
using Int.Domain.Services.Contrct;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Int.Domain.DTOs.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Int.Domain.Entities;

namespace Int.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public AuthController(IAuthService authService, UserManager<User> userManager, IMapper mapper)
		{
			_authService = authService;
			_userManager = userManager;
			_mapper = mapper;
		}


		[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var userDto = await _authService.RegisterAsync(registerDto);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _authService.LoginAsync(loginDto);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


		[HttpGet("external-login")]
		public IActionResult ExternalLogin()
		{
			var properties = new AuthenticationProperties { RedirectUri = "/api/auth/google-callback" };
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

		[HttpGet("google-callback")]
		public async Task<IActionResult> GoogleCallback()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			if (!result.Succeeded)
				return Unauthorized();

			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
			var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

			if (email is null)
				return BadRequest("Email not found from Google");

			// ابحث عن اليوزر أو أنشئه لو مش موجود
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				user = new User
				{
					Email = email,
					UserName = email,
					firstName = name?.Split(' ').FirstOrDefault() ?? "",
					lastName = name?.Split(' ').LastOrDefault() ?? ""
				};
				await _userManager.CreateAsync(user);
				await _userManager.AddToRoleAsync(user, "User");
			}

			// توليد JWT
			var userDto = _mapper.Map<UserDTO>(user);
			userDto.token = _authService.GenerateJwtToken(user); // لازم تكون public في AuthService

			// ترجع JWT في response
			return Ok(userDto);
		}



	}
}
