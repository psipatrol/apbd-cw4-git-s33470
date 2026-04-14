namespace LegacyRenewalApp.pricing;

public class TaxCalculator : ITaxCalculator
{
    public decimal GetTaxRateForCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country)) return 0.20m;

        return country.Trim() switch
        {
            "Poland" => 0.23m,
            "Germany" => 0.19m,
            "Czech Republic" => 0.21m,
            "Norway" => 0.25m,
            _ => 0.20m
        };    }

    public decimal CalculateTax(decimal taxBase, string country)
    {
        var rate = GetTaxRateForCountry(country);
        return MoneyRounder.RoundMoney(taxBase * rate);    }

    public string GetTaxNote(string country)
    {
        return $"tax rate applied: {GetTaxRateForCountry(country):P0}";
        
    }
}