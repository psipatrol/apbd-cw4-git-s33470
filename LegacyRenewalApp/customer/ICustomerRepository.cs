namespace LegacyRenewalApp.interfaces;

public interface ICustomerRepository
{
    Customer GetById(int customerId);
}