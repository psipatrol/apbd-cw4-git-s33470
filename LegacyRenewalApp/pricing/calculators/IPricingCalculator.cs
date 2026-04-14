using LegacyRenewalApp.pricing;

namespace LegacyRenewalApp.interfaces;

public interface IPricingCalculator
{
    PricingResult CalculatePrice(Customer customer, SubscriptionPlan plan, int seatCount, bool useLoyaltyPoints);
}