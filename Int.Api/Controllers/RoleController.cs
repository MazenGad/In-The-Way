using Int.Domain.DTOs.Auth;
using Int.Domain.Services.Contrct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Int.Api.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
    {
		private IRoleService _roleService;
		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;

		}
		[HttpPost("CreateRole")]
		public async Task<IActionResult> CreateRole([FromBody] string roleName)
		{
			if (await _roleService.CreateRoleAsync(roleName))
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}

		}


		[HttpGet("GetRoles")]

		public async Task<IActionResult> GetRoles()
		{
			var roles = await _roleService.GetRolesAsync();
			return Ok(roles);
		}

		[HttpPost("AddToRole")]

		public async Task<IActionResult> AddToRole([FromBody] AddToRoleDto model)
		{
			if (await _roleService.AddToRoleAsync(model.Id, model.RoleName))
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
