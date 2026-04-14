using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.pricing;

public class TeamSizeDiscountRule : IDiscountRule
{
    public string? Note { get; private set; }
    public decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount)
    {
        decimal discountAmount = 0m;
        
        switch (seatCount)
        {
            case >= 50:
                discountAmount += baseAmount * 0.12m;
                Note = "large team discount";
                break;
            case >= 20:
                discountAmount += baseAmount * 0.08m;
                Note = "medium team discount";
                break;
            case >= 10:
                discountAmount += baseAmount * 0.04m;
                Note = "small team discount";
                break;
            default:
                Note = null;
                break;
        }
        
        return discountAmount;
    }
}