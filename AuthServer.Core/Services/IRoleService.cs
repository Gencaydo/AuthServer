using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services;

public interface IRoleService
{
    Task<Response<IEnumerable<RoleDto>>> GetAllRolesAsync();
    Task<Response<RoleDto>> GetRoleByIdAsync(string id);
    Task<Response<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<Response<RoleDto>> UpdateRoleAsync(RoleDto roleDto);
    Task<Response<NoDataDto>> DeleteRoleAsync(string id);
    Task<Response<IEnumerable<RoleClaimDto>>> GetRoleClaimsAsync(string roleId);
    Task<Response<NoDataDto>> AddClaimToRoleAsync(string roleId, RoleClaimDto roleClaimDto);
    Task<Response<NoDataDto>> RemoveClaimFromRoleAsync(string roleId, RoleClaimDto roleClaimDto);
}
