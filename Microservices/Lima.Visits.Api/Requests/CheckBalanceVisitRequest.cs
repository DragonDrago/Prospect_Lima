using System.Collections.Generic;

namespace Lima.Visits.Api.Requests
{
    public class CheckBalanceVisitRequest : NewVisitRequest
    {
        public List<DrugBalanceRequest> Balance { get; set; }
    }
}
