
using System;

namespace Lima.SalesProCRM.Api.Domains
{
    public class SalesProCrmDomain
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Check { get; set; }
        public int LdiId { get; set; }
        public string LdiName { get; set; }
        public decimal TotalSum { get; set; }
        public string ProductsId { get; set; }
        public string LdiAttachedTo { get; set; }
        public string LdiStatus { get; set; }

    }
}
