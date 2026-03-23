using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<UserAppDto>> UpdateUserAsync(UserAppDto userAppDto);
    Task<Response<UserAppDto>> GetUserByEmailAsync(GetUserDto getUserDto);
    Task<Response<IEnumerable<UserAppDto>>> GetAllUsersAsync();
    Task<Response<NoDataDto>> DeleteUserAsync(string id);
    Task<Response<NoDataDto>> CreateUserRoles(string email);

    // AspNetUserClaims
    Task<Response<IEnumerable<RoleClaimDto>>> GetUserClaimsAsync(string userId);
    Task<Response<NoDataDto>> AddClaimToUserAsync(UserClaimDto userClaimDto);
    Task<Response<NoDataDto>> RemoveClaimFromUserAsync(UserClaimDto userClaimDto);

    // AspNetUserRoles
    Task<Response<IEnumerable<string>>> GetUserRolesAsync(string userId);
    Task<Response<NoDataDto>> AddUserToRoleAsync(UserRoleDto userRoleDto);
    Task<Response<NoDataDto>> RemoveUserFromRoleAsync(UserRoleDto userRoleDto);

    // AspNetUserLogins
    Task<Response<IEnumerable<UserLoginDto>>> GetUserLoginsAsync(string userId);
    Task<Response<NoDataDto>> RemoveUserLoginAsync(string userId, string loginProvider, string providerKey);
}