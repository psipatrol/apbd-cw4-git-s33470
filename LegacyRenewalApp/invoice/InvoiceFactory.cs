using System;
using LegacyRenewalApp.pricing;

namespace LegacyRenewalApp.invoice;

public class InvoiceFactory : IInvoiceFactory
{
    public RenewalInvoice CreateInvoice(
        int customerId,
        Customer customer,
        SubscriptionPlan plan,
        string normalizedPlanCode,
        string normalizedPaymentMethod,
        int seatCount,
        decimal baseAmount,
        decimal discountAmount,
        decimal subtotalAfterDiscount,
        decimal supportFee,
        decimal paymentFee,
        decimal taxAmount,
        decimal finalAmount,
        string notes,
        DateTime generatedAt)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));

        return new RenewalInvoice
        {
            InvoiceNumber = $"INV-{generatedAt:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
            CustomerName = customer.FullName,
            PlanCode = normalizedPlanCode,
            PaymentMethod = normalizedPaymentMethod,
            SeatCount = seatCount,
            BaseAmount = MoneyRounder.RoundMoney(baseAmount),
            DiscountAmount = MoneyRounder.RoundMoney(discountAmount),
            SupportFee = MoneyRounder.RoundMoney(supportFee),
            PaymentFee = MoneyRounder.RoundMoney(paymentFee),
            TaxAmount = MoneyRounder.RoundMoney(taxAmount),
            FinalAmount = MoneyRounder.RoundMoney(finalAmount),
            Notes = (notes ?? string.Empty).Trim(),
            GeneratedAt = generatedAt
        };
    }
}
    
