using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NorthwindData;
using NorthwindBusiness;
using Microsoft.EntityFrameworkCore;
using NorthwindData.Services;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _sut;

        [Test]
        public void BeAbleToBeConstructed()
        {
            //Arrange
            var mockCustomerService = new Mock<ICustomerServices>();

            //Act
            _sut = new CustomerManager(mockCustomerService.Object);

            //Assert 
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        // BEHAVE LIKE mockCustomer exists and read the code / go through the methods.
        [Test]
        public void ReturnTrue_WhenUpdateIsCalled_WithValidId()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer { CustomerId = "ROCK" };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.That(result);
        }
        
        [Test]
        public void UpdateSelectedCustomer_WhenUpdateIsCalled()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer 
            { 
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", "Rocky Raccoon", "UK", "Chester", null);

            //Assert
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("UK"));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Chester"));
        }

        [Test]
        public void ReturnFalse_WhenUpdateIsCalledWithInvalidId()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer { CustomerId = "ROCK" };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void DoesNotUpdateSelectedCustomer_WhenUpdateIsCalled_WithInvalidId()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);

            _sut = new CustomerManager(mockCustomerService.Object);

            _sut.SelectedCustomer = originalCustomer;

            //Act
            var result = _sut.Update("ROCK", "Rocky Raccoon", "UK", "Chester", null);

            //Assert
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.Null);
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
        }

        [Test]
        public void DoesRemoveCustomer_WhenRemoveFuncIsCalled()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;

            //Act
            _sut.Delete(originalCustomer.CustomerId);

            //Assert
            Assert.That(_sut.SelectedCustomer, Is.Null);

        }

        [Test]
        public void DoesNotRemoveCustomer_WhenRemoveFunctionIsCalled_WithNullCustomer_ReturnsFalse()
        {
            //arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            };

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;

            //Act
            //_sut.Delete(originalCustomer.CustomerId);

            //Assert
            Assert.That(_sut.Delete(originalCustomer.CustomerId), Is.False);
        }

    }
}
