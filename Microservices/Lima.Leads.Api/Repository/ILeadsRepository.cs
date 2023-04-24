using Lima.Leads.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Leads.Api.Repository
{
    public interface ILeadsRepository
    {
        Task<LeadsDomain> GetLeadsById(int id);
        IEnumerable<LeadsDomain> GetAllLeads(LeadsDomain LeadsDomain);
        Task<int> AddLeads(LeadsDomain Leads);
        Task<int> UpdateLeads(LeadsDomain Leads);
        Task DeleteLeads(LeadsDomain Leads);
    }
}
