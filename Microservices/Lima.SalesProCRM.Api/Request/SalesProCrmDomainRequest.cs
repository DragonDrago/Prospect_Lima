using System;

namespace Lima.SalesProCRM.Api.Request
{
    public class SalesProCrmDomainRequest
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string LdiName { get; set; }
        public decimal TotalSum { get; set; }
        public string LdiAttachedTo { get; set; }
        public string LdiStatus { get; set; }
    }
}
