namespace LegacyRenewalApp.pricing;

public interface IPaymentFeeCalculator
{
    decimal CalculatePaymentFee(string paymentMethod, decimal amountBase);
    string GetNoteForPaymentMethod(string paymentMethod);
}