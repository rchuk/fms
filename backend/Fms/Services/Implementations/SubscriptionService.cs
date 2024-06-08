using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities.Enums;
using Fms.Exceptions;
using Fms.Repositories;
using Fms.Repositories.Implementations;

namespace Fms.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly SubscriptionKindRepository _subscriptionKindRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    
    public SubscriptionService(
        SubscriptionKindRepository subscriptionKindRepository,
        IUserRepository userRepository,
        IAuthService authService
    )
    {
        _subscriptionKindRepository = subscriptionKindRepository;
        _userRepository = userRepository;
        _authService = authService;
    }
    
    [Transactional]
    public async Task BuySubscription(BuySubscriptionRequestDto request)
    {
        var user = await _authService.GetCurrentUser();
        var currentSubscriptionLevel = SubscriptionKindToLevel(user.SubscriptionKind?.ToEnum());
        var newSubscriptionLevel = SubscriptionKindToLevel(request.Kind);
        if (currentSubscriptionLevel >= newSubscriptionLevel)
            throw new PublicClientException();

        user.SubscriptionKind = await _subscriptionKindRepository.Read(request.Kind);
        if (!await _userRepository.Update(user))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task<SubscriptionKind?> GetCurrentUserSubscription()
    {
        var user = await _authService.GetCurrentUser();
        
        return user.SubscriptionKind?.ToEnum();
    }

    private static int SubscriptionKindToLevel(SubscriptionKind? kind)
    {
        return kind switch
        {
            SubscriptionKind.Family => 10,
            SubscriptionKind.Business => 20,
            SubscriptionKind.BusinessUnlimited => 30,
            _ => 0
        };
    }
}