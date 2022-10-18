using Core.Redis;
using Dal.Abstract;
using Dal.Concrete;
using Dal.Concrete.EF;
using DataAccess.Concrete.EF.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DataAccess.Concrete.EF;
using Dal.UnitOfWork;

namespace Bll.Extension
{
    public static class ApiExtension
    {
        public static IServiceCollection DependencySet(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
            services.AddScoped<ICategoryAttributeRepository, CategoryAttributeRepository>();
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
            //redis
            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
        public static void CreateDb(this IApplicationBuilder app)

        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

                var context = serviceScope.ServiceProvider.GetRequiredService<BoynerContext>();

                context.Database.EnsureCreated();

            }

        }
    }
}
