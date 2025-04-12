using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Services.Contrct
{
	public interface IRoleService
	{
		Task<bool> CreateRoleAsync(string roleName);

		Task<List<string>> GetRolesAsync();

		Task<bool> AddToRoleAsync(string userId, string roleName);
	}
}
