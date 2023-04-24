using System.Collections.Generic;

namespace Lima.Admin.Api.Domain
{
    public class CompanyFilter
    {
        public string SectionName { get; set; }
        public List<Filter> Filters { get; set; }
    }
}
