using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using NorthwindData;
using NorthwindData.Services;


namespace NorthwindBusiness
{
    public class CustomerManager
    {

        private ICustomerServices _service;

        public CustomerManager()
        {
            _service = new CustomerService();
        }
        public CustomerManager(ICustomerServices service)
        {
            if(service == null)
            {
                throw new ArgumentException("ICustomerService object cannot be null.");
            }
            _service = service;
        }


        public Customer SelectedCustomer { get; set; }

        public void SetSelectedCustomer(object selectedItem)
        {
            SelectedCustomer = (Customer)selectedItem;
        }

        public List<Customer> RetrieveAll()
        {
            //using (var db = new NorthwindContext())
            //{
            //    return db.Customers.ToList();
            //}
            return _service.GetCustomerList();
        }

        public void Create(string customerId, string contactName, string companyName, string city = null)
        {
            var newCust = new Customer() { CustomerId = customerId, ContactName = contactName, CompanyName = companyName };
            _service.CreateCustomer(newCust);

            //using (var db = new NorthwindContext())
            //{
            //    db.Customers.Add(newCust);
            //    db.SaveChanges();
            //}

        }

        public bool Update(string customerId, string contactName, string country, string city, string postcode)
        {
            //using (var db = new NorthwindContext())
            //{
                
            //}
            var customer = _service.GetCustomerById(customerId);
            if (customer == null)
            {
                Debug.WriteLine($"Customer {customerId} not found");
                return false;
            }
            customer.ContactName = contactName;
            customer.City = city;
            customer.PostalCode = postcode;
            customer.Country = country;
            // write changes to database
            try
            {
                //db.SaveChanges();
                _service.SaveCustomerChanges();
                SelectedCustomer = customer;
            }
            catch (Exception e) // an exception can be thrown if the database has been updated since last loaded 
            {
                Debug.WriteLine($"Error updating {customerId}");
                return false;
            }
            return true;
        }

        public bool Delete(string customerId)
        {
            var customer = _service.GetCustomerById(customerId);
            if (customer == null)
            {
                Debug.WriteLine($"Customer {customerId} not found");
                return false;
            }
            _service.RemoveCustomer(customer);
            SelectedCustomer = null;

            return true;
        }
    }
}
