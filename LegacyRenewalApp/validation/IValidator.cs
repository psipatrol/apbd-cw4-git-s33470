namespace LegacyRenewalApp.validation;

public interface IValidator<T>
{
    ValidationResult Validate(T instance);
}