using System;

namespace LegacyRenewalApp.pricing;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    public decimal CalculatePaymentFee(string paymentMethod, decimal amountBase)
    {
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required");

        var normalized = paymentMethod.Trim().ToUpperInvariant();
        return normalized switch
        {
            "CARD" => MoneyRounder.RoundMoney(amountBase * 0.02m),
            "BANK_TRANSFER" => MoneyRounder.RoundMoney(amountBase * 0.01m),
            "PAYPAL" => MoneyRounder.RoundMoney(amountBase * 0.035m),
            "INVOICE" => 0m,
            _ => throw new ArgumentException("Unsupported payment method")
        };    
    }

    public string GetNoteForPaymentMethod(string paymentMethod)
    {
        var normalized = paymentMethod?.Trim().ToUpperInvariant();
        return normalized switch
        {
            "CARD" => "card payment fee",
            "BANK_TRANSFER" => "bank transfer fee",
            "PAYPAL" => "paypal fee",
            "INVOICE" => "invoice payment",
            _ => "unsupported payment method"
        };
    }
}