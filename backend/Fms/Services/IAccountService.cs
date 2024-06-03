using Fms.Dtos;

namespace Fms.Services;

public interface IAccountService
{
    Task<AccountResponseDto> GetAccount(int id);
}