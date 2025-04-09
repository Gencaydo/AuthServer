using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{

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
            var result = await _userService.CreateUserAsync(createUserDto);
            return ActionResultInstance(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserAppDto userAppDto)
        {
            var result = await _userService.UpdateUserAsync(userAppDto);
            return ActionResultInstance(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> GetUserByEmail(GetUserDto getUserDto)
        {
            return ActionResultInstance(await _userService.GetUserByEmailAsync(getUserDto));
        }


        [HttpPost("CreateUserRoles/{email}")]
        public async Task<IActionResult> CreateUserRoles(string email)
        {
            await _userService.CreateUserRoles(email);
            return Ok();
        }
    }
}
