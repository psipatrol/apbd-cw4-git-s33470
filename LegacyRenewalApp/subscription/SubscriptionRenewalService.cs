using System;
using System.Collections.Generic;
using LegacyRenewalApp.adapters;
using LegacyRenewalApp.interfaces;
using LegacyRenewalApp.invoice;
using LegacyRenewalApp.pricing;
using LegacyRenewalApp.validation;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private IBillingGateway _billingGateway;
        private ICustomerRepository _customerRepository;
        private ISubscriptionPlanRepository _planRepository;
        private IPricingCalculator _pricingCalculator;
        private ISupportFeeCalculator _supportFeeCalculator;
        private IPaymentFeeCalculator _paymentFeeCalculator;
        private ITaxCalculator _taxCalculator;
        private IInvoiceFactory _invoiceFactory;
        private IValidator<CreateRenewalRequest> _requestValidator;
        
        public SubscriptionRenewalService()
        {
            this._billingGateway = new LegacyBillingGatewayAdapter();
            this._customerRepository = new CustomerRepositoryAdapter();
            this._planRepository = new SubscriptionPlanRepositoryAdapter();
            this._pricingCalculator = CreateDefaultPricingCalculator();
            this._supportFeeCalculator = new SupportFeeCalculator();
            this._paymentFeeCalculator = new PaymentFeeCalculator();
            this._taxCalculator = new TaxCalculator();
            this._invoiceFactory =  new InvoiceFactory();
            this._requestValidator = new CreateRenewalRequestValidator();
        }

        IPricingCalculator CreateDefaultPricingCalculator()
        {
            var rules = new IDiscountRule[]
            {
                new SegmentDiscountRule(),
                new LoyaltyYearsDiscountRule(),
                new TeamSizeDiscountRule(),
                new LoyaltyPointsRule()
            };
            return new PricingCalculator(rules);
        }
        
        public SubscriptionRenewalService
        (
            IBillingGateway billingGateway,
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IPricingCalculator pricingCalculator,
            ISupportFeeCalculator supportFeeCalculator,
            IPaymentFeeCalculator paymentFeeCalculator,
            ITaxCalculator taxCalculator,
            IInvoiceFactory invoiceFactory,
            IValidator<CreateRenewalRequest> requestValidator
        )
        {
            this._billingGateway = billingGateway;
            this._customerRepository = customerRepository;
            this._planRepository = planRepository;
            this._pricingCalculator = pricingCalculator;
            this._supportFeeCalculator = supportFeeCalculator;
            this._paymentFeeCalculator = paymentFeeCalculator;
            this._taxCalculator = taxCalculator;
            this._invoiceFactory = invoiceFactory;
            this._requestValidator = requestValidator;
        }
        
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {

            var request = new CreateRenewalRequest
            {
                CustomerId = customerId,
                PlanCode = planCode,
                SeatCount = seatCount,
                PaymentMethod = paymentMethod,
                IncludePremiumSupport = includePremiumSupport,
                UseLoyaltyPoints = useLoyaltyPoints
            };
            
            var validationResult = _requestValidator.Validate(request);
            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join("; ", validationResult.Errors));
            

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }
            var pricing = _pricingCalculator.CalculatePrice(customer, plan, seatCount, useLoyaltyPoints);

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            decimal discountAmount = pricing.DiscountAmount;
            decimal subtotalAfterDiscount = pricing.SubtotalAfterDiscount;
            
            var noteParts = new List<string>();
            string notes = pricing.Notes;
            if (!string.IsNullOrWhiteSpace(pricing.Notes)) 
                noteParts.Add(pricing.Notes);
            
            decimal supportFee = _supportFeeCalculator.CalculateSupportFee(normalizedPlanCode, includePremiumSupport);
            if (!string.IsNullOrWhiteSpace(_supportFeeCalculator.GetNoteForSupportFee(normalizedPlanCode, includePremiumSupport)))
                noteParts.Add(_supportFeeCalculator.GetNoteForSupportFee(normalizedPlanCode, includePremiumSupport));
            
            decimal paymentFee = _paymentFeeCalculator.CalculatePaymentFee(normalizedPaymentMethod, subtotalAfterDiscount + supportFee);
            noteParts.Add(_paymentFeeCalculator.GetNoteForPaymentMethod(normalizedPaymentMethod));
            
            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = _taxCalculator.CalculateTax(taxBase, customer.Country);

            decimal finalAmount = taxBase + taxAmount;
            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                noteParts.Add("minimum invoice amount applied; ");
            }

            var invoice = _invoiceFactory.CreateInvoice(
                customerId: customerId,
                customer: customer,
                plan: plan,
                normalizedPlanCode: normalizedPlanCode,
                normalizedPaymentMethod: normalizedPaymentMethod,
                seatCount: seatCount,
                baseAmount: baseAmount,
                discountAmount: discountAmount,
                subtotalAfterDiscount: subtotalAfterDiscount,
                supportFee: supportFee,
                paymentFee: paymentFee,
                taxAmount: taxAmount,
                finalAmount: finalAmount,
                notes: string.Join("; ", noteParts).Trim() + ";",
                generatedAt: DateTime.UtcNow
                );

            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
