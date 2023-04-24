using Lima.Leads.Api.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Lima.Leads.Api
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<LeadsDbContext>();
                context.Database.EnsureCreated();
                if (!context.LeadsDomains.Any())
                {
                    context.LeadsDomains.AddRange(new List<LeadsDomain>()
                    {
                        new LeadsDomain()
                        {
                            Id =0,
                            Status="Delivered",
                            Source = "Somewhere",
                            AttachedTo="Someone",
                            FullName ="My Name Is KHAN",
                            Phone="00000000",
                            JobTitle ="Ceo of the all ceo",
                            Country ="Somewhere in the world",
                            City = "Capital city of somewhere",
                            Address ="Capital City, Somewhere st/ nowhere home",
                            Company ="Any Holding",
                            WebSite = "anywhere//somewhere.com",
                            EmailAddress ="someone@gmail.com",
                            MailAddress ="116/200/SomeWhere",
                            Facebook = "helloItsMe",
                            Instagram ="HelloItsMe",
                            Comments="BlaBlaBla. and hello"
                        },
                        new LeadsDomain()
                        {
                            Id =1,
                            Status="Delivered",
                            Source = "Somewhere",
                            AttachedTo="Someone",
                            FullName ="My Name Is KHAN",
                            Phone="00000000",
                            JobTitle ="Ceo of the all ceo",
                            Country ="Somewhere in the world",
                            City = "Capital city of somewhere",
                            Address ="Capital City, Somewhere st/ nowhere home",
                            Company ="Any Holding",
                            WebSite = "anywhere//somewhere.com",
                            EmailAddress ="someone@gmail.com",
                            MailAddress ="116/200/SomeWhere",
                            Facebook = "helloItsMe",
                            Instagram ="HelloItsMe",
                            Comments="BlaBlaBla. and hello"
                        }
                    }); ;
                    context.SaveChanges();
                }
            }
        }
    }
}
