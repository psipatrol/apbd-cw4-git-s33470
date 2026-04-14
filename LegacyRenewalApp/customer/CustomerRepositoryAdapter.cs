using LegacyRenewalApp.interfaces;

namespace LegacyRenewalApp.adapters;

public class CustomerRepositoryAdapter : ICustomerRepository
{
    private CustomerRepository _customerRepository = new CustomerRepository();
    
    public Customer GetById(int customerId)
    {
        return _customerRepository.GetById(customerId);
    }
}