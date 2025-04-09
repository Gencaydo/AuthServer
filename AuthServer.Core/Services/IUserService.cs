using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<UserAppDto>> UpdateUserAsync(UserAppDto userAppDto);
    Task<Response<UserAppDto>> GetUserByEmailAsync(GetUserDto getUserDto);
    Task<Response<NoDataDto>> CreateUserRoles(string email);
}