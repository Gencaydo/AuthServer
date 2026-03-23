using System.Security.Claims;
using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;

namespace AuthServer.Service.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Response<IEnumerable<RoleDto>>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
            .ToList();

        return Response<IEnumerable<RoleDto>>.Success(roles, 200);
    }

    public async Task<Response<RoleDto>> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Response<RoleDto>.Fail("Role not found", 404, true);

        return Response<RoleDto>.Success(new RoleDto { Id = role.Id, Name = role.Name }, 200);
    }

    public async Task<Response<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = new IdentityRole(createRoleDto.Name!);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<RoleDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<RoleDto>.Success(new RoleDto { Id = role.Id, Name = role.Name }, 201);
    }

    public async Task<Response<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
    {
        var role = await _roleManager.FindByIdAsync(roleDto.Id!);
        if (role is null)
            return Response<RoleDto>.Fail("Role not found", 404, true);

        role.Name = roleDto.Name;
        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<RoleDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<RoleDto>.Success(new RoleDto { Id = role.Id, Name = role.Name }, 200);
    }

    public async Task<Response<NoDataDto>> DeleteRoleAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Response<NoDataDto>.Fail("Role not found", 404, true);

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<IEnumerable<RoleClaimDto>>> GetRoleClaimsAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Response<IEnumerable<RoleClaimDto>>.Fail("Role not found", 404, true);

        var claims = await _roleManager.GetClaimsAsync(role);
        var claimDtos = claims.Select(c => new RoleClaimDto { ClaimType = c.Type, ClaimValue = c.Value });

        return Response<IEnumerable<RoleClaimDto>>.Success(claimDtos, 200);
    }

    public async Task<Response<NoDataDto>> AddClaimToRoleAsync(string roleId, RoleClaimDto roleClaimDto)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Response<NoDataDto>.Fail("Role not found", 404, true);

        var result = await _roleManager.AddClaimAsync(role, new Claim(roleClaimDto.ClaimType!, roleClaimDto.ClaimValue!));

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(201);
    }

    public async Task<Response<NoDataDto>> RemoveClaimFromRoleAsync(string roleId, RoleClaimDto roleClaimDto)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Response<NoDataDto>.Fail("Role not found", 404, true);

        var result = await _roleManager.RemoveClaimAsync(role, new Claim(roleClaimDto.ClaimType!, roleClaimDto.ClaimValue!));

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }
}
