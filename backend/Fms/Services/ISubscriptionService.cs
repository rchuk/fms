using Fms.Dtos;
using Fms.Entities.Enums;

namespace Fms.Services;

public interface ISubscriptionService
{
    public Task BuySubscription(BuySubscriptionRequestDto request);
    public Task<SubscriptionKind?> GetCurrentUserSubscription();
}