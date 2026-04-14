using System;
using System.Collections.Generic;
using System.Linq;

namespace LegacyRenewalApp.validation;

public class ValidationResult
{
    public bool IsValid { get; }
    public List<string> Errors { get; }

    public ValidationResult(bool isValid, IEnumerable<string>? errors = null)
    {
        IsValid = isValid;
        Errors = errors?.ToList() ?? new List<string>();
    }

    public static ValidationResult Success() => new ValidationResult(true);
    public static ValidationResult Failure(params string[] errors) => new ValidationResult(false, errors);
}