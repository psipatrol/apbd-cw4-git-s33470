using System;

namespace LegacyRenewalApp.invoice;

public interface IInvoiceFactory
{
    RenewalInvoice CreateInvoice(
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
        DateTime generatedAt
    );
}