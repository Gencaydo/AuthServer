using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
public class RoleController : CustomBaseController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET api/Role/GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return ActionResultInstance(await _roleService.GetAllRolesAsync());
    }

    // GET api/Role/GetById/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return ActionResultInstance(await _roleService.GetRoleByIdAsync(id));
    }

    // POST api/Role/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto createRoleDto)
    {
        return ActionResultInstance(await _roleService.CreateRoleAsync(createRoleDto));
    }

    // PUT api/Role/Update
    [HttpPut]
    public async Task<IActionResult> Update(RoleDto roleDto)
    {
        return ActionResultInstance(await _roleService.UpdateRoleAsync(roleDto));
    }

    // DELETE api/Role/Delete/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return ActionResultInstance(await _roleService.DeleteRoleAsync(id));
    }

    // GET api/Role/GetClaims/{roleId}
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetClaims(string roleId)
    {
        return ActionResultInstance(await _roleService.GetRoleClaimsAsync(roleId));
    }

    // POST api/Role/AddClaim/{roleId}
    [HttpPost("{roleId}")]
    public async Task<IActionResult> AddClaim(string roleId, RoleClaimDto roleClaimDto)
    {
        return ActionResultInstance(await _roleService.AddClaimToRoleAsync(roleId, roleClaimDto));
    }

    // DELETE api/Role/RemoveClaim/{roleId}
    [HttpDelete("{roleId}")]
    public async Task<IActionResult> RemoveClaim(string roleId, RoleClaimDto roleClaimDto)
    {
        return ActionResultInstance(await _roleService.RemoveClaimFromRoleAsync(roleId, roleClaimDto));
    }
}
