using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using NorthwindData;
using NorthwindData.Services;

namespace NorthwindTests
{
    public class CustomerServiceTests
    {
        private CustomerService _sut;
        private NorthwindContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "Example_DB").Options;

            _context = new NorthwindContext(options);
            _sut = new CustomerService(_context);


            //seed the database
            _sut.CreateCustomer(new Customer
            {
                CustomerId = "MAND",
                ContactName = "Nish Mandal",
                CompanyName = "Spart Global",
                City = "Paris"
            }
            );
            _sut.CreateCustomer(new Customer
            {
                CustomerId = "FREN",
                ContactName = "Cathy French",
                CompanyName = "Spart Global",
                City = "Ottawa"
            }
            );

        }

        [Test]
        public void GivenAValidId_CorrectCustomerIsReturned()
        {
            var result = _sut.GetCustomerById("MAND");

            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Nish Mandal"));

        }

        [Test]
        public void GivenANewCustomer_CreateCustomerAddsItToTheDatabase()
        {

            //Arrange 
            var numberOfCustomersBefore = _context.Customers.Count();
            var newCustomer = new Customer() { CustomerId = "BEAR", ContactName = "Martin Beard", CompanyName = "Sparta Global", City = "Rome" };

            //ACT
            _sut.CreateCustomer(newCustomer);

            var numberOfCustomersAfter = _context.Customers.Count();
            var result = _sut.GetCustomerById("BEAR");

            //Assert
            Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));
            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Martin Beard"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(result.City, Is.EqualTo("Rome"));

            //Clean Up
            _context.Customers.Remove(newCustomer);
            _context.SaveChanges();

        }

        [Test]
        public void GivenIWantToRemoveACustomer_ReturnMinusOne()
        {
            var numberOfCustomersBefore = _context.Customers.Count();
            
            _sut.RemoveCustomer(_sut.GetCustomerById("MAND"));

            var numberOfCustomersAfter = _context.Customers.Count();

            Assert.That(numberOfCustomersBefore - 1, Is.EqualTo(numberOfCustomersAfter));

        }

        [Test]
        public void RetrieveCustomerList()
        {
            Assert.That(_sut.GetCustomerList, Is.EqualTo(_context.Customers.ToList()));
        }

        
    }
}
