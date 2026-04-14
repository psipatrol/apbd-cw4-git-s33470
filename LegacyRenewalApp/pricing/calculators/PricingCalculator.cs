using System;
using System.Collections;
using System.Collections.Generic;
using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.pricing;

public class PricingCalculator : IPricingCalculator
{
    private IEnumerable<IDiscountRule> _discountRules;

    public PricingCalculator(IEnumerable<IDiscountRule> discountRules)
    {
        _discountRules = discountRules;
    }
    
    public PricingResult CalculatePrice(Customer customer, SubscriptionPlan plan, int seatCount, bool useLoyaltyPoints)
    {
        if (customer == null)
            throw new ArgumentException("customer cannot be null");
        if (plan == null)
            throw new ArgumentException("plan cannot be null");
        
        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
        decimal discountAmount = 0m;
        var notes = new List<string>();

        foreach (var rule in _discountRules)
        {
            if (rule is LoyaltyPointsRule && !useLoyaltyPoints)
                continue;

            decimal d = rule.CalculateDiscount(customer, plan, seatCount, baseAmount);
            if (d > 0)
            {
                discountAmount += d;
                if (!string.IsNullOrWhiteSpace(rule.Note))
                    notes.Add(rule.Note);
            }
        }
        
        decimal subtotalAfterDiscount = baseAmount - discountAmount;
        if (subtotalAfterDiscount < 300m)
        {
            subtotalAfterDiscount = 300m;
            notes.Add("minimum discounted subtotal applied");
        }

        return new PricingResult {
            BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
            DiscountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero),
            SubtotalAfterDiscount = Math.Round(subtotalAfterDiscount, 2, MidpointRounding.AwayFromZero),
            Notes =  string.Join("; ", notes).Trim()
        };
    }
}