namespace LegacyRenewalApp.interfaces;

public interface IDiscountRule
{
    string? Note { get; }
    decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount);
}