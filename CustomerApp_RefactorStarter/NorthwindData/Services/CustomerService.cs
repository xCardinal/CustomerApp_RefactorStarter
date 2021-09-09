using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindData.Services
{
    class CustomerService : ICustomerServices
    {
        private readonly NorthwindContext _context;

        // Use dependency injection !

        public CustomerService(NorthwindContext context)
        {
            _context = context;
        }
        public CustomerService()
        {
            _context = new NorthwindContext();
        }

        public void CreateCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById()
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetCustomerList()
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        public void SaveCustomerChanges()
        {
            throw new NotImplementedException();
        }
    }
}
