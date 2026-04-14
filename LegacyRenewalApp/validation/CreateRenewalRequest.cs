namespace LegacyRenewalApp.validation;

public class CreateRenewalRequest
{
    public int CustomerId { get; init; }
    public string PlanCode { get; init; } = string.Empty;
    public int SeatCount { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public bool IncludePremiumSupport { get; init; }
    public bool UseLoyaltyPoints { get; init; }
}