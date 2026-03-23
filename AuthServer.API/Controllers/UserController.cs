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

    // POST api/User/CreateUser
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
    }

    // PUT api/User/UpdateUser
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserAppDto userAppDto)
    {
        return ActionResultInstance(await _userService.UpdateUserAsync(userAppDto));
    }

    // DELETE api/User/DeleteUser/{id}
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return ActionResultInstance(await _userService.DeleteUserAsync(id));
    }

    // GET api/User/GetAllUsers
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return ActionResultInstance(await _userService.GetAllUsersAsync());
    }

    // GET api/User/GetUserByEmail?email=...
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetUserByEmail([FromQuery] GetUserDto getUserDto)
    {
        return ActionResultInstance(await _userService.GetUserByEmailAsync(getUserDto));
    }

    // POST api/User/CreateUserRoles/{email}
    [HttpPost("{email}")]
    public async Task<IActionResult> CreateUserRoles(string email)
    {
        return ActionResultInstance(await _userService.CreateUserRoles(email));
    }

    // ── AspNetUserClaims ──────────────────────────────────────────────────────

    // GET api/User/GetUserClaims/{userId}
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserClaims(string userId)
    {
        return ActionResultInstance(await _userService.GetUserClaimsAsync(userId));
    }

    // POST api/User/AddUserClaim
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> AddUserClaim(UserClaimDto userClaimDto)
    {
        return ActionResultInstance(await _userService.AddClaimToUserAsync(userClaimDto));
    }

    // DELETE api/User/RemoveUserClaim
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserClaim(UserClaimDto userClaimDto)
    {
        return ActionResultInstance(await _userService.RemoveClaimFromUserAsync(userClaimDto));
    }

    // ── AspNetUserRoles ───────────────────────────────────────────────────────

    // GET api/User/GetUserRoles/{userId}
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        return ActionResultInstance(await _userService.GetUserRolesAsync(userId));
    }

    // POST api/User/AddUserToRole
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> AddUserToRole(UserRoleDto userRoleDto)
    {
        return ActionResultInstance(await _userService.AddUserToRoleAsync(userRoleDto));
    }

    // DELETE api/User/RemoveUserFromRole
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserFromRole(UserRoleDto userRoleDto)
    {
        return ActionResultInstance(await _userService.RemoveUserFromRoleAsync(userRoleDto));
    }

    // ── AspNetUserLogins ──────────────────────────────────────────────────────

    // GET api/User/GetUserLogins/{userId}
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserLogins(string userId)
    {
        return ActionResultInstance(await _userService.GetUserLoginsAsync(userId));
    }

    // DELETE api/User/RemoveUserLogin/{userId}/{loginProvider}/{providerKey}
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("{userId}/{loginProvider}/{providerKey}")]
    public async Task<IActionResult> RemoveUserLogin(string userId, string loginProvider, string providerKey)
    {
        return ActionResultInstance(await _userService.RemoveUserLoginAsync(userId, loginProvider, providerKey));
    }
}
