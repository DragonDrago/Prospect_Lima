using Lima.Leads.Api.Domain;
using System;
using System.Collections.Generic;

namespace Lima.SalesProCRM.Api.Dtos
{
    public class SalesProCrmViewDomain
    {
        public LeadsDomain leadsDomain { get; set; }
        public List<ProductViewDomain> productViewDomains { get; set; }

        public DateTime DateTime = DateTime.Now;
        public string Check = "";

    }
}
