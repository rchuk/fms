using Fms.Dtos;
using Fms.Entities;

namespace Fms.Services;

public interface IAuthService
{
    public Task<AccessTokenResponseDto> Register(UserRegisterRequestDto requestDto);
    public Task<AccessTokenResponseDto> Login(UserLoginRequestDto requestDto);
    public Task<int> GetCurrentUserId();
    public Task<UserEntity> GetCurrentUser();
}