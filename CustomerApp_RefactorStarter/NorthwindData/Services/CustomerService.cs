﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindData.Services
{
    public class CustomerService : ICustomerServices
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
            _context.Customers.Add(c);
            SaveCustomerChanges();
        }

        public Customer GetCustomerById(string customerId)
        {
            return _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
        }

        public List<Customer> GetCustomerList()
        {
            return _context.Customers.ToList();
        }

        public void RemoveCustomer(Customer c)
        {
            _context.Customers.Remove(c);
            SaveCustomerChanges();
        }

        public void SaveCustomerChanges()
        {
            _context.SaveChanges();
        }
    }
}
