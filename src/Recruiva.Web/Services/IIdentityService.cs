using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;

namespace Recruiva.Web.Services;

public interface IIdentityService
{
    Task<UserLoginResponse> Login(UserLoginRequest userLogin);

    Task<UserLoginResponse> LoginWithoutPassword(string userId);

    Task<UserCreateResponse> RegisterUser(UserCreateRequest registrationUser);
}