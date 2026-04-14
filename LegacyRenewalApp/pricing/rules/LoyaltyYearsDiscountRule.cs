using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.pricing;

public class LoyaltyYearsDiscountRule : IDiscountRule
{
    public string? Note { get; private set; }
    public decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount)
    {
        decimal discountAmount = 0m;
        
        switch (customer.YearsWithCompany)
        {
            case >= 5:
                discountAmount += baseAmount * 0.07m;
                Note = "long-term loyalty discount";
                break;
            case >= 2:
                discountAmount += baseAmount * 0.03m;
                Note = "basic loyalty discount";
                break;
            default:
                Note = null;
                break;
        }
        
        return discountAmount;
    }
}