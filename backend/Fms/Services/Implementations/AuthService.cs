using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fms.Application.Attributes;
using Fms.Data;
using Fms.Dtos;
using Fms.Entities;
using Fms.Exceptions;
using Fms.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace Fms.Services.Implementations;

public class AuthService : IAuthService
{
    private const string UserIdClaim = "user_id";

    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    
    private readonly int _jwtExpirationTime;
    private readonly int _pbkdf2Iterations;

    public AuthService(
        IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IStringLocalizer<ErrorMessages> errorLocalizer,
        IUserRepository userRepository, IUserService userService
        )
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _errorLocalizer = errorLocalizer;
        _userRepository = userRepository;
        _userService = userService;
        _jwtExpirationTime = int.TryParse(configuration["Jwt:ExpirationTime"], out var jwtExpirationTime) ? jwtExpirationTime : 3600;
        _pbkdf2Iterations = int.TryParse(configuration["Security:Pbkdf2Iterations"], out var pbkdf2Iterations) ? pbkdf2Iterations : 100001;
    }
        
    [Transactional]
    public async Task<AccessTokenResponseDto> Register(UserRegisterRequestDto requestDto)
    {
        if (await _userRepository.FindByEmail(requestDto.Email) is not null)
            throw new PublicClientException(_errorLocalizer[Localization.ErrorMessages.user_already_exists_by_email]);

        var passwordHash = Convert.ToBase64String(HashPassword(requestDto.Email, requestDto.Password));
        var user = await _userService.CreateUser(new UserEntity
        {
            Email = requestDto.Email,
            PasswordHash = passwordHash,
            FirstName = "Test",
            LastName = "Test"
        });

        return CreateAccessTokenResponseDto(CreateJwtToken(user.Id));
    }

    public async Task<AccessTokenResponseDto> Login(UserLoginRequestDto requestDto)
    {
        if (await _userRepository.FindByEmail(requestDto.Email) is { } userEntity)
        {
            if (VerifyPassword(userEntity, requestDto.Password))
            {
                return CreateAccessTokenResponseDto(CreateJwtToken(userEntity.Id));
            }
            
            throw new PublicClientException(_errorLocalizer[Localization.ErrorMessages.wrong_password]);
        }
        
        throw new PublicClientException(_errorLocalizer[Localization.ErrorMessages.user_doesnt_exist_by_email]);
    }

    public Task<int> GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext!.User;
        
        if (user.Identity is ClaimsIdentity identity)
        {
            if (identity.FindFirst(UserIdClaim) is { } claim)
                return Task.FromResult(int.Parse(claim.Value));
        }
        
        throw new PublicClientException(_errorLocalizer[Localization.ErrorMessages.unathorized]);
    }

    public async Task<UserEntity> GetCurrentUser()
    {
        var id = await GetCurrentUserId();
        if (await _userRepository.Read(id) is {} user)
            return user;
        
        throw new PublicClientException(_errorLocalizer[Localization.ErrorMessages.unathorized]);
    }

    private AccessTokenResponseDto CreateAccessTokenResponseDto(string token)
    {
        return new AccessTokenResponseDto {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresIn = _jwtExpirationTime
        };
    }
    
    private string CreateJwtToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Secrets:JwtSecret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddSeconds(_jwtExpirationTime),
            signingCredentials: credentials,
            claims: new [] {
                new Claim(UserIdClaim, userId.ToString())
            });

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(UserEntity userEntity, string attemptedPassword)
    {
        var realPasswordHash = Convert.FromBase64String(userEntity.PasswordHash);
    
        return realPasswordHash.SequenceEqual(HashPassword(userEntity.Email, attemptedPassword));
    }

    private byte[] HashPassword(string email, string password)
    {
        return new Rfc2898DeriveBytes(
            Encoding.UTF8.GetBytes(password),
            SaltPassword(email),
            _pbkdf2Iterations,
            HashAlgorithmName.SHA256
        ).GetBytes(32);
    }

    private byte[] SaltPassword(string email)
    {
        var passwordSalt = Convert.FromBase64String(_configuration["Secrets:PasswordSalt"]!);
        
        var bytes = new byte[passwordSalt.Length + email.Length];
        Array.Copy(passwordSalt, 0, bytes, 0, passwordSalt.Length);
        Array.Copy(Encoding.UTF8.GetBytes(email), 0, bytes, passwordSalt.Length, email.Length);

        return bytes;
    }
}
