using Lima.Leads.Api.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Leads.Api.Repository
{
    public class LeadsRepository:ILeadsRepository
    {
        private readonly LeadsDbContext leadsDbContext;

        public LeadsRepository(LeadsDbContext leadsDbContext)
        {
            this.leadsDbContext = leadsDbContext;
        }
        public async Task<int> AddLeads(LeadsDomain leads)
        {
            var entity = new LeadsDomain()
            {
                Status = leads.Status,
                Source = leads.Source,
                AttachedTo = leads.AttachedTo,
                FullName = leads.FullName,
                Phone = leads.Phone,
                JobTitle = leads.JobTitle,
                Country = leads.Country,
                City = leads.City,
                Address = leads.Address,
                Company = leads.Company,
                WebSite = leads.WebSite,
                EmailAddress = leads.EmailAddress,
                MailAddress = leads.MailAddress,
                Facebook = leads.Facebook,
                Instagram = leads.Instagram,
                Comments = leads.Comments
            };
            leadsDbContext.LeadsDomains.Add(entity);
            await leadsDbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteLeads(LeadsDomain leadsDomain)
        {
            var entity = leadsDbContext.LeadsDomains.FirstOrDefault(x => x.Id == leadsDomain.Id);
            leadsDbContext.LeadsDomains.Remove(entity);
            await leadsDbContext.SaveChangesAsync();
        }

        public IEnumerable<LeadsDomain> GetAllLeads(LeadsDomain leadsDomain)
        {
            var entities = leadsDbContext.LeadsDomains.AsQueryable();
            if (leadsDomain != null)
            {
                if (leadsDomain.Id > 0)
                {
                    entities = entities.Where(w => w.Id == leadsDomain.Id);
                }
                if (!string.IsNullOrEmpty(leadsDomain.FullName?.Trim()))
                {
                    string fullName = leadsDomain.FullName.ToLower().Trim();
                    entities = entities.Where(w => w.FullName.ToLower().Contains(fullName));
                }
                if (!string.IsNullOrEmpty(leadsDomain.AttachedTo?.Trim()))
                {
                    string attachedTo = leadsDomain.AttachedTo.ToLower().Trim();
                    entities = entities.Where(w => w.AttachedTo.ToLower().Contains(attachedTo));
                }
                if (!string.IsNullOrEmpty(leadsDomain.JobTitle?.Trim()))
                {
                    string jobTitle = leadsDomain.JobTitle.ToLower().Trim();
                    entities = entities.Where(w => w.JobTitle.ToLower().Contains(jobTitle));
                }
                if (!string.IsNullOrEmpty(leadsDomain.Status?.Trim()))
                {
                    string status = leadsDomain.Status.ToLower().Trim();
                    entities = entities.Where(w => w.Status.ToLower().Contains(status));
                }
            }
            return entities.ToList();
        }

        public async Task<LeadsDomain> GetLeadsById(int id)
        {
            var entity = await leadsDbContext.LeadsDomains.FindAsync(id);
            return entity;
        }

        public async Task<int> UpdateLeads(LeadsDomain leads)
        {
            var dbleads = await leadsDbContext.LeadsDomains.FindAsync(leads.Id);
            if (dbleads != null)
            {
                dbleads.Status = leads.Status;
                dbleads.Source = leads.Source;
                dbleads.AttachedTo = leads.AttachedTo;
                dbleads.FullName = leads.FullName;
                dbleads.Phone = leads.Phone;
                dbleads.JobTitle = leads.JobTitle;
                dbleads.Country = leads.Country;
                dbleads.City = leads.City;
                dbleads.Address = leads.Address;
                dbleads.Company = leads.Company;
                dbleads.WebSite = leads.WebSite;
                dbleads.EmailAddress = leads.EmailAddress;
                dbleads.MailAddress = leads.MailAddress;
                dbleads.Facebook = leads.Facebook;
                dbleads.Instagram = leads.Instagram;
                dbleads.Comments = leads.Comments;
            }
            await leadsDbContext.SaveChangesAsync();
            return dbleads.Id;
        }
    }
}
