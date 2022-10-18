using Dal.Concrete.EF;
using DataAccess.Concrete.EF.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class Product
    {
        [Fact]
        public void GetAll()
        {
            // arrange
            var context = CreateOrderContext();
            var service = new ProductRepository(context.Object);

            // act
            var results = service.GetAll();
            var count = results.Data.Count();

            // assert
            Assert.Equal(2, count);

        }
        private Mock<BoynerContext> CreateOrderContext()
        {
            var persons = FakeData().AsQueryable();

            var dbSet = new Mock<DbSet<Core.Entity.Product>>();
            dbSet.As<IQueryable<Core.Entity.Product>>().Setup(m => m.Provider).Returns(persons.Provider);
            dbSet.As<IQueryable<Core.Entity.Product>>().Setup(m => m.Expression).Returns(persons.Expression);
            dbSet.As<IQueryable<Core.Entity.Product>>().Setup(m => m.ElementType).Returns(persons.ElementType);
            dbSet.As<IQueryable<Core.Entity.Product>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            var context = new Mock<BoynerContext>();
            context.Setup(c => c.Product).Returns(dbSet.Object);
            return context;

        }
        private IEnumerable<Core.Entity.Product> FakeData()
        {
            return new List<Core.Entity.Product>
           {
               new Core.Entity.Product
               {
                   Id = 1,
                   Name = "Test 1",
                   Price = 100,
                   IsActive = true
               },
                new Core.Entity.Product
               {
                   Id = 2,
                   Name = "Test 2",
                   Price =200,
                   IsActive = true
               }
           };
        }

    }
}
