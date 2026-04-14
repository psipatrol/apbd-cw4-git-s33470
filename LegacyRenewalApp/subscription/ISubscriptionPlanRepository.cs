namespace LegacyRenewalApp.interfaces;

public interface ISubscriptionPlanRepository
{
    SubscriptionPlan GetByCode(string code);
}