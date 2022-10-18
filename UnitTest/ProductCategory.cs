using Dal.Concrete.EF;
using DataAccess.Concrete.EF.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class ProductCategory
    {
        [Fact]
        public void GetAll()
        {
            // arrange
            var context = CreateOrderContext();
            var service = new ProductCategoryRepository(context.Object);

            // act
            var results = service.GetAll();
            var count = results.Data.Count();

            // assert
            Assert.Equal(2, count);

        }
        private Mock<BoynerContext> CreateOrderContext()
        {
            var persons = FakeData().AsQueryable();

            var dbSet = new Mock<DbSet<Core.Entity.ProductCategory>>();
            dbSet.As<IQueryable<Core.Entity.ProductCategory>>().Setup(m => m.Provider).Returns(persons.Provider);
            dbSet.As<IQueryable<Core.Entity.ProductCategory>>().Setup(m => m.Expression).Returns(persons.Expression);
            dbSet.As<IQueryable<Core.Entity.ProductCategory>>().Setup(m => m.ElementType).Returns(persons.ElementType);
            dbSet.As<IQueryable<Core.Entity.ProductCategory>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            var context = new Mock<BoynerContext>();
            context.Setup(c => c.ProductCategory).Returns(dbSet.Object);
            return context;

        }
        private IEnumerable<Core.Entity.ProductCategory> FakeData()
        {
            return new List<Core.Entity.ProductCategory>
           {
               new Core.Entity.ProductCategory
               {
                   Id = 1,
                   Name = "Test 1",
                   IsActive = true
               },
                new Core.Entity.ProductCategory
               {
                   Id = 2,
                   Name = "Test 2",
                   IsActive = true
               }
           };
        }

    }
}
