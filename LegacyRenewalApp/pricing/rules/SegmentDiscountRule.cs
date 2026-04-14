using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.pricing;

public class SegmentDiscountRule : IDiscountRule
{
    public string? Note { get; private set; }

    public decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount)
    {
        decimal discountAmount = 0m;

        switch (customer.Segment)
        {
            case "Silver":
                discountAmount += baseAmount * 0.05m;
                Note = "silver discount";
                break;
            case "Gold":
                discountAmount += baseAmount * 0.10m;
                Note = "gold discount";
                break;
            case "Platinum":
                discountAmount += baseAmount * 0.15m;
                Note = "platinum discount";
                break;
            case "Education" when plan.IsEducationEligible:
                discountAmount += baseAmount * 0.20m;
                Note = "education discount";
                break;
            default:
                Note = null;
                break;
        }
        
        return discountAmount;
    }
}