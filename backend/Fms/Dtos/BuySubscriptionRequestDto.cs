using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class BuySubscriptionRequestDto
{
    [Required]
    public required SubscriptionKind Kind { get; set; }
}