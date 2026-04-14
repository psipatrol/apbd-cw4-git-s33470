using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.pricing;

public class LoyaltyPointsRule : IDiscountRule
{
    public string Note { get; private set; }
    public decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount)
    {
        decimal discountAmount = 0m;
        if (customer.LoyaltyPoints > 0)
        {
            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            discountAmount += pointsToUse;
            Note = $"loyalty points used: {pointsToUse}";
        }
        else
        {
            Note = null;
        }
        return discountAmount;
    }
}