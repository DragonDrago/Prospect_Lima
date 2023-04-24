using Lima.SalesProCRM.Api.Domains;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lima.SalesProCRM.Api
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SalesProCrmDbContext>();
                context.Database.EnsureCreated();
                if (!context.SalesProCrmDomains.Any())
                {
                    context.SalesProCrmDomains.AddRange(new List<SalesProCrmDomain>()
                    {
                        new SalesProCrmDomain()
                        {
                            Id =0,
                            DateTime=DateTime.Now,
                            Check = "something",
                            LdiId=0,
                            LdiName ="My Name Is KHAN",
                            TotalSum=120,
                            ProductsId ="Ceo of the all ceo",
                            LdiAttachedTo ="Somewhere in the world",
                            LdiStatus = "Delivered"
                        },
                        new SalesProCrmDomain()
                        {
                            Id =1,
                            DateTime=DateTime.Now,
                            Check = "something",
                            LdiId=1,
                            LdiName ="My Name Is KHAN",
                            TotalSum=140,
                            ProductsId ="Ceo of the all ceo",
                            LdiAttachedTo ="Somewhere in the world",
                            LdiStatus = "On the way"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
