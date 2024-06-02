using System.Security.Claims;
using Fms.Dtos;

namespace Fms.Services;

public interface IAuthService
{
    public Task<AccessTokenResponseDto> Register(UserRegisterRequestDto requestDto);
    public Task<AccessTokenResponseDto> Login(UserLoginRequestDto requestDto);
    public Task<int> GetUserId();
}