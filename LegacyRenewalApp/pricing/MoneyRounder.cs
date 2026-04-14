using System;

namespace LegacyRenewalApp.pricing;

public static class MoneyRounder
{
    public static decimal RoundMoney(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}