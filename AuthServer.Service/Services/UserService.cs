using System.Security.Claims;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;

namespace AuthServer.Service.Services;

public class UserService : IUserService
{

    private readonly UserManager<UserApp> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp()
        {
            Id = Guid.NewGuid().ToString(),
            Email = createUserDto.Email,
            UserName = createUserDto.UserName,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            MobilePhoneNumber = createUserDto.MobilePhoneNumber
        };

        var result = await _userManager.CreateAsync(user, password: createUserDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);

        return Response<UserAppDto>.Success(userDto, 201);
    }

    public async Task<Response<UserAppDto>> UpdateUserAsync(UserAppDto userAppDto)
    {
        var user = await _userManager.FindByIdAsync(userAppDto.Id);


        user.FirstName = userAppDto.FirstName;
        user.LastName = userAppDto.LastName;
        user.MobilePhoneNumber = userAppDto.MobilePhoneNumber;


        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);

        return Response<UserAppDto>.Success(userDto, 200);
    }

    public async Task<Response<UserAppDto>> GetUserByEmailAsync(GetUserDto getUserDto)
    {
        var user = await _userManager.FindByEmailAsync(getUserDto.Email);
        if (user is null)
        {
            return Response<UserAppDto>.Fail("user not found", 404, true);
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return Response<UserAppDto>.Success(userDto, 200);
    }

    public async Task<Response<NoDataDto>> CreateUserRoles(string email)
    {
        if (!(await _roleManager.RoleExistsAsync("admin")))
        {
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("manager"));
        }
        var user = await _userManager.FindByEmailAsync(email);
        await _userManager.AddToRoleAsync(user, "admin");
        await _userManager.AddToRoleAsync(user, "manager");

        return Response<NoDataDto>.Success(200);
    }

    public async Task<Response<IEnumerable<UserAppDto>>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var userDtos = ObjectMapper.Mapper.Map<List<UserAppDto>>(users);
        return Response<IEnumerable<UserAppDto>>.Success(userDtos, 200);
    }

    public async Task<Response<NoDataDto>> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<IEnumerable<RoleClaimDto>>> GetUserClaimsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Response<IEnumerable<RoleClaimDto>>.Fail("User not found", 404, true);

        var claims = await _userManager.GetClaimsAsync(user);
        var claimDtos = claims.Select(c => new RoleClaimDto { ClaimType = c.Type, ClaimValue = c.Value });
        return Response<IEnumerable<RoleClaimDto>>.Success(claimDtos, 200);
    }

    public async Task<Response<NoDataDto>> AddClaimToUserAsync(UserClaimDto userClaimDto)
    {
        var user = await _userManager.FindByIdAsync(userClaimDto.UserId!);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.AddClaimAsync(user, new Claim(userClaimDto.ClaimType!, userClaimDto.ClaimValue!));

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(201);
    }

    public async Task<Response<NoDataDto>> RemoveClaimFromUserAsync(UserClaimDto userClaimDto)
    {
        var user = await _userManager.FindByIdAsync(userClaimDto.UserId!);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.RemoveClaimAsync(user, new Claim(userClaimDto.ClaimType!, userClaimDto.ClaimValue!));

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<IEnumerable<string>>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Response<IEnumerable<string>>.Fail("User not found", 404, true);

        var roles = await _userManager.GetRolesAsync(user);
        return Response<IEnumerable<string>>.Success(roles, 200);
    }

    public async Task<Response<NoDataDto>> AddUserToRoleAsync(UserRoleDto userRoleDto)
    {
        var user = await _userManager.FindByIdAsync(userRoleDto.UserId!);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.AddToRoleAsync(user, userRoleDto.RoleName!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(201);
    }

    public async Task<Response<NoDataDto>> RemoveUserFromRoleAsync(UserRoleDto userRoleDto)
    {
        var user = await _userManager.FindByIdAsync(userRoleDto.UserId!);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.RemoveFromRoleAsync(user, userRoleDto.RoleName!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<IEnumerable<UserLoginDto>>> GetUserLoginsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Response<IEnumerable<UserLoginDto>>.Fail("User not found", 404, true);

        var logins = await _userManager.GetLoginsAsync(user);
        var loginDtos = logins.Select(l => new UserLoginDto
        {
            UserId = userId,
            LoginProvider = l.LoginProvider,
            ProviderKey = l.ProviderKey,
            ProviderDisplayName = l.ProviderDisplayName
        });

        return Response<IEnumerable<UserLoginDto>>.Success(loginDtos, 200);
    }

    public async Task<Response<NoDataDto>> RemoveUserLoginAsync(string userId, string loginProvider, string providerKey)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Response<NoDataDto>.Fail("User not found", 404, true);

        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<NoDataDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<NoDataDto>.Success(204);
    }
}
