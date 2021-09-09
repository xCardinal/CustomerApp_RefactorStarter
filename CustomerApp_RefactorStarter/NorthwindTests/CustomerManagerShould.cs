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
        //private CustomerManager _sut;

        [Test]
        public void BeAbleToBeConstructed()
        {
            //Arrange
            var mockCustomerService = new Mock<ICustomerServices>();

            //Act
            var _sut = new CustomerManager(mockCustomerService.Object);

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
            var _sut = new CustomerManager(mockCustomerService.Object);

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
            var _sut = new CustomerManager(mockCustomerService.Object);

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
            var _sut = new CustomerManager(mockCustomerService.Object);

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

            var _sut = new CustomerManager(mockCustomerService.Object);

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
            var _sut = new CustomerManager(mockCustomerService.Object);
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
            var _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;

            //Act
            //_sut.Delete(originalCustomer.CustomerId);

            //Assert
            Assert.That(_sut.Delete(originalCustomer.CustomerId), Is.False);
        }

        [Test]
        public void ReturnFalse_WhenUpdateIsCalled_AndADatabaseExceptionIsThrown()
        {
            //Arrange
            var mockCustomerService = new Mock<ICustomerServices>();
            var originalCustomer = new Customer();

            mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(originalCustomer);
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();

            var _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert 
            Assert.That(result, Is.False);
        }

        [Test]
        public void NotChangedTheSelectedCustomer_WhenUpdateIsCalled_AndADAtabaseExceptionIsThrown()
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
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();

            var _sut = new CustomerManager(mockCustomerService.Object);

            _sut.SelectedCustomer = new Customer
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            }; ;

            //Act
            var result = _sut.Update("ROCK", "Rocky Raccoon", "UK", "Chester", null);

            //Assert - THESE ARE CHECKING THE LOCAL VARIABLES 
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.Null);
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
        }


        [Test]
        public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithValidId()
        {
            //Arrange 
            var mockCustomerService = new Mock<ICustomerServices>();
            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(new Customer());

            var _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Once);
        }

        [Test]
        public void LetsSeeWhatHappens_WhenUpdateIsCalled_AndAllInvocationsArentSetUp()
        {
            //Arrange 
            var mockCustomerService = new Mock<ICustomerServices>(MockBehavior.Strict);

            //MockBheavior.Stric means that I need to set up all of the methods that will be called inside of the Mock
            //, otherwise they don't run.

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges());
            var _sut = new CustomerManager(mockCustomerService.Object);

            //Act
            var result = _sut.Update("ROCK", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.That(result);
        }
    }
}
