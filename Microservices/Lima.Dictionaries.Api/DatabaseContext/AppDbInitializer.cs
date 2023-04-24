using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Lima.Dictionaries.Api.Domain;

namespace Lima.Dictionaries.Api.DatabaseContext
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DictionaryDbContext>();
                context.Database.EnsureCreated();
                if (!context.ProductDomains.Any())
                {
                    context.ProductDomains.AddRange(new List<ProductDomain>()
                    {
                        new ProductDomain()
                        {
                            Id =0,
                            Title="Worker",
                            Manufacturer = "DreamJob",
                            Country="Somewhere",
                            Price = 100,
                            Quantity=44,
                            UnitOfMeasurements ="In packs",
                            PhotoLink ="",
                        },
                        new ProductDomain()
                        {
                            Id =1,
                            Title="Worker1",
                            Manufacturer = "DreamJob1",
                            Country="Somewhere1",
                            Price = 1001,
                            Quantity=441,
                            UnitOfMeasurements ="In packs1",
                            PhotoLink ="",
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
