using System.Collections.Generic;

namespace NorthwindData.Services
{
    public interface ICustomerServices
    {
        List<Customer> GetCustomerList();
        public Customer GetCustomerById();
        public void CreateCustomer(Customer c);
        public void SaveCustomerChanges();
        public void RemoveCustomer(Customer c);
    }
}
