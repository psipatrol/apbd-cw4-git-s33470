namespace LegacyRenewalApp.pricing;

public interface ISupportFeeCalculator
{
    decimal CalculateSupportFee(string planCode, bool includePremiumSupport);
    string GetNoteForSupportFee(string planCode, bool includePremiumSupport);
}