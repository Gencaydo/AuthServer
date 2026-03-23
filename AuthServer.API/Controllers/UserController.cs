using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers;

public class UserController : CustomBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserAppDto userAppDto)
    {
        return ActionResultInstance(await _userService.UpdateUserAsync(userAppDto));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return ActionResultInstance(await _userService.DeleteUserAsync(id));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return ActionResultInstance(await _userService.GetAllUsersAsync());
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetUserByEmail([FromQuery] GetUserDto getUserDto)
    {
        return ActionResultInstance(await _userService.GetUserByEmailAsync(getUserDto));
    }

    [HttpPost("{email}")]
    public async Task<IActionResult> CreateUserRoles(string email)
    {
        return ActionResultInstance(await _userService.CreateUserRoles(email));
    }

    // ── AspNetUserClaims ──────────────────────────────────────────────────────

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserClaims(string userId)
    {
        return ActionResultInstance(await _userService.GetUserClaimsAsync(userId));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> AddUserClaim(UserClaimDto userClaimDto)
    {
        return ActionResultInstance(await _userService.AddClaimToUserAsync(userClaimDto));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserClaim(UserClaimDto userClaimDto)
    {
        return ActionResultInstance(await _userService.RemoveClaimFromUserAsync(userClaimDto));
    }

    // ── AspNetUserRoles ───────────────────────────────────────────────────────

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        return ActionResultInstance(await _userService.GetUserRolesAsync(userId));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> AddUserToRole(UserRoleDto userRoleDto)
    {
        return ActionResultInstance(await _userService.AddUserToRoleAsync(userRoleDto));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserFromRole(UserRoleDto userRoleDto)
    {
        return ActionResultInstance(await _userService.RemoveUserFromRoleAsync(userRoleDto));
    }

    // ── AspNetUserLogins ──────────────────────────────────────────────────────

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserLogins(string userId)
    {
        return ActionResultInstance(await _userService.GetUserLoginsAsync(userId));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("{userId}/{loginProvider}/{providerKey}")]
    public async Task<IActionResult> RemoveUserLogin(string userId, string loginProvider, string providerKey)
    {
        return ActionResultInstance(await _userService.RemoveUserLoginAsync(userId, loginProvider, providerKey));
    }
}
