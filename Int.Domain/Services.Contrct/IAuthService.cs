using Int.Domain.DTOs.Auth;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
namespace Int.Domain.Services.Contrct
{
    public interface IAuthService
    {

        Task<UserDTO?> RegisterAsync(RegisterDTO registerDto);
        Task<UserDTO?> LoginAsync(LoginDTO loginDto);


        string GenerateJwtToken(User user);



	}
}
