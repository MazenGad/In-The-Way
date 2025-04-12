using Int.Domain.Entities;
using Int.Domain.Services.Contrct;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Application.Services
{
	public class RoleService : IRoleService
	{
		private RoleManager<IdentityRole> _roleManager;
		private UserManager<User> _userManager;

		public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<bool> AddToRoleAsync(string userId, string roleName)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return false;
			}
			var result = await _userManager.AddToRoleAsync(user, roleName);
			return result.Succeeded;
		}

		public async Task<bool> CreateRoleAsync(string roleName)
		{
			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				var role = new IdentityRole(roleName);
				var resutl = await _roleManager.CreateAsync(role);
				return resutl.Succeeded;
			}
			return false;
		}

		public async Task<List<string>> GetRolesAsync()
		{
			return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
		}

	}
}
