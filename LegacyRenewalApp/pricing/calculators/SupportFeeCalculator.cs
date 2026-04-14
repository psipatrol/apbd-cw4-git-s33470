namespace LegacyRenewalApp.pricing;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    public decimal CalculateSupportFee(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport) return 0m;

        return planCode?.ToUpperInvariant() switch
        {
            "START" => 250m,
            "PRO" => 400m,
            "ENTERPRISE" => 700m,
            _ => 0m
        };
    }

    public string GetNoteForSupportFee(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport) return null;
        return "premium support included";    
    }
}