namespace LegacyRenewalApp.pricing;

public interface ITaxCalculator
{
    decimal GetTaxRateForCountry(string country);
    decimal CalculateTax(decimal taxBase, string country);
    string GetTaxNote(string country);
}