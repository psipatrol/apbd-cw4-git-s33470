using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.adapters;

public class SubscriptionPlanRepositoryAdapter : ISubscriptionPlanRepository
{
    private SubscriptionPlanRepository _planRepository = new SubscriptionPlanRepository();


    public SubscriptionPlan GetByCode(string code)
    {
        return _planRepository.GetByCode(code);
    }
}