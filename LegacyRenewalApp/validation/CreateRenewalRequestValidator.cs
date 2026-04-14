using System.Collections.Generic;

namespace LegacyRenewalApp.validation;

public class CreateRenewalRequestValidator : IValidator<CreateRenewalRequest>
{
    public ValidationResult Validate(CreateRenewalRequest request)
    {
        var errors = new List<string>();

        if (request.CustomerId <= 0) errors.Add("Customer id must be positive");
        if (string.IsNullOrWhiteSpace(request.PlanCode)) errors.Add("Plan code is required");
        if (request.SeatCount <= 0) errors.Add("Seat count must be positive");
        if (string.IsNullOrWhiteSpace(request.PaymentMethod)) errors.Add("Payment method is required");
        if (request.SeatCount > 10000) errors.Add("Seat count is unreasonably large");

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors.ToArray());
    }
}