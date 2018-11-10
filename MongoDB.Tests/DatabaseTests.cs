using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        public DatabaseTests()
        {
            const string connectionString = "mongodb://localhost/Database";
            var databaseContext = new DatabaseContext(connectionString);
            CustomerRepository = new CustomerRepository(databaseContext);
        }

        public ICustomerRepository CustomerRepository { get; }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAdd()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAddAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.AddAsync(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAddRange()
        {
            var count = CustomerRepository.Count();
            CustomerRepository.AddRange(new List<Customer> { CreateCustomer() });
            Assert.IsTrue(CustomerRepository.Count() > count);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAddRangeAsync()
        {
            var count = CustomerRepository.Count();
            CustomerRepository.AddRangeAsync(new List<Customer> { CreateCustomer() });
            Assert.IsTrue(CustomerRepository.Count() > count);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAny()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.Any());
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAnyAsync()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.AnyAsync().Result);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAnyWhere()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.Any(x => x.Id == customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryAnyWhereAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.AnyAsync(x => x.Id == customer.Id).Result);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryCount()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.Count() > 0);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryCountAsync()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.CountAsync().Result > 0);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryCountWhere()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.Count(x => x.Id == customer.Id) == 1);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryCountWhereAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.CountAsync(x => x.Id == customer.Id).Result == 1);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryDelete()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
            CustomerRepository.Delete(customer.Id);
            Assert.IsNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryDeleteAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
            CustomerRepository.DeleteAsync(customer.Id);
            Assert.IsNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryDeleteWhere()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
            CustomerRepository.Delete(x => x.Id == customer.Id);
            Assert.IsNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryDeleteWhereAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
            CustomerRepository.DeleteAsync(x => x.Id == customer.Id);
            Assert.IsNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryFirstOrDefault()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.FirstOrDefault(x => x.Id == customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryFirstOrDefaultAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.FirstOrDefaultAsync(x => x.Id == customer.Id).Result);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryList()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.List().Any());
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryListAsync()
        {
            CustomerRepository.Add(CreateCustomer());
            Assert.IsTrue(CustomerRepository.ListAsync().Result.Any());
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryListWhere()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.List(x => x.Id == customer.Id).Any());
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryListWhereAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsTrue(CustomerRepository.ListAsync(x => x.Id == customer.Id).Result.Any());
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositorySelect()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.Select(customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositorySelectAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.SelectAsync(customer.Id).Result);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositorySingleOrDefault()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.SingleOrDefault(x => x.Id == customer.Id));
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositorySingleOrDefaultAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            Assert.IsNotNull(CustomerRepository.SingleOrDefaultAsync(x => x.Id == customer.Id).Result);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryUpdate()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            var oldName = customer.Name;
            customer.Name = Guid.NewGuid().ToString();
            CustomerRepository.Update(customer, customer.Id);
            customer = CustomerRepository.Select(customer.Id);
            Assert.AreNotEqual(oldName, customer.Name);
        }

        [TestMethod]
        public void DatabaseTestsCustomerRepositoryUpdateAsync()
        {
            var customer = CreateCustomer();
            CustomerRepository.Add(customer);
            var oldName = customer.Name;
            customer.Name = Guid.NewGuid().ToString();
            CustomerRepository.UpdateAsync(customer, customer.Id);
            customer = CustomerRepository.Select(customer.Id);
            Assert.AreNotEqual(oldName, customer.Name);
        }

        private static Customer CreateCustomer()
        {
            return new Customer { Id = ObjectId.GenerateNewId(), Name = Guid.NewGuid().ToString() };
        }
    }
}
