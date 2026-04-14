namespace LegacyRenewalApp.pricing;

public class PricingResult
{
    public decimal BaseAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubtotalAfterDiscount { get; set; }
    public string Notes { get; set; } = string.Empty;
}