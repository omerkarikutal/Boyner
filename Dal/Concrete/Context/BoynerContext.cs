using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EF.Context
{
    public class BoynerContext : DbContext
    {
        public BoynerContext() : base()
        {

        }
        public BoynerContext(DbContextOptions<BoynerContext> options) : base(options) { }



        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Core.Entity.Attribute> Attribute { get; set; }
        public DbSet<AttributeValue> AttributeValue { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<CategoryAttribute> CategoryAttribute { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var attr = new List<Core.Entity.Attribute>();
            var attrValue = new List<AttributeValue>();
            var prdList = new List<Product>();
            var prdCategoryList = new List<ProductCategory>();
            var prdAttributeList = new List<ProductAttribute>();
            var categoryAttribute = new List<CategoryAttribute>();


            for (int i = 1; i < 10; i++)
            {
                var att = new Core.Entity.Attribute { IsActive = true, Name = $"Test-{i}", Id = i };

                var attvl = new AttributeValue { IsActive = true, AttributeId = att.Id, Value = $"Test Value - {i}", Id = i };

                var prdCategory = new ProductCategory { Id = i, IsActive = true, Name = $"Kategori-{i}" };

                var prd = new Product { Id = i, IsActive = true, Name = $"Product-{i}", Price = i * 10, ProductCategoryId = i };

                var prdAttribute = new ProductAttribute { Id = i, IsActive = true, AttributeValueId = i, ProductId = i };

                var catAtrr = new CategoryAttribute { AttributeValueId = i, IsActive = true, ProductCategoryId = i, Id = i };

                attr.Add(att);
                attrValue.Add(attvl);
                prdCategoryList.Add(prdCategory);
                prdList.Add(prd);
                prdAttributeList.Add(prdAttribute);
                categoryAttribute.Add(catAtrr);
            }

            modelBuilder.Entity<Core.Entity.Attribute>()
                .HasData(attr);
            modelBuilder.Entity<AttributeValue>()
                .HasData(attrValue);
            modelBuilder.Entity<Product>()
                .HasData(prdList);
            modelBuilder.Entity<ProductCategory>()
                .HasData(prdCategoryList);
            modelBuilder.Entity<ProductAttribute>()
                .HasData(prdAttributeList);
            modelBuilder.Entity<CategoryAttribute>()
                .HasData(categoryAttribute);


            base.OnModelCreating(modelBuilder);
        }
    }
}
